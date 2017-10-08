using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileManager {
	public class FileInfo {
		public int OwnerPID { get; set; }
		public string FileName { get; set; }
		public int StartSector { get; set; }
		public int FileSize { get; set; }
	}
}
