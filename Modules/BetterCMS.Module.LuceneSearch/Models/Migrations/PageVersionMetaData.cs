using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentMigrator.VersionTableInfo;

namespace BetterCms.Module.LuceneSearch.Models.Migrations
{
    [VersionTableMetaData]
    public class PageVersionMetaData : IVersionTableMetaData
    {
        public string SchemaName
        {
            get
            {
                return "bcms_" + LuceneSearchModuleDescriptor.ModuleName;
            }
        }

        public string TableName {
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
                return "uc_VersionInfo_Verion_" + LuceneSearchModuleDescriptor.ModuleName;
            }
        }
    }
}