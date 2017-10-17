using MbOS.ProcessDomain.DataStructures;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MbOS.MemoryDomain;

namespace MbOS.ProcessDomain.ProcessManager {
	/// <summary>
	/// Classe responsável pelo gerenciamento da fila
	/// </summary>
	public class ProcessScheduler {
		/// <summary>
		/// Processos agrupados em prioridade
		/// </summary>
		private IEnumerable<IGrouping<int, Process>> prioridades;

		/// <summary>
		/// Lista contendo todos os processos
		/// </summary>
		public List<Process> Processos { get; set; }

		/// <summary>
		/// Processo sendo executado
		/// </summary>
		private Process RunningProcess;

		/// <summary>
		/// Gerenciador de memória
		/// </summary>
		private MemoryManager memoryManager;

		int processosCount;
		int processosCompletos;

		public ProcessScheduler(List<Process> processes) {

			processosCount = processes.Count;

			memoryManager = new MemoryManager();
			Processos = processes ?? new List<Process>();

			prioridades = Processos.GroupBy(p => p.Priority).OrderBy(p => p.Key);
		}

		public void RunScheduler() {
			while (processosCompletos < processosCount) {
				var proc = GetNextProcess();
				if (proc != null) {
					Preempcao(proc);
				}
				TickClock();
			}
		}

		public void Preempcao(Process novoProcesso) {
			if (RunningProcess != novoProcesso) {
				RunningProcess.Promote();
			}
			RunningProcess = novoProcesso;
		}

		public void TickClock() {

			if (RunningProcess != null) {
				RunningProcess.Run();
				if (RunningProcess.Concluido) {
					RunningProcess = null;
					processosCompletos++;
				}
			}

			foreach (var proc in Processos) {
				proc.InitializationTime--;
			}
		}

		/// <summary>
		/// Retorna qual processo deve ser executado á seguir. Devolve nulo
		/// caso não seja necessária a execução de nenhum processo
		/// </summary>
		/// <returns></returns>
		public Process GetNextProcess() {

			var prioridadesAvaliadas = RunningProcess == null ? prioridades
				: prioridades.Where(p => p.Key < RunningProcess.Priority);

			foreach (var Prioridade in prioridadesAvaliadas) {
				var naoConcluidos = Prioridade.Where(p => !p.Concluido);
				var realTime = Prioridade.Key == 0;
				foreach (var processo in naoConcluidos) {
					if (memoryManager.AllocateMemory(processo.MemoryUsed, realTime)) {

					}
				}
			}
		}

	}
}
