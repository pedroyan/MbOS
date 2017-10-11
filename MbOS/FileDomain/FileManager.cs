using MbOS.FileDomain.DataStructures;
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
				throw;
			} catch (DirectoryNotFoundException) {
				Console.WriteLine($"Diretório do arquivo inválido: {location}");
				throw;
			} catch(FileFormatException ex) {
				Console.WriteLine($"Arquivo inválido: {ex.Message}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Erro ao ler arquivo: {ex.Message}");
				throw;
			}
		}

		private void InitializeHDD(StreamReader reader) {
			var line = reader.ReadLine();
			if (!int.TryParse(line, out int hdSize)) {
				throw new FileFormatException($@"A primeira linha deve conter um valor numérico representando o tamanho do HDD. \n Valor encontrado:{line}");
			}

			line = reader.ReadLine();
			if (!int.TryParse(line,out int initializationListSize)) {
				throw new FileFormatException($@"A segunda linha deve conter um valor numérico representando o tamanho da lista de inicialização. \n Valor encontrado:{line}");
			}
		}
	}
}
