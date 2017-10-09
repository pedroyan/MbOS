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
				InitializeHDD(file);
			} catch (FileNotFoundException) {
				Console.WriteLine($"Erro ao ler arquivo: arquivo não encontrado no path de execução {location}");
			} catch (DirectoryNotFoundException) {
				Console.WriteLine($"Diretório do arquivo inválido: {location}");
			} catch (Exception ex) {
				Console.WriteLine($"Erro ao ler arquivo{ex.Message}");
			}
		}

		private void InitializeHDD(StreamReader reader) {
			var line = reader.ReadLine();
			if (!int.TryParse(reader.ReadLine(), out int hdSize)) {
			}
		}
	}
}
