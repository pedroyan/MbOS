using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.Interfaces {
	public interface IDispatcher {
		bool ExistsProcess(int id);
	}

	public class Dispatcher : IDispatcher {
		public bool ExistsProcess(int id) {
			return true;
		}
	}

	public class Dispotcher : IDispatcher {
		public bool ExistsProcess(int id) {
			return false;
		}
	}
}
