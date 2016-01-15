// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTestBase.cs" company="Devbridge Group LLC">
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
