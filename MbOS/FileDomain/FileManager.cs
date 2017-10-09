using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MbOS.FileDomain {
	public class FileManager {
		public string FileName { get; private set; }

		public FileManager(string filename) {
			FileName = filename;
		}

		public void RunFileManager() {
			ReadFile();
		}

		private void ReadFile() {
			var location = AppDomain.CurrentDomain.BaseDirectory;
			try {
				StreamReader file = new StreamReader($"{location}\\{FileName}");
				string line;

				while ((line = file.ReadLine()) != null) {
					System.Console.WriteLine(line);
				}

			} catch (Exception ex) {
				if (ex is FileNotFoundException) {
					Console.WriteLine($"Erro ao ler arquivo: arquivo não encontrado no path de execução {location}");
				} else if (ex is DirectoryNotFoundException) {
					Console.WriteLine("Diretório do arquivo inválido");
				} else {
					Console.WriteLine(ex.Message);
				}
				throw;
			}
		}
	}
}
