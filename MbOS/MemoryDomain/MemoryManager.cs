﻿using MbOS.Common;
using MbOS.Common.DataStructures;
using MbOS.Interfaces;
using MbOS.MemoryDomain.DataStructures;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.MemoryDomain {
	public class MemoryManager {
		public const int RealTimeSize = 64;
		public const int UserSize = 960;
		private BlockChain<MemoryBlock> RealTime = new BlockChain<MemoryBlock>(RealTimeSize);
		private BlockChain<MemoryBlock> User = new BlockChain<MemoryBlock>(UserSize);

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

		/// <summary>
		/// Verifica se é possivel alocar um bloco de memória de tamanho <paramref name="size"/>
		/// </summary>
		/// <param name="size">Tamanho do bloco</param>
		/// <param name="isRealTime">
		/// Indica se o processo que vai realizar a alocação
		/// é um processo de Tempo Real
		/// </param>
		/// <returns></returns>
		public bool CanAllocate(int size, bool isRealTime) {
			return isRealTime ? RealTime.CanFit(size) : User.CanFit(size);
		}

		/// <summary>
		/// Desaloca a memória para o processo id <paramref name="PID"/>
		/// </summary>
		/// <param name="PID">Id do processo a ter a memória desalocada</param>
		/// <param name="isRealTime">Flag indicando se o processo é de tempo real</param>
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
