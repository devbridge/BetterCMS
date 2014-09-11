using System.Collections.Generic;
using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(ModuleElement), AddItemName = "module")]
    public class ModulesCollection : ConfigurationElementCollection, IEnumerable<ModuleElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleElement)(element)).Name;
        }

        public ModuleElement this[int idx]
        {
            get { return (ModuleElement)BaseGet(idx); }
        }

        public ModuleElement GetByName(string moduleName)
        {
            var enumerator = GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Name == moduleName) return enumerator.Current;
            }

            return null;
        }


        public new IEnumerator<ModuleElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ModuleElement;
            }
        }
    }
}
