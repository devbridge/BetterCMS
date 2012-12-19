using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;

namespace BetterCms.Test.Module
{
    public abstract class SerializationTestBase : TestBase
    {
        public void RunSerializationAndDeserialization<TData>(TData obj, Action<TData> resultAssertions = null)
        {
            RunXmlSerializationAndDeserialization(obj, resultAssertions);
            RunBinarySerializationAndDeserialization(obj, resultAssertions);
        }

        public void RunXmlSerializationAndDeserialization<TData>(TData obj, Action<TData> resultAssertions = null)
        {
            Assert.IsNotNull(obj);
            var xml = SerializeToXml(obj);
            TData deserialized = DeserializeFromXml<TData>(xml);
            Assert.IsNotNull(deserialized);
            if (resultAssertions != null)
            {
                resultAssertions(deserialized);
            }
        }

        public void RunBinarySerializationAndDeserialization<TData>(TData obj, Action<TData> resultAssertions = null)
        {
            Assert.IsNotNull(obj);
            var bytes = SerializeToBinary(obj);
            TData deserialized = DeserializeFromBinary<TData>(bytes);
            Assert.IsNotNull(deserialized);
            if (resultAssertions != null)
            {
                resultAssertions(deserialized);
            }
        }
       
        protected virtual TData DeserializeFromXml<TData>(string xml)
        {
            byte[] b = Convert.FromBase64String(xml);
            using (var stream = new MemoryStream(b))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (TData)formatter.Deserialize(stream);
            }
        }

        protected virtual string SerializeToXml<TData>(TData obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                stream.Position = 0;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        protected virtual TData DeserializeFromBinaryAsBase64String<TData>(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return DeserializeFromBinary<TData>(bytes);
        }

        protected virtual string SerializeToBinaryAsBase64String<TData>(TData obj)
        {
            byte[] bytes = SerializeToBinary(obj);
            return Convert.ToBase64String(bytes);           
        }

        protected virtual TData DeserializeFromBinary<TData>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (TData)formatter.Deserialize(stream);
            }
        }

        protected virtual byte[] SerializeToBinary<TData>(TData obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }       
    }
}
