using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbOS.Common.DataStructures {
	public class BlocoContiguo {

		public int StartIndex { get; set; }
		public int FileSize { get; set; }

		/// <summary>
		/// Verifica se o arquivo ja ocupa o espaço de um outro arquivo
		/// </summary>
		/// <param name="block">bloco a ser analisado</param>
		/// <returns></returns>
		public bool IntersectSpace(BlocoContiguo block) {
			return StartIndex <= block.StartIndex && block.StartIndex <= StartIndex + FileSize - 1;
		}


		public string GetOccupiedBlocks() {
			string toReturn = $"blocos {StartIndex}";
			for (int i = 1; i < FileSize; i++) {
				var separador = i == FileSize - 1 ? " e " : ", ";
				toReturn += $"{separador}{StartIndex + i}";
			}
			return toReturn;
		}

		/// <summary>
		/// Insere o bloco contíguo no conjunto de elementos utilizando o algorítmo FirstFit
		/// </summary>
		/// <typeparam name="T">Tipo do bloco contíguo</typeparam>
		/// <param name="element">Bloco a ser inserido</param>
		/// <param name="blockChain">Lista contendo os blocos ocupados</param>
		/// <param name="maxSize">Tamanho máximo do container</param>
		/// <returns>Uma flag indicando se a inserção foi bem sucedida</returns>
		public static bool FirstFit<T>(T element, List<T> blockChain, int maxSize) where T : BlocoContiguo {

			bool hasInserted = false;

			var firstFile = blockChain.FirstOrDefault();
			bool startsWithFile = firstFile != null && firstFile.StartIndex == 0;

			//itera do começo até o penultimo elemento
			for (int i = -1; i < blockChain.Count; i++) {

				if (i < 0 && startsWithFile) {
					continue;
				}

				var primeiroIndiceLivre = i < 0 ? 0 : blockChain[i].StartIndex + blockChain[i].FileSize;

				var holeSize = i != blockChain.Count - 1 ?
					blockChain[i + 1].StartIndex - primeiroIndiceLivre
					: (maxSize) - primeiroIndiceLivre;

				if (element.FileSize <= holeSize) {
					hasInserted = true;
					element.StartIndex = primeiroIndiceLivre;
					blockChain.Insert(i + 1, element);
					break;
				}

			}

			return hasInserted;
		}
	}
}
