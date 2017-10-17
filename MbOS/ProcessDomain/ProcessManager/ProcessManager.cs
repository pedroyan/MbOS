using MbOS.Common;
using MbOS.Interfaces;
using MbOS.MemoryDomain.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MbOS.ProcessDomain.ProcessManager {
	public class ProcessManager : IProcessService {

		StreamReader initializationFile;
		int lineCount;

		public ProcessManager(string fileName) {
			initializationFile = FileHelper.OpenFile(fileName);
		}

		public void Run() {
			RegistrationService.RegisterInstance<IProcessService>(this);
			initializationFile.Dispose();
		}

		private void ReadProcessesFromFile() {
			string line;
			while ((line = GetNextLine()) != null) {
				//Parse na instrução e transforma ela num objeto Process
				//não pode ter mais de 1000 linhas
				//insere esse processo numa lista depois passa pro ProcessLine agrupar
			}
		}

		public bool ExistsProcess(int id) {
			return id < 5 && id != 2;
		}

		public bool IsRealTimeProcess(int PID) {
			//Somente o processo com PID = 0 é considerado processo de tempo real no dipatcher de testes
			return PID == 0;
		}

		private string GetNextLine() {
			lineCount++;
			return initializationFile.ReadLine();
		}
	}
}
