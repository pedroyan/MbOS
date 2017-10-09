using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileManager.DataStructures {
	public class HardDriveOperationException : Exception {
		public HardDriveOperationException(string message) : base(message) { }
	}
}
