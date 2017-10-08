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
				AddFile(file);
			}
		}

		public void AddFile(FileInfo file) {

			if (file == null) {
				throw new ArgumentException("Arquivo não pode ser nulo", nameof(file));
			}

			if (file.StartSector >= size || file.StartSector < 0) {
				throw new ArgumentOutOfRangeException(nameof(file.StartSector), $"Arquivo inicializado fora do disco. (Indice {file.StartSector})");
			}

			//itera do começo até o penultimo elemento
			for (int i = 0; i < hardDrive.Count - 1; i++) {
				var primeiroIndiceLivre = hardDrive[i].StartSector + hardDrive[i].FileSize;
				var holeSize = primeiroIndiceLivre -
			}
		}

		public void InitializeFile(FileInfo file) {

		}
	}
}
