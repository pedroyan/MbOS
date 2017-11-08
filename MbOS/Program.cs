using System;
using MbOS.Common;
using MbOS.Interfaces;
using MbOS.ProcessDomain.ProcessManager;
using MbOS.FileDomain;

namespace MbOS {
	class Program {
		static void Main(string[] args) {

#if DEBUG
			string processPath = "Resources/processes.txt";
			string filesPath = "Resources/files2.txt";
#else
			AppDomain.CurrentDomain.UnhandledException += MasterHandler;
			if (args.Length != 2) {
				Console.WriteLine("São necessários dois argumentos para executar o programa:");
				Console.WriteLine("<Path para o arquivo de Processos> <Path para o arquivo do sistema de arquivos>");
				return;
			}

			var processPath = args[0];
			var filesPath = args[1];
#endif
			var processes = new ProcessManager(processPath);
			processes.Run();

			var manager = new FileManager(filesPath);
			manager.Run();
			Console.ReadKey();
		}

		private static void MasterHandler(object sender, UnhandledExceptionEventArgs e) {
			Environment.Exit(1);
		}
	}
}
