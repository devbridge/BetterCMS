namespace BetterCms.Core.DataContracts
{
    public interface IDynamicContentContainer : IContent
    {
        string Html { get; set; }
    }
}
