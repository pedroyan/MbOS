using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures.Instructions {
	public class CreateFileInstruction : FileInstruction {

		public int FileSize { get; set; }

		/// <summary>
		/// Cria um instrução de criação de arquivos
		/// </summary>
		/// <param name="pid">Id do processo que solicita a instrução</param>
		/// <param name="fileName">Nome do arquivo a ser criado</param>
		/// <param name="fileSize">Tamanho do arquivo a ser criado</param>
		public CreateFileInstruction(int pid, string fileName, int fileSize) : base(FileOperationCode.CreateFile, pid, fileName) {
			FileSize = fileSize;
		}
	}
}
