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
using System.IO;

namespace MbOS.UnitTest {
    [TestClass]
    public class IntegracaoTest {
        string filesPath = "Resources/files4.txt";

        [TestInitialize]
        public void Initialization() {
            RegistrationService.RegisterInstance<IProcessService>(new MockProcessService());
        }

        /// <summary>
        /// Verifica se as mudancas de prioridades sao feitas corretamente
        /// </summary>
		[TestMethod]
        public void IntegrationProcessTest() {

           
            var ListaProcessos = new List<Process>() {
            new Process(1,0,4,8,64, PrinterEnum.Printer1 , true, false, SataEnum.Sata2),
            new Process(2,2,0,11,64, PrinterEnum.Printer2, false, false, SataEnum.None),
            new Process(3,3,4,6,64, PrinterEnum.None, false, false, SataEnum.None),
            new Process(4,3,1,6,64, PrinterEnum.None, false, false, SataEnum.None),
            new Process(5,0,10,8,64, PrinterEnum.Printer1 , true, false, SataEnum.Sata2)};
            var scheduler = new ProcessScheduler(ListaProcessos);
            scheduler.RunScheduler();


            var fileManager = new FileManager(filesPath);

            fileManager.hardDrive = fileManager.InitializeHDD(fileManager.initializationFile);
            //le a primeira instrucao
          ExecuteInstructionsByStep(fileManager,1);//Informacoes iniciais do arquivo
          //  ExecuteInstructionsByStep(fileManager, 1);//executando a primeira instrucao
            //hardDrive.HardDriveMap();
            var teste = fileManager.hardDrive;

            //for (int i = 0; i < fileManager.hardDrive.diskDrive.list.Count(); i++) {
            //    if (!listaProcessosIdeal[i].Compare(processList[i])) {
           //         Assert.Fail();
            //    }
         //   }





        }

        /// <summary>
        /// Executa n steps linhas da instrucao dada no arquivo txt de testes a fim de testar o FileManager junto com o ProcessManager
        /// </summary>
        /// <param name="file">Arquivo de testes</param>
        /// <param name="fileManager">Objeto referendo-se ao fileManager a qual sera testado</param>
        /// <param name="steps">Numero de linhas a serem executadas</param>
        private void ExecuteInstructionsByStep( FileManager fileManager, int steps) {
            string line;
            int i = 1;
            for (int y = 0; y < steps; y++) {
                if ((line = fileManager.GetNextLine()) != null) {
                    var inst = fileManager.ParseInstruction(line);
                    try {
                        //Testes podem ser feitos por injeção de dependência
                        inst.Execute(fileManager.hardDrive, i);
                    } catch (HardDriveOperationException ex) {
                        Console.WriteLine($"Operacao {i} => Falha");
                        Console.WriteLine(ex.Message);
                    }
                    Console.WriteLine();
                    i++;
                }
            }
        }
    }
}
