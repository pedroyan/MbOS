using MbOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.UnitTest.FileManager {
	class MockDispatcher : IDispatcher {
		public bool ExistsProcess(int id) {
			return id < 5;
		}
	}
}
