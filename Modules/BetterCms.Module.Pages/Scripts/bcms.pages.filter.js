/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.pages.filter.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.pages.filter', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.tags', 'bcms.categories'],
    function ($, bcms, ko, tags, categories) {
        'use strict';

        var filter = {},
            selectors = {
                filterTemplate: '#bcms-filter-template',
                filterByLanguage: '#bcms-js-filter-languages',
                filterByStatus: '#bcms-js-filter-status',
                filterBySeoStatus: '#bcms-js-filter-seostatus',
                filterByLayout: '#bcms-js-filter-layout',
                filterSort: '#bcms-js-filter-sort',
                hiddenSortColumnField: '#bcms-grid-sort-column',
                hiddenSortDirectionField: '#bcms-grid-sort-direction',
                hiddenPageNumberField: '#bcms-grid-page-number',
                siteSettingsPagesListForm: '#bcms-pages-form'
            },
            links = {},
            globalization = {};

        /**
        * Assign objects to module.
        */
        filter.links = links;
        filter.globalization = globalization;

        function SortAliasModel(title, column, direction) {
            var self = this;
            self.title = title;
            self.column = column;
            self.direction = direction;
            return self;
        };

        function FilterViewModel(tagsViewModel, categoriesViewModel, container, onSearchClick, jsonData) {
            var self = this;

            self.isVisible = ko.observable(false);
            self.tags = tagsViewModel;
            self.categories = categoriesViewModel;
            self.includeArchived = ko.observable(false);
            self.includeMasterPages = ko.observable(false);
            self.languageId = ko.observable(jsonData.LanguageId);
            self.languages = jsonData.Languages || [];
            self.status = ko.observable(jsonData.Status);
            self.statuses = jsonData.Statuses || [];
            self.seoStatus = ko.observable(jsonData.SeoStatus);
            self.seoStatuses = jsonData.SeoStatuses || [];
            self.layout = ko.observable(jsonData.Layout);
            self.layouts = jsonData.Layouts || [];

            self.languages.unshift({ Key: '', Value: '' });
            self.statuses.unshift({ Key: '', Value: '' });
            self.seoStatuses.unshift({ Key: '', Value: '' });
            self.layouts.unshift({ Key: '', Value: '' });

            self.showSorting = ko.observable(false);
            self.sortColumn = ko.observable(jsonData.GridOptions.Column);
            self.sortDirection = ko.observable(jsonData.GridOptions.Direction);
            self.sortFields = ko.observableArray([]);
            self.suspendSortingSelector = false;

            if (jsonData.SortAliases) {
                jsonData.SortAliases.forEach(function(sortAlias) {
                    self.sortFields.push(new SortAliasModel(sortAlias.Title, sortAlias.Column, sortAlias.Direction));
                });
            }
            
            self.isEdited = ko.computed(function () {
                if (self.includeArchived()) {
                    return true;
                }
                if (self.includeMasterPages()) {
                    return true;
                }
                if (self.tags != null && self.tags.items() != null && self.tags.items().length > 0) {
                    return true;
                }
                if (self.categories != null && self.categories.items() != null && self.categories.items().length > 0) {
                    return true;
                }
                if (self.languageId() || self.seoStatus() || self.status() || self.layout()) {
                    return true;
                }
                return false;
            });
            self.form = container.find(selectors.siteSettingsPagesListForm);
            // Actions.
            self.toggleShowSorting = function() {
                self.showSorting(!self.showSorting());
                self.suspendSortingSelector = true;
                setTimeout(function() {
                    self.suspendSortingSelector = false;
                }, 100);
            };
            self.applySort = function(data, column, direction) {
                self.sortColumn(column);
                self.sortDirection(direction);
                self.showSorting(false);
                self.form.find(selectors.hiddenSortColumnField).val(column);
                self.form.find(selectors.hiddenSortDirectionField).val(direction);
                onSearchClick(true);
            };
            self.toggleFilter = function() {
                self.isVisible(!self.isVisible());
            };
            self.closeFilter = function () {
                self.isVisible(false);
            };
            self.searchWithFilter = function () {
                if ($.isFunction(onSearchClick)) {
                    onSearchClick();
                }
            };
            self.clearFilter = function () {
                self.tags.items([]);
                self.categories.items([]);
                self.includeArchived(false);
                self.includeMasterPages(false);
                self.languageId('');
                self.status('');
                self.seoStatus('');
                self.layout('');

                self.searchWithFilter();
            };
            self.changeIncludeArchived = function () {
                self.includeArchived(!(self.includeArchived()));
            };
            self.changeIncludeMasterPages = function () {
                self.includeMasterPages(!(self.includeMasterPages()));
            };
            bcms.on(bcms.events.bodyClick, function (event) {
                if (!self.suspendSortingSelector) {
                    if (self.showSorting() == true) {
                        self.showSorting(false);
                    }
                }
            });
        }

        filter.bind = function (container, jsonData, onSearchClick) {
            var tagsViewModel = new tags.TagsListViewModel(jsonData.Tags),
                categoriesViewModel = new categories.CategoriesSelectListModel(jsonData.Categories),
            filterViewModel = new FilterViewModel(tagsViewModel, categoriesViewModel, container, onSearchClick, jsonData);
            categories.initCategoriesSelect(categoriesViewModel, jsonData.CategoriesLookupList);
            filterViewModel.includeArchived(jsonData.IncludeArchived ? true : false);
            filterViewModel.includeMasterPages(jsonData.IncludeMasterPages ? true : false);
            ko.applyBindings(filterViewModel, container.find(selectors.filterTemplate).get(0));
        };

        /**
        * Initializes page module.
        */
        filter.init = function() {
            bcms.logger.debug('Initializing bcms.pages.filter module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(filter.init);

        return filter;
    });
