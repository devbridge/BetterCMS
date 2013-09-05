namespace BetterCms.Core.DataContracts
{
    public interface IDeletableOption<TEntity> : IOption
        where TEntity : IEntity
    {
        bool IsDeletable { get; set; }

        TEntity Entity { get; set; }
    }
}
