using MbOS.Common;
using MbOS.FileDomain;
using MbOS.FileDomain.DataStructures;
using MbOS.Interfaces;
using MbOS.ProcessDomain.DataStructures;
using MbOS.ProcessDomain.ProcessManager;
using MbOS.ResourcesDomain;
using MbOS.UnitTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            var InicilizationFiles = new List<HardDriveEntry>() {
                new HardDriveEntry("X", null, 2) { StartIndex = 0 },
                new HardDriveEntry("Y", null, 1) {StartIndex = 3},
                new HardDriveEntry("Z", null, 1) {StartIndex = 5} };
           
            var hdIdeal = new HardDrive(10,InicilizationFiles);//selecionado tamanho maximo que nem o arquivo
                                            //valores inciais encontrados no arquivo de teste
          


            var ListaProcessos = new List<Process>() {
            new Process(1,0,4,8,64, PrinterEnum.Printer1 , true, false, SataEnum.Sata2),
            new Process(0,2,0,11,64, PrinterEnum.Printer2, false, false, SataEnum.None),
            new Process(3,3,4,6,64, PrinterEnum.None, false, false, SataEnum.None),
           };
            var scheduler = new ProcessScheduler(ListaProcessos);
            scheduler.RunScheduler();


            var fileManager = new FileManager(filesPath);
            fileManager.hardDrive = fileManager.InitializeHDD(fileManager.initializationFile);
            ProcessManager oi;
            



            TestExecutarInstrucao(fileManager,true);//1,Executa a primeira instrucao,tem de adicionar B corretamente
            hdIdeal.AddFile(new HardDriveEntry("B", 3, 2));
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager, false);//2,Devera ter uma falha ao adioconar A,entao o hd devera se manter o mesmo
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager, false);//3,Devera ter uma falha ao deletarX ,entao o hd devera se manter o mesmo
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager, true);//4,Devera deletar X corretamente
            hdIdeal.RemoveFile("X", 0);
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager, true);//5,Devera deletar B corretamente
            hdIdeal.RemoveFile("B", 3);
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager, true); ;//6,tem de adicionar D corretamente
            hdIdeal.AddFile(new HardDriveEntry("D", 1, 1));
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager, false); ;//7,devera dar erro ao adicionar E,mantendo o hd o mesmo
            CompareHD(hdIdeal, fileManager.hardDrive);

            TestExecutarInstrucao(fileManager,true);//8,devera adicionar F corretamente
            hdIdeal.AddFile(new HardDriveEntry("F", 3, 4));
            CompareHD(hdIdeal, fileManager.hardDrive);



        }
        private void CompareHD(HardDrive hdIdeal,HardDrive hd) {
                if (hdIdeal.diskDrive.list.Count != hd.diskDrive.list.Count) {
                   Assert.Fail();
              }

            for (int i = 0; i < hd.diskDrive.list.Count; i++) {
                if (!hdIdeal.diskDrive.list[i].Compare(hd.diskDrive.list[i])) {
                    Assert.Fail();
                }

            }
        }
        private void TestExecutarInstrucao(FileManager fileManager, bool deveFuncionar) {
            try {
                ExecuteInstructionsByStep(fileManager, 1);
                
            } catch (HardDriveOperationException ex) {
                Console.WriteLine(ex.Message);
                if (deveFuncionar) {
                    Assert.Fail();
                }
            }
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
