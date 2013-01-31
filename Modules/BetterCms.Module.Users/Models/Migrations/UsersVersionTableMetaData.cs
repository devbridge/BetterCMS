using BetterCms.Module.Users;

using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [VersionTableMetaData]
    public class UsersVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + UsersModuleDescriptor.ModuleName;
            }
        }

        public string TableName
        {
            get
            {
                return "VersionInfo";
            }
        }

        public string ColumnName
        {
            get
            {
                return "Version";
            }
        }

        public string UniqueIndexName
        {
            get
            {
                return "uc_VersionInfo_Verion_" + UsersModuleDescriptor.ModuleName;
            }
        }
    }
}