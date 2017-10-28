using MbOS.Common.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.MemoryDomain.DataStructures {
	public class MemoryBlock : BlocoContiguo {
		public int OwnerPID { get; set; }

		public MemoryBlock(int tamanho, int ownerPid) : base(tamanho) {
			OwnerPID = ownerPid;
		}
	}
}
