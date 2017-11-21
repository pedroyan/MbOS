using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MbOS.Common {
	public static class FileHelper {

		/// <summary>
		/// Abre o arquivo txt passado no path de execução do programa com exceções mais user friendly
		/// </summary>
		/// <param name="fileName">Nome do arquivo a ser aberto</param>
		/// <returns>Uma instancia de um StreamReader que lê o txt</returns>
		public static StreamReader OpenFile(string fileName) {
			var location = AppDomain.CurrentDomain.BaseDirectory;
			try {
				return new StreamReader(Path.Combine(location,fileName));
			} catch (FileNotFoundException) {
				Console.WriteLine($"Erro ao ler arquivo: arquivo {fileName} não encontrado no path de execução {location}");
				throw;
			} catch (DirectoryNotFoundException) {
				Console.WriteLine($"Diretório do arquivo inválido: {location}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Erro ao ler arquivo: {ex.Message}");
				throw;
			}
		}
	}
}
