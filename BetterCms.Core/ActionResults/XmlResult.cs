// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResult.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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