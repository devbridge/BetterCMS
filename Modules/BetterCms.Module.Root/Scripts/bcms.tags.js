/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.tags.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.tags', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.grid', 'bcms.ko.extenders', 'bcms.autocomplete', 'bcms.antiXss'],
    function ($, bcms, dynamicContent, siteSettings, editor, grid, ko, autocomplete, antiXss) {
    'use strict';

    var tags = {},
        selectors = {
            deleteTagLink: 'div.bcms-action-delete',
            addTagButton: '#bcms-site-settings-add-tag',
            tagName: '.bcms-tag-name',
            tagOldName: 'input.bcms-tag-old-name',
            tagNameEditor: 'input.bcms-tag-name',
            tagsListForm: '#bcms-tags-form',
            tagsSearchButton: '#bcms-tags-search-btn',
            tagsSearchField: '.bcms-search-query',
            
            deleteCategoryLink: 'div.bcms-action-delete',
            addCategoryButton: '#bcms-site-settings-add-category',
            categoryName: '.bcms-category-name',
            categoryOldName: 'input.bcms-category-old-name',
            categoryNameEditor: 'input.bcms-category-name',
            categoriesListForm: '#bcms-categories-form',
            categoriesSearchButton: '#bcms-categories-search-btn',
            categoriesSearchField: '.bcms-search-query',
    },
        links = {
            loadSiteSettingsCategoryListUrl: null,
            loadSiteSettingsTagListUrl: null,
            saveTagUrl: null,
            deleteTagUrl: null,
            saveCategoryUrl: null,
            deleteCategoryUrl: null,
            tagSuggestionServiceUrl: null
        },
        globalization = {
            confirmDeleteTagMessage: 'Delete tag?',
            confirmDeleteCategoryMessage: 'Delete category?'
        };

    /**
    * Assign objects to module.
    */
    tags.links = links;
    tags.globalization = globalization;

    /**
    * Retrieves tag field values from table row.
    */
    tags.getTagData = function(row) {
        var tagId = row.find(selectors.deleteTagLink).data('id'),
            tagVersion = row.find(selectors.deleteTagLink).data('version'),
            name = row.find(selectors.tagNameEditor).val();

        return {
            Id: tagId,
            Version: tagVersion,
            Name: name
        };
    };

    /**
    * Search site settings tags
    */
    tags.searchSiteSettingsTags = function (form, container) {
        grid.submitGridForm(form, function (data) {
            siteSettings.setContent(data);
            tags.initSiteSettingsTagsEvents(data, true);   
            var searchInput = container.find(selectors.tagsSearchField);
            grid.focusSearchInput(searchInput);
        });
    };

    /**
    * Initializes site settings tags list and list items events
    */
    tags.initSiteSettingsTagsEvents = function (data, isSearchResult) {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;
        
        var form = dialog.container.find(selectors.tagsListForm);
        grid.bindGridForm(form, function (data) {
            siteSettings.setContent(data);
            tags.initSiteSettingsTagsEvents(data);
        });

        form.on('submit', function (event) {
            event.preventDefault();
            tags.searchSiteSettingsTags(form, container);
            return false;
        });

        container.find(selectors.tagsSearchButton).on('click', function () {
            var parent = $(this).parent();
            if (!parent.hasClass('bcms-active-search')) {
                form.find(selectors.tagsSearchField).prop('disabled', false);
                parent.addClass('bcms-active-search');
                form.find(selectors.tagsSearchField).focus();
            } else {
                form.find(selectors.tagsSearchField).prop('disabled', true);
                parent.removeClass('bcms-active-search');
                form.find(selectors.tagsSearchField).val('');
            }
        });

        container.find(selectors.addTagButton).on('click', function () {
            editor.addNewRow(container);
        });
        
        if (isSearchResult === true) {
            form.find(selectors.tagsSearchButton).parent().addClass('bcms-active-search');
        } else {
            form.find(selectors.tagsSearchField).prop('disabled', true);
        }

        editor.initialize(container, {
            saveUrl: links.saveTagUrl,
            deleteUrl: links.deleteTagUrl,
            onSaveSuccess: tags.setTagFields,
            rowDataExtractor: tags.getTagData,
            deleteRowMessageExtractor: function (rowData) {
                return $.format(globalization.confirmDeleteTagMessage, rowData.Name);
            }
        });

        // Select search.
        dialog.setFocus();
    };

    /**
    * Set values, returned from server to row fields
    */
    tags.setTagFields = function (row, json) {
        if (json.Data) {
            row.find(selectors.tagName).html(antiXss.encodeHtml(json.Data.Name));
            row.find(selectors.tagNameEditor).val(json.Data.Name);
            row.find(selectors.tagOldName).val(json.Data.Name);
        }
    };

    /**
    * Retrieves category field values from table row.
    */
    tags.getCategoryData = function (row) {
        var categoryId = row.find(selectors.deleteCategoryLink).data('id'),
            categoryVersion = row.find(selectors.deleteCategoryLink).data('version'),
            name = row.find(selectors.categoryNameEditor).val();

        return {
            Id: categoryId,
            Version: categoryVersion,
            Name: name
        };
    };

    /**
    * Search site settings categories
    */
    tags.searchSiteSettingsCategories = function (form, container) {
        grid.submitGridForm(form, function (data) {
            siteSettings.setContent(data);
            tags.initSiteSettingsCategoriesEvents(data);                  
            var searchInput = container.find(selectors.categoriesSearchField);
            grid.focusSearchInput(searchInput);
        });
    };

    /**
    * Initializes site settings categories list and list items events
    */
    tags.initSiteSettingsCategoriesEvents = function () {
        var dialog = siteSettings.getModalDialog(),
            container = dialog.container;

        var form = dialog.container.find(selectors.categoriesListForm);
        grid.bindGridForm(form, function (data) {
            siteSettings.setContent(data);
            tags.initSiteSettingsCategoriesEvents(data);
        });

        form.on('submit', function (event) {
            event.preventDefault();
            tags.searchSiteSettingsCategories(form, container);
            return false;
        });

        container.find(selectors.categoriesSearchButton).on('click', function () {
            tags.searchSiteSettingsCategories(form, container);
        });

        container.find(selectors.addCategoryButton).on('click', function () {
            editor.addNewRow(container);
        });

        editor.initialize(container, {
            saveUrl: links.saveCategoryUrl,
            deleteUrl: links.deleteCategoryUrl,
            onSaveSuccess: tags.setCategoryFields,
            rowDataExtractor: tags.getCategoryData,
            deleteRowMessageExtractor: function (rowData) {
                return $.format(globalization.confirmDeleteCategoryMessage, rowData.Name);
            }
        });
        
        // Select search.
        dialog.setFocus();
    };

    /**
    * Set values, returned from server to row fields
    */
    tags.setCategoryFields = function (row, json) {
        if (json.Data) {
            row.find(selectors.categoryName).html(antiXss.encodeHtml(json.Data.Name));
            row.find(selectors.categoryNameEditor).val(json.Data.Name);
            row.find(selectors.categoryOldName).val(json.Data.Name);
        }
    };
    
    /**
      * Loads site settings category list.
      */
    tags.loadSiteSettingsCategoryList = function () {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsCategoryListUrl, {
            contentAvailable: function () {
                tags.initSiteSettingsCategoriesEvents();
            }
        });
    };

    /**
      * Loads site settings tag list.
      */
    tags.loadSiteSettingsTagList = function () {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsTagListUrl, {
            contentAvailable: function () {
                tags.initSiteSettingsTagsEvents();
            }
        });
    };

    /**
    * Tags autocomplete list view model
    */
    var TagsListViewModel = (function (_super) {
        bcms.extendsClass(TagsListViewModel, _super);

        function TagsListViewModel(tagsList) {
            var options = {
                serviceUrl: links.tagSuggestionServiceUrl,
                pattern: 'Tags[{0}]'
            };

            _super.call(this, tagsList, options);
        }

        return TagsListViewModel;
    })(autocomplete.AutocompleteListViewModel);

    tags.TagsListViewModel = TagsListViewModel;

    /**
    * Initializes tags module.
    */
    tags.init = function () {
        bcms.logger.debug('Initializing bcms.tags module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(tags.init);
    
    return tags;
});
