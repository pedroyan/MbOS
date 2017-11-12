using MbOS.Common;
using MbOS.FileDomain;
using MbOS.FileDomain.DataStructures;
using MbOS.Interfaces;
using MbOS.ProcessDomain.DataStructures;
using MbOS.ProcessDomain.ProcessManager;
using MbOS.UnitTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using MbOS.ResourcesDomain;
using System.Text;
using System.Linq;

namespace MbOS.UnitTest {
	[TestClass]
	public class ProcessScheduleTest {
        string processPath = "Resources/processes.txt";

        [TestInitialize]
		public void Initialization() {
			RegistrationService.RegisterInstance<IProcessService>(new MockProcessService());
		}

        /// <summary>
        /// Verifica se as mudancas de prioridades sao feitas corretamente
        /// </summary>
		[TestMethod]
		public void PriorityProcessTest() {
            var ListaProcessos = new List<Process>() {

            new Process(1,1,4,8,64, PrinterEnum.Printer1 , true, false, SataEnum.Sata2),
            new Process(2,2,0,11,64, PrinterEnum.Printer2, false, false, SataEnum.None),
            new Process(3,3,4,6,64, PrinterEnum.None, false, false, SataEnum.None),
            new Process(4,3,1,6,64, PrinterEnum.None, false, false, SataEnum.None),
            new Process(5,0,10,8,64, PrinterEnum.Printer1 , true, false, SataEnum.Sata2)};
            var scheduler = new ProcessScheduler(ListaProcessos);

            ScheduleStep(scheduler, 2);//segundo 2,verifica se o processo 1 ficou retido pelo 5 por falta de recurso(no caso printer 1 e sata2)
            Assert.AreEqual(scheduler.CPU.PID, 5);
            //verifica se a cpu fez a preemcpao correta com a prioridade 0,sendo que utilizam o mesmo recurso mas com ids diferentes
            ScheduleStep(scheduler,1);//segundo 3
            Assert.AreEqual(scheduler.CPU.PID, 2);

            ScheduleStep(scheduler,1);//segundo 4

            //verifica se a prioridade foi aumentada corretamente,como se passaram 2 segundos entao deve se abaixar por 2
            TestPriority(scheduler,5,8);
            TestPriority(scheduler,3,4);

            //apos 4 cicls 
            ScheduleStep(scheduler, 8);//segundo 12
            //verifica se a prioridade foi 1 e nao abaixou pra 0 e se o processo 3 realmente nao mudou a prioridade mesmoe stando esperando esperando
            TestPriority(scheduler, 5, 1);
            TestPriority(scheduler, 3, 4);



        }
        /// <summary>
        /// Verifica se
        /// </summary>
        [TestMethod]
		public void MemoryTest() {
            var ListaProcessos = new List<Process>() {
            new Process(1,0,5,2,50, PrinterEnum.None , false, false, SataEnum.None),
            new Process(2,1,3,8,900, PrinterEnum.Printer1 , true, false, SataEnum.Sata2),
            new Process(3,2,1,11,64, PrinterEnum.Printer2, false, false, SataEnum.None) };
            var scheduler = new ProcessScheduler(ListaProcessos);

            ScheduleStep(scheduler, 2);//no segundo 2,verifica se o offset do processo 2 esta correto
            Assert.AreEqual(scheduler.CPU.PID, 2);
            Assert.AreEqual(scheduler.CPU.MemoryUsed.StartIndex, 50);//berifica se o offset da memoria se encontra em 50,por causa do processo 1 (alocado de 0 a 49)

            ScheduleStep(scheduler, 5);//segundo 7,verifica se mesmo com o processo de numero 2 pronto pra rodar e com prioridade maior,ele nao roda por falta de espaco na memoria
            Assert.AreNotEqual(scheduler.CPU.PID,3);
            Assert.AreEqual(scheduler.CPU.PID,2);

            ScheduleStep(scheduler, 5);//segundo 12,verifica se o processo 2 foi pra cpu depois de desalocar memoria
            Assert.AreEqual(scheduler.CPU.PID, 3);

        }
        /// <summary>
        /// Verifica se a estrutura de dados lida no arquivo txt se encontra como desejado no relatorio,indluindo a atribuicao do PID
        /// </summary>
        [TestMethod]
        public void EstruturaDeDadosTest() {
            var processes = new ProcessManager(processPath);
            var processList = processes.ReadProcessesFromFile();

            var listaProcessosIdeal = new List<Process>() {
            new Process(1,0, 2, 5, 64, PrinterEnum.Printer1, true, true, SataEnum.Sata2),
            new Process(2,2, 0, 3, 64, PrinterEnum.Printer1, false, false, SataEnum.None),
            new Process(3,3, 1, 2, 64, PrinterEnum.Printer2, false, false, SataEnum.None)};

            CollectionAssert.Equals(listaProcessosIdeal, processList);
      
        }
		private void TestPriority(ProcessScheduler scheduler,int PID,int prioridadeEsperada) {
            var processos = scheduler.Processos;

            var t1 = processos.Where(p => p.PID == PID).FirstOrDefault();
            Assert.AreEqual(t1.Priority, prioridadeEsperada);

        }



        private void ScheduleStep(ProcessScheduler scheduler,int steps) {
            for (int i = 0; i < steps; i++) {
                if (scheduler.processosCompletos < scheduler.processosCount) {
                    var proc = scheduler.GetNextProcess();
                    if (proc != null) {
                        scheduler.Preempcao(proc);
                    }
                    scheduler.TickClock();
                }
            }
        }
    }
}
