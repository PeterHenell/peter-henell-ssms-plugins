# What is this? #
This is a collection of plugins for SQL Server Management Studio 2012. The plugins are all OPT IN meaning that they will need to be activated to run (no background running tasks).

# Supported Versions #
SQL Server Management Studio 2012.


# Installation: #
These installation instructions will only work with 64bit version.

  1. Download and install the Redgate SSMS Ecosystem project from http://documentation.red-gate.com/display/MA/SSMS+ecosystem+project
  1. Download the latest version of peter-henell-plugins (found here https://code.google.com/p/peter-henell-ssms-plugins/source/browse/#git%2Freleased-binaries) and unzip all the files to a location of your choice, for example c:\plugins\peter-henell-plugins\
  1. Edit the "addToReg.reg" file such that the path is pointing to the path of the unzipped dll called "`PeterHenell.SSMS.Plugins.dll`".
  1. Save the file
  1. run the "addToReg.reg" file by double clicking it.

Note that you will need to use double-backslash in the reg-file.

Example "addToReg.reg":
```
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Red Gate\SIPFramework\Plugins]
"PeterHenellSSMSPlugins"="c:\\plugins\\peter-henell-plugins\\PeterHenell.SSMS.Plugins.dll"
```

# Plugins #
See the wiki pages for documentation of plugins in this package.

# Notes for developers #
Built using Visual Studio Express 2013 for Windows Desktop. All dependencies have been added to the repository. I suspect that you will need to install the SQL Server Management Objects for SSMS 2012.