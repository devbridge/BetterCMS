/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.languages.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.languages', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.autocomplete', 'bcms.antiXss'],
function ($, bcms, dynamicContent, siteSettings, ko, kogrid, autocomplete, antiXss) {
    'use strict';

    var languages = {},
        selectors = {},
        links = {
            saveLanguageUrl: null,
            deleteLanguageUrl: null,
            loadLanguagesUrl: null,
            loadSiteSettingsLanguagesUrl: null,
            languageSuggestionUrl: null
        },
        globalization = {
            deleteLanguageConfirmMessage: null
        },
        rowId = 0;

    /**
    * Assign objects to module.
    */
    languages.links = links;
    languages.globalization = globalization;

    /**
    * Language autocomplete list view model
    */
    var LanguageAutocompleteListViewModel = (function (_super) {
        bcms.extendsClass(LanguageAutocompleteListViewModel, _super);

        function LanguageAutocompleteListViewModel(onItemSelect, onBeforeSearchStart) {
            var options = {
                serviceUrl: links.languageSuggestionUrl,
                onItemSelect: onItemSelect
            };

            _super.call(this, options);

            this.onBeforeSearchStartCallback = onBeforeSearchStart;
        }

        LanguageAutocompleteListViewModel.prototype.onBeforeSearchStart = function () {
            if ($.isFunction(this.onBeforeSearchStartCallback)) {
                this.onBeforeSearchStartCallback();
            }
        };
            
        return LanguageAutocompleteListViewModel;
    })(autocomplete.AutocompleteViewModel);

    /**
    * Languages list view model
    */
    var LanguagesListViewModel = (function(_super) {

        bcms.extendsClass(LanguagesListViewModel, _super);

        function LanguagesListViewModel(container, items, gridOptions) {
            _super.call(this, container, links.loadLanguagesUrl, items, gridOptions);
        }

        LanguagesListViewModel.prototype.createItem = function (item) {
            var newItem = new LanguageViewModel(this, item);
            return newItem;
        };

        return LanguagesListViewModel;

    })(kogrid.ListViewModel);

    /**
    * Language view model
    */
    var LanguageViewModel = (function(_super) {

        bcms.extendsClass(LanguageViewModel, _super);

        function LanguageViewModel(parent, item) {
            _super.call(this, parent, item);

            var self = this,
                onSelectItem = function (suggestionItem) {
                    self.isSelected = true;

                    var name = self.name(),
                        suggestedName = suggestionItem.name();

                    if (!name || self.oldAutocompleteValue == self.name()) {
                        self.name(suggestedName);
                    }

                    self.oldAutocompleteValue = suggestedName;
                    self.shortCode(suggestionItem.id());
                    self.code(suggestedName);
                    self.hasNameFocus(true);
                }, onBeforeSearchStart = function () {
                    
                    if (this.autocompleteInstance
                        && this.autocompleteInstance.options) {
                        this.autocompleteInstance.options.tabDisabled = true;
                    }

                    self.shortCode('');
                };

            self.hasNameFocus = ko.observable(false);
            self.name = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name }, preventHtml: "" });
            self.code = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name }, preventHtml: "" });
            self.shortCode = ko.observable();
            self.oldAutocompleteValue = '';

            self.registerFields(self.name, self.code);

            self.name(item.Name);
            self.code(item.Code);

            self.autocompleteViewModel = new LanguageAutocompleteListViewModel(onSelectItem, onBeforeSearchStart);

            if (!self.isNew()) {
                self.code.editingIsDisabled = ko.observable(true);
                self.hasNameFocus(true);
            }
        }

        LanguageViewModel.prototype.getDeleteConfirmationMessage = function () {
            return $.format(globalization.deleteLanguageConfirmMessage, antiXss.encodeHtml(this.name()));
        };

        LanguageViewModel.prototype.onAfterItemSaved = function (json) {
            _super.prototype.onAfterItemSaved.call(this, json);
            
            if (json.Success === true && !this.code.editingIsDisabled) {
                this.code.editingIsDisabled = ko.observable(true);
                this.hasNameFocus(true);
                this.code(json.Data.Code);
            }
        };

        LanguageViewModel.prototype.getSaveParams = function () {
            var params = _super.prototype.getSaveParams.call(this);
            params.Name = this.name();
            params.Code = this.shortCode() || this.code();

            return params;
        };

        LanguageViewModel.prototype.onSave = function (data, event) {
            if (event.type
                && event.type == "keydown"
                && this.autocompleteViewModel
                && this.autocompleteViewModel.autocompleteInstance
                && this.autocompleteViewModel.autocompleteInstance.visible) {
                // If autocomplete is visible, cancel enter press event
                return;
            }

            _super.prototype.onSave.call(this, data, event);
        };

        LanguageViewModel.prototype.onCancelEdit = function (data, event) {
            if (event.type
                && event.type == "keydown"
                && this.autocompleteViewModel
                && this.autocompleteViewModel.autocompleteInstance
                && this.autocompleteViewModel.autocompleteInstance.visible) {
                // If autocomplete is visible, cancel esc press event
                return;
            }

            _super.prototype.onCancelEdit.call(this, data, event);
        };

        LanguageViewModel.prototype.getRowId = function () {
            if (!this.rowId) {
                this.rowId = 'bcms-lang-row-' + rowId++;
            }
            return this.rowId;
        };

        return LanguageViewModel;

    })(kogrid.ItemViewModel);

    /**
    * Initializes site settings languages grid
    */
    function initializeSiteSettingsLanguages(json) {
        var container = siteSettings.getMainContainer(),
            data = (json.Success == true) ? json.Data : {};

        var viewModel = new LanguagesListViewModel(container, data.Items, data.GridOptions);
        viewModel.deleteUrl = links.deleteLanguageUrl;
        viewModel.saveUrl = links.saveLanguageUrl;

        ko.cleanNode(container.get(0));
        ko.applyBindings(viewModel, container.get(0));
    }

    /**
    * Loads site settings languages list.
    */
    languages.loadSiteSettingsLanguagesList = function() {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsLanguagesUrl, {
            contentAvailable: initializeSiteSettingsLanguages
        });
    };

    /**
    * Initializes languages module.
    */
    languages.init = function() {
        bcms.logger.debug('Initializing bcms.languages module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(languages.init);

    return languages;
});
