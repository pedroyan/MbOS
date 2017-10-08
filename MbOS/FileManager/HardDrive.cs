using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileManager {
	public class HardDrive {

		private List<FileInfo> hardDrive;
		private int size;

		public HardDrive(int size, List<FileInfo> initialFiles) {

			var endOfDisk = new FileInfo() {
				FileName = null,
				FileSize = 0,
				StartSector = size
			};

			this.size = size;

			hardDrive = new List<FileInfo>() { endOfDisk };

			foreach (var file in initialFiles) {
				InitializeFile(file);
			}
		}

		public void AddFile(FileInfo file, int PID) {

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
				throw new HardDriveOperationException($"O processo {PID} não pode criar o arquivo {file.FileName} (falta de espaço).");
			}
		}

		public void InitializeFile(FileInfo file) {

			if (file.StartSector >= size || file.StartSector < 0) {
				throw new ArgumentOutOfRangeException(nameof(file.StartSector), $"Arquivo {file.FileName} inicializado fora do disco. (Indice {file.StartSector})");
			}
		}
	}
}
