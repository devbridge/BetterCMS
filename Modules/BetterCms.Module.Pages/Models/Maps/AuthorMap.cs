using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class AuthorMap : EntityMapBase<Author>
    {
        public AuthorMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Authors");

            Map(x => x.Name).Not.Nullable().Length(MaxLength.Name);

            References(x => x.Image).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
