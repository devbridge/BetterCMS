using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

using Common.Logging;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public class DefaultVersionChecker : IVersionChecker
    {
        private readonly string FolderPath = HostingEnvironment.MapPath(@"~/App_Data/BetterCMS/");

        private const string Filename = @"versions.info.cache";

        private string FilePath
        {
            get
            {
                return string.Concat(FolderPath, Filename);
            }
        }

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private Dictionary<long, HashSet<string>> versionHolder = null;

        private Dictionary<long, HashSet<string>> versions
        {
            get
            {
                return versionHolder ?? (versionHolder = new Dictionary<long, HashSet<string>>());
            }
        }

        public bool VersionExists(string moduleName, long version)
        {
            try
            {
                return versions.ContainsKey(version) && versions[version].Contains(moduleName);
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker version check failed.", ex);
                return false;
            }
        }

        public void AddVersion(string moduleName, long version)
        {
            try
            {
                AddVersion(moduleName, version, true);
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker version adding failed.", ex);
            }
        }

        public DefaultVersionChecker()
        {
            try
            {
                LoadFromFile();
            }
            catch (Exception ex)
            {
                Log.Error("DefaultVersionChecker loading from file failed.", ex);
            }
        }

        private void AddVersion(string moduleName, long version, bool writeTofile)
        {
            if (VersionExists(moduleName, version))
            {
                return;
            }

            if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrWhiteSpace(moduleName))
            {
                if (writeTofile && !SaveToFile(moduleName, version))
                {
                    return;
                }

                if (!versions.ContainsKey(version))
                {
                    versions.Add(version, new HashSet<string>());
                }

                versions[version].Add(moduleName);

            }
        }

        private void LoadFromFile()
        {
            versionHolder = null;

            if (File.Exists(FilePath))
            {
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
            }
        }

        private bool SaveToFile(string moduleName, long version)
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

            return true;
        }
    }
}
