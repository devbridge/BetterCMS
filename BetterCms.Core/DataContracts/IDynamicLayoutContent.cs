namespace BetterCms.Core.DataContracts
{
    public interface IDynamicLayoutContent : IDynamicContent
    {
        ILayout Layout { get; }
    }
}