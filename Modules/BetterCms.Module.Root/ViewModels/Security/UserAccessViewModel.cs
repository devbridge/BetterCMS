using System;

using BetterCms.Core.Security;

namespace BetterCms.Module.Root.ViewModels.Security
{
    [Serializable]
    public class UserAccessViewModel : IUserAccess
    {
        public Guid Id { get; set; }

        public Guid ObjectId { get; set; }

        public string RoleOrUser { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, ObjectId: {1}, RoleOrUser: {2}, AccessLevel: {3}", Id, ObjectId, RoleOrUser, AccessLevel);
        }
    }
}