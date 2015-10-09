using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(SectionElement), AddItemName = "section", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SectionElementCollection : ConfigurationElementCollection
    {
        #region Indexers

        public SectionElement this[int index]
        {
            get
            {
                return (SectionElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        #endregion

        #region Overrides

        protected override ConfigurationElement CreateNewElement()
        {
            return new SectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as SectionElement).Name;
        }

        #endregion
    }
}