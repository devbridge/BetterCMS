using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Newsletter.Models
{
    [Serializable]
    public class Subscriber : EquatableEntity<Subscriber>
    {
        public virtual string Email { get; set; }
    }
}