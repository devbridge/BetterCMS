using BetterModules.Core.Models;

namespace BetterCms.Module.Newsletter.Models.Maps
{
    public class SubscriberMap : EntityMapBase<Subscriber>
    {
        public SubscriberMap()
            : base(NewsletterModuleDescriptor.ModuleName)
        {
            Table("Subscribers");

            Map(f => f.Email).Not.Nullable().Length(MaxLength.Email);
        }
    }
}