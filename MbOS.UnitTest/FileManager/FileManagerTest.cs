using MbOS.Common;
using MbOS.FileManager;
using MbOS.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.UnitTest.FileManager {
	[TestClass]
	public class FileManagerTest {
		[TestInitialize]
		public void Initialization() {
			RegistrationService.RegisterInstance<IDispatcher>(new MockDispatcher());
		}

		[TestMethod]
		public void InitializationTest() {

			// Testa arquivos inicializados em setores inválidos
			#region ValidSectors
			var ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("Arquivo A",0,1){ StartSector = -1},
			};
			TestSetorInvalido(ListaArquivos, 2);

			ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("Arquivo B",0,1){StartSector = 5}
			};
			TestSetorInvalido(ListaArquivos, 5);
			#endregion

			//Testa arquivos inicializados com relação ao tamanho total no hd
			#region HDSize
			ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("Arquvio A",0,5)
			};
			TestEspacoEstourado(ListaArquivos, 5, deveFuncionar: true);

			ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("A",0,1){StartSector = 0},
				new HardDriveEntry("B",0,2){StartSector = 1},
				new HardDriveEntry("C",0,2){StartSector = 3},
			};
			TestEspacoEstourado(ListaArquivos, 5, deveFuncionar: true);

			ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("A",0,1){StartSector = 0},
				new HardDriveEntry("B",0,2){StartSector = 1},
				new HardDriveEntry("C",0,3){StartSector = 3},
			};
			TestEspacoEstourado(ListaArquivos, 5, deveFuncionar: false);

			ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("A",0,6){StartSector = 0}
			};
			TestEspacoEstourado(ListaArquivos, 5, deveFuncionar: false);
			#endregion

			//Testas falhas com Overlap
			#region Overlap
			ListaArquivos = new List<HardDriveEntry>() {
				new HardDriveEntry("A",0,3),
				new HardDriveEntry("B",0,1){StartSector = 2}
			};

			TestOverlap(ListaArquivos, 6, false);
			#endregion
		}

		private void TestSetorInvalido(List<HardDriveEntry> initList, int hdSize) {
			try {
				var hd = new HardDrive(hdSize, initList);
				Assert.Fail();
			} catch (ArgumentOutOfRangeException ex) {
				Assert.IsTrue(ex.ParamName == nameof(HardDriveEntry.StartSector));
			}
		}

		private void TestEspacoEstourado(List<HardDriveEntry> initList, int hdSize, bool deveFuncionar) {
			try {
				var hd = new HardDrive(hdSize, initList);
				if (!deveFuncionar) {
					Assert.Fail();
				}
			} catch (ArgumentOutOfRangeException) {
				if (deveFuncionar) {
					Assert.Fail();
				}
			}
		}

		private void TestOverlap(List<HardDriveEntry> initList, int hdSize, bool deveFuncionar) {
			try {
				var hd = new HardDrive(hdSize, initList);
				if (!deveFuncionar) {
					Assert.Fail();
				}
			} catch (HardDriveOperationException) {
				if (deveFuncionar) {
					Assert.Fail();
				}
			}
		}
	}
}
