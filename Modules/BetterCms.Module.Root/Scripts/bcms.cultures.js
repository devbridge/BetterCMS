/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.cultures', ['bcms.jquery', 'bcms', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.autocomplete'],
function ($, bcms, dynamicContent, siteSettings, ko, kogrid, autocomplete) {
    'use strict';

    var cultures = {},
        selectors = {},
        links = {
            saveCultureUrl: null,
            deleteCultureUrl: null,
            loadCulturesUrl: null,
            loadSiteSettingsCulturesUrl: null,
            cultureSuggestionServiceUrl: null
        },
        globalization = {
            deleteCultureConfirmMessage: null
        };

    /**
    * Assign objects to module.
    */
    cultures.links = links;
    cultures.globalization = globalization;

    /**
    * Culture autocomplete list view model
    */
    var CultureAutocompleteListViewModel = (function (_super) {
        bcms.extendsClass(CultureAutocompleteListViewModel, _super);

        function CultureAutocompleteListViewModel(onItemSelect) {
            var options = {
                serviceUrl: links.cultureSuggestionServiceUrl,
                onItemSelect: onItemSelect
            };

            _super.call(this, options);
        }
            
        return CultureAutocompleteListViewModel;
    })(autocomplete.AutocompleteViewModel);

    /**
    * Cultures list view model
    */
    var CulturesListViewModel = (function(_super) {

        bcms.extendsClass(CulturesListViewModel, _super);

        function CulturesListViewModel(container, items, gridOptions) {
            _super.call(this, container, links.loadCulturesUrl, items, gridOptions);

            var self = this;

            self.newCulture = null;
            self.isAddModeActive = ko.observable(false);

            self.addCulture = function() {
                self.isAddModeActive(true);
            };

            self.closeAddCulture = function() {
                self.isAddModeActive(false);
            };

            self.autocompleteViewModel = new CultureAutocompleteListViewModel(function (suggestionItem) {
                self.newCulture = suggestionItem;
                self.addNewItem();
                self.closeAddCulture();
            });
        }

        CulturesListViewModel.prototype.onAfterNewItemAdded = function (newItem) {
            newItem.code(this.newCulture.id());
            newItem.name(this.newCulture.name());
        };


        CulturesListViewModel.prototype.createItem = function(item) {
            var newItem = new CultureViewModel(this, item);
            return newItem;
        };

        return CulturesListViewModel;

    })(kogrid.ListViewModel);

    /**
    * Culture view model
    */
    var CultureViewModel = (function(_super) {

        bcms.extendsClass(CultureViewModel, _super);

        function CultureViewModel(parent, item) {
            _super.call(this, parent, item);

            var self = this;

            self.name = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name } });
            self.code = ko.observable();

            self.registerFields(self.name, self.code);

            self.name(item.Name);
            self.code(item.Code);
        }

        CultureViewModel.prototype.getDeleteConfirmationMessage = function() {
            return $.format(globalization.deleteCultureConfirmMessage, this.name());
        };

        CultureViewModel.prototype.getSaveParams = function() {
            var params = _super.prototype.getSaveParams.call(this);
            params.Name = this.name();
            params.Code = this.code();

            return params;
        };

        return CultureViewModel;

    })(kogrid.ItemViewModel);

    /**
    * Initializes site settings cultures grid
    */
    function initializeSiteSettingsCultures(json) {
        var container = siteSettings.getMainContainer(),
            data = (json.Success == true) ? json.Data : {};

        var viewModel = new CulturesListViewModel(container, data.Items, data.GridOptions);
        viewModel.deleteUrl = links.deleteCultureUrl;
        viewModel.saveUrl = links.saveCultureUrl;

        ko.applyBindings(viewModel, container.get(0));
    }

    /**
    * Loads site settings cultures list.
    */
    cultures.loadSiteSettingsCulturesList = function() {
        dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsCulturesUrl, {
            contentAvailable: initializeSiteSettingsCultures
        });
    };

    /**
    * Initializes cultures module.
    */
    cultures.init = function() {
        bcms.logger.debug('Initializing bcms.cultures module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(cultures.init);

    return cultures;
});
