using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures.Instructions {
	public abstract class FileInstruction {

		protected FileInstruction (int pid, string fileName) {
			PID = pid;
			FileName = fileName;
		}

		/// <summary>
		/// ID do processo realizando a operação
		/// </summary>
		public int PID { get; set; }
		
		/// <summary>
		/// Nome do arquivo a ser operado
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Executa a instrução
		/// </summary>
		/// <param name="hdd">HDD que será executada a instrução</param>
		/// <param name="operationNumber">Número da instrução</param>
		public abstract void Execute(HardDrive hdd, int operationNumber);
	}
}
