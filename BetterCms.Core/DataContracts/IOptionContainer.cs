using System.Collections.Generic;

using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access the the list of entity options.
    /// </summary>
    public interface IOptionContainer<TEntity> : IEntity
        where TEntity : IEntity
    {
        IEnumerable<IDeletableOption<TEntity>> Options { get; set; }
    }
}
