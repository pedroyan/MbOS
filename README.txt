# MbOS
COMO BUILDAR O PROJETO e em seguida rodar o programa:
1)Instale o .net core em sua maquina utilizando o link a segguir " https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x "
2)Acesse a  pasta MbOs(utilizando o comando cd pelo terminal)
3)execute o comando "dotnet restore "
4) execute o comando "dotnet publish -c Release --runtime linux-x64"
5)Acesse a pasta bin
6)Acesse a pasta Release
7)Acesse a pasta netcoreapp2.0
8)Acesse a pasta linux-x64
9)execute o programa MbOS utilizando o comando " ./MbOs <nome do arquivo de processos> <Nome do arquivo de files> "
exemplo:
./MbOS Resources/processes.txt Resources/files.txt

COMO RODAR O PROGRAMA sem fazer o build:
1)Extraia o arquivo RunPackage.zip 
2)Acesse a pasta para onde o pacote foi extraído via terminal
3)execute o programa MbOS utilizando o comando " ./MbOs <nome do arquivo de processos> <Nome do arquivo de files> "
exemplo:
./MbOS Resources/processes.txt Resources/files.txt
