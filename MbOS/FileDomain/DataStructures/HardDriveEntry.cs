﻿using MbOS.Common.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.FileDomain.DataStructures {
	public class HardDriveEntry : BlocoContiguo {

		public int? OwnerPID { get; set; }
		public string FileName { get; set; }


		/// <summary>
		/// Constroi uma instância de um arquivo
		/// </summary>
		/// <param name="nomeArquivo">Nome do Arquivo a ser criado</param>
		/// <param name="ownerPID">Processo criador do arquivo. Nulo quando o arquivo foi inicializado no disco</param>
		/// <param name="fileSize">Tamannho do arquivo</param>
		public HardDriveEntry(string nomeArquivo,int? ownerPID, int fileSize) : base(fileSize) {
			if (fileSize < 1) {
				throw new ArgumentOutOfRangeException(nameof(fileSize), "O arquivo deve ocupar pelo menos um bloco");
			}

			OwnerPID = ownerPID;
			FileName = nomeArquivo;
		}
        public bool Compare(object other) {
            var toCompareWith = other as HardDriveEntry;
            if (toCompareWith == null)
                return false;
            return this.BlockSize == toCompareWith.BlockSize &&
                this.FileName == toCompareWith.FileName &&
                this.OwnerPID == toCompareWith.OwnerPID &&
                this.StartIndex == toCompareWith.StartIndex;
               
        }

    }
}
