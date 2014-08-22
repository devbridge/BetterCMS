using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [VersionTableMetaData]
    public class MediaManagerVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return MediaManagerModuleDescriptor.MediaManagerSchemaName;
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
                return "uc_VersionInfo_Verion_" + MediaManagerModuleDescriptor.ModuleName;
            }
        }
    }
}