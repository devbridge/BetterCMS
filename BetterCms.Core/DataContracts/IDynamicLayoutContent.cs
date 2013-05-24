namespace BetterCms.Core.DataContracts
{
    public interface IDynamicLayoutContent : IContent
    {
        ILayout Layout { get; }
    }
}