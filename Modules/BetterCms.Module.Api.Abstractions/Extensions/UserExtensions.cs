// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;

namespace BetterCms.Module.Api.Extensions
{
    public static class UserExtensions
    {
        public static PostUserRequest ToPostRequest(this GetUserResponse response)
        {
            var model = MapModel(response);

            return new PostUserRequest { Data = model };
        }

        public static PutUserRequest ToPutRequest(this GetUserResponse response)
        {
            var model = MapModel(response);

            return new PutUserRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveUserModel MapModel(GetUserResponse response)
        {
            var model = new SaveUserModel
                        {
                            Version = response.Data.Version,
                            FirstName = response.Data.FirstName,
                            LastName = response.Data.LastName,
                            UserName = response.Data.UserName,
                            Email = response.Data.Email,
                            ImageId = response.Data.ImageId
                        };
            if (response.Roles != null)
            {
                model.Roles = response.Roles.Select(r => r.Name).ToList();
            }

            return model;
        }
    }
}
