using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileManager {
	public class HardDriveOperationException : Exception {
		public HardDriveOperationException(string message) : base(message) { }
	}
}
