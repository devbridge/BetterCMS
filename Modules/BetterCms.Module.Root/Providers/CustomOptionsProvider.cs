using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Dependencies;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Root.Providers
{
    public static class CustomOptionsProvider
    {
        /// <summary>
        /// The list of available custom options
        /// </summary>
        private static List<CustomOptionViewModel> customOptions;

        /// <summary>
        /// The list of custom options providers
        /// </summary>
        private static Dictionary<string, ICustomOptionProvider> providers = new Dictionary<string, ICustomOptionProvider>();

        /// <summary>
        /// Gets or sets the custom options.
        /// </summary>
        /// <value>
        /// The custom options.
        /// </value>
        public static List<CustomOptionViewModel> CustomOptions
        {
            get
            {
                if (customOptions == null)
                {
                    using (var container = ContextScopeProvider.CreateChildContainer())
                    {
                        var repository = container.Resolve<IRepository>();

                        customOptions = repository
                            .AsQueryable<CustomOption>()
                            .OrderBy(o => o.Title)
                            .Select(o => new CustomOptionViewModel
                                             {
                                                 Identifier = o.Identifier,
                                                 Title = o.Title
                                             })
                            .ToList();
                    }
                }

                return customOptions;
            }
        }

        /// <summary>
        /// Registers the provider.
        /// </summary>
        public static void RegisterProvider(string identifier, ICustomOptionProvider provider)
        {
            if (!providers.ContainsKey(identifier))
            {
                providers.Add(identifier, provider);
            }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public static ICustomOptionProvider GetProvider(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return providers[identifier];
        }
    }
}