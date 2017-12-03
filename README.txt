# MbOS
COMO BUILDAR O PROJETO e em seguida rodar o programa:
1)Instale o .net core em sua maquina utilizando o link a segguir " https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x "
2)Acesse a  pasta MbOs(utilizando o comando cd pelo terminal)
3)execute o comando "dotnet restore "
4)execute o comando "dotnet build -r linux-x64"
5) execute o comando "dotnet publish -c release -r linux-x64 "
6)Acesse a pasta bin
7)Acesse a pasta Release
8)Acesse a pasta netcoreapp2.0
9)Acesse a pasta linux-x64
10)Acesse a pasta publish publish
11)execute o programa MbOS utilizando o comando " ./MbOs <nome do arquivo de processos> <Nome do arquivo de files> "
exemplo:
./MbOs Resources/process.txt Resources/files.txt
12)Para mais duvidas,nos pergunte ou acesse o link " https://www.hanselman.com/blog/SelfcontainedNETCoreApplications.aspx " que possuira o tutorial que foi segeuido para buildar em nossas maquinas. 

COMO RODAR O PROGRAMA sem fazer o build:
2)Extraia a pasta RunHere encontrada no arquivo ExtractHere.rar ou ExtractHere.zip ou ExtractHere.tar
1)ACESSE A PASTA "RunHere" pelo terminal (o comando para acessar a pasta vai ser o cd) que foi extraida anteriormente
2)execute o programa MbOS utilizando o comando " ./MbOs <nome do arquivo de processos> <Nome do arquivo de files> "
exemplo:
./MbOs Resources/process.txt Resources/files.txt
