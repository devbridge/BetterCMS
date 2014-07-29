using System;
using System.Collections.Generic;
using System.Text;

using BetterCms.Core.Modules.Registration;

using Common.Logging;

using NHibernate.Transform;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public class DefaultVersionChecker : IVersionChecker
    {
        private readonly IUnitOfWork unitOfWork;
        
        private readonly IModulesRegistration modulesRegistration;

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IList<long>> versionHolder = new Dictionary<string, IList<long>>();

        public DefaultVersionChecker(IUnitOfWork unitOfWork, IModulesRegistration modulesRegistration)
        {
            this.unitOfWork = unitOfWork;
            this.modulesRegistration = modulesRegistration;

            try
            {
                LoadFromDatabase();
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker loading from file failed.", ex);
            }
        }

        public bool VersionExists(string moduleName, long version)
        {
            try
            {
                return versionHolder.ContainsKey(moduleName) && versionHolder[moduleName].Contains(version);
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker version check failed.", ex);
                return false;
            }
        }
        
        private void LoadFromDatabase()
        {
            var sqlQueryBuilder = new StringBuilder();
            var first = true;

            foreach (var module in modulesRegistration.GetModules())
            {
                var schemaName = module.ModuleDescriptor.SchemaName;
                if (!string.IsNullOrWhiteSpace(schemaName))
                {
                    if (!first)
                    {
                        sqlQueryBuilder.AppendLine("UNION ALL");
                    }

                    sqlQueryBuilder
                        .AppendFormat("SELECT {0}, '{1}' AS {2} FROM {3}.VersionInfo", 
                            VersionInfo.VersionFieldName, 
                            module.ModuleDescriptor.Name, 
                            VersionInfo.ModuleFieldName,
                            module.ModuleDescriptor.SchemaName)
                        .AppendLine();

                    first = false;
                }
            }

            var sqlQuery = sqlQueryBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(sqlQuery))
            {
                var versions = unitOfWork
                        .Session
                        .CreateSQLQuery(sqlQuery)
                        .SetResultTransformer(Transformers.AliasToBean<VersionInfo>())
                        .List<VersionInfo>();
                
                foreach (var version in versions)
                {
                    if (!versionHolder.ContainsKey(version.ModuleName))
                    {
                        versionHolder.Add(version.ModuleName, new List<long>());
                    }

                    versionHolder[version.ModuleName].Add(version.Version);
                }
            }
        }
    }
}
