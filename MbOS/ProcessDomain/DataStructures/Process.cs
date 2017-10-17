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
		public int InitializationTime { get; private set; }

		/// <summary>
		/// Ticks para concluir o processamento
		/// </summary>
		public int ProcessingTime { get; private set; }

		/// <summary>
		/// Quantos ticks foram executados
		/// </summary>
		public int TicksRan { get; private set; }

		/// <summary>
		/// Quanto menor o valor, maior a prioridade
		/// </summary>
		public int Priority { get; private set; }

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

		public bool Concluido { get { return TicksRan >= ProcessingTime; } }

		/// <summary>
		/// Printa na tela a mensagem de execução do processo
		/// </summary>
		public void PrintRunningMessage() {

			if (TicksRan == 1) {
				Console.WriteLine($"P{PID} STARTED");
			} else {
				Console.WriteLine($"P{PID} RESTARTED");
			}

			Console.WriteLine($"P{PID} instruction {TicksRan}");

			if (Concluido) {
				Console.WriteLine($"P{PID} return SIGINT");
			}
		}

		/// <summary>
		/// Printa na tela a mensagem de preempção
		/// </summary>
		public void PrintPreemptionMessage() {

		}

		public void Promote() {
			PrintPreemptionMessage();
			Priority--;
		}

		public void Run() {
			TicksRan++;
			PrintRunningMessage();
		}

	}
}
