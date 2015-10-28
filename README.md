# peter-henell-ssms-plugins
SQL Server Management Studio Plugins by Peter Henell

Only SSMS 2014 supported at this time.

How to install:
* Download the latest framework from http://documentation.red-gate.com/display/MA/Redistributing+the+framework
(This is probably already installed if you are using any of the RedGate Toolkits.)
* Download the latest version of peter-henell-ssms-plugins from this location: https://github.com/PeterHenell/peter-henell-ssms-plugins/tree/master/released-binaries
* Unzip the binaries at a location of your choice (You will need to reference this path in a later step).
* Make an entry in the Registry to tell the SIPframework where to load the add-in from. 

You should create the registry entry in either:
* 32-bit machines: HKEY_LOCAL_MACHINE\SOFTWARE\Red Gate\SIPFramework\Plugins
* 64-bit machines: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Red Gate\SIPFramework\Plugins

Create a new String Value (REG_SZ) with a unique name (of your choice) and set the value to the path of the plugin.

For example: C:\CoolSoftware\PeterHenell.SSMS.Plugins.dll

Now start SSMS and verify that the plugin have been loaded.

You may need to install this first
 https://chocolatey.org/packages/SQL2014.SMO
