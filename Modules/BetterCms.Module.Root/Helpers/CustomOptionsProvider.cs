using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Dependencies;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Root.Helpers
{
    public static class CustomOptionsProvider
    {
        private static List<CustomOptionViewModel> customOptions;

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
    }
}