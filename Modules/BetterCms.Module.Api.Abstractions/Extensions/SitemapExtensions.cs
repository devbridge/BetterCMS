// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;

namespace BetterCms.Module.Api.Extensions
{
    public static class SitemapExtensions
    {
        public static PutSitemapRequest ToPutRequest(this GetSitemapResponse response)
        {
            var model = MapPageModel(response, false);

            return new PutSitemapRequest { Data = model, Id = response.Data.Id };
        }

        public static PostSitemapRequest ToPostRequest(this GetSitemapResponse response)
        {
            var model = MapPageModel(response, true);

            return new PostSitemapRequest { Data = model };
        }

        public static PutNodeRequest ToPutRequest(this GetNodeResponse response)
        {
            var model = MapSitemapNodeModel(response, false);

            return new PutNodeRequest { Data = model, SitemapId = response.Data.SitemapId, Id = response.Data.Id };
        }

        public static PostSitemapNodeRequest ToPostRequest(this GetNodeResponse response)
        {
            var model = MapSitemapNodeModel(response, true);

            return new PostSitemapNodeRequest { Data = model };
        }

        private static SaveSitemapModel MapPageModel(GetSitemapResponse response, bool resetIds)
        {
            var model = new SaveSitemapModel
            {
                Version = response.Data.Version,
                Title = response.Data.Title,
                Tags = response.Data.Tags,
                AccessRules = response.AccessRules,
            };

            if (response.Nodes != null)
            {
                model.Nodes = GetSubNodes(response.Nodes.Where(n => n.ParentId == null).Select(n => ToModel(n, resetIds)).ToList(), response.Nodes, resetIds);
            }

            return model;
        }

        private static SaveSitemapNodeModel ToModel(SitemapNodeWithTranslationsModel n, bool resetIds)
        {
            var model = new SaveSitemapNodeModel
                            {
                                Id = resetIds ? default(Guid) : n.Id,
                                Version = n.Version,
                                PageId = n.PageId,
                                UsePageTitleAsNodeTitle = n.UsePageTitleAsNodeTitle,
                                Title = n.NodeTitle,
                                Url = n.NodeUrl,
                                DisplayOrder = n.DisplayOrder,
                                Macro = n.Macro
                            };

            if (n.Translations != null)
            {
                model.Translations =
                    n.Translations.Select(
                        t =>
                        new SaveSitemapNodeTranslation
                            {
                                Id = t.Id,
                                Version = t.Version,
                                Title = t.Title,
                                UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
                                Url = t.Url,
                                Macro = t.Macro,
                                LanguageId = t.LanguageId
                            }).ToList();
            }

            return model;
        }

        private static IList<SaveSitemapNodeModel> GetSubNodes(IList<SaveSitemapNodeModel> nodes, IList<SitemapNodeWithTranslationsModel> allNodes, bool resetIds)
        {
            foreach (var node in nodes)
            {
                node.Nodes = GetSubNodes(allNodes.Where(n => n.ParentId == node.Id).Select(n => ToModel(n, resetIds)).ToList(), allNodes, resetIds);
            }

            return nodes;
        }

        private static SaveNodeModel MapSitemapNodeModel(GetNodeResponse response, bool resetIds)
        {
            var model = new SaveNodeModel
            {
                Id = resetIds ? default(Guid) : response.Data.Id,
                Version = response.Data.Version,
                ParentId = response.Data.ParentId,
                PageId = response.Data.PageId,
                UsePageTitleAsNodeTitle = response.Data.UsePageTitleAsNodeTitle,
                Title = response.Data.NodeTitle,
                Url = response.Data.NodeUrl,
                DisplayOrder = response.Data.DisplayOrder,
                Macro = response.Data.Macro,
            };

            if (response.Translations != null)
            {
                model.Translations =
                    response.Translations.Select(
                        t =>
                        new SaveNodeTranslation
                            {
                                Id = t.Id,
                                Version = t.Version,
                                Title = t.Title,
                                UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
                                Url = t.Url,
                                Macro = t.Macro,
                                LanguageId = t.LanguageId
                            }).ToList();
            }

            return model;
        }
    }
}
