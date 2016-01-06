using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    [Serializable]
    public class MultilingualOptionValueModel : OptionValueModel
    {
        [DataMember]
        public IList<OptionTranslationModel> Translations { get; set;}
    }
}
