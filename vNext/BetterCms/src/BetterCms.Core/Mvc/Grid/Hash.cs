/*
    Copyright 2010 CodePlex Foundation, Inc.
    Copyright 2007-2010 Eric Hexter, Jeffrey Palermo
    For Licence, see http://www.codeplex.com/MVCContrib/license

    Modified by Devbridge Group: updated source code to support ASP.NET 5 MVC6
*/

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Framework.WebEncoders;

namespace BetterCms.Core.Mvc.Grid
{
    /// <summary>
    /// Untyped, case insensitive dictionary that can be initialised using lambdas.
    /// </summary>
    /// <example>
    /// <![CDATA[
    ///	IDictionary dict = new Hash(id => "foo", @class => "bar");
    /// ]]>
    /// </example>
    public class Hash : Hash<object>
    {
        public Hash(params Func<object, object>[] hash) : base(hash)
        {
        }
    }

    /// <summary>Case insensitive, strongly typed dictionary with string keys and <typeparamref name="TValue"/> values that can be initialised using lambdas.</summary>
    /// <typeparam name="TValue">The type of values to create.</typeparam>
    /// <example lang="c#">
    /// <![CDATA[
    /// //IDictionary<string,string> dict = new Dictionary<string, string>(2, StringComparer.OrdinalIgnoreCase);
    /// //dict.Add("id", "foo");
    /// //dict.Add("class", "bar")
    /// IDictionary<string, string> dict = new Hash<string>(id => "foo", @class => "bar");
    /// ]]>
    /// </example>
    public class Hash<TValue> : Dictionary<string, TValue>
    {
        public Hash(params Func<object, TValue>[] hash)
            : base(hash == null ? 0 : hash.Length, StringComparer.OrdinalIgnoreCase)
        {
            if (hash != null)
            {
                foreach (var func in hash)
                {
                    Add(func.Method.GetParameters()[0].Name, func(null));
                }
            }
        }

        /// <summary>Creates an empty case insensitive dictionary of <see cref="string"/> keys and <typeparam name="TValue" /> values.</summary>
        public static Dictionary<string, TValue> Empty
        {
            get { return new Dictionary<string, TValue>(0, StringComparer.OrdinalIgnoreCase); }
        }
    }

    public static class DictionaryExtensions
    {
        /// <summary>Extension Method to initialize an <see cref="Dictionary{TKey,TValue}"/></summary>
        /// <param name="dict"></param>
        /// <param name="hash">The key / value pairs to add to the dictionary.</param>
        /// <returns>The the dictionary.</returns>
        public static IDictionary<string, T> Add<T>(this IDictionary<string, T> dict, params Func<object, T>[] hash)
        {
            if (dict == null || hash == null)
            {
                return dict;
            }

            foreach (var func in hash)
            {
                dict.Add(func.Method.GetParameters()[0].Name, func(null));
            }
            return dict;
        }

        /// <summary>Extension Method to initialize an <see cref="IDictionary"/></summary>
        /// <param name="dict"></param>
        /// <param name="hash">The key / value pairs to add to the dictionary.</param>
        /// <returns>The the dictionary.</returns>
        public static IDictionary Add(this IDictionary dict, params Func<object, object>[] hash)
        {
            if (dict == null || hash == null)
            {
                return dict;
            }

            foreach (var func in hash)
            {
                dict.Add(func.Method.GetParameters()[0].Name, func(null));
            }

            return dict;
        }

        /// <summary>
        /// Takes an anonymous object and converts it to a <see cref="Dictionary{String,Object}"/>
        /// </summary>
        /// <param name="objectToConvert">The object to convert</param>
        /// <returns>A generic dictionary</returns>
        public static Dictionary<string, object> AnonymousObjectToCaseSensitiveDictionary(object objectToConvert)
        {
            var dictionary = new Dictionary<string, object>(StringComparer.Ordinal);

            if (objectToConvert != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(objectToConvert))
                {
                    dictionary[property.Name] = property.GetValue(objectToConvert);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts a dictionary into a string of HTML attributes
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string ToHtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            const string attributeFormat = "{0}=\"{1}\"";

            var attributesEncoded = from pair in attributes
                                    let value = pair.Value == null ? null : HtmlEncoder.Default.HtmlEncode(pair.Value.ToString()) /*HttpUtility.HtmlAttributeEncode(pair.Value.ToString())*/
                                    select string.Format(attributeFormat, pair.Key, value);

            return string.Join(" ", attributesEncoded.ToArray());
        }
    }
}