using FluentNHibernate.Mapping;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Fluent nHibernate sub-entity base map class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to map.</typeparam>
    public abstract class EntitySubClassMapBase<TEntity> : SubclassMap<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySubClassMapBase{TEntity}" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        protected EntitySubClassMapBase(string moduleName)
        {            
            Schema("bcms_" + moduleName);

            KeyColumn("Id");
        }
    }
}
