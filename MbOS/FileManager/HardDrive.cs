using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbOS.FileManager {
	public class HardDrive {

		private List<FileInfo> hardDrive;
		private int size;

		public HardDrive(int size, List<FileInfo> initialFiles) {
			var endOfDisk = new FileInfo(null, 0, 1) {
				StartSector = size
			};
			this.size = size;

			hardDrive = new List<FileInfo>() { endOfDisk };
			InitilizeFiles(initialFiles);
		}

		/// <summary>
		/// Adiciona um arquivo no disco utilizando o algoritmo First-Fit
		/// </summary>
		/// <param name="file">Arquivo á ser adicionado</param>
		private void AddFile(FileInfo file) {

			if (file == null) {
				throw new ArgumentException("Arquivo não pode ser nulo", nameof(file));
			}

			bool hasInserted = false;
			//itera do começo até o penultimo elemento
			for (int i = 0; i < hardDrive.Count - 1; i++) {

				var primeiroIndiceLivre = hardDrive[i].StartSector + hardDrive[i].FileSize;
				var holeSize = primeiroIndiceLivre - hardDrive[i + 1].StartSector;

				if (file.FileSize < holeSize) {
					hasInserted = true;
					file.StartSector = primeiroIndiceLivre;
					hardDrive.Insert(i + 1, file);
					break;
				}

			}

			if (!hasInserted) {
				throw new HardDriveOperationException($"O processo {file.OwnerPID} não pode criar o arquivo {file.FileName} (falta de espaço).");
			}
		}

		/// <summary>
		/// Realiza a inicialização do disco
		/// </summary>
		/// <param name="intializationList">Arquivos a serem inicializados</param>
		private void InitilizeFiles(List<FileInfo> intializationList) {
			var orderedFiles = intializationList.OrderBy(f => f.StartSector);
			foreach (var file in orderedFiles) {
				InitializeFile(file);
			}
		}

		private void InitializeFile(FileInfo file) {

			if (file.StartSector >= size || file.StartSector < 0) {
				throw new ArgumentOutOfRangeException(nameof(file.StartSector), $"Arquivo {file.FileName} inicializado fora do disco. (Indice {file.StartSector})");
			}

			var fileOnSpace = hardDrive.FirstOrDefault(f => f.IntersectSpace(file));
			if (fileOnSpace != null) {
				throw new HardDriveOperationException(
					$"O Arquivo {file.FileName} não pode ser adicionado pois o arquivo {fileOnSpace.FileName} já ocupa o setor apontado"
				);
			}

			hardDrive.Add(file);
		}
	}
}
