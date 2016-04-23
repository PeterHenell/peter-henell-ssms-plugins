# peter-henell-ssms-plugins
SQL Server Management Studio Plugins by Peter Henell

Only SSMS 2014 supported at this time.

## Release notes 1.9 
* During plugin execution a dialog is shown where the execution can be aborted.
* Plugins now support cancellation through cancellation token.
* DatabaseQueryManager now supports cancellation through cancellation token.
* Most of the plugins can now be cancelled.


## How to install:
* Download the latest framework from http://documentation.red-gate.com/display/MA/Redistributing+the+framework
(This is probably already installed if you are using any of the RedGate Toolkits.)
* Download the latest version of peter-henell-ssms-plugins from this location: https://github.com/PeterHenell/peter-henell-ssms-plugins/tree/master/released-binaries
* Unzip it at a location of your choice like C:\CoolSoftware\ (You will need to reference this path in a later step).
* Make an entry in the Registry to tell the SIPframework where to load the add-in from. 

You should create the registry entry in either:
* 32-bit machines: HKEY_LOCAL_MACHINE\SOFTWARE\Red Gate\SIPFramework\Plugins
* 64-bit machines: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Red Gate\SIPFramework\Plugins

Create a new String Value (REG_SZ) and name it "PeterHenellPlugins". Right click and choose modifty, then set the value to the path of the plugin.

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

###### Input (select this text and then run the plugin):
```SQL
EXEC sys.sp_who2
```

###### Output:
```SQL
An excel file with the data from the query
```

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

###### Input (select this text and then run the plugin):
```SQL
EXEC sys.sp_who2
```

###### Output:
```SQL
INSERT INTO ### (
[SPID],
	[Status],
	[Login],
	[HostName],
	[BlkBy],
	[DBName],
	[Command],
	[CPUTime],
	[DiskIO],
	[LastBatch],
	[ProgramName],
	[SPID1],
	[REQUESTID]
)
VALUES 
	('1    ', 'BACKGROUND                    ', 'sa', '  .', '  .', NULL, 'UNKNOWN TOKEN  ', '0', '0', '01/22 15:46:04', '', '1    ', '0    '), 
	('2    ', 'BACKGROUND                    ', 'sa', '  .', '  .', NULL, 'UNKNOWN TOKEN  ', '0', '0', '01/22 15:46:04', '', '2    ', '0    '), 
	('3    ', 'BACKGROUND                    ', 'sa', '  .', '  .', NULL, 'UNKNOWN TOKEN  ', '670', '0', '01/22 15:46:04', '', '3    ', '0    '), 
	('4    ', 'BACKGROUND                    ', 'sa', '  .', '  .', NULL, 'LOG WRITER     ', '80574', '0', '01/22 15:46:04', '', '4    ', '0    '), 
	('5    ', 'BACKGROUND                    ', 'sa', '  .', '  .', NULL, 'RECOVERY WRITER', '109', '0', '01/22 15:46:04', '', '5    ', '0    ');
```


## Generate X rows for selected table
This plugin will generate an insertstatement with "random" data. The target table will be the selected table.

This plugin is useful for:
* Generating an insert statement for a complex table with example.
* Generate valid VALUES clause to be used in subqueries.


###### Input (select this text and then run the plugin):
```SQL
master.dbo.spt_values
```

###### Output:
```SQL
INSERT INTO [master].[dbo].[spt_values] (
[name],
	[number],
	[type],
	[low],
	[high],
	[status]
)
VALUES 
	('name1', 1, 'type1', 1, 1, 1), 
	('name2', 2, 'type2', 2, 2, 2), 
	('name3', 3, 'type3', 3, 3, 3), 
	('name4', 4, 'type4', 4, 4, 4), 
	('name5', 5, 'type5', 5, 5, 5), 
	('name6', 6, 'type6', 6, 6, 6), 
	('name7', 7, 'type7', 7, 7, 7), 
	('name8', 8, 'type8', 8, 8, 8), 
	('name9', 9, 'type9', 9, 9, 9), 
	('name10', 10, 'type10', 10, 10, 10
```


## Generate Temp Tables from Selected Queries
This plugin will create a matching temporary table for each of the queries in the selected text.

This plugin is useful for:
* Generating temporary tables based on a stored procedure
* Generating a copy of a table

###### Input (select this text and then run the plugin):
```SQL
EXEC sys.sp_who2
```

###### Output:
```SQL
IF OBJECT_ID('TempDB..#temp1') IS NOT NULL DROP TABLE #temp1;
CREATE TABLE #temp1 (
	[SPID]	NVARCHAR(MAX),
	[Status]	NVARCHAR(MAX),
	[Login]	NVARCHAR(MAX),
	[HostName]	NVARCHAR(MAX),
	[BlkBy]	NVARCHAR(MAX),
	[DBName]	NVARCHAR(MAX),
	[Command]	NVARCHAR(MAX),
	[CPUTime]	NVARCHAR(MAX),
	[DiskIO]	NVARCHAR(MAX),
	[LastBatch]	NVARCHAR(MAX),
	[ProgramName]	NVARCHAR(MAX),
	[SPID1]	NVARCHAR(MAX),
	[REQUESTID]	NVARCHAR(MAX)
);
INSERT INTO #Temp1
EXEC sys.sp_who2

SELECT
[SPID],
	[Status],
	[Login],
	[HostName],
	[BlkBy],
	[DBName],
	[Command],
	[CPUTime],
	[DiskIO],
	[LastBatch],
	[ProgramName],
	[SPID1],
	[REQUESTID]

FROM #Temp1
```


## Import Excel File
This plugin will import the first sheet of an excel file into a new table. You will select the name of the new table during the execution of the plugin.

## tSQLt - Create #Actual and #Expected tables from selected query
This plugin will generate two temporary tables: #Actual and #Expected.
The #Actual table will be populated by the selected query.
The #Expected table will be populated by some rows from the existing rows from the selected query.

This plugin is very useful when writing tSQLT unit tests.

###### Input (select this text and then run the plugin):
```SQL
EXEC sp_who2
```

###### Output:
```SQL
IF OBJECT_ID('TempDB..#Actual') IS NOT NULL DROP TABLE #Actual;
IF OBJECT_ID('TempDB..#Expected') IS NOT NULL DROP TABLE #Expected;

CREATE TABLE #Actual (
	[SPID]	NVARCHAR(MAX),
	[Status]	NVARCHAR(MAX),
	[Login]	NVARCHAR(MAX),
	[HostName]	NVARCHAR(MAX),
	[BlkBy]	NVARCHAR(MAX),
	[DBName]	NVARCHAR(MAX),
	[Command]	NVARCHAR(MAX),
	[CPUTime]	NVARCHAR(MAX),
	[DiskIO]	NVARCHAR(MAX),
	[LastBatch]	NVARCHAR(MAX),
	[ProgramName]	NVARCHAR(MAX),
	[SPID1]	NVARCHAR(MAX),
	[REQUESTID]	NVARCHAR(MAX)
);

INSERT INTO #Actual
EXEC sys.sp_who2

SELECT [SPID],
	[Status],
	[Login],
	[HostName],
	[BlkBy],
	[DBName],
	[Command],
	[CPUTime],
	[DiskIO],
	[LastBatch],
	[ProgramName],
	[SPID1],
	[REQUESTID]
INTO #Expected
FROM #Actual
WHERE 1=0;

INSERT INTO [dbo].[#Expected] (
[SPID],	[Status],	[Login],	[HostName],	[BlkBy],	[DBName],	[Command],	[CPUTime],	[DiskIO],	[LastBatch],	[ProgramName],	[SPID1],	[REQUESTID])
VALUES	('1    ', 'BACKGROUND                    ', 'sa', '  .', '  .', NULL, 'UNKNOWN TOKEN', '0', '0', '01/22 15:46:04', '', '1    ', '0    ');

```

