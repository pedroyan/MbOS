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

		[TestMethod]
		public void AddFileTest() {

			// A|A|0|0|0|B|B|B|C|0| D| D| 0|
			// 0|1|2|3|4|5|6|7|8|9|10|11|12|
			var initializationList = new List<HardDriveEntry>() {
				new HardDriveEntry("A",0,2),
				new HardDriveEntry("B",0,3){StartSector = 5},
				new HardDriveEntry("C",0,1){StartSector=8},
				new HardDriveEntry("D",0,2){StartSector=10}
			};

			var hd = new HardDrive(13, initializationList);
			var file = new HardDriveEntry("E", 0, 3);
			TestAdicionarArquivo(hd, file, deveFuncionar: true);

			// A|A|E|E|E|B|B|B|C|0| D| D| 0|
			// 0|1|2|3|4|5|6|7|8|9|10|11|12|
			var resultFile = hd.GetEntryAt(1);
			Assert.AreEqual(resultFile, file);

			file = new HardDriveEntry("F", 0, 2);
			TestAdicionarArquivo(hd, file, deveFuncionar: false);

			// A|A|E|E|E|B|B|B|C|G| D| D| 0|
			// 0|1|2|3|4|5|6|7|8|9|10|11|12|
			file = new HardDriveEntry("G", 0, 1);
			TestAdicionarArquivo(hd, file, deveFuncionar: true);

			resultFile = hd.GetEntryAt(4);
			Assert.AreEqual(resultFile, file);

			// A|A|E|E|E|B|B|B|C|G| D| D| H|
			// 0|1|2|3|4|5|6|7|8|9|10|11|12|
			file = new HardDriveEntry("H", 0, 1);
			TestAdicionarArquivo(hd, file, deveFuncionar: true);

			resultFile = hd.GetEntryAt(6);
			Assert.AreEqual(resultFile, file);
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

		private void TestAdicionarArquivo(HardDrive hd, HardDriveEntry file, bool deveFuncionar) {
			try {
				hd.AddFile(file);
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
