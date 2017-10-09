using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileManager {
	public class FileManager {
		public string FileName { get; private set; }

		public FileManager(string filename) {
			FileName = filename;
		}


	}
}
