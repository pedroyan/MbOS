using MbOS.ResourcesDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MbOS.UnitTest {
	[TestClass]
	public	class ResourceManagerTest {
		[TestMethod]
		public void SingleResourceAllocationTest() {
			var manager = new ResourceManager();

			//Processo 3 aloca um modem
			manager.Allocate(3, ResourceAllocationId.Modem);

			//Processo 4 verifica de pode alocar o Modem
			var result = manager.CanAllocateModem(4);
			Assert.IsFalse(result);

			//Lança exceção caso o processo 4 tente alocar o modem de qualquer jeito
			try {
				manager.Allocate(4, ResourceAllocationId.Modem);
				Assert.Fail();
			} catch (ArgumentException ex) {
				Console.WriteLine(ex.Message);
			}
		}

		[TestMethod]
		public void MultipleResourceAllocationTest() {
			var manager = new ResourceManager();

			manager.Allocate(3, ResourceAllocationId.Impressora1);
			manager.Allocate(3, ResourceAllocationId.SATA2);

			Assert.IsFalse(manager.CanAllocateSata(4, SataEnum.Sata2));
			Assert.IsFalse(manager.CanAllocatePrinter(4, PrinterEnum.Printer1));
		}

		[TestMethod]
		public void DeallocationTest() {
			var manager = new ResourceManager();

			manager.Allocate(1, ResourceAllocationId.Impressora1);
			manager.Allocate(1, ResourceAllocationId.Impressora2);
			manager.Allocate(1, ResourceAllocationId.Modem);

			Assert.IsFalse(manager.CanAllocatePrinter(2, PrinterEnum.Printer1));
			Assert.IsFalse(manager.CanAllocatePrinter(2, PrinterEnum.Printer2));
			Assert.IsFalse(manager.CanAllocateModem(2));

			manager.FreeResources(1);

			Assert.IsTrue(manager.CanAllocatePrinter(2, PrinterEnum.Printer1));
			Assert.IsTrue(manager.CanAllocatePrinter(2, PrinterEnum.Printer2));
			Assert.IsTrue(manager.CanAllocateModem(2));
		}
	}
}
