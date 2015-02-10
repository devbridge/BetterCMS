using System;
using System.Collections.Generic;

using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Type;

namespace Devbridge.Platform.Core.Models
{
    /// <summary>
    /// Id generator for entity id mapping.
    /// For a new entity if GUID Id is provided as not empty it will be used instead of generating new one.
    /// </summary>
    public class EntityIdGenerator : IIdentifierGenerator, IConfigurable
    {
        /// <summary>
        /// The assigned generator.
        /// </summary>
        private readonly Assigned assignedGenerator = new Assigned();

        /// <summary>
        /// The guid.comb generator.
        /// </summary>
        private readonly GuidCombGenerator guidCombGenerator = new GuidCombGenerator();

        /// <summary>
        /// Generate a new identifier
        /// </summary>
        /// <param name="session">The <see cref="T:NHibernate.Engine.ISessionImplementor" /> this id is being generated in.</param>
        /// <param name="obj">The entity for which the id is being generated.</param>
        /// <returns>
        /// The new identifier.
        /// </returns>
        public object Generate(ISessionImplementor session, object obj)
        {
            try
            {
                var id = assignedGenerator.Generate(session, obj);
                if (id is Guid && (Guid)id != default(Guid))
                {
                    return id;
                }
            }
            catch (IdentifierGenerationException)
            {
            }

            return guidCombGenerator.Generate(session, obj);
        }

        /// <summary>
        /// Configure this instance, given the values of parameters
        ///             specified by the user as 
        /// <c>param</c> elements.
        ///             This method is called just once, followed by instantiation.
        /// </summary>
        /// <param name="type">The <see cref="T:NHibernate.Type.IType" /> the identifier should be.</param>
        /// <param name="parms">An <see cref="T:System.Collections.IDictionary" /> of Param values that are keyed by parameter name.</param>
        /// <param name="dialect">The <see cref="T:NHibernate.Dialect.Dialect" /> to help with Configuration.</param>
        public void Configure(IType type, IDictionary<string, string> parms, Dialect dialect)
        {
            assignedGenerator.Configure(type, parms, dialect);
        }
    }
}
