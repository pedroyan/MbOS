using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures.Instructions {
	public class CreateFileInstruction : FileInstruction {

		public int FileSize { get; set; }

		/// <summary>
		/// Cria um instrução de criação de arquivos
		/// </summary>
		/// <param name="fileName">Nome do arquivo a ser criado</param>
		/// <param name="PID">Id do processo que solicita a instrução</param>
		/// <param name="fileSize">Tamanho do arquivo a ser criado</param>
		public CreateFileInstruction(string fileName, int PID, int fileSize) : base(PID, fileName) {
			FileSize = fileSize;
		}

		public override void Execute(HardDrive hdd, int operationNumber) {
			var file = hdd.AddFile(new HardDriveEntry(FileName, PID, FileSize));
			Console.WriteLine($"Operacao {operationNumber} => Sucesso");
			Console.WriteLine($"O processo {PID} criou o arquivo ");
		}
	}
}
