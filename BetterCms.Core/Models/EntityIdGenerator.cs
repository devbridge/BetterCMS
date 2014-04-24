using System;
using System.Collections.Generic;

using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Type;

namespace BetterCms.Core.Models
{
    public class EntityIdGenerator : IIdentifierGenerator, IConfigurable
    {
        private readonly Assigned assignedGenerator = new Assigned();
        private readonly GuidCombGenerator guidCombGenerator = new GuidCombGenerator();

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

        public void Configure(IType type, IDictionary<string, string> parms, Dialect dialect)
        {
            assignedGenerator.Configure(type, parms, dialect);
        }
    }
}
