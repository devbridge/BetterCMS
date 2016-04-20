// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSONDataBinder.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BetterCms.Core.Mvc.Binders
{
    public class SingleValueArrayConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var retVal = new Object();
            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.String)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }


    public class JSONDataBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext != null
                && controllerContext.HttpContext != null
                && controllerContext.HttpContext.Request != null
                && controllerContext.HttpContext.Request.InputStream != null)
            {
                controllerContext.HttpContext.Request.InputStream.Position = 0;
                var json = new StreamReader(controllerContext.HttpContext.Request.InputStream).ReadToEnd();
                var type = bindingContext.ModelType;

                var model = JsonConvert.DeserializeObject(json, type,
                    new JsonSerializerSettings {
                        // Adding ISO date time converter, because dates are sent as strings, not as integers
                        Converters = new List<JsonConverter> { new IsoDateTimeConverter() }
                    });
                return model;
            }

            return null;
        }
    }
}