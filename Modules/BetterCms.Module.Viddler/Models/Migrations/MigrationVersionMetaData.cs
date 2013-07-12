using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Viddler.Models.Migrations
{
    [VersionTableMetaData]
    public class ViddlerVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + ViddlerModuleDescriptor.ModuleName;
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
                return "uc_VersionInfo_Version_" + ViddlerModuleDescriptor.ModuleName;
            }
        }
    }
}