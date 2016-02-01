@echo off

call "C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_12.0.0.0__b03f5f7f11d50a3a\MSBuild.exe"

for /f "tokens=1* delims=" %%a in ('date /T') do set datestr=%%a
mkdir %datestr%

set /p version=<.\released-binaries\release-version.txt
for /F "tokens=1,2,3 delims=. " %%a in ("%version%") do (
   set major=%%a
   set minor=%%b
   set bugfix=%%c
)

set /A minor=minor+1
set newversion=%major%.%minor%.%bugfix%

xcopy .\PeterHenell.SSMS.Plugins\bin\Release\*.* .\released-binaries\%newversion% /y /i
xcopy .\PeterHenell.SSMS.DefaultCommandPlugins\bin\Release\*.dll .\released-binaries\%newversion%\Plugins /y /i

echo %newversion%>.\released-binaries\release-version.txt