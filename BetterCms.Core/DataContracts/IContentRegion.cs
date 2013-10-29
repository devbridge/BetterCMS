namespace BetterCms.Core.DataContracts
{
    public interface IContentRegion
    {
        IDynamicContentContainer Content { get; set; }

        IRegion Region { get; set; }
    }
}
