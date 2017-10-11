using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures.Instructions {
	public abstract class FileInstruction {

		protected FileInstruction (FileOperationCode operationCode, int pid, string fileName) {
			Operation = operationCode;
			PID = pid;
			FileName = FileName;
		}
		/// <summary>
		/// Código da operação realizada
		/// </summary>
		public FileOperationCode Operation { get; set; }

		/// <summary>
		/// ID do processo realizando a operação
		/// </summary>
		public int PID { get; set; }
		
		/// <summary>
		/// Nome do arquivo a ser operado
		/// </summary>
		public string FileName { get; set; }
	}
}
