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

		public ProcessManager(string fileName) {
			initializationFile = FileHelper.OpenFile(fileName);
		}

		public void Run() {
			RegistrationService.RegisterInstance<IProcessService>(this);
			initializationFile.Dispose();
		}

		public bool ExistsProcess(int id) {
			return id < 5 && id != 2;
		}

		public bool IsRealTimeProcess(int PID) {
			//Somente o processo com PID = 0 é considerado processo de tempo real no dipatcher de testes
			return PID == 0;
		}
	}
}
