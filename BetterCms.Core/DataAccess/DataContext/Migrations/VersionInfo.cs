namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    public class VersionInfo
    {
        internal const string VersionFieldName = "Version";
        
        internal const string ModuleFieldName = "ModuleName";

        internal const string TableName = "VersionInfo";

        internal const string SqlQuery = "SELECT {0}, '{1}' AS {2} FROM {3}.VersionInfo";
        
        internal const string SqlUnion = "UNION ALL";

        public long Version { get; set; }
        
        public string ModuleName { get; set; }
        
        public string SchemaName { get; set; }
    }
}
