namespace BetterCms.Api.Interfaces.Models
{
    public interface IHtmlContentWidget : IContent
    {
        bool UseHtml { get; }   
     
        string CustomCss { get; }

        bool UseCustomCss { get; }

        string Html { get; }

        string CustomJs { get; }

        bool UseCustomJs { get; }
    }
}