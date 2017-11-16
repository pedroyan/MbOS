using MbOS.Common;
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
		private StreamReader initializationFile;
		private string fileName;

		/// <summary>
		/// Constroi uma instância de um file managar
		/// </summary>
		/// <param name="filename">Path para o arquivo (a partir do diretório do executável) de inicialização do disco</param>
		public FileManager(string filename) {
			initializationFile = FileHelper.OpenFile(filename);
			fileName = filename;
		}

		/// <summary>
		/// Executa o gerenciado de arquivos
		/// </summary>
		public void Run() {
			try {
				hardDrive = InitializeHDD(initializationFile);
				ExecuteInstructions(initializationFile);
				hardDrive.HardDriveMap();
				initializationFile.Dispose();
			} catch (FileFormatException ex) {
				Console.WriteLine($"Arquivo {fileName} inválido: {ex.Message}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Erro ao ler arquivo {fileName}: {ex.Message}");
				throw;
			}
		}

		private void ExecuteInstructions(StreamReader file) {
			string line;
			int i = 1;
			while ((line = GetNextLine()) != null) {
				var inst = ParseInstruction(line);
				try {
					//Testes podem ser feitos por injeção de dependência
					inst.Execute(hardDrive, i);
				} catch (HardDriveOperationException ex) {
					Console.WriteLine($"Operacao {i} => Falha");
					Console.WriteLine(ex.Message);
				}
				Console.WriteLine();
				i++;
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
				if (!int.TryParse(lineArguments[1], out int startSector)) {
					throw new FileFormatException($"Registro na linha {lineCount} possui um indíce de setor inválido");
				}

				if (!int.TryParse(lineArguments[2], out int entrySize)) {
					throw new FileFormatException($"Arquivo na linha {lineCount} não possui um tamanho válido. Certifique-se de que o tamanho é um número inteiro. \nTamanho passado: {lineArguments[2]}");
				}

				var entry = new HardDriveEntry(entryName, null, entrySize) {
					StartIndex = startSector
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

			if (!int.TryParse(arguments[0], out int pid)) {
				throw new FileFormatException($"Linha {lineCount} - primeiro argumento precisa ser um numero inteiro");
			}

			if (!Enum.TryParse(arguments[1], out FileOperationCode code)) {
				throw new FileFormatException($"Linha {lineCount} - segundo argumento precisa ser um código de operação válido");
			}

			var filename = arguments[2];

			if (code == FileOperationCode.CreateFile) {
				if (!int.TryParse(arguments[3], out int fileSize)) {
					throw new FileFormatException($"Linha {lineCount} - quarto argumento precisa ser um numero inteiro");
				}
				return new CreateFileInstruction(filename, pid, fileSize);
			}


			return new DeleteFileInstruction(filename, pid);
		}

		private string GetNextLine() {
			lineCount++;
			return initializationFile.ReadLine();
		}
	}
}
