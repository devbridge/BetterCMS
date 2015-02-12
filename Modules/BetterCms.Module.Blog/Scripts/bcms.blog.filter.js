/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.blog.filter', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.tags', 'bcms.categories'],
    function ($, bcms, ko, tags, categories) {
        'use strict';

        var filter = {},
            selectors = {
                filterTemplate: '#bcms-filter-template'
            },
            links = {},
            globalization = {};

        /**
        * Assign objects to module.
        */
        filter.links = links;
        filter.globalization = globalization;

        function FilterViewModel(tagsViewModel, categoriesViewModel, container, onSearchClick, jsonData) {
            var self = this;
            
            self.isVisible = ko.observable(false);
            self.tags = tagsViewModel;
            self.categories = categoriesViewModel;
            self.includeArchived = ko.observable(false);
            self.languageId = ko.observable(jsonData.LanguageId);
            self.languages = jsonData.Languages || [];          
            self.status = ko.observable(jsonData.Status);
            self.statuses = jsonData.Statuses || [];
            self.seoStatus = ko.observable(jsonData.SeoStatus);
            self.seoStatuses = jsonData.SeoStatuses || [];

            self.languages.unshift({ Key: '', Value: '' });
            self.statuses.unshift({ Key: '', Value: '' });
            self.seoStatuses.unshift({ Key: '', Value: '' });

            self.isEdited = ko.computed(function () {
                if (self.includeArchived()) {
                    return true;
                }
                if (self.tags != null && self.tags.items() != null && self.tags.items().length > 0) {
                    return true;
                }
                if (self.categories != null && self.categories.items() != null && self.categories.items().length > 0) {
                    return true;
                }
                if (self.languageId() || self.seoStatus() || self.status()) {
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
                self.categories.items([]);
                self.includeArchived(false);
                self.languageId('');
                self.status('');
                self.seoStatus('');
                self.searchWithFilter();
            };

            self.changeIncludeArchived = function () {
                self.includeArchived(!(self.includeArchived()));
            };
        }

        filter.bind = function (container, jsonData, onSearchClick) {
            var tagsViewModel = new tags.TagsListViewModel(jsonData.Tags),
                categoriesViewModel = new categories.CategoriesListViewModel(jsonData.Categories, 'Blog Posts'),
                filterViewModel = new FilterViewModel(tagsViewModel, categoriesViewModel, container, onSearchClick, jsonData);

            filterViewModel.includeArchived(jsonData.IncludeArchived ? true : false);
            ko.applyBindings(filterViewModel, container.find(selectors.filterTemplate).get(0));
        };

        /**
        * Initializes blog filter module.
        */
        filter.init = function() {
            bcms.logger.debug('Initializing bcms.blog.filter module.');
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(filter.init);

        return filter;
    });
