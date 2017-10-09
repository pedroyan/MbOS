using System;
using MbOS.Common;
using MbOS.Interfaces;
using MbOS.Processes.ProcessDispatcher;

namespace MbOS {
	class Program {
		static void Main(string[] args) {
			RegistrationService.RegisterInstance<IDispatcher>(new ProcessDispatcher());
			Console.WriteLine("Hello World!");
			Console.ReadKey();
		}
	}
}
