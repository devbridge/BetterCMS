/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.blog.filter', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.pages.tags'],
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

        function FilterViewModel(tagsViewModel, onSearchClick, onClearClick) {
            var self = this;
            
            self.isVisible = ko.observable(false);
            self.tags = tagsViewModel;

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
                if ($.isFunction(onClearClick)) {
                    onClearClick();
                }
            };
        }

        filter.bind = function (container, jsonData, onSearchClick) {
            var tagsViewModel = new tags.TagsListViewModel(jsonData.Tags),
                filterViewModel = new FilterViewModel(tagsViewModel, onSearchClick, function() {
                    container.find(selectors.filterCategory).get(0).selectedIndex = 0;
                });
            ko.applyBindings(filterViewModel, container.find(selectors.filterTemplate).get(0));
        };

        /**
        * Initializes blog filter module.
        */
        filter.init = function() {
            console.log('Initializing bcms.blog.filter module.');
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(filter.init);

        return filter;
    });
