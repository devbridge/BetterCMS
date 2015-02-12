namespace Devbridge.Platform.Core.Models
{
    public static class SchemaNameProvider
    {
        static SchemaNameProvider()
        {
            SchemaNamePattern = "module_{0}";
        }

        /// <summary>
        /// Default database schema name pattern.
        /// </summary>
        public static string SchemaNamePattern { get; set; }

        public static string GetSchemaName(string module)
        {
            return string.Format(SchemaNamePattern, module);
        }
    }
}
