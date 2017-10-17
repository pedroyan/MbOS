using MbOS.MemoryDomain.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.ProcessDomain.DataStructures {
	public class Process {

		public int PID { get; set; }

		/// <summary>
		/// Quantos ticks para a inicialização
		/// </summary>
		public int InitializationTime { get; set; }

		/// <summary>
		/// Ticks para concluir o processamento
		/// </summary>
		public int ProcessingTime { get; set; }

		/// <summary>
		/// Quanto menor o valor, maior a prioridade
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// Espaço da memória alocado
		/// </summary>
		public MemoryBlock MemoryUsed { get; set; }

		/// <summary>
		/// Id do scanner utilizado. 0 quando o processo não requsita o scanner
		/// </summary>
		public int PrinterId { get; set; }

		/// <summary>
		/// Id do dispositivo Sata. 0 quando o processo não requisita nenhum dispositivo
		/// </summary>
		public int SataID { get; set; }

		public bool UsingScanner { get; set; }

		public bool UsingModem { get; set; }

		public bool UsingPrinter { get { return PrinterId > 0; } }

		public bool UsingSata { get { return SataID > 0; } }

	}
}
