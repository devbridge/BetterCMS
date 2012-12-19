using System;

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace BetterCms.Core.DataAccess.DataContext.Conventions
{
    public class EnumConvention : IPropertyConvention, IPropertyConventionAcceptance
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.CustomType(instance.Property.PropertyType);
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(
                x =>
                x.Property.PropertyType.IsEnum || 
                x.Property.PropertyType.IsGenericType && x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && x.Property.PropertyType.GetGenericArguments()[0].IsEnum);
        }
    }
}
