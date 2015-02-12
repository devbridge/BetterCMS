using FluentNHibernate.Mapping;

namespace Devbridge.Platform.Core.Models
{
    /// <summary>
    /// Fluent nHibernate sub-entity base map class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to map.</typeparam>
    public abstract class EntitySubClassMapBase<TEntity> : SubclassMap<TEntity>
    {
        /// <summary>
        /// The module name.
        /// </summary>
        private readonly string moduleName;

        /// <summary>
        /// The schema name
        /// </summary>
        private string schemaName;

        /// <summary>
        /// Gets the name of the schema.
        /// </summary>
        /// <value>
        /// The name of the schema.
        /// </value>
        protected string SchemaName
        {
            get
            {
                return schemaName ?? (schemaName = SchemaNameProvider.GetSchemaName(moduleName));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySubClassMapBase{TEntity}" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        protected EntitySubClassMapBase(string moduleName)
        {
            this.moduleName = moduleName;

            if (SchemaName != null)
            {
                Schema(SchemaName);
            }

            KeyColumn("Id");
        }
    }
}
