/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

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
            self.dropDown = container.find(selectors.filterCategory).get(0);
            self.dropDownValue = ko.observable(0);
            $(self.dropDown).change(function () {
                self.dropDownValue(this.selectedIndex);
            });
            self.isEdited = ko.computed(function () {
                if (self.includeArchived()) {
                    return true;
                }
                if (self.tags != null && self.tags.tags() != null && self.tags.tags().length > 0) {
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
                self.tags.tags([]);
                self.includeArchived(false);
                self.dropDownValue(0);
                if (self.dropDown) {
                    self.dropDown.selectedIndex = 0;
                }
                self.searchWithFilter();
            };
        }

        filter.bind = function (container, jsonData, onSearchClick) {
            var tagsViewModel = new tags.TagsListViewModel(jsonData.Tags),
                filterViewModel = new FilterViewModel(tagsViewModel, container, onSearchClick);
            filterViewModel.includeArchived(jsonData.IncludeArchived ? true : false);
            ko.applyBindings(filterViewModel, container.find(selectors.filterTemplate).get(0));
        };

        /**
        * Initializes page module.
        */
        filter.init = function() {
            console.log('Initializing bcms.pages.filter module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(filter.init);

        return filter;
    });
