/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.autocomplete.js" company="Devbridge Group LLC">
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
bettercms.define('bcms.autocomplete', ['bcms.jquery', 'bcms', 'bcms.jquery.autocomplete', 'bcms.ko.extenders', 'bcms.antiXss'],
    function($, bcms, jqAutoComplete, ko, antiXss) {
        'use strict';

        var autocomplete = {},
            links = {},
            globalization = {};

        /**
        * Assign objects to module.
        */
        autocomplete.links = links;
        autocomplete.globalization = globalization;

        /**
        * Creates knockout binding for autocomplete
        */
        function addAutoCompleteBindings() {
            var transformResult = function(response) {
                var result = typeof response === 'string' ? $.parseJSON(response) : response;
                return {
                    suggestions: $.map(result.suggestions, function(dataItem) {
                        return { value: dataItem.Value, data: dataItem.Key, jsonItem: dataItem };
                    })
                };
            }, onSearchStart = function (params, autocompleteViewModel) {
                var additionalParams = autocompleteViewModel.getAdditionalParameters(),
                    param;

                if (autocompleteViewModel.params) {
                    for (param in autocompleteViewModel.params) {
                        params[param] = autocompleteViewModel.params[param];
                    }
                }

                for (param in additionalParams) {
                    params[param] = additionalParams[param];
                }

            }, onBeforeSearchStart = function (params, autocompleteViewModel) {
                return autocompleteViewModel.onBeforeSearchStart(params);
            };

            ko.bindingHandlers.autocomplete = {
                init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                    var autocompleteViewModel = viewModel && viewModel.autocompleteViewModel
                            ? viewModel.autocompleteViewModel : viewModel,
                        onlyExisting = valueAccessor() == "onlyExisting",
                        complete = new jqAutoComplete(element, {
                            serviceUrl: autocompleteViewModel.serviceUrl,
                            type: 'POST',
                            appendTo: $(element).parent(),
                            autoSelectFirst: onlyExisting,
                            transformResult: transformResult,
                            onSelect: function(suggestion) {
                                autocompleteViewModel.setItem(suggestion.data, suggestion.value, suggestion.jsonItem);
                            },
                            onSearchStart: function (params) {
                                onSearchStart(params, autocompleteViewModel);
                            },
                            onBeforeSearchStart: function(params) {
                                onBeforeSearchStart(params, autocompleteViewModel);
                            }
                        });
                    
                    autocompleteViewModel.autocompleteInstance = complete;
                }
            };

            ko.bindingHandlers.autocompleteList = {
                init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                    var autocompleteViewModel = viewModel && viewModel.autocompleteViewModel
                            ? viewModel.autocompleteViewModel : viewModel,
                        onlyExisting = valueAccessor() == "onlyExisting",
                        complete = new jqAutoComplete(element, {
                            serviceUrl: autocompleteViewModel.serviceUrl,
                            type: 'POST',
                            appendTo: $(element).parent(),
                            autoSelectFirst: onlyExisting,
                            transformResult: transformResult,
                            onSelect: function(suggestion) {
                                autocompleteViewModel.newItem(suggestion.value);
                                if (onlyExisting) {
                                    autocompleteViewModel.addItemWithId(suggestion.value, suggestion.data);
                                } else {
                                    autocompleteViewModel.addItem();
                                }
                                autocompleteViewModel.clearItem();
                                if (autocompleteViewModel.autocompleteInstance) {
                                    autocompleteViewModel.autocompleteInstance.ignoreValueChange = false;
                                }
                            },
                            onSearchStart: function(params) {
                                onSearchStart(params, autocompleteViewModel);
                            },
                            onBeforeSearchStart: function(params) {
                                onBeforeSearchStart(params, autocompleteViewModel);
                            },
                            width: autocompleteViewModel.width
                });
                    
                    autocompleteViewModel.autocompleteInstance = complete;
                }
            };
        }

        /**
        * Autocomplete view model for single item
        */
        autocomplete.AutocompleteViewModel = (function () {

            autocomplete.AutocompleteViewModel = function (opts) {
                var self = this,
                    options = $.extend({
                        serviceUrl: null,
                        onItemSelect: function (item) {}
                    }, opts);

                self.autocompleteInstance = null;
                self.serviceUrl = options.serviceUrl;

                self.item = null;

                self.setItem = function (key, value, jsonItem) {
                    if (key || value) {
                        if (key && value) {
                            self.item = new autocomplete.AutocompleteItemViewModel(self, value, key);
                        } else {
                            self.item = new autocomplete.AutocompleteItemViewModel(self, value);
                        }

                        options.onItemSelect(self.item, jsonItem);
                    }
                };
            };

            autocomplete.AutocompleteViewModel.prototype.getAdditionalParameters = function () {
                return {};
            };

            autocomplete.AutocompleteViewModel.prototype.onBeforeSearchStart = function (params) {
                return true;
            };

            return autocomplete.AutocompleteViewModel;
        })();
        

        /**
        * Autocomplete items list view model
        */
        autocomplete.AutocompleteListViewModel = (function() {

            autocomplete.AutocompleteListViewModel = function(itemsList, opts) {
                var self = this,
                    options = $.extend({
                        serviceUrl: null,
                        pattern: null
                    }, opts);

                self.autocompleteInstance = null;
                self.serviceUrl = options.serviceUrl;
                self.pattern = options.pattern;
                self.params = options.params;
                self.width = options.width;

                self.isExpanded = ko.observable(true);
                self.hasfocus = ko.observable(false);
                self.items = ko.observableArray();
                self.newItem = ko.observable().extend({ maxLength: { maxLength: ko.maxLength.name }, activeDirectoryCompliant: "" });

                self.expandCollapse = function () {
                    var isExpanded = !self.isExpanded();
                    self.isExpanded(isExpanded);
                    self.hasfocus(isExpanded);
                    self.clearItem();
                };

                self.addItem = function() {
                    var newItem = $.trim(self.newItem()),
                        itemViewModel;

                    if (newItem && !self.alreadyExists(newItem) && !self.newItem.hasError()) {
                        itemViewModel = new autocomplete.AutocompleteItemViewModel(self, newItem);
                        self.items.push(itemViewModel);
                    }
                    self.clearItem();
                };

                self.addItemWithId = function(name, id) {

                    if (name && id && !self.alreadyExists(id)) {
                        var itemViewModel = new autocomplete.AutocompleteItemViewModel(self, name, id);
                        self.items.push(itemViewModel);
                    }
                    self.clearItem();
                };

                self.alreadyExists = function(newItem) {
                    var i,
                        item;

                    for (i = 0; i < self.items().length; i++) {
                        item = self.items()[i];

                        if (item.name().toLowerCase() == newItem.toLowerCase()) {
                            item.isActive(true);
                            setTimeout(function() {
                                item.isActive(false);
                            }, 4000);
                            self.clearItem();

                            return true;
                        }
                    }

                    return false;
                };

                self.getExistingItems = function () {
                    var existingItems = '',
                        i, item;

                    for (i = 0; i < self.items().length; i++) {
                        item = self.items()[i];
                        if (existingItems.length > 0) {
                            existingItems = existingItems + '|';
                        }
                        existingItems += item.id() || item.name();
                    }

                    return existingItems;
                };

                self.clearItem = function() {
                    self.newItem('');
                    if (self.autocompleteInstance) {
                        self.autocompleteInstance.currentValue = '';
                    }
                };

                self.applyItemList = function(itemToApplyList) {
                    self.removeAll();
                    if (itemToApplyList) {
                        for (var i = 0; i < itemToApplyList.length; i++) {
                            if (itemToApplyList[i].Value && itemToApplyList[i].Key) {
                                self.items.push(new autocomplete.AutocompleteItemViewModel(self, itemToApplyList[i].Value, itemToApplyList[i].Key));
                            } else {
                                self.items.push(new autocomplete.AutocompleteItemViewModel(self, itemToApplyList[i]));
                            }
                        }
                    }
                };

                self.removeAll = function() {
                    self.items.removeAll();
                };

                self.applyItemList(itemsList);
            };

            autocomplete.AutocompleteListViewModel.prototype.getAdditionalParameters = function () {
                return {
                    ExistingItems: this.getExistingItems()
                };
            };
            
            autocomplete.AutocompleteListViewModel.prototype.onBeforeSearchStart = function (params) {
                return true;
            };

            return autocomplete.AutocompleteListViewModel;
        })();

        /**
        * Autocomplete item view model
        */
        autocomplete.AutocompleteItemViewModel = (function () {
            
            autocomplete.AutocompleteItemViewModel = function (parent, name, itemId) {
                var self = this;

                self.parent = parent;

                self.isActive = ko.observable(false);
                self.name = ko.observable(name);
                self.id = ko.observable(itemId);

                self.remove = function () {
                    parent.items.remove(self);
                };

                self.getItemInputName = function (index) {
                    if (self.parent.pattern) {
                        return $.format(self.parent.pattern, index);
                    }
                    return null;
                };
            };
            
            return autocomplete.AutocompleteItemViewModel;
        })();

        /**
        * Initializes autocomplete module.
        */
        autocomplete.init = function() {
            bcms.logger.debug('Initializing bcms.autocomplete module.');

            addAutoCompleteBindings();
        };

        /**
        * Register initialization
        */
        bcms.registerInit(autocomplete.init);

        return autocomplete;
    });
