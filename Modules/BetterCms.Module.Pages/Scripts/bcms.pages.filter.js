/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.filter', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.tags'],
    function($, bcms, ko, tags) {
        'use strict';

        var filter = {},
            selectors = {
                filterTemplate: '#bcms-filter-template',
                filterCategory: '#bcms-filter-category-selection',
            },
            links = {},
            globalization = {};

        /**
        * Assign objects to module.
        */
        filter.links = links;
        filter.globalization = globalization;

        function FilterViewModel(tagsViewModel, container, onSearchClick) {
            var self = this;

            self.isVisible = ko.observable(false);
            self.tags = tagsViewModel;
            self.includeArchived = ko.observable(false);
            self.includeMasterPages = ko.observable(false);
            self.dropDown = container.find(selectors.filterCategory).get(0);
            var dropDownValue = 0;
            if ($(self.dropDown).get(0) && $(self.dropDown).get(0).selectedIndex) {
                dropDownValue = $(self.dropDown).get(0).selectedIndex;
            }
            self.dropDownValue = ko.observable(dropDownValue);
            $(self.dropDown).change(function () {
                self.dropDownValue(this.selectedIndex);
            });
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
                if (self.dropDownValue() != 0) {
                    return true;
                }
                return false;
            });

            // Actions.
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
                self.includeArchived(false);
                self.includeMasterPages(false);
                self.dropDownValue(0);
                if (self.dropDown) {
                    self.dropDown.selectedIndex = 0;
                }
                self.searchWithFilter();
            };
            self.changeIncludeArchived = function () {
                self.includeArchived(!(self.includeArchived()));
            };
            self.changeIncludeMasterPages = function () {
                self.includeMasterPages(!(self.includeMasterPages()));
            };
        }

        filter.bind = function (container, jsonData, onSearchClick) {
            var tagsViewModel = new tags.TagsListViewModel(jsonData.Tags),
                filterViewModel = new FilterViewModel(tagsViewModel, container, onSearchClick);
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
