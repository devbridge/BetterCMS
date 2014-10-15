/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.setting', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.messages', 'bcms.ko.extenders', 'bcms.grid', 'bcms.security', 'bcms.tags'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, messages, ko, grid, security, tags) {
        'use strict';

        var settings = {},
            selectors = {
                deleteSettingLink: 'a.bcms-setting-delete',
                addSettingButton: '#bcms-site-settings-add-setting',

                settingName: 'a.bcms-setting-name',
                settingOldName: 'input.bcms-setting-old-name',
                settingNameEditor: 'input.bcms-setting-name',

                settingValue: 'a.bcms-setting-value',
                settingOldValue: 'input.bcms-setting-old-value',
                settingValueEditor: 'input.bcms-setting-value',

                settingModuleId: 'a.bcms-setting-moduleId',

                settingsListForm: '#bcms-settings-form',
                settingsSearchButton: '#bcms-setting-search-btn',
                settingsSearchField: '.bcms-search-query',

                settingRowTemplate: '#bcms-setting-list-row-template',
                settingRowTemplateFirstRow: 'tr:first',
                settingsTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                settingsGrid: '#bcms-settings-grid'
            },
            links = {
                loadConfigurationSettingsListUrl: null,
                saveSettingUrl: null,
                deleteSettingUrl: null,
                settingSuggestionServiceUrl: null
            },
            globalization = {
                confirmDeleteSettingMessage: 'Delete setting?'
            };

        /**
        * Assign objects to module.
        */
        settings.links = links;
        settings.globalization = globalization;

        /**
        * Opens configuration settings list dialog
        */
        settings.loadConfigurationSettingsList = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadConfigurationSettingsListUrl, {
                contentAvailable: initializeConfigurationSettingsList
            });
        };

        /**
        * Initializes configuration settings list and list items
        */
        function initializeConfigurationSettingsList() {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container;

            var form = dialog.container.find(selectors.settingsListForm);
            grid.bindGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeConfigurationSettingsList(data);
            });

            form.on('submit', function (event) {
                event.preventDefault();
                searchSiteSettingsSettings(form);
                return false;
            });

            form.find(selectors.settingsSearchButton).on('click', function () {
                searchSiteSettingsSettings(form);
            });

            initializeConfigurationSettingsListEvents(container);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(dialog.container.find(selectors.settingsSearchField), true);
        };

        /**
        * Initializes site settings widgets list items.
        */
        function initializeConfigurationSettingsListEvents(container) {
            
        };

        /**
        * Search site settings widgets.
        */
        function searchSiteSettingsSettings(form) {
            grid.submitGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeConfigurationSettingsList();
            });
        };

        /**
        * Initializes settings module.
        */
        settings.init = function () {
            bcms.logger.debug('Initializing bcms.pages.setting module.');
        };

        return settings;
    });