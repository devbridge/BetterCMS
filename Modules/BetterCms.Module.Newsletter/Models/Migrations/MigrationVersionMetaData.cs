using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Newsletter.Models.Migrations
{
    [VersionTableMetaData]
    public class NewsletterVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + NewsletterModuleDescriptor.ModuleName;
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
                return "uc_VersionInfo_Version_" + NewsletterModuleDescriptor.ModuleName;
            }
        }
    }
}