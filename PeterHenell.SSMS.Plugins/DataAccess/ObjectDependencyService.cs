using PeterHenell.SSMS.Plugins.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterHenell.SSMS.Plugins.DataAccess
{
    public class ObjectDependencyService
    {
        public List<ObjectReference> GetDependencies(ObjectMetadata meta, string connectionString, System.Threading.CancellationToken token)
        {
            return GetObjectReferencesFor(token, connectionString, meta);
        }

        string sql = @"WITH ObjectDepends(entity_name,referenced_schema, referenced_entity, referenced_id,referencing_id,referenced_database_name,referenced_schema_name,is_ambiguous,is_caller_dependent, referenced_class_desc, level)
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
          AND OBJECT_SCHEMA_NAME(referencing_id) = @referencing_schema
    )

SELECT    entity_name AS referencing_entity
        , referenced_entity
        , referenced_class_desc
        , level
        , referencing_id
        , referenced_id
        , COALESCE(referenced_database_name, DB_NAME()) as referenced_database_name
        , referenced_schema_name
        , is_ambiguous
        , is_caller_dependent
FROM ObjectDepends
WHERE referenced_schema_name IS NOT NULL
ORDER BY level;";

        private List<ObjectReference> GetObjectReferencesFor(System.Threading.CancellationToken token, string connectionString, ObjectMetadata meta)
        {
            var refs = new List<ObjectReference>();
            QueryManager.Run(connectionString, token, (queryManager) =>
            {
                var dt = new DataTable();
                queryManager.Fill(sql, dt, new Dictionary<string, string>() {
                    { "referencing_schema", meta.SchemaName },
                    { "referencing_entity", meta.ObjectName },
                });

                foreach (DataRow row in dt.Rows)
                {
                    var reference = new ObjectReference
                    {
                        EntityName = row.Field<string>("referencing_entity"),
                        ReferencedObject = ObjectMetadata.FromParts(
                                                              row.Field<string>("referenced_database_name"), 
                                                              row.Field<string>("referenced_schema_name"),
                                                              row.Field<string>("referenced_entity"))
                    };
                    refs.Add(reference);
                }
            });
            return refs;
        }
    }
}
