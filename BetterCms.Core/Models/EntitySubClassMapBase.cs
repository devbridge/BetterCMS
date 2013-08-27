using FluentNHibernate.Mapping;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Fluent nHibernate sub-entity base map class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to map.</typeparam>
    public abstract class EntitySubClassMapBase<TEntity> : SubclassMap<TEntity>
    {
        private readonly string moduleName;

        /// <summary>
        /// Gets the name of the database schema.
        /// </summary>
        /// <value>
        /// The name of the database schema.
        /// </value>
        protected string SchemaName
        {
            get
            {
                return "bcms_" + moduleName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySubClassMapBase{TEntity}" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        protected EntitySubClassMapBase(string moduleName)
        {
            this.moduleName = moduleName;

            Schema(SchemaName);

            KeyColumn("Id");
        }
    }
}
