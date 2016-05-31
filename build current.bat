@echo off

call "C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_12.0.0.0__b03f5f7f11d50a3a\MSBuild.exe"

xcopy .\PeterHenell.SSMS.Plugins\bin\Release\*.* .\released-binaries\current /y /i /R
xcopy .\PeterHenell.SSMS.DefaultCommandPlugins\bin\Release\*.* .\released-binaries\current\Plugins /y /i /R

@ECHO *******************************************
@ECHO      Current Completed
