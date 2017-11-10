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
		public IEnumerable<IGrouping<int, Process>> prioridades;

		/// <summary>
		/// Lista contendo todos os processos
		/// </summary>
		public List<Process> Processos { get; set; }

		/// <summary>
		/// Processo sendo executado
		/// </summary>
		public Process CPU;

		/// <summary>
		/// Gerenciador de memória
		/// </summary>
		public MemoryManager memoryManager;

		/// <summary>
		/// Gerenciador de Dispositivos
		/// </summary>
		public DeviceManager deviceManager;

		public int processosCount;
		public int processosCompletos;
		public int tickCount;

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
			deviceManager = new DeviceManager();

			Processos = processes ?? new List<Process>();

			//Ordena os processos de acordo com prioridades, exemplo:
			//|Grupo prioridade 0(p1,p2,p3)|Grupo prioridade 1 (p4,p7,p8) | Grupo prioridade 2 (p5,p6,p9)| ...
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
			if (CPU != null && CPU != novoProcesso) {
				//Padding entre a instrução executada e o print do dispatcher
				Console.WriteLine("");
			}

			PrintNewProcessInfo(novoProcesso);
			CPU = novoProcesso;

		}

		/// <summary>
		/// Imprime na tela as informações do novo processo 
		/// <paramref name="process"/> que entrou na CPU
		/// </summary>
		/// <param name="process">Processo que entrou na CPU</param>
		public void PrintNewProcessInfo(Process process) {
			if (process.TicksRan < 1) {
				Console.WriteLine("dispatcher =>");
				Console.WriteLine($"\t PID: {process.PID}");
				Console.WriteLine($"\t ofsset: {process.MemoryUsed.StartIndex}");
				Console.WriteLine($"\t blocks: {process.MemoryUsed.BlockSize}");
				Console.WriteLine($"\t priority: {process.Priority}");
				Console.WriteLine($"\t time: {process.ProcessingTime}");

				var PrinterIdMessage = process.UsingPrinter ? $" - Printer Id {(int)process.PrinterId}" : "";
				Console.WriteLine($"\t Using Printer? {process.UsingPrinter}{PrinterIdMessage}");

				Console.WriteLine($"\t Using Scanner? {process.UsingScanner}");
				Console.WriteLine($"\t Using Modem? {process.UsingModem}");

				var sataIdMessage = process.UsingSata ? $" - Sata Id {(int)process.SataID}" : "";
				Console.WriteLine($"\t Using SATA? {process.UsingSata}{sataIdMessage}\n");
			}

			Console.WriteLine($"process {process.PID} =>");
		}

		public void TickClock() {

			if (CPU != null) {
				CPU.Run();

				if (CPU.Concluido) {
					FinishProcess(CPU);
				}
			}

			foreach (var proc in Processos) {
                if (proc.Priority > 1 && proc != CPU && proc.TicksRan>0 && proc.Concluido==false ) {
                    proc.Promote();
                    //reorganiza os processos
                    prioridades = Processos.GroupBy(p => p.Priority).OrderBy(p => p.Key);

                }
                proc.InitializationTime--;

            }

            tickCount++;
		}

		private void FinishProcess(Process process) {
			memoryManager.DeallocateMemory(process.PID, process.Priority == 0);
			deviceManager.FreeResources(process.PID);

			CPU = null;
			processosCompletos++;
		}

		/// <summary>
		/// Retorna qual processo deve ser executado á seguir. Devolve nulo
		/// caso não seja necessária a execução de nenhum processo no próximo "clock"
		/// </summary>
		/// <returns>O processo que deve ser executado</returns>
		public Process GetNextProcess() {

			var processosPrioritarios = CPU == null ? prioridades
				: prioridades.Where(p => p.Key < CPU.Priority);

			foreach (var grupoPrioridade in processosPrioritarios) {

				//Para cada grupo prioridade, pega somente os processos dentro daquele grupo que estão pronto para executar.
				var readyToRun = grupoPrioridade.Where(p => !p.Concluido && p.InitializationTime == 0).OrderBy(p=>p.PID);
				var realTime = grupoPrioridade.Key == 0;

				foreach (var processo in readyToRun) {

                    var canAllocateMemory = memoryManager.CanAllocate(processo.MemoryUsed.BlockSize, realTime);
                    var canAllocateDevice = deviceManager.CanAllocateDevices(processo);

                    //Cado o processo possa alocar os recursos pedidos
                    if (canAllocateMemory&&canAllocateDevice ) {

						// aloca memoria
						memoryManager.AllocateMemory(processo.MemoryUsed, realTime);

						// aloca recurso
						deviceManager.Allocate(processo);

						//retorna para execução
						return processo;
					}
				}
			}
			return null;
		}

	}
}
