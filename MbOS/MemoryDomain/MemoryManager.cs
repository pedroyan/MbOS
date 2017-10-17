using MbOS.Common;
using MbOS.Common.DataStructures;
using MbOS.Interfaces;
using MbOS.MemoryDomain.DataStructures;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.MemoryDomain {
	public class MemoryManager {
		private BlockChain<MemoryBlock> RealTime = new BlockChain<MemoryBlock>(64);
		private BlockChain<MemoryBlock> User = new BlockChain<MemoryBlock>(960);

		/// <summary>
		/// Aloca um bloco contíguo de memória para o processo
		/// </summary>
		/// <param name="PID">Id do processo</param>
		/// <param name="memorySize">Tamanho da memória alocada</param>
		/// <param name="isRealTime">Indica se o processo passado é de tempo real</param>
		/// <returns>O bloco alocado. Retorna nulo caso a operação falhe</returns>
		public MemoryBlock AllocateMemory(MemoryBlock block, bool isRealTime) {
			bool success = isRealTime ? RealTime.FirstFit(block) : User.FirstFit(block);

			if (!success) {
				return null;
			}

			return block;
		}

		public bool CanAllocate(MemoryBlock block, bool isRealTime) {
			return isRealTime ? RealTime.CanFit(block.BlockSize) : User.CanFit(block.BlockSize);
		}

		public void DeallocateMemory(int PID, bool isRealTime) {
			var block = isRealTime ? RealTime.Collection.FirstOrDefault(m => m.OwnerPID == PID) 
				: User.Collection.FirstOrDefault(m => m.OwnerPID == PID);

			if (block == null) {
				throw new ArgumentException("PID não existente");
			}

			if (isRealTime) {
				RealTime.Remove(block);
			} else {
				User.Remove(block);
			}
		}
	}
}
