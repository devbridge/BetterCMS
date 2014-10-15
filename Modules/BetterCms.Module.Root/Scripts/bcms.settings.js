/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.settings', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.grid', 'bcms.ko.extenders', 'bcms.autocomplete'],
    function($, bcms, dynamicContent, siteSettings, editor, grid, ko, autocomplete) {
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
                settingsSearchButton: '#bcms-settings-search-btn',
                settingsSearchField: '.bcms-search-query',
            },
            links = {
                loadSiteSettingsConfigurationSettingsUrl: null,
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
        * Retrieves setting field values from table row.
        */
        settings.getSettingData = function (row) {
            var settingId = row.find(selectors.deleteSettingLink).data('id'),
                settingVersion = row.find(selectors.deleteSettingLink).data('version'),
                name = row.find(selectors.settingNameEditor).val(),
                value = row.find(selectors.settingValueEditor).val(),
                moduleId = row.find(selectors.settingModuleIdEditor).val();

            return {
                Id: settingId,
                Version: settingVersion,
                Name: name,
                Value: value,
                ModuleId: moduleId
            };
        };

        /**
        * Search configuration settings
        */
        settings.searchSiteSettingsConfigurationSettings = function (form, container) {
            grid.submitGridForm(form, function (data) {
                siteSettings.setContent(data);
                settings.initSiteSettingsConfigurationSettingsEvents(data);
                var searchInput = container.find(selectors.settingsSearchField);
                grid.focusSearchInput(searchInput);
            });
        };

        /**
        * Initializes configuration settings list and list items events
        */
        settings.initSiteSettingsConfigurationSettingsEvents = function () {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container;

            var form = dialog.container.find(selectors.settingsListForm);
            grid.bindGridForm(form, function (data) {
                siteSettings.setContent(data);
                settings.initSiteSettingsConfigurationSettingsEvents(data);
            });

            form.on('submit', function (event) {
                event.preventDefault();
                tags.searchSiteSettingsConfigurationSettings(form, container);
                return false;
            });

            container.find(selectors.settingsSearchButton).on('click', function () {
                tags.searchSiteSettingsConfigurationSettings(form, container);
            });

            container.find(selectors.addSettingButton).on('click', function () {
                editor.addNewRow(container);
            });

            editor.initialize(container, {
                saveUrl: links.saveSettingUrl,
                deleteUrl: links.deleteSettingUrl,
                onSaveSuccess: tags.setSettingFields,
                rowDataExtractor: tags.getSettingData,
                deleteRowMessageExtractor: function (rowData) {
                    return $.format(globalization.confirmDeleteSettingMessage, rowData.Name);
                }
            });

            // Select search.
            dialog.setFocus();
        };

        /**
        * Set values, returned from server to row fields
        */
        settings.setSettingFields = function (row, json) {
            if (json.Data) {
                row.find(selectors.settingName).html(json.Data.Name);
                row.find(selectors.settingNameEditor).val(json.Data.Name);
                row.find(selectors.settingOldName).val(json.Data.Name);

                row.find(selectors.settingValue).html(json.Data.Value);
                row.find(selectors.settingValueEditor).val(json.Data.Value);
                row.find(selectors.settingOldValue).val(json.Data.Value);

                row.find(selectors.settingModuleId).val(json.Data.ModuleId);
            }
        };

        /**
        * Loads configuration settings list.
        */
        settings.loadSiteSettingsConfigurationSettingsList = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsConfigurationSettingsListUrl, {
                contentAvailable: function () {
                    settings.initSiteSettingsConfigurationSettingsEvents();
                }
            });
        };

        /**
        * Settings autocomplete list view model
        */
        var SettingsListViewModel = (function (_super) {
            bcms.extendsClass(SettingsListViewModel, _super);

            function SettingsListViewModel(settingsList) {
                var options = {
                    serviceUrl: links.settingSuggestionServiceUrl,
                    pattern: 'Settings[{0}]'
                };

                _super.call(this, settingsList, options);
            }

            return SettingsListViewModel;
        })(autocomplete.AutocompleteListViewModel);

        settings.SettingsListViewModel = SettingsListViewModel;


        /**
        * Initializes settings module.
        */
        settings.init = function () {
            bcms.logger.debug('Initializing bcms.settings module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(settings.init);

        return settings;
    });