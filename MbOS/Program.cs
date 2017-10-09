using System;
using MbOS.Common;
using MbOS.Interfaces;
using MbOS.Processes.ProcessManager;

namespace MbOS {
	class Program {
		static void Main(string[] args) {
			RegistrationService.RegisterInstance<IProcessService>(new ProcessManager());
			Console.WriteLine("Hello World!");
			Console.ReadKey();
		}
	}
}
