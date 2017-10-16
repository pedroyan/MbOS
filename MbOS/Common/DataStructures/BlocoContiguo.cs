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
	}
}
