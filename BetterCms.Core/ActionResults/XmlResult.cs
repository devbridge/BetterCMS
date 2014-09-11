using System.Web.Mvc;
using System.Xml.Serialization;

namespace BetterCms.Core.ActionResults
{
    public class XmlResult : ActionResult
    {
        private readonly object objectToSerialize;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        public XmlResult(object objectToSerialize)
        {
            this.objectToSerialize = objectToSerialize;
        }

        /// <summary>
        /// Gets the object to be serialized to XML.
        /// </summary>
        public object ObjectToSerialize
        {
            get { return objectToSerialize; }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (objectToSerialize != null)
            {
                context.HttpContext.Response.Clear();
                var xs = new XmlSerializer(objectToSerialize.GetType());
                context.HttpContext.Response.ContentType = "text/xml";
                
                if (objectToSerialize is IHaveCustomXmlSettings)
                {
                    xs.Serialize(context.HttpContext.Response.Output, objectToSerialize, ((IHaveCustomXmlSettings)objectToSerialize).Namespaces);
                }
                else
                {
                    xs.Serialize(context.HttpContext.Response.Output, objectToSerialize);
                }
            }
        }
    }
}