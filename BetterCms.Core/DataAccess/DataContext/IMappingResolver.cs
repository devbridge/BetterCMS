using FluentNHibernate.Cfg;

namespace BetterCms.Core.DataAccess.DataContext
{
    /// <summary>
    /// </summary>
    public interface IMappingResolver
    {
        /// <summary>
        /// Adds all available mappings from modules.
        /// </summary>
        /// <param name="fluentConfiguration">The fluent configuration to update.</param>
        void AddAvailableMappings(FluentConfiguration fluentConfiguration);
    }
}
