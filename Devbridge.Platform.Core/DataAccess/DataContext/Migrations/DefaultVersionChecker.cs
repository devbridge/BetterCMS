using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Common.Logging;

using Devbridge.Platform.Core.Environment.FileSystem;
using Devbridge.Platform.Core.Modules.Registration;

using NHibernate.Transform;

namespace Devbridge.Platform.Core.DataAccess.DataContext.Migrations
{
    public class DefaultVersionChecker : IVersionChecker
    {
        private string folderPath;

        private const string filename = @"versions.info.cache";

        private string FolderPath
        {
            get
            {
                if (folderPath == null)
                {
                    folderPath = workingDirectory.GetWorkingDirectoryPath();
                }

                return folderPath;
            }
        }

        private string FilePath
        {
            get
            {
                return string.Concat(FolderPath, filename);
            }
        }

        private readonly IUnitOfWork unitOfWork;

        private readonly IWorkingDirectory workingDirectory;
        
        private readonly IModulesRegistration modulesRegistration;

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IList<long>> versionHolder = new Dictionary<string, IList<long>>();

        private bool isLoadedFromDatabase;

        public DefaultVersionChecker(IUnitOfWork unitOfWork, IModulesRegistration modulesRegistration, IWorkingDirectory workingDirectory)
        {
            this.unitOfWork = unitOfWork;
            this.modulesRegistration = modulesRegistration;
            this.workingDirectory = workingDirectory;

            try
            {
                var isLoaded = LoadFromFile();
                
                if (!isLoaded)
                {
                    LoadFromDatabase();
                }
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker loading from file / database failed.", ex);
            }
        }

        public bool VersionExists(string moduleName, long version)
        {
            try
            {
                var exists = versionHolder.ContainsKey(moduleName) && versionHolder[moduleName].Contains(version);

                if (!exists && !isLoadedFromDatabase)
                {
                    LoadFromDatabase();

                    return VersionExists(moduleName, version);
                }

                return exists;
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker version check failed.", ex);
                return false;
            }
        }
        
        private void LoadFromDatabase()
        {
            isLoadedFromDatabase = true;
            Log.Trace("Loading migration files list from database");

            // Select which tables are available
            var tablesQuery = string.Format("SELECT TABLE_SCHEMA AS SchemaName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", VersionInfo.TableName);
            var schemas = unitOfWork
                .Session
                .CreateSQLQuery(tablesQuery)
                .SetResultTransformer(Transformers.AliasToBean<VersionInfo>())
                .List<VersionInfo>();

            if (!schemas.Any())
            {
                return;
            }

            // Construct query with existing tables
            var sqlQueryBuilder = new StringBuilder();
            var first = true;
            foreach (var module in modulesRegistration.GetModules())
            {
                var schemaName = module.ModuleDescriptor.SchemaName;
                if (!string.IsNullOrWhiteSpace(schemaName) && schemas.Any(s => s.SchemaName.ToLowerInvariant() == schemaName.ToLowerInvariant()))
                {
                    if (!first)
                    {
                        sqlQueryBuilder.AppendLine("UNION ALL");
                    }

                    sqlQueryBuilder
                        .AppendFormat("SELECT {0}, '{1}' AS {2} FROM {3}.{4}", 
                            VersionInfo.VersionFieldName, 
                            module.ModuleDescriptor.Name, 
                            VersionInfo.ModuleFieldName,
                            module.ModuleDescriptor.SchemaName,
                            VersionInfo.TableName)
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
                    if (!VersionExists(version.ModuleName, version.Version))
                    {
                        AddVersion(version.ModuleName, version.Version, true);
                    }
                }
            }
        }

        private bool LoadFromFile()
        {
            if (File.Exists(FilePath))
            {
                Log.Trace("Loading migration files list from cache file");

                StreamReader file = null;
                try
                {
                    file = new StreamReader(FilePath);
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                        {
                            var segments = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (segments.Length == 2)
                            {
                                long version;
                                if (long.TryParse(segments[0], out version))
                                {
                                    AddVersion(segments[1], version, false);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (file != null)
                    {
                        file.Close();
                    }
                }

                return true;
            }

            return false;
        }

        public void AddVersion(string moduleName, long version)
        {
            AddVersion(moduleName, version, true);
        }

        private void AddVersion(string moduleName, long version, bool writeToFile)
        {
            try
            {
                if (writeToFile)
                {
                    SaveToFile(moduleName, version);
                }

                if (!versionHolder.ContainsKey(moduleName))
                {
                    versionHolder.Add(moduleName, new List<long>());
                }

                if (!versionHolder[moduleName].Contains(version))
                {
                    versionHolder[moduleName].Add(version);
                }
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker version adding failed.", ex);
            }
        }

        private void SaveToFile(string moduleName, long version)
        {
            StreamWriter file = null;
            try
            {
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                file = new StreamWriter(FilePath, true);
                file.WriteLine("{0} {1}", version, moduleName);
                file.Flush();
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }
    }
}
