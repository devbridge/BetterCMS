using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.Navigation.Models.Migrations
{
    /// <summary>
    /// The blog version table meta data.
    /// </summary>
    [VersionTableMetaData]
    public class SitemapVersionTableMetaData : IVersionTableMetaData
    {
        /// <summary>
        /// Gets the schema name.
        /// </summary>
        public string SchemaName
        {
            get
            {
                return "bcms_" + NavigationModuleDescriptor.ModuleName;
            }
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string TableName
        {
            get
            {
                return "VersionInfo";
            }
        }

        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string ColumnName
        {
            get
            {
                return "Version";
            }
        }

        /// <summary>
        /// Gets the unique index name.
        /// </summary>
        public string UniqueIndexName
        {
            get
            {
                return "uc_VersionInfo_Verion_" + NavigationModuleDescriptor.ModuleName;
            }
        }
    }
}