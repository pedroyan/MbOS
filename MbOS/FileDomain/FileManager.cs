using MbOS.FileDomain.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MbOS.FileDomain {
	public class FileManager {
		private HardDrive hardDrive;

		public string FileName { get; private set; }

		public FileManager(string filename) {
			FileName = filename;
		}

		public void RunFileManager() {
			hardDrive = ReadFile();
		}

		private HardDrive ReadFile() {
			var location = AppDomain.CurrentDomain.BaseDirectory;
			try {
				StreamReader file = new StreamReader($"{location}\\{FileName}");
				return InitializeHDD(file);
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

		private HardDrive InitializeHDD(StreamReader reader) {
			var line = reader.ReadLine();
			if (!int.TryParse(line, out int hdSize)) {
				throw new FileFormatException($@"A primeira linha deve conter um valor numérico representando o tamanho do HDD. \n Valor encontrado:{line}");
			}
			var list = GetInitializationList(reader);

			return new HardDrive(hdSize, list);
		}

		private List<HardDriveEntry> GetInitializationList(StreamReader reader) {
			var initializationList = new List<HardDriveEntry>();

			var line = reader.ReadLine();
			if (!int.TryParse(line, out int initializationListSize)) {
				throw new FileFormatException($@"A segunda linha deve conter um valor numérico representando o tamanho da lista de inicialização. \n Valor encontrado:{line}");
			}

			for (int i = 0; i < initializationListSize; i++) {
				line = reader.ReadLine();
				if (line == null) {
					throw new FileFormatException($"Lista de inicialização deve conter pelo menos {initializationListSize} registros");
				}

				if (string.Empty == line) {
					throw new FileFormatException($"Registro na linha {i+3} vazio");
				}

				var lineArguments = line.Replace(" ", "").Split(",");
				if (lineArguments.Length != 3) {
					throw new FileFormatException($"Registro na linha {i+3} possui a quantidade errada de argumentos");
				}

				var entryName = lineArguments[0];
				if (!int.TryParse(lineArguments[1],out int startSector)) {
					throw new FileFormatException($"Registro na linha {i+3} possui um indíce de setor inválido");
				}

				if (!int.TryParse(lineArguments[2], out int entrySize)) {
					throw new FileFormatException($"Arquivo na linha {i+3} não possui um tamanho válido. Certifique-se de que o tamanho é um número inteiro. \n Tamanho passado: {lineArguments[2]}");
				}

				var entry = new HardDriveEntry(entryName, null, entrySize) {
					StartSector = startSector
				};
				initializationList.Add(entry);
			}

			return initializationList;

		}
	}
}
