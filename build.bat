cls
set initialPath=%cd%
set srcPath=%cd%\CatFactory.NetCore
set testPath=%cd%\CatFactory.NetCore.Tests
cd %srcPath%
dotnet build
cd %testPath%
dotnet test
cd %srcPath%
dotnet pack
cd %initialPath%
pause
