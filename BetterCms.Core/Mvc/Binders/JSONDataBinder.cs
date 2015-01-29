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
            object retVal = new Object();
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