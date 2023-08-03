cls
set initialPath=%cd%
set srcPath=%cd%\CatFactory.NetCore
set testPath=%cd%\CatFactory.NetCore.Tests
set outputBasePath=C:\Temp\CatFactory.NetCore
cd %srcPath%
dotnet build
cd %testPath%
dotnet test
cd %outputBasePath%\DesignPatterns
cd %outputBasePath%\CleanArchitecture\Infrastructure.Tests
dotnet test
cd %srcPath%
dotnet pack
cd %initialPath%
pause
