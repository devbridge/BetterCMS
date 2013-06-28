using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api
{
    public class DefaultApiFasade : IApiFasade
    {
        private readonly IRootApiOperations root;

        private readonly IPagesApiOperations pages;

        public DefaultApiFasade(IRootApiOperations root, IPagesApiOperations pages)
        {            
            this.root = root;
            this.pages = pages;
        }

        public IRootApiOperations Root
        {
            get
            {
                return root;
            }
        }

        public IPagesApiOperations Pages
        {
            get
            {
                return pages;
            }
        }
    }
}