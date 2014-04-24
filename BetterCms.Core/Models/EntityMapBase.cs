using BetterCms.Core.DataContracts;

using FluentNHibernate.Mapping;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Fluent nHibernate entity map base class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to map.</typeparam>
    public abstract class EntityMapBase<TEntity> : ClassMap<TEntity>
        where TEntity : IEntity
    {
        /// <summary>
        /// The module name.
        /// </summary>
        private readonly string moduleName;

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
                return "bcms_" + moduleName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMapBase{TEntity}" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        protected EntityMapBase(string moduleName)
        {
            this.moduleName = moduleName;

            Schema(SchemaName);

            Id(x => x.Id).GeneratedBy.Custom<EntityIdGenerator>();

            Map(x => x.IsDeleted).Not.Nullable();
            
            Map(x => x.CreatedOn).Not.Nullable();
            Map(x => x.ModifiedOn).Not.Nullable();
            Map(x => x.DeletedOn).Nullable();

            Map(x => x.CreatedByUser).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.ModifiedByUser).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.DeletedByUser).Nullable().Length(MaxLength.Name);
            
            Version(x => x.Version);

            OptimisticLock.Version();
        }
    }
}
