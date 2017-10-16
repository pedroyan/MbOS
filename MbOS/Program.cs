using System;
using MbOS.Common;
using MbOS.Interfaces;
using MbOS.Processes.ProcessManager;
using MbOS.FileDomain;

namespace MbOS {
	class Program {
		static void Main(string[] args) {

			var processes = new ProcessManager();
			processes.Run();

			var manager = new FileManager("files.txt");
			manager.Run();
			Console.ReadKey();
		}
	}
}
