using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MbOS.Common.DataStructures {
	public class BlockChain<T> where T : BlocoContiguo {
		private List<T> list;

		public int MaxSize { get; private set; }

		public IEnumerable<T> Collection {
			get {
				return list;
			}
		}

		public BlockChain(List<T> collection, int maxContainerSize) {
			list = collection ?? new List<T>();
			MaxSize = maxContainerSize;
		}

		public BlockChain(int maxContainerSize) {
			list = new List<T>();
			MaxSize = maxContainerSize;
		}

		/// <summary>
		/// Insere o bloco contíguo no conjunto de elementos utilizando o algorítmo FirstFit
		/// </summary>
		/// <typeparam name="T">Tipo do bloco contíguo</typeparam>
		/// <param name="element">Bloco a ser inserido</param>
		/// <param name="blockChain">Lista contendo os blocos ocupados</param>
		/// <param name="maxSize">Tamanho máximo do container</param>
		/// <returns>Uma flag indicando se a inserção foi bem sucedida</returns>
		public bool FirstFit(T element){

			var result = CanFitDetailed(element.BlockSize);
			if (result.CanFit) {
				element.StartIndex = result.PrimeiroIndice ;
				list.Insert(result.Index + 1, element);
			}
			 
			return result.CanFit;
		}

		private CanFitResult CanFitDetailed(int size) {
			var firstFile = list.FirstOrDefault();
			bool startsWithFile = firstFile != null && firstFile.StartIndex == 0;

			//itera do começo até o penultimo elemento
			for (int i = -1; i < list.Count; i++) {

				if (i < 0 && startsWithFile) {
					continue;
				}

				var primeiroIndiceLivre = i < 0 ? 0 : list[i].StartIndex + list[i].BlockSize;

				var holeSize = i != list.Count - 1 ?
					list[i + 1].StartIndex - primeiroIndiceLivre
					: (MaxSize) - primeiroIndiceLivre;

				if (size <= holeSize) {
					return new CanFitResult {
						Index = i,
						CanFit = true,
						PrimeiroIndice = primeiroIndiceLivre
					};
				}

			}

			return new CanFitResult {
				CanFit = false,
			}; 
		}

		public bool CanFit(int size) {
			return CanFitDetailed(size).CanFit;
		}

		public bool Remove(T element) {
			return list.Remove(element);
		}
	}

	public struct CanFitResult {
		public int Index { get; set; }
		public int PrimeiroIndice { get; set; }
		public bool CanFit { get; set; }
	}
}
