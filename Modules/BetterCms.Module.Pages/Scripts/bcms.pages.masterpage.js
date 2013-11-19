/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.masterpage', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.pages', 'bcms.grid'],
    function ($, bcms, siteSettings, page, grid) {
        'use strict';

        var module = {},
            links = {
                loadMasterPagesListUrl: null,
            },
            globalization = {
                masterPagesTabTitle: null
            },
            selectors = {
                searchField: '.bcms-search-query',
                searchButton: '#bcms-pages-search-btn',

                siteSettingsMasterPagesForm: "#bcms-master-pages-form",
                siteSettingsMasterPageCreateButton: '#bcms-create-page-button',
                siteSettingsRowCells: 'td',
                siteSettingsPageParentRow: 'tr:first',
                siteSettingPageTitleCell: '.bcms-page-title',
                siteSettingsPageEditButton: '.bcms-grid-item-edit-button',
                siteSettingsPageDeleteButton: '.bcms-grid-item-delete-button',
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
            
            form.find(selectors.searchField).keypress(function (event) {
                if (event.which == 13) {
                    bcms.stopEventPropagation(event);
                    searchMasterPages(form, container);
                }
            });

            form.find(selectors.searchButton).on('click', function () {
                searchMasterPages(form, container);
            });

            initializeListItems(container);
            
            // Select search.
            dialog.setFocus();
        };

        function searchMasterPages(form, container) {
            grid.submitGridForm(form, function (htmlContent, data) {
                container.html(htmlContent);
                module.initializeMasterPagesList(container);
                var searchInput = container.find(selectors.searchField);
                grid.focusSearchInput(searchInput);
            });
        };

        function initializeListItems(container) {
            container.find(selectors.siteSettingsMasterPageCreateButton).on('click', function () {
                page.addSiteSettingsPage(container, true);
            });

            container.find(selectors.siteSettingsRowCells).on('click', function () {
                var editButton = $(this).parents(selectors.siteSettingsPageParentRow).find(selectors.siteSettingsPageEditButton);
                if (editButton.length > 0) {
                    page.editSiteSettingsPage(editButton, container);
                }
            });

            container.find(selectors.siteSettingPageTitleCell).on('click', function (event) {
                bcms.stopEventPropagation(event);
                var url = $(this).data('url');
                window.open(url);
            });

            container.find(selectors.siteSettingsPageDeleteButton).on('click', function (event) {
                bcms.stopEventPropagation(event);
                page.deleteSiteSettingsPage($(this), container);
            });
        };
        
        return module;
    });
