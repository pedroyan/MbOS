using MbOS.ProcessDomain.DataStructures;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MbOS.MemoryDomain;
using MbOS.ResourcesDomain;

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
		private ResourceManager resourceManager;

		int processosCount;
		int processosCompletos;
		int tickCount;

		/// <summary>
		/// Constrói uma instância de um process scheduler
		/// com os processos <paramref name="processes"/> passados
		/// </summary>
		/// <param name="processes">Processos que serão gerenciados pelo escalonador</param>
		public ProcessScheduler(List<Process> processes) {

			if (processes.Count > 1000) {
				throw new ArgumentException("Não é possível entrar com mais de 1000 processos");
			}

			processosCount = processes.Count;

			memoryManager = new MemoryManager();
			resourceManager = new ResourceManager();

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

		private void Preempcao(Process novoProcesso) {
			if (RunningProcess != null && RunningProcess != novoProcesso) {
				RunningProcess.Promote();

				//reorganiza os processos
				prioridades = Processos.GroupBy(p => p.Priority).OrderBy(p => p.Key);
			}

			RunningProcess = novoProcesso;
		}

		private void TickClock() {

			if (RunningProcess != null) {
				RunningProcess.Run();

				if (RunningProcess.Concluido) {
					FinishProcess(RunningProcess);
				}
			}

			foreach (var proc in Processos) {
				proc.InitializationTime--;
			}

			tickCount++;
		}

		private void FinishProcess(Process process) {
			memoryManager.DeallocateMemory(process.PID, process.Priority == 0);
			resourceManager.FreeResources(process.PID);

			RunningProcess = null;
			processosCompletos++;
		}

		/// <summary>
		/// Retorna qual processo deve ser executado á seguir. Devolve nulo
		/// caso não seja necessária a execução de nenhum processo
		/// </summary>
		/// <returns></returns>
		private Process GetNextProcess() {

			var processosPrioritarios = RunningProcess == null ? prioridades
				: prioridades.Where(p => p.Key < RunningProcess.Priority);

			foreach (var grupoPrioridade in processosPrioritarios) {
				var readyToRun = grupoPrioridade.Where(p => !p.Concluido && p.InitializationTime == 0).OrderBy(p=>p.PID);
				var realTime = grupoPrioridade.Key == 0;
				foreach (var processo in readyToRun) {
					if (memoryManager.CanAllocate(processo.MemoryUsed.BlockSize, realTime) 
						&& resourceManager.CanAllocateResources(processo)) {

						// aloca memoria
						memoryManager.AllocateMemory(processo.MemoryUsed, realTime);

						// aloca recurso
						resourceManager.Allocate(processo);

						//retorna para execução
						return processo;
					}
				}
			}
			return null;
		}

	}
}
