using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using Kore.Data.Common;

namespace Kore.Data.EntityFramework
{
    public static class DbContextExtensions
    {
        public static T ExecuteScalar<T>(this DbContext dbContext, string queryText)
        {
            return dbContext.Database.Connection.ExecuteScalar<T>(queryText);
        }

        public static DataSet ExecuteStoredProcedure(this DbContext dbContext, string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            return dbContext.Database.Connection.ExecuteStoredProcedure(storedProcedure, parameters);
        }

        public static DataSet ExecuteStoredProcedure(this DbContext dbContext, string storedProcedure, IEnumerable<DbParameter> parameters, out Dictionary<string, object> outputValues)
        {
            return dbContext.Database.Connection.ExecuteStoredProcedure(storedProcedure, parameters, out outputValues);
        }

        public static int ExecuteNonQueryStoredProcedure(this DbContext dbContext, string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            return dbContext.Database.Connection.ExecuteNonQueryStoredProcedure(storedProcedure, parameters);
        }

        public static DbParameter CreateParameter(this DbContext dbContext, string parameterName, object value)
        {
            return dbContext.Database.Connection.CreateParameter(parameterName, value);
        }

        public static System.Data.Entity.Core.EntityKey GetEntityKey<T>(this DbContext context, T entity) where T : class
        {
            var oc = ((IObjectContextAdapter)context).ObjectContext;
            ObjectStateEntry ose;
            if (null != entity && oc.ObjectStateManager.TryGetObjectStateEntry(entity, out ose))
            {
                return ose.EntityKey;
            }
            return null;
        }

        public static string GetEntitySetName<T>(this DbContext dbContext) where T : class
        {
            var set = dbContext.Set<T>();
            var regex = new Regex("FROM (?<table>.*) AS");
            string sql = set.ToString();
            var match = regex.Match(sql);

            return match.Groups["table"].Value;
        }

        public static string GetEntitySetName(this DbContext dbContext, Type entityType)
        {
            //TODO: Test which way is faster: this (Regex) or below (using ObjectContext)...

            var set = dbContext.Set(entityType);
            var regex = new Regex("FROM (?<table>.*) AS");
            string sql = set.ToString();
            var match = regex.Match(sql);

            return match.Groups["table"].Value;

            //var metaWs = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;
            //EntityType cspaceEntityType;
            //var ospaceEntityTypes = metaWs.GetItems<EntityType>(DataSpace.OSpace);
            //if (ospaceEntityTypes.Any())
            //{
            //    var ospaceEntityType = ospaceEntityTypes.First(oet => oet.FullName == entityType.FullName);
            //    cspaceEntityType = (EntityType)metaWs.GetEdmSpaceType(ospaceEntityType);
            //}
            //else
            //{
            //    // Old EDMX ObjectContext has empty OSpace, so we get cspaceEntityType directly
            //    var cspaceEntityTypes = metaWs.GetItems<EntityType>(DataSpace.CSpace);
            //    cspaceEntityType = cspaceEntityTypes.First(et => et.FullName == entityType.FullName);
            //}

            //// note CSpace below - not OSpace - evidently the entityContainer is only in the CSpace.
            //var entitySets = metaWs.GetItems<EntityContainer>(DataSpace.CSpace)
            //    .SelectMany(c => c.BaseEntitySets.Where(es => es.ElementType.BuiltInTypeKind == BuiltInTypeKind.EntityType)).ToList();

            //return GetDefaultEntitySetName(cspaceEntityType, entitySets);
        }

        //private static string GetDefaultEntitySetName(EntityType cspaceEntityType, IList<EntitySetBase> entitySets)
        //{
        //    // 1st entity set with matching entity type, otherwise with matching assignable type.
        //    EdmType baseType = cspaceEntityType;
        //    EntitySetBase entitySet = null;
        //    while (baseType != null)
        //    {
        //        entitySet = entitySets.FirstOrDefault(es => es.ElementType == baseType);
        //        if (entitySet != null) return entitySet.Name;
        //        baseType = baseType.BaseType;
        //    }
        //    return string.Empty;
        //}
    }
}