using MbOS.FileDomain.DataStructures;
using MbOS.FileDomain.DataStructures.Instructions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MbOS.FileDomain {
	public class FileManager {

		private HardDrive hardDrive;
		private int lineCount;
		private StreamReader file;

		public FileManager(string filename) {
			var location = AppDomain.CurrentDomain.BaseDirectory;
			try {
				file = new StreamReader($"{location}\\{filename}");
			} catch (FileNotFoundException) {
				Console.WriteLine($"Erro ao ler arquivo: arquivo não encontrado no path de execução {location}");
				throw;
			} catch (DirectoryNotFoundException) {
				Console.WriteLine($"Diretório do arquivo inválido: {location}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Erro ao ler arquivo: {ex.Message}");
				throw;
			}
		}

		public void RunFileManager() {
			try {
				hardDrive = InitializeHDD(file);
				ExecuteInstructions(file);
			} catch (FileFormatException ex) {
				Console.WriteLine($"Arquivo inválido: {ex.Message}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Erro ao ler arquivo: {ex.Message}");
				throw;
			}
		}

		private void ExecuteInstructions(StreamReader file) {
			string line;
			while ((line = GetNextLine()) != null) {
				Console.WriteLine(line);
			}
		}

		private HardDrive InitializeHDD(StreamReader reader) {
			var line = GetNextLine();
			if (!int.TryParse(line, out int hdSize)) {
				throw new FileFormatException($@"A primeira linha deve conter um valor numérico representando o tamanho do HDD. \n Valor encontrado:{line}");
			}
			var list = GetInitializationList(reader);

			return new HardDrive(hdSize, list);
		}

		private List<HardDriveEntry> GetInitializationList(StreamReader reader) {
			var initializationList = new List<HardDriveEntry>();

			var line = GetNextLine();
			if (!int.TryParse(line, out int initializationListSize)) {
				throw new FileFormatException($@"A segunda linha deve conter um valor numérico representando o tamanho da lista de inicialização. \nValor encontrado:{line}");
			}

			for (int i = 0; i < initializationListSize; i++) {
				line = GetNextLine();
				if (line == null) {
					throw new FileFormatException($"Lista de inicialização deve conter pelo menos {initializationListSize} registros");
				}

				if (string.Empty == line) {
					throw new FileFormatException($"Registro na linha {lineCount} vazio");
				}

				var lineArguments = line.Replace(" ", "").Split(",");
				if (lineArguments.Length != 3) {
					throw new FileFormatException($"Registro inicializado na linha {lineCount} possui a quantidade errada de argumentos. \nArgumentos passados: {lineArguments.Length}\nArgumentos esperados: 3");
				}

				var entryName = lineArguments[0];
				if (!int.TryParse(lineArguments[1],out int startSector)) {
					throw new FileFormatException($"Registro na linha {lineCount} possui um indíce de setor inválido");
				}

				if (!int.TryParse(lineArguments[2], out int entrySize)) {
					throw new FileFormatException($"Arquivo na linha {lineCount} não possui um tamanho válido. Certifique-se de que o tamanho é um número inteiro. \nTamanho passado: {lineArguments[2]}");
				}

				var entry = new HardDriveEntry(entryName, null, entrySize) {
					StartSector = startSector
				};
				initializationList.Add(entry);
			}

			return initializationList;

		}

		private FileInstruction ParseInstruction(string line) {
			if (string.IsNullOrWhiteSpace(line)) {
				throw new FileFormatException($"Instrução na linha {lineCount} vaziq");
			}

			var arguments = line.Replace(" ", "").Split(",");
			if (arguments.Length < 3) {
				throw new FileFormatException($"Instrução na linha {lineCount} não possui o número de argumentos necessário");
			}

			if (!int.TryParse(arguments[0],out int pid)) {
				throw new FileFormatException($"Linha {lineCount} - primeiro argumento precisa ser um numero inteiro");
			}


			return null;
		}

		private string GetNextLine() {
			lineCount++;
			return file.ReadLine();
		}
	}
}
