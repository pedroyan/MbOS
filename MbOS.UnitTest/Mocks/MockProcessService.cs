﻿using MbOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.UnitTest.Mocks {
	class MockProcessService : IProcessService {
		public bool ExistsProcess(int id) {
			return id < 5;
		}

		public bool IsRealTimeProcess(int PID) {
			//Somente o processo com PID = 0 é considerado processo de tempo real no dipatcher de testes
			return PID == 0;
		}
	}
}
