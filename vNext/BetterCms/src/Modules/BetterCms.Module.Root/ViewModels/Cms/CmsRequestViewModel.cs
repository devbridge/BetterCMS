using System;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    /// <summary>
    /// View model for rendering CMS page
    /// </summary>
    [Serializable]
    public class CmsRequestViewModel
    {
        public RedirectViewModel Redirect { get; set; }

        public RenderPageViewModel RenderPage { get; set; }

        public CmsRequestViewModel()
        {
        }

        public CmsRequestViewModel(RedirectViewModel redirect)
        {
            Redirect = redirect;
        }

        public CmsRequestViewModel(RenderPageViewModel renderPage)
        {
            RenderPage = renderPage;
        }

        public override string ToString()
        {
            return string.Format("Redirect: {0}, RenderPage: {1}", Redirect, RenderPage);
        }
    }
}