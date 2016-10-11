@echo off

call "C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_12.0.0.0__b03f5f7f11d50a3a\MSBuild.exe" PeterHenell.SSMS.Plugins\PeterHenell.SSMS.Plugins.csproj /t:Build /p:Configuration=Debug
call "C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_12.0.0.0__b03f5f7f11d50a3a\MSBuild.exe" "PeterHenell.SSMS.DefaultCommandPlugins\PeterHenell.SSMS.DefaultCommandPlugins.csproj" /t:Build /p:Configuration=Debug

call "C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_12.0.0.0__b03f5f7f11d50a3a\MSBuild.exe" PeterHenell.SSMS.Plugins\PeterHenell.SSMS.Plugins.csproj /t:Build /p:Configuration=Debug
call "C:\Windows\Microsoft.NET\assembly\GAC_64\MSBuild\v4.0_12.0.0.0__b03f5f7f11d50a3a\MSBuild.exe" "PeterHenell.SSMS.DefaultCommandPlugins\PeterHenell.SSMS.DefaultCommandPlugins.csproj" /t:Build /p:Configuration=Debug




xcopy .\PeterHenell.SSMS.Plugins\bin\Debug\*.* .\released-binaries\current /y /i /R
xcopy .\PeterHenell.SSMS.DefaultCommandPlugins\bin\Debug\*.* .\released-binaries\current\Plugins /y /i /R

@ECHO *******************************************
@ECHO      Current Completed
