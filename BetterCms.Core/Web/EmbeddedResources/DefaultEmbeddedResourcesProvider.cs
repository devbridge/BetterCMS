using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules.Registration;

using Common.Logging;

namespace BetterCms.Core.Web.EmbeddedResources
{
    /// <summary>
    /// Default implementation of embedded resources provider.
    /// </summary>
    public class DefaultEmbeddedResourcesProvider : IEmbeddedResourcesProvider
    {  
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// Contains resource name - assembly name pairs dictionary.
        /// </summary>
        private readonly ConcurrentDictionary<string, EmbeddedResourceDescriptor> resourceNameEmbeddedResource;

        /// <summary>
        /// Contains virtual path - embedded resource name pairs dictionary.
        /// </summary>
        private readonly ConcurrentDictionary<string, string> virtualPathResourceName;

        /// <summary>
        /// Assembly loader contract.
        /// </summary>
        private readonly IAssemblyLoader assemblyLoader;

        /// <summary>
        /// Modules registry contract.
        /// </summary>
        private readonly IModulesRegistration modulesRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEmbeddedResourcesProvider" /> class.
        /// </summary>
        /// <param name="modulesRegistry">The modules registry.</param>
        /// <param name="assemblyLoader">The assembly loader contract.</param>
        public DefaultEmbeddedResourcesProvider(IModulesRegistration modulesRegistry, IAssemblyLoader assemblyLoader)
        {
            resourceNameEmbeddedResource = new ConcurrentDictionary<string, EmbeddedResourceDescriptor>();
            virtualPathResourceName = new ConcurrentDictionary<string, string>();
            this.modulesRegistry = modulesRegistry;
            this.assemblyLoader = assemblyLoader;            
        }

        /// <summary>
        /// Scans and adds an embedded resources from assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        public void AddEmbeddedResourcesFrom(Assembly assembly)
        {
            if (Logger.IsTraceEnabled)
            {
                Logger.TraceFormat("Adds embedded resources from assembly {0}.", assembly.FullName);
            }

            var resourceNames = assembly.GetManifestResourceNames();            
            var assemblyName = assembly.GetName();

            foreach (var resourceName in resourceNames)
            {
                resourceNameEmbeddedResource.TryAdd(
                    resourceName.ToLowerInvariant(), 
                    new EmbeddedResourceDescriptor
                        {
                            AssemblyName = assemblyName,
                            ResourceName = resourceName
                        });
            }
        }

        /// <summary>
        /// Checks if virtual path exists as embedded resource.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        /// <c>true</c> if virtual path is embedded resource path; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmbeddedResourceVirtualPath(string virtualPath)
        {
            string resourceName;
            if (TryConvertVirtualPathToEmbeddedResourceName(virtualPath, out resourceName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the embedded resource virtual file.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Embedded resource virtual file.</returns>
        public EmbeddedResourcesVirtualFile GetEmbeddedResourceVirtualFile(string virtualPath)
        {
            string resourceName;

            if (TryConvertVirtualPathToEmbeddedResourceName(virtualPath, out resourceName))
            {
                var embeddedResourceDescriptor = resourceNameEmbeddedResource[resourceName];
                var assembly = assemblyLoader.Load(embeddedResourceDescriptor.AssemblyName);

                return new EmbeddedResourcesVirtualFile(assembly, embeddedResourceDescriptor.ResourceName, virtualPath);
            }

            return null;
        }

        /// <summary>
        /// Gets the embedded resource JavaScript virtual files.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>List of embedded resource virtual files.</returns>
        public IEnumerable<EmbeddedResourcesVirtualFile> GetEmbeddedResourceJsVirtualFiles(Assembly assembly)
        {
            var javaScriptResources = resourceNameEmbeddedResource
                                    .Where(f => f.Key.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
                                    .Where(f => f.Value.AssemblyName.FullName == assembly.FullName);

            foreach (var resource in javaScriptResources)
            {
                yield return new EmbeddedResourcesVirtualFile(assembly, resource.Key, "/file/");
            }
        }

        /// <summary>
        /// Converts the name of the virtual path to embedded resource.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="embeddedResourceName">Name of the resource.</param>
        /// <returns>Returns true if virtual path was successfully parsed to resource name; false otherwise.</returns>
        private bool TryConvertVirtualPathToEmbeddedResourceName(string virtualPath, out string embeddedResourceName)
        {
            bool success = false;
            embeddedResourceName = null;

            try
            {
                string virtualPathLowered = virtualPath.ToLowerInvariant();

                if (virtualPathResourceName.ContainsKey(virtualPathLowered))
                {
                    embeddedResourceName = virtualPathResourceName[virtualPathLowered];
                    success = true;                    
                }
                
                if (!success)
                {
                    string rawResourceName;
                    var areaName = ParseAreaNameFromVirtualPath(virtualPathLowered, out rawResourceName);
                    if (!string.IsNullOrEmpty(areaName))
                    {
                        var module = modulesRegistry.FindModuleByAreaName(areaName);
                        if (module != null)
                        {
                            string predicatedResourceName = (module.AssemblyName.Name + rawResourceName).ToLowerInvariant();
                            if (resourceNameEmbeddedResource.ContainsKey(predicatedResourceName))
                            {
                                virtualPathResourceName.TryAdd(virtualPathLowered, predicatedResourceName);
                                embeddedResourceName = predicatedResourceName;
                                success = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WarnFormat("Failed to convert virtual path '{0}' to embedded resource name.", ex, virtualPath);                
            }

            return success;
        }

        /// <summary>
        /// Parses the name of area from virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="rawResourceName">Name of the embedded resource without assembly name.</param>
        /// <returns>
        /// Name of area; null if area name not exists in virtual path.
        /// </returns>
        private string ParseAreaNameFromVirtualPath(string virtualPath, out string rawResourceName)
        {
            int startIndex = virtualPath.IndexOf("/Areas/", StringComparison.OrdinalIgnoreCase);
            if (startIndex > -1)
            {
                startIndex += 7;                
                int endIndex = virtualPath.IndexOf('/', startIndex);
                if (endIndex > -1)
                {
                    rawResourceName = string.Empty;

                    string[] parts = virtualPath.Substring(endIndex).Split('/');
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        string part = parts[i].Replace("-", "_");
                        part = Regex.Replace(part, @"^([0-9])", "_$1");

                        if (i > 0)
                        {
                            rawResourceName = string.Concat(rawResourceName, ".");
                        }

                        rawResourceName = string.Concat(rawResourceName, part);
                    }
                    rawResourceName = string.Concat(rawResourceName, ".", parts[parts.Length - 1]);
                    
                    string areaName = virtualPath.Substring(startIndex, endIndex - startIndex);
                    if (modulesRegistry.IsModuleRegisteredByAreaName(areaName))
                    {
                        return areaName;
                    }
                }
            }

            rawResourceName = null;
            return null;
        }
    }
}