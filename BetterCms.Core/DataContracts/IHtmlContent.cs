using System;

namespace BetterCms.Core.DataContracts
{
    public interface IHtmlContent : IContent
    {
        string CustomCss { get; }

        bool UseCustomCss { get; }

        string Html { get; }        

        string CustomJs { get; }

        bool UseCustomJs { get; }

        DateTime ActivationDate { get; set; }

        DateTime? ExpirationDate { get; set; }
    }
}