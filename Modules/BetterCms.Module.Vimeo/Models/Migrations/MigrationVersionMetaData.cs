using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Vimeo.Models.Migrations
{
    [VersionTableMetaData]
    public class VimeoVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + VimeoModuleDescriptor.ModuleName;
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
                return "uc_VersionInfo_Version_" + VimeoModuleDescriptor.ModuleName;
            }
        }
    }
}