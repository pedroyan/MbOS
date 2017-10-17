using MbOS.Common.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.MemoryDomain.DataStructures {
	public class MemoryBlock : BlocoContiguo {
		public MemoryBlock(int tamanho) : base(tamanho) { }
		public int OwnerPID { get; set; }
	}
}
