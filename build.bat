cls
set initialPath=%cd%
set srcPath=%cd%\src\CatFactory.DotNetCore
set testPath=%cd%\test\CatFactory.DotNetCore.Tests
cd %srcPath%
dotnet build
cd %testPath%
dotnet test
cd %srcPath%
dotnet pack
cd %initialPath%
pause
