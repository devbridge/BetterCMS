using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [VersionTableMetaData]
    public class BlogVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return BlogModuleDescriptor.BlogSchemaName;
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
                return "uc_VersionInfo_Verion_" + BlogModuleDescriptor.ModuleName;
            }
        }
    }
}