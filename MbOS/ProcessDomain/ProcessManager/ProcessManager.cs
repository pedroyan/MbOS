using MbOS.Common;
using MbOS.FileDomain.DataStructures;
using MbOS.Interfaces;
using MbOS.MemoryDomain.DataStructures;
using MbOS.ProcessDomain.DataStructures;
using MbOS.ResourcesDomain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MbOS.ProcessDomain.ProcessManager {
	public class ProcessManager : IProcessService {

		StreamReader initializationFile;
		int lineCount;
		string fileName;
		private ProcessScheduler scheduler;

		public ProcessManager(string fileName) {
			initializationFile = FileHelper.OpenFile(fileName);
			this.fileName = fileName;
		}

		public void Run() {
			RegistrationService.RegisterInstance<IProcessService>(this);
			try {
				var processList = ReadProcessesFromFile();
				scheduler = new ProcessScheduler(processList);
				scheduler.RunScheduler();
			} catch (FileFormatException ex) {
				Console.WriteLine($"Arquivo {fileName} inválido: {ex.Message}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Erro no módulo de processos: {ex.Message}");
				throw;
			}

			initializationFile.Dispose();
		}

		private List<Process> ReadProcessesFromFile() {
			string line;
			var pList = new List<Process>();
			int PID = 0;

			while ((line = GetNextLine()) != null) {
				if(!string.IsNullOrEmpty(line)) {
					PID++;
					pList.Add(ParseLine(line,PID));
				}
			}

			return pList;
		}

		private Process ParseLine(string line, int PID) {

			var parameters = line.Replace(" ", "").Split(",");
			if (parameters.Length != 8) {
				throw new FileFormatException($"Erro na linha {PID}: São necessário 8 parâmetros por linha");
			}

			var initTime = ParseParameter(parameters, 0, PID);
			var priority = ParseParameter(parameters, 1, PID);
			var processingTime = ParseParameter(parameters, 2, PID);
			var memoryBlocks = ParseParameter(parameters, 3, PID);

			if (!Enum.TryParse(parameters[4], out PrinterEnum printerId)) {
				throw new FileFormatException($"Erro na linha {PID}: Id de impressora inválido");
			}

			var scannerRequested = ParseParameter(parameters, 5, PID) != 0; 
			var modemRequest = ParseParameter(parameters, 6, PID) != 0;

			if (!Enum.TryParse(parameters[7],out SataEnum sataId)) {
				throw new FileFormatException($"Erro na linha {PID}: Id do dispositivo SATA inválida");
			}

			return new Process(PID, initTime, priority, processingTime, memoryBlocks, printerId,
				scannerRequested, modemRequest, sataId);
		}

		private int ParseParameter(string[] parameter, int index, int PID) {
			if (!int.TryParse(parameter[index], out int param)) {
				throw new FileFormatException($"Erro na linha {PID}: Parametro de índice {index} não é um inteiro");
			}
			return param;
		}

		public bool ExistsProcess(int id) {
			return scheduler.Processos.Any(p => p.PID == id);
		}

		public bool IsRealTimeProcess(int PID) {
			var proc = scheduler.Processos.FirstOrDefault(p => p.PID == PID);
			return proc == null ? false : proc.Priority == 0;
		}

		private string GetNextLine() {
			lineCount++;
			return initializationFile.ReadLine();
		}
	}
}
