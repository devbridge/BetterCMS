using System;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class RedirectViewModel
    {
        public string RedirectUrl { get; set; }

        public RedirectViewModel()
        {
        }

        public RedirectViewModel(string redirectUrl)
        {
            RedirectUrl = redirectUrl;
        }

        public override string ToString()
        {
            return string.Format("RedirectUrl: {0}", RedirectUrl);
        }
    }
}