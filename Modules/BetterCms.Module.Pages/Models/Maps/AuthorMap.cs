using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class AuthorMap : EntityMapBase<Author>
    {
        public AuthorMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Authors");

            Map(x => x.FirstName).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.LastName).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.DisplayName).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Title).Nullable().Length(MaxLength.Name);
            Map(x => x.Email).Nullable().Length(MaxLength.Email);
            Map(x => x.Twitter).Nullable().Length(MaxLength.Name);
            Map(x => x.ProfileImageUrl).Nullable().Length(MaxLength.Url);
            Map(x => x.ProfileThumbnailUrl).Nullable().Length(MaxLength.Url);
            Map(x => x.ShortDescription).Nullable().Length(MaxLength.Text).LazyLoad();
            Map(x => x.LongDescription).Nullable().Length(MaxLength.Max).LazyLoad();
        }
    }
}
