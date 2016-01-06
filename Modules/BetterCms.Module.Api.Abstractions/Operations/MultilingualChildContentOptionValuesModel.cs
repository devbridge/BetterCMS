using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    [Serializable]
    public class MultilingualChildContentOptionValuesModel : ChildContentOptionValuesModel
    {
        [DataMember]
        public IList<MultilingualOptionValueModel> MultilingualOptionValues { get; set; }

        [Obsolete]
        [DataMember]
        public override IList<OptionValueModel> OptionValues
        {
            get
            {
                return MultilingualOptionValues.Cast<OptionValueModel>().ToList();
            }
            set
            {
                MultilingualOptionValues = value.Cast<MultilingualOptionValueModel>().ToList();
            }
        }
    }
}
