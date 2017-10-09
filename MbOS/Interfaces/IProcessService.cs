using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.Interfaces {
	public interface IProcessService {
		/// <summary>
		/// Verifica se um processo existe
		/// </summary>
		/// <param name="PID">Id do processo</param>
		bool ExistsProcess(int PID);

		/// <summary>
		/// Verifica se o processo é de tempo real
		/// </summary>
		/// <param name="PID">Id do processo</param>
		bool IsRealTimeProcess(int PID);
	}
}
