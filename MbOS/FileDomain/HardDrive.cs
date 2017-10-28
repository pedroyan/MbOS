using MbOS.Common;
using MbOS.Common.DataStructures;
using MbOS.FileDomain.DataStructures;
using MbOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbOS.FileDomain {
	public class HardDrive {

		private BlockChain<HardDriveEntry> diskDrive;
		private IProcessService processService = RegistrationService.Resolve<IProcessService>();
		//public string HardDriveMap {
		//	get {

		//	}
		//}

		public HardDrive(int size, List<HardDriveEntry> initialFiles) {

			var initializedFiles = InitilizeFiles(initialFiles, size);
			diskDrive = new BlockChain<HardDriveEntry>(initializedFiles, size);
		}

		/// <summary>
		/// Adiciona um arquivo no disco utilizando o algoritmo First-Fit
		/// </summary>
		/// <param name="file">Arquivo á ser adicionado</param>
		/// <returns>O arquivo adicionado no hd</returns>
		public HardDriveEntry AddFile(HardDriveEntry file) {
			if (file == null) {
				throw new ArgumentException("Arquivo não pode ser nulo", nameof(file));
			}

			if (file.OwnerPID.HasValue && !processService.ExistsProcess(file.OwnerPID.Value)) {
				throw new HardDriveOperationException($"Erro ao criar arquivo: Processo {file.OwnerPID} não existe");
			}

			var hasInserted = diskDrive.FirstFit(file);

			if (!hasInserted) {
				throw new HardDriveOperationException($"O processo {file.OwnerPID} não pode criar o arquivo {file.FileName} (falta de espaço).");
			}

			return file;
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

			var file = diskDrive.Collection.FirstOrDefault(f => f.FileName == fileName);
			if (file == null) {
				throw new HardDriveOperationException($"Arquivo {fileName} não encontrado");
			}

			///Arquivos inicializados podem ser deletados por qualquer um, conforme o exemplo exibido no PDF
			if (file.OwnerPID != PID && !processService.IsRealTimeProcess(PID)) {
				throw new HardDriveOperationException($"Processo {PID} não possui permissão para deletar o arquivo");
			}

			diskDrive.Remove(file);
		}

		/// <summary>
		/// Realiza a inicialização dos arquivos no disco
		/// </summary>
		/// <param name="intializationList">Arquivos a serem inicializados</param>
		/// <param name="maxSize">Tamnho máximo do disco</param>
		private List<HardDriveEntry> InitilizeFiles(List<HardDriveEntry> intializationList, int maxSize) {

			var orderedFiles = intializationList.OrderBy(f => f.StartIndex);
			var toReturn = new List<HardDriveEntry>();

			foreach (var file in orderedFiles) {
				InitializeFile(file, toReturn, maxSize);
			}

			return toReturn;
		}

		/// <summary>
		/// Realiza inicializa um arquivo no disco
		/// </summary>
		/// <param name="file">Arquivo a ser inicializado</param>
		/// <param name="list">Lista para inserção dos arquivos inicializado</param>
		/// <param name="diskSize">Tamnho máximo do disco</param>
		private void InitializeFile(HardDriveEntry file, List<HardDriveEntry> list, int diskSize) {

			if (file.StartIndex >= diskSize || file.StartIndex < 0) {
				throw new ArgumentOutOfRangeException(nameof(file.StartIndex), $"Arquivo {file.FileName} inicializado fora do disco. (Indice {file.StartIndex})");
			}

			var fileOnSpace = list.FirstOrDefault(f => f.IntersectSpace(file));
			if (fileOnSpace != null) {
				throw new HardDriveOperationException(
					$"O Arquivo {file.FileName} não pode ser adicionado pois o arquivo {fileOnSpace.FileName} já ocupa o setor apontado"
				);
			}

			if (file.StartIndex + file.BlockSize > diskSize) {
				throw new ArgumentOutOfRangeException(nameof(file), $"Arquivo {file.FileName} está ultrapassando os limites do disco");
			}

			list.Add(file);
		}

		public HardDriveEntry GetEntryAt(int index) {
			return diskDrive.Collection.ElementAt(index);
		}
	}
}
