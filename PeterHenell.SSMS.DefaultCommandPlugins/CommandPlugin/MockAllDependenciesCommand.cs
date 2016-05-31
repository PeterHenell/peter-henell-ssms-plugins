using PeterHenell.SSMS.Plugins.DataAccess;
using PeterHenell.SSMS.Plugins.Plugins;
using System;
using System.Text;

namespace PeterHenell.SSMS.DefaultCommandPlugins.CommandPlugin
{
    public class MockAllDependenciesCommand : CommandPluginBase
    {
        public static readonly string COMMAND_NAME = "MockAllDeps_Command";

        public MockAllDependenciesCommand() :
            base(COMMAND_NAME,
                 CommandPluginBase.MenuGroups.TSQLTTools,
                 "tSQLt - Mock All Dependencies for selected object",
                 "global::Ctrl+ALT+N")
        {
        }

        public override void ExecuteCommand(System.Threading.CancellationToken token)
        {
            var selectedText = ShellManager.GetSelectedText();
            //var meta = TableMetadata.FromQualifiedString(selectedText);
            
            string sql = string.Format(@"DECLARE @referencing_entity AS sysname;
SET @referencing_entity = N'{0}';

WITH ObjectDepends(entity_name,referenced_schema, referenced_entity, referenced_id,referencing_id,referenced_database_name,referenced_schema_name,is_ambiguous,is_caller_dependent, referenced_class_desc, level)
AS
(
    SELECT entity_name = 
       CASE referencing_class
          WHEN 1 THEN OBJECT_NAME(referencing_id)
          WHEN 12 THEN (SELECT t.name FROM sys.triggers AS t 
                       WHERE t.object_id = sed.referencing_id)
          WHEN 13 THEN (SELECT st.name FROM sys.server_triggers AS st
                       WHERE st.object_id = sed.referencing_id) COLLATE database_default
       END
    ,referenced_schema_name
    ,referenced_entity_name
    ,referenced_id
    ,referencing_id
    ,sed.referenced_database_name
    ,sed.referenced_schema_name
    ,sed.is_ambiguous
    ,sed.is_caller_dependent
    ,sed.referenced_class_desc
    ,0 AS level 
    FROM sys.sql_expression_dependencies AS sed 
    WHERE OBJECT_NAME(referencing_id) = @referencing_entity 
    
    UNION ALL
    
    SELECT entity_name = 
       CASE sed.referencing_class
          WHEN 1 THEN OBJECT_NAME(sed.referencing_id)
          WHEN 12 THEN (SELECT t.name FROM sys.triggers AS t 
                       WHERE t.object_id = sed.referencing_id)
          WHEN 13 THEN (SELECT st.name FROM sys.server_triggers AS st
                       WHERE st.object_id = sed.referencing_id) COLLATE database_default
       END
    ,sed.referenced_schema_name
    ,sed.referenced_entity_name
    ,sed.referenced_id
    ,sed.referencing_id
    ,sed.referenced_database_name
    ,sed.referenced_schema_name
    ,sed.is_ambiguous
    ,sed.is_caller_dependent
    ,sed.referenced_class_desc
    ,level + 1   
    FROM ObjectDepends AS o
    JOIN sys.sql_expression_dependencies AS sed ON sed.referencing_id = o.referenced_id
)

SELECT entity_name AS referencing_entity, referenced_entity, referenced_class_desc, level, referencing_id, referenced_id, referenced_database_name,referenced_schema_name, is_ambiguous, is_caller_dependent
FROM ObjectDepends
ORDER BY level;", selectedText.Trim());
            var sb = new StringBuilder();
            
            var options = new PeterHenell.SSMS.Plugins.Utils.TsqltManager.MockOptionsDictionary();
            options.EachColumnInSelectOnNewRow = false;
            options.EachColumnInValuesOnNewRow = false;
            var connectionString = ConnectionManager.GetConnectionStringForCurrentWindow();

            QueryManager.Run(connectionString, token, (queryManager) =>
               {
                   queryManager.ExecuteQuery(sql, new Action<System.Data.SqlClient.SqlDataReader>(r =>
                   {
                       var reference = new
                       {
                             //referencing_entity,
                             //referenced_entity,
                             //referenced_class_desc,
                             //level,
                             //referencing_id,
                             //referenced_id,
                             //referenced_database_name,
                             //referenced_schema_name,
                             //is_ambiguous,
                             //is_caller_dependent

                           EntityName = r.GetString(r.GetOrdinal("referencing_entity")),
                           ReferencedSchemaName = r.GetString(r.GetOrdinal("referenced_schema_name")),
                           ReferencedEntityName = r.GetString(r.GetOrdinal("referenced_entity")),
                           //ReferencedId = r.GetString(r.GetOrdinal("referenced_id")),
                           //ReferencingId = r.GetString(r.GetOrdinal("referencing_id")),
                           ReferencedDatabaseName = r.GetString(r.GetOrdinal("referenced_database_name")),
                           //IsAmbiguous = r.GetString(r.GetOrdinal("is_ambiguous")),
                           //IsCallerDependent = r.GetString(r.GetOrdinal("is_caller_dependent")),
                           //ReferencedClassDesc = r.GetString(r.GetOrdinal("referenced_class_desc")),
                       };

                       //if (reference.ReferencedClassDesc == "")
                       //{
                       //    var referencedMeta = TableMetadata.FromParts(reference.ReferencedSchemaName, reference.ReferencedEntityName);
                       //    sb.AppendLine(TsqltManager.MockTableWithRows(token, options, 1, referencedMeta, connectionString));
                       //}
                       //else
                       //{
                       //    sb.AppendLine(string.Format("-- {0}", reference.ReferencedEntityName));
                       //}
                       sb.AppendLine(string.Format("-- {0}.{1}.{2}", reference.ReferencedDatabaseName, reference.ReferencedSchemaName, reference.ReferencedEntityName));

                   }));
               });

            ShellManager.AppendToEndOfSelection(sb.ToString());
        }
    }
}
