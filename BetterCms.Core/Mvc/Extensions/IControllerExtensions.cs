using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace BetterCms.Core.Mvc.Extensions
{
    /// <summary>
    /// Defines the contract for controller extensions.
    /// </summary>
    public interface IControllerExtensions
    {        
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>Controller name.</returns>
        string GetControllerName(Type controllerType);

        /// <summary>
        /// Determines whether type is controller type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if type is controller type; otherwise, <c>false</c>.
        /// </returns>
        bool IsControllerType(Type type);

        /// <summary>
        /// Determines whether type is controller type.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if type is controller type; otherwise, <c>false</c>.
        /// </returns>
        bool IsControllerType<TController>(Type type);

        /// <summary>
        /// Gets the controller types from given assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Controller types from assembly.</returns>
        IEnumerable<Type> GetControllerTypes(Assembly assembly);

        /// <summary>
        /// Gets the controller types from given assembly.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// Controller types from assembly.
        /// </returns>
        IEnumerable<Type> GetControllerTypes<TController>(Assembly assembly);

        /// <summary>
        /// Gets all actions from given controller type.
        /// </summary>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>Controller action names.</returns>
        IEnumerable<MethodInfo> GetControllerActions(Type controllerType);

        /// <summary>
        /// Gets all actions from given controller type.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <returns>Controller action names.</returns>
        IEnumerable<MethodInfo> GetControllerActions<TController>() where TController : ControllerBase; 
    }
}
