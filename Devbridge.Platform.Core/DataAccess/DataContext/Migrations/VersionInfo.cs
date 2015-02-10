namespace Devbridge.Platform.Core.DataAccess.DataContext.Migrations
{
    public class VersionInfo
    {
        internal const string VersionFieldName = "Version";
        
        internal const string ModuleFieldName = "ModuleName";

        internal const string TableName = "VersionInfo";

        public long Version { get; set; }
        
        public string ModuleName { get; set; }
        
        public string SchemaName { get; set; }
    }
}
