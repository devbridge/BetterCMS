using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(LinkElement), AddItemName = "link", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class LinkCollection : ConfigurationElementCollection
    {
        #region Indexers

        public LinkElement this[int index]
        {
            get
            {
                return (LinkElement)BaseGet(index);
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
            return new LinkElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as LinkElement).Name;
        }

        #endregion
    }
}