using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures.Instructions {
	public class DeleteFileInstruction : FileInstruction {
		public DeleteFileInstruction(string fileName, int PID) : base(PID, fileName) {}

		public override void Execute(HardDrive hdd, int operationNumber) {
			hdd.RemoveFile(FileName,PID);
		}
	}
}
