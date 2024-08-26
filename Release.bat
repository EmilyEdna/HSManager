chcp 65001
@echo 开始自动化发布
cd /d %~dp0
dotnet publish HSManager\HSManager.csproj -c Release -o ..\HSManager\Release -f net8.0-windows --sc false -r win-x64 /p:DebugType=None /p:DebugSymbols=false /p:PublishSingleFile=true

rd /S /Q HSManager\obj HSManager\bin\Release

cd Release
del *.pdb
@echo 已完成处理
pause
