using System;
using System.IO;

namespace BetterCms.Api.Tests.Tools
{
    public class DatabaseRefresher
    {
        private const string OriginalDatabaseFilename = "BetterCmsTestsDataSet";

        private const string ConnectionStringPattern = @"Data Source=(LocalDb)\v11.0; Initial Catalog={0}; Integrated Security=SSPI; AttachDBFilename=|DataDirectory|\{0}.mdf";

        private readonly string basePath;

        public string CurrentConnectionString
        {
            get; private set;
        }

        public DatabaseRefresher(string basePath)
        {
            this.basePath = basePath;
        }        

        public void BindNewDatabase()
        {            
            string newName = string.Format("BetterCmsTestsDataSet_Temp_{0}", DateTime.Now.Ticks);
            CurrentConnectionString = string.Format(ConnectionStringPattern, newName);

            File.Copy(Path.Combine(basePath, OriginalDatabaseFilename + ".mdf"), Path.Combine(basePath, newName + ".mdf"));
            File.Copy(Path.Combine(basePath, OriginalDatabaseFilename + "_log.ldf"), Path.Combine(basePath, newName + "_log.ldf"));            
        }

        public void ClearTempDatabases()
        {
        }
    }
}