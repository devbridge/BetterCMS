/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.masterpage', ['bcms.jquery', 'bcms', 'bcms.siteSettings', 'bcms.pages'],
    function ($, bcms, siteSettings, page) {
        'use strict';

        var module = {},
            links = {
                loadMasterPagesListUrl: null,
            },
            globalization = {
                masterPagesTabTitle: null
            },
            selectors = {
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
        module.initializeMasterPagesList = function (container, json) {
            var html = json.Html,
                data = (json.Success == true) ? json.Data : null,
                dialog = siteSettings.getModalDialog();
            container.html(html);

            initializeListItems(container);
            
            // Select search.
            dialog.setFocus();
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
