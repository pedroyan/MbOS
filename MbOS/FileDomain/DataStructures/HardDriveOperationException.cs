using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures {
	public class HardDriveOperationException : Exception {
		public HardDriveOperationException(string message) : base(message) { }
	}
}
