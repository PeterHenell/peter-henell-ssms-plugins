# peter-henell-ssms-plugins
SQL Server Management Studio Plugins by Peter Henell

Only SSMS 2014 supported at this time.

You may need to install this first
 https://chocolatey.org/packages/SQL2014.SMO

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

Now start SSMS and verify that the plugin have been loaded. There should be a new top menu called PeterHenell.

# How to use the Plugins and what they do
Most queries work by selecting a query or table and then running the plugin.

## Execute and save result as Excel
This plugin will execute the selected query and export the result into an excel file.

This plugin is useful for:
* Exporting to excel for non-sql-developers to use.
* Quickly exporting backup of data

How to use:
* Select the query or procedure
* Run the plugin
* A dialoge appear asking for the save location.
* Select a location for the file and click OK
* The result of the query is exported to an excel file at the selected location
 
## Generate insert statement for selected query
This plugin will create an insert statement WITH data from the query.
The target table of the insert statement will need to be changed for the query to work.

This plugin is useful for:
* Moving data between databases and servers
* Generating data scripts for use in the deployment pipeline.

How to use:
* Select a query or procedure
* Run the plugin
* A dialog asks you how many rows you wish to fetch from the result of the query. Select 0 for max number of rows.
* The insert statement will be generated and inserted into your worksheet.

## Generate X rows for selected table
This plugin will generate an insertstatement with "random" data. The target table will be the selected table.

This plugin is useful for:
* Generating an insert statement for a complex table with example.
* Generate valid VALUES clause to be used in subqueries.
 
## Generate Temp Tables from Selected Queries
This plugin will create a matching temporary table for each of the queries in the selected text.

This plugin is useful for:
* Generating temporary tables based on a stored procedure
* Generating a copy of a table

## Import Excel File
This plugin will import the first sheet of an excel file into a new table. You will select the name of the new table during the execution of the plugin.

## tSQLt - Create #Actual and #Expected tables from selected query
This plugin will generate two temporary tables: #Actual and #Expected.
The #Actual table will be populated by the selected query.
The #Expected table will be populated by some rows from the existing rows from the selected query.

This plugin is very useful when writing tSQLT unit tests.
