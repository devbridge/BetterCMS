using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Pages.Models.Migrations
{
    [VersionTableMetaData]
    public class PagesVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + PagesModuleDescriptor.ModuleName;
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
                return "uc_VersionInfo_Verion_" + PagesModuleDescriptor.ModuleName;
            }
        }
    }
}