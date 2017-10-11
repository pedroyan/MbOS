using MbOS.Common;
using MbOS.FileDomain.DataStructures;
using MbOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbOS.FileDomain {
	public class HardDrive {

		private List<HardDriveEntry> diskDrive;
		private int diskSize;
		private IProcessService processService = RegistrationService.Resolve<IProcessService>();

		public HardDrive(int size, List<HardDriveEntry> initialFiles) {
			var endOfDisk = new HardDriveEntry(null, 0, 1) {
				StartSector = size
			};
			this.diskSize = size;

			diskDrive = new List<HardDriveEntry>();
			InitilizeFiles(initialFiles);
			diskDrive.Add(endOfDisk);
		}

		/// <summary>
		/// Adiciona um arquivo no disco utilizando o algoritmo First-Fit
		/// </summary>
		/// <param name="file">Arquivo á ser adicionado</param>
		public void AddFile(HardDriveEntry file) {
			if (file == null) {
				throw new ArgumentException("Arquivo não pode ser nulo", nameof(file));
			}

			if (file.OwnerPID.HasValue && !processService.ExistsProcess(file.OwnerPID.Value)) {
				throw new HardDriveOperationException($"Erro ao criar arquivo: Processo {file.OwnerPID} não existe");
			}

			bool hasInserted = false;
			//itera do começo até o penultimo elemento
			for (int i = 0; i < diskDrive.Count - 1; i++) {

				var primeiroIndiceLivre = diskDrive[i].StartSector + diskDrive[i].FileSize;
				var holeSize =  diskDrive[i + 1].StartSector - primeiroIndiceLivre;

				if (file.FileSize <= holeSize) {
					hasInserted = true;
					file.StartSector = primeiroIndiceLivre;
					diskDrive.Insert(i + 1, file);
					break;
				}

			}

			if (!hasInserted) {
				throw new HardDriveOperationException($"O processo {file.OwnerPID} não pode criar o arquivo {file.FileName} (falta de espaço).");
			}
		}

		/// <summary>
		/// Realiza a remoção de um arquivo do HD
		/// </summary>
		/// <param name="fileName">Nome do arquivo a ser removido</param>
		/// <param name="PID">ID do processo solicitando uma remoção</param>
		public void RemoveFile(string fileName, int PID) {
			if (!processService.ExistsProcess(PID)) {
				throw new HardDriveOperationException($"Falha ao deletar arquivo: Processo de ID {PID} não existe");
			}

			var file = diskDrive.FirstOrDefault(f => f.FileName == fileName);
			if (file == null) {
				throw new HardDriveOperationException($"Arquivo {fileName} não encontrado");
			}

			if (file.OwnerPID != PID && !processService.IsRealTimeProcess(PID)) {
				throw new HardDriveOperationException($"Processo {PID} não possui permissão para deletar o arquivo");
			}

			diskDrive.Remove(file);
		}

		/// <summary>
		/// Realiza a inicialização dos arquivos no disco
		/// </summary>
		/// <param name="intializationList">Arquivos a serem inicializados</param>
		private void InitilizeFiles(List<HardDriveEntry> intializationList) {
			var orderedFiles = intializationList.OrderBy(f => f.StartSector);
			foreach (var file in orderedFiles) {
				InitializeFile(file);
			}
		}

		/// <summary>
		/// Realiza inicializa um arquivo no disco
		/// </summary>
		/// <param name="file">Arquivo a ser inicializado</param>
		private void InitializeFile(HardDriveEntry file) {

			if (file.StartSector >= diskSize || file.StartSector < 0) {
				throw new ArgumentOutOfRangeException(nameof(file.StartSector), $"Arquivo {file.FileName} inicializado fora do disco. (Indice {file.StartSector})");
			}

			var fileOnSpace = diskDrive.FirstOrDefault(f => f.IntersectSpace(file));
			if (fileOnSpace != null) {
				throw new HardDriveOperationException(
					$"O Arquivo {file.FileName} não pode ser adicionado pois o arquivo {fileOnSpace.FileName} já ocupa o setor apontado"
				);
			}

			if (file.StartSector + file.FileSize > diskSize) {
				throw new ArgumentOutOfRangeException(nameof(file), $"Arquivo {file.FileName} está ultrapassando os limites do disco");
			}

			diskDrive.Add(file);
		}

		public HardDriveEntry GetEntryAt(int index) {
			if (index >= diskDrive.Count -1) {
				throw new IndexOutOfRangeException();
			}
			return diskDrive[index];
		}
	}
}
