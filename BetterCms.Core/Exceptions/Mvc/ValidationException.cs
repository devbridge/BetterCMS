// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationException.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Core.Exceptions.Mvc
{
    /// <summary>
    /// Validation exception with attached resource function.
    /// </summary>
    [Serializable]
    public class ValidationException : CmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        public ValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        /// <param name="resource">A function to retrieve a globalized resource associated with this exception.</param>
        /// <param name="message">The exception message.</param>
        public ValidationException(Func<string> resource, string message)
            : base(message)
        {
            Resource = resource;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        /// <param name="resource">A function to retrieve a globalized resource associated with this exception.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(Func<string> resource, string message, Exception innerException)
            : base(message, innerException)
        {
            Resource = resource;
        }

        /// <summary>
        /// Gets a function to retrieve a globalized resource associated with this exception.
        /// </summary>
        /// <value>
        /// A function to retrieve a globalized resource associated with this exception.
        /// </value>
        public Func<string> Resource { get; private set; }
    }
}
