using Devbridge.Platform.Core.Models;

using FluentMigrator.VersionTableInfo;

namespace Devbridge.Platform.Sample.Module.Models.Migrations
{
    [VersionTableMetaData]
    public class RootVersionTableMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return SchemaNameProvider.GetSchemaName(SampleModuleDescriptor.ModuleName);
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
                return "uc_VersionInfo_Verion_" + SampleModuleDescriptor.ModuleName;
            }
        }
    }
}