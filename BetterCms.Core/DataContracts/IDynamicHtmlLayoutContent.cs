namespace BetterCms.Core.DataContracts
{
    public interface IDynamicHtmlLayoutContent : IDynamicContent
    {
        string Html { get; }
    }
}