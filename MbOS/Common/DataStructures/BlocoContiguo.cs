using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbOS.Common.DataStructures {
	public class BlocoContiguo {

		public int StartIndex { get; set; }
		public int BlockSize { get; set; }

		/// <summary>
		/// Verifica se o arquivo ja ocupa o espaço de um outro arquivo
		/// </summary>
		/// <param name="block">bloco a ser analisado</param>
		/// <returns></returns>
		public bool IntersectSpace(BlocoContiguo block) {
			return StartIndex <= block.StartIndex && block.StartIndex <= StartIndex + BlockSize - 1;
		}

		public BlocoContiguo(int tamanho) {
			BlockSize = tamanho;
		}

		public string GetOccupiedBlocks() {
			string toReturn = $"blocos {StartIndex}";
			for (int i = 1; i < BlockSize; i++) {
				var separador = i == BlockSize - 1 ? " e " : ", ";
				toReturn += $"{separador}{StartIndex + i}";
			}
			return toReturn;
		}
	}
}
