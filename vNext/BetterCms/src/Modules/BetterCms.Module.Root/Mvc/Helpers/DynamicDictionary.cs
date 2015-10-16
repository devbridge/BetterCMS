using System;
using System.Collections.Generic;
using System.Dynamic;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    [Serializable]
    public class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, dynamic> properties = new Dictionary<string, dynamic>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Tries to get dynamic dictionary member.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out dynamic result)
        {
            result = properties.ContainsKey(binder.Name) ? properties[binder.Name] : null;

            return true;
        }

        /// <summary>
        /// Tries to set dynamic dictionary member.
        /// </summary>
        /// <param name="binder">The binder.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, dynamic value)
        {
            if (value == null)
            {
                if (properties.ContainsKey(binder.Name))
                {
                    properties.Remove(binder.Name);
                }
            }
            else
            {
                properties[binder.Name] = value;
            }

            return true;
        }
    }
}