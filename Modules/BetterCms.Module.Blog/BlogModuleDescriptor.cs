using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Modules.Registration;
using BetterCms.Module.Blog.Content.Resources;

namespace BetterCms.Module.Blog
{
    public class BlogModuleDescriptor : ModuleDescriptor
    {
        private JavaScriptModuleDescriptor blogJavaScriptModuleDescriptor;

        public BlogModuleDescriptor()
        {
            InitializeBlogJavaScriptModule();
        }

        public override string Name
        {
            get
            {
                return "Blog";
            }
        }

        public override string Description
        {
            get
            {
                return "Blog module for BetterCMS.";
            }
        }

        public override System.Collections.Generic.IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(Autofac.ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {
                    new ButtonActionProjection(blogJavaScriptModuleDescriptor, page => "postNewArticle")
                        {
                            Title = () => BlogGlobalization.Sidebar_AddNewPostButtonTitle,
                            Order = 200,
                            CssClass = page => "bcms-sidemenu-btn bcms-btn-blog-add"
                        }
                };
        }

        public override System.Collections.Generic.IEnumerable<JavaScriptModuleDescriptor> RegisterJavaScriptModules(Autofac.ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new[]
                {
                    blogJavaScriptModuleDescriptor
                };
        }

        public override System.Collections.Generic.IEnumerable<string> RegisterStyleSheetFiles(Autofac.ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new[]
                {
                    "/file/bcms-blog/Content/Styles/bcms.blog.css"
                };
        }

        private void InitializeBlogJavaScriptModule()
        {
            blogJavaScriptModuleDescriptor = new JavaScriptModuleDescriptor(this, "bcms.blog", "/file/bcms-blog/scripts/bcms.blog");          
        }
    }
}
