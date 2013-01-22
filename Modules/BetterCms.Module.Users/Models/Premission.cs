using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models
{
    [Serializable]
    public class Premission : EquatableEntity<Premission>
    {
        public virtual string Name { get; set; }
    }
}