using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [Route("/widgets", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetWidgetsRequest : RequestBase<GetWidgetsModel>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Data.HasColumnInSortBySection("WidgetType") || Data.HasColumnInWhereSection("WidgetType"))
            {
                yield return new ValidationResult("A WidgetType field is a dynamically calculated field. You can't sort or add filter by this column.", new List<string> {"Data"});
            }
        }
    }
}