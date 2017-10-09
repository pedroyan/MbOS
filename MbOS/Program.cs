using System;
using MbOS.Common;
using MbOS.Interfaces;
using MbOS.Processes.ProcessManager;
using MbOS.FileDomain;

namespace MbOS {
	class Program {
		static void Main(string[] args) {
			RegistrationService.RegisterInstance<IProcessService>(new ProcessManager());
			var manager = new FileManager("files.txt");
			manager.RunFileManager();
			Console.WriteLine("Hello World!");
			Console.ReadKey();
		}
	}
}
