using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class RedirectMap : EntityMapBase<Redirect>
    {
        public RedirectMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Redirects");
            
            Map(x => x.PageUrl).Not.Nullable().Length(MaxLength.Url);
            Map(x => x.RedirectUrl).Not.Nullable().Length(MaxLength.Url);
        }
    }
}
