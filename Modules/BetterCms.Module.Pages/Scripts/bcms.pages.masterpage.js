/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.pages.masterpage.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.pages.masterpage', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.pages', 'bcms.grid', 'bcms.pages.properties', 'bcms.messages', 'bcms.antiXss'],
    function ($, bcms, siteSettings, page, grid, pageProperties, messages, antiXss) {
        'use strict';

        var module = {},
            links = {
                loadMasterPagesListUrl: null,
            },
            globalization = {
                masterPagesTabTitle: null,
                editMasterPagePropertiesModalTitle: null
            },
            selectors = {
                searchField: '.bcms-search-query',
                searchButton: '#bcms-pages-search-btn',

                siteSettingsMasterPagesForm: "#bcms-master-pages-form",
                siteSettingsMasterPageCreateButton: '#bcms-create-page-button',
                siteSettingsMasterPageCreateButtonSidePanel: '#bcms-create-page-button-side-panel',
                siteSettingsPageParentRow: 'tr:first',
                siteSettingsPageTitleCell: '.bcms-page-title',
                siteSettingsPageEditButton: '.bcms-grid-item-edit-button',
                siteSettingsPageDeleteButton: '.bcms-grid-item-delete-button',
                siteSettingsPageUsageLink: '.bcms-action-usage',
                siteSettingsPagesListFormFilterIncludeArchived: "#IncludeArchived",
                
                siteSettingsPageRowTemplate: '#bcms-pages-list-row-template',
                siteSettingsPageRowTemplateFirstRow: 'tr:first',
                siteSettingsPagesTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                siteSettingsRowCells: 'td',
            };

        /**
        * Assign objects to module.
        */
        module.links = links;
        module.globalization = globalization;

        /**
        * Initializes site settings master pages list.
        */
        module.initializeMasterPagesList = function (container) {
            var dialog = siteSettings.getModalDialog(),
                form = dialog.container.find(selectors.siteSettingsMasterPagesForm);
            
            grid.bindGridForm(form, function (htmlContent, data) {
                container.html(htmlContent);
                module.initializeMasterPagesList(container);
            });
            
            form.on('submit', function (event) {
                event.preventDefault();
                searchMasterPages(form, container);
                return false;
            });
            
            bcms.preventInputFromSubmittingForm(form.find(selectors.searchField), {
                preventedEnter: function () {
                    searchMasterPages(form, container);
                },
            });

            form.find(selectors.searchButton).on('click', function () {
                searchMasterPages(form, container);
            });

            initializeListItems(container);
            
            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(container.find(selectors.searchField), true);
        };

        function searchMasterPages(form, container) {
            grid.submitGridForm(form, function (htmlContent, data) {
                container.html(htmlContent);
                module.initializeMasterPagesList(container);
            });
        };

        function initializeListItems(container) {
            container.find(selectors.siteSettingsMasterPageCreateButton).on('click', function () {
                addMasterPage(container);
            });

            container.find(selectors.siteSettingsMasterPageCreateButtonSidePanel).on('click', function () {
                addMasterPage(container);
            });

            container.find(selectors.siteSettingsRowCells).on('click', function () {
                var editButton = $(this).parents(selectors.siteSettingsPageParentRow).find(selectors.siteSettingsPageEditButton);
                if (editButton.length > 0) {
                    editMasterPage(editButton, container);
                }
            });

            container.find(selectors.siteSettingsPageTitleCell).on('click', function (event) {
                bcms.stopEventPropagation(event);
                var url = $(this).data('url');
                window.open(url);
            });

            container.find(selectors.siteSettingsPageDeleteButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                deleteMasterPage($(this), container);
            });

            container.find(selectors.siteSettingsPageUsageLink).on('click', function (event) {
                bcms.stopEventPropagation(event);
                filterPagesByMasterPage($(this), container);
            });
        };
        
        function addMasterPage(container) {
            page.openCreatePageDialog(function (data) {
                if (data.Data != null) {
                    var template = $(selectors.siteSettingsPageRowTemplate),
                        newRow = $(template.html()).find(selectors.siteSettingsPageRowTemplateFirstRow);

                    newRow.find(selectors.siteSettingsPageTitleCell).html(antiXss.encodeHtml(data.Data.Title));
                    if (container.hasClass('js-redirect-to-new-page') && data.Data.IsMasterPage) {
                        window.location.href = data.Data.PageUrl;
                    }
                    
                    newRow.find(selectors.siteSettingsPageTitleCell).data('url', data.Data.PageUrl);
                    newRow.find(selectors.siteSettingsPageEditButton).data('id', data.Data.PageId);
                    newRow.find(selectors.siteSettingsPageDeleteButton).data('id', data.Data.PageId);
                    newRow.find(selectors.siteSettingsPageDeleteButton).data('version', data.Data.Version);

                    newRow.insertBefore($(selectors.siteSettingsPagesTableFirstRow, container));

                    initializeListItems(newRow);

                    grid.showHideEmptyRow(container);
                }
            }, true);
        };
        
        function editMasterPage(self, container) {
            var id = self.data('id');

            pageProperties.openEditPageDialog(id, function (data) {
                if (data.Data != null) {
                    if (data.Data.IsArchived) {
                        var form = container.find(selectors.siteSettingsMasterPagesForm),
                            includeArchivedField = form.find(selectors.siteSettingsPagesListFormFilterIncludeArchived);
                        if (!includeArchivedField.is(":checked")) {
                            self.parents(selectors.siteSettingsPageParentRow).remove();
                            grid.showHideEmptyRow(container);
                            return;
                        }
                    }

                    var row = self.parents(selectors.siteSettingsPageParentRow),
                        cell = row.find(selectors.siteSettingsPageTitleCell);
                    cell.html(antiXss.encodeHtml(data.Data.Title));
                    cell.data('url', data.Data.PageUrl);
                }
            }, globalization.editMasterPagePropertiesModalTitle);
        };

        function filterPagesByMasterPage(self) {
            var id = self.data('id');

            page.openPageSelectDialog({
                params: {
                    Layout: 'm-' + id
                },
                canBeSelected: false,
                title: page.globalization.pagesListTitle,
                disableAccept: true
            });
        }

        function deleteMasterPage(self, container) {
            var id = self.data('id');

            page.deletePage(id, function (json) {
                self.parents(selectors.siteSettingsPageParentRow).remove();
                messages.refreshBox(selectors.siteSettingsMasterPagesForm, json);
                grid.showHideEmptyRow(container);
            });
        };

        module.addMasterPage = addMasterPage;

        return module;
    });
