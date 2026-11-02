@echo off
setlocal
cd /d "%~dp0"
set "COMPILADOR=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if not exist "%COMPILADOR%" set "COMPILADOR=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if not exist "%COMPILADOR%" (echo Instale o SDK .NET e execute dotnet run. & pause & exit /b 1)
"%COMPILADOR%" /nologo /utf8output /r:System.Core.dll /out:TechFix.exe Program.cs
if errorlevel 1 (pause & exit /b 1)
TechFix.exe --demo
pause
