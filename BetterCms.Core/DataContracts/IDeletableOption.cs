using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    public interface IDeletableOption<TEntity> : IOptionEntity
        where TEntity : IEntity
    {
        bool IsDeletable { get; set; }

        TEntity Entity { get; set; }
    }
}
