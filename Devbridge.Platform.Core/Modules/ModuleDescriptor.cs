using System.Reflection;

using Autofac;

using Devbridge.Platform.Core.Modules.Registration;

namespace Devbridge.Platform.Core.Modules
{
    /// <summary>
    /// Abstract module descriptor. 
    /// </summary>
    public abstract class ModuleDescriptor
    {
        private AssemblyName assemblyName;

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public virtual int Order
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public virtual string SchemaName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>        
        public AssemblyName AssemblyName
        {
            get
            {
                if (assemblyName == null)
                {
                    assemblyName = GetType().Assembly.GetName();
                }

                return assemblyName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public virtual void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
        }

        /// <summary>
        /// Creates the registration context.
        /// </summary>
        /// <returns>Module registration context</returns>
        public virtual ModuleRegistrationContext CreateRegistrationContext()
        {
            return new ModuleRegistrationContext(this);
        }
    }
}
