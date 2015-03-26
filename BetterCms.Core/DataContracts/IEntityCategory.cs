using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    public interface IEntityCategory: IEntity
    {
        ICategory Category { get; set; }

        IEntity Entity { get; set; }

        void SetEntity(IEntity entity);
    }
}
