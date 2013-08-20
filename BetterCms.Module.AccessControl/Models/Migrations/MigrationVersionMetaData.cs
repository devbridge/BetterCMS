using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.AccessControl.Models.Migrations
{
    [VersionTableMetaData]
    public class BlogVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + UserAccessModuleDescriptor.ModuleName.ToLowerInvariant();
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
                return "uc_VersionInfo_Verion_" + UserAccessModuleDescriptor.ModuleName;
            }
        }
    }
}