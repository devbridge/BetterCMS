using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Users.Models.Migrations
{
    [VersionTableMetaData]
    public class UsersVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return UsersModuleDescriptor.UsersSchemaName;
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