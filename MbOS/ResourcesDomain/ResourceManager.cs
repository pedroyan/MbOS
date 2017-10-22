using MbOS.ProcessDomain.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.ResourcesDomain {
	public class ResourceManager {

		private int? Scanner;

		private int? Impressora1;
		private int? Impressora2;

		private int? Modem;

		private int? SATA1;
		private int? SATA2;

		#region Checking

		/// <summary>
		/// Verifica se é possivel alocar todos os recursos 
		/// pedidos por um processo <paramref name="process"/>
		/// </summary>
		public bool CanAllocateResources(Process process) {
			if (process.UsingModem && !CanAllocateModem(process.PID)) {
				return false;
			}

			if (process.UsingPrinter && !CanAllocatePrinter(process.PID,process.PrinterId)) {
				return false;
			}

			if (process.UsingSata && !CanAllocateSata(process.PID,process.SataID)) {
				return false;
			}

			if (process.UsingScanner && !CanAllocateScanner(process.PID)) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Valida se o processo de id <paramref name="PID"/> pode alocar impressora <paramref name="printer"/>
		/// </summary>
		/// <param name="PID">Id do processo</param>
		/// <param name="printer">Id da impressora</param>
		/// <returns></returns>
		public bool CanAllocatePrinter(int PID, PrinterEnum printer) {
			switch (printer) {
				case PrinterEnum.Printer1:
					return CanAllocate(PID, Impressora1);
				case PrinterEnum.Printer2:
					return CanAllocate(PID, Impressora2);
				default:
					return false;
			}
		}

		/// <summary>
		/// Valida se o processo de id <paramref name="PID"/> pode alocar o barramento <paramref name="sata"/>
		/// </summary>
		/// <param name="PID">Id do processo</param>
		/// <param name="sata">Id do barramento SATA</param>
		/// <returns></returns>
		public bool CanAllocateSata(int PID, SataEnum sata) {
			switch (sata) {
				case SataEnum.Sata1:
					return CanAllocate(PID, SATA1);
				case SataEnum.Sata2:
					return CanAllocate(PID, SATA2);
				default:
					return false;
			}
		}

		public bool CanAllocateModem(int PID) {
			return CanAllocate(PID, Modem);
		}

		public bool CanAllocateScanner(int PID) {
			return CanAllocate(PID, Scanner);
		}

		private bool CanAllocate(int PID, int? fieldId) {
			return !fieldId.HasValue || fieldId.Value == PID;
		}
		#endregion

		#region Allocation

		/// <summary>
		/// Aloca os recursos pedidos por um processo <paramref name="process"/>
		/// </summary>
		/// <param name="process">Processo que solicita os recursos</param>
		public void Allocate(Process process) {
			if (process.UsingScanner) {
				Allocate(process.PID, ResourceAllocationId.Scanner);
			}

			if (process.UsingModem) {
				Allocate(process.PID, ResourceAllocationId.Modem);
			}

			if (process.UsingPrinter) {
				if (process.PrinterId == PrinterEnum.Printer1) {
					Allocate(process.PID, ResourceAllocationId.Impressora1);
				} else {
					Allocate(process.PID, ResourceAllocationId.Impressora2);
				}
			}

			if (process.UsingSata) {
				if (process.SataID == SataEnum.Sata1) {
					Allocate(process.PID, ResourceAllocationId.SATA1);
				} else {
					Allocate(process.PID, ResourceAllocationId.SATA2);
				}
			}
		}

		/// <summary>
		/// Realiza a alocação do recurso passado
		/// </summary>
		/// <param name="PID">Processo que deseja alocar o recurso</param>
		/// <param name="resource">Recurso pedido</param>
		public void Allocate(int PID, ResourceAllocationId resource) {
			bool success = false;
			switch (resource) {
				case ResourceAllocationId.Scanner:
					success = Allocate(PID, ref Scanner);
					break;
				case ResourceAllocationId.Impressora1:
					success = Allocate(PID, ref Impressora1);
					break;
				case ResourceAllocationId.Impressora2:
					success = Allocate(PID, ref Impressora2);
					break;
				case ResourceAllocationId.Modem:
					success = Allocate(PID, ref Modem);
					break;
				case ResourceAllocationId.SATA1:
					success = Allocate(PID, ref SATA1);
					break;
				case ResourceAllocationId.SATA2:
					success = Allocate(PID, ref SATA2);
					break;
				default:
					break;
			}

			if (!success) {
				throw new ArgumentException($"Não foi possível alocar o recurso {resource.ToString()} para o processo {PID}");
			}
		}

		private bool Allocate(int PID,ref int? resource) {
			if (!CanAllocate(PID,resource)) {
				return false;
			}

			resource = PID;
			return true;
		}
		#endregion

		#region Deallocation
		/// <summary>
		/// Libera todos os recursos alocados, se houverem, para o processo de ID<paramref name="PID"/>
		/// </summary>
		/// <param name="PID">ID do processo liberado</param>
		public void FreeResources(int PID) {
			FreeResource(PID, ref Impressora1);
			FreeResource(PID, ref Impressora2);
			FreeResource(PID, ref Modem);
			FreeResource(PID, ref SATA1);
			FreeResource(PID, ref SATA2);
			FreeResource(PID, ref Scanner);
		}

		private void FreeResource(int PID, ref int? resource) {
			if (PID == resource) {
				resource = null;
			}
		}
		#endregion


	}

	public enum PrinterEnum {
		None,
		Printer1,
		Printer2
	}

	public enum SataEnum {
		None,
		Sata1,
		Sata2
	}

	public enum ResourceAllocationId {
		Scanner,
		Impressora1,
		Impressora2,
		Modem,
		SATA1,
		SATA2
	}
}
