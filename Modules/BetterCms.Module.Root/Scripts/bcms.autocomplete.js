/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms, console */

bettercms.define('bcms.autocomplete', ['bcms.jquery', 'bcms', 'bcms.jquery.autocomplete', 'bcms.ko.extenders'],
    function($, bcms, jqAutoComplete, ko) {
        'use strict';

        var autocomplete = {},
            selectors = {},
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
        function addAutoCompleteBinding() {
            ko.bindingHandlers.autocomplete = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel) {
                    var onlyExisting = valueAccessor() == "onlyExisting",
                        complete = new jqAutoComplete(element, {
                            serviceUrl: viewModel.serviceUrl,
                            type: 'POST',
                            appendTo: $(element).parent(),
                            autoSelectFirst: onlyExisting,
                            transformResult: function(response) {
                                var result = typeof response === 'string' ? $.parseJSON(response) : response;
                                return {
                                    suggestions: $.map(result.suggestions, function(dataItem) {
                                        return { value: dataItem.Value, data: dataItem.Key };
                                    })
                                };
                            },
                            onSelect: function(suggestion) {
                                viewModel.newItem(suggestion.value);
                                if (onlyExisting) {
                                    viewModel.addItemWithId(suggestion.value, suggestion.data);
                                } else {
                                    viewModel.addItem();
                                }
                                viewModel.clearItem();
                            }
                        });
                }
            };
        }

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

                self.serviceUrl = options.serviceUrl;
                self.pattern = options.pattern;

                self.isExpanded = ko.observable(true);
                self.items = ko.observableArray();
                self.newItem = ko.observable().extend({ maxLength: { maxLength: ko.maxLength.name } });

                self.expandCollapse = function() {
                    self.isExpanded(!self.isExpanded());
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

                    if (name && id && !self.alreadyExists(name)) {
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

                        if (item.name() == newItem) {
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

                self.clearItem = function() {
                    self.newItem('');
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
            console.log('Initializing bcms.autocomplete module.');

            addAutoCompleteBinding();
        };

        /**
        * Register initialization
        */
        bcms.registerInit(autocomplete.init);

        return autocomplete;
    });
