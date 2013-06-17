using Autofac;

namespace BetterCms.Core.Dependencies
{
    /// <summary>
    /// The context scope provider.
    /// </summary>
    public static class ContextScopeProvider
    {
        /// <summary>
        /// The root application container.
        /// </summary>
        internal static readonly IContainer ApplicationContainer;

        /// <summary>
        /// Initializes the <see cref="ContextScopeProvider" /> class.
        /// </summary>
        static ContextScopeProvider()
        {
            ApplicationContainer = new ContainerBuilder().Build();
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        public static void RegisterTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.Update(ApplicationContainer);
        }

        /// <summary>
        /// Creates the child container.
        /// </summary>
        /// <returns>New nested lifetime scope.</returns>
        public static ILifetimeScope CreateChildContainer()
        {
            return ApplicationContainer.BeginLifetimeScope();
        }
    }
}