/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.ko.grid', ['jquery', 'bcms', 'knockout', 'bcms.messages', 'bcms.modal'],
    function ($, bcms, ko, messages, modal) {
    'use strict';

    var grid = {},
        selectors = {},
        links = {},
        globalization = {},
        sortDirections = {
            ascending: 0,
            descending: 1
        },
        staticDomId = 1;

    /**
    * Assign objects to module.
    */
    grid.links = links;
    grid.globalization = globalization;
    grid.selectors = selectors;

    /**
    * Grid options view model
    */
    grid.OptionsViewModel = (function() {
        function OptionsViewModel(options) {
            var self = this;

            self.searchQuery = ko.observable(options.SearchQuery || '');
            self.column = ko.observable(options.Column);
            self.isDescending = ko.observable(options.Direction == sortDirections.descending);
        }

        return OptionsViewModel;
    })();

    /**
    * Grid list view model
    */
    grid.ListViewModel = (function() {

        grid.ListViewModel = function(container, loadUrl, items, gridOptions) {
            var self = this;

            self.loadUrl = loadUrl;
            self.saveUrl = null;
            self.deleteUrl = null;
            self.items = ko.observableArray();
            self.options = ko.observable();
            self.container = container;

            // Set items
            if (items && $.isArray(items)) {
                this.setItems(items);
            }

            // Set options
            if (gridOptions) {
                this.setOptions(gridOptions);
            }
            
            self.messagesDomId = function () {
                var id = staticDomId++;
                return 'bcms-editable-grid-messages-' + id;
            };
        };

        grid.ListViewModel.prototype.setOptions = function (gridOptions) {
            if (!gridOptions) {
                return;
            }
            var options = new grid.OptionsViewModel(gridOptions);
            this.options(options);
        };

        grid.ListViewModel.prototype.setItems = function (items) {
            if (!items) {
                return;
            }
            for (var i = 0; i < items.length; i++) {
                this.setItem(items[i]);
            }
        };

        grid.ListViewModel.prototype.setItem = function(item) {
            var itemViewModel = this.createItem(item);

            this.items.push(itemViewModel);
        };

        grid.ListViewModel.prototype.addNewItem = function () {
            var newItem = this.createItem({
                IsActive: true
            });
            this.items.unshift(newItem);
        };

        grid.ListViewModel.prototype.createItem = function(item) {
            var newItem = new grid.ItemViewModel(this, item);
            return newItem;
        };

        grid.ListViewModel.prototype.searchItems = function () {
            var params = this.toJson();
            this.load(params);
        };

        grid.ListViewModel.prototype.sortItems = function (column) {
            var columnBefore = this.options().column(),
                    wasDescending = this.options().isDescending();
            if (columnBefore == column) {
                this.options().isDescending(!wasDescending);
            } else {
                this.options().isDescending(false);
            }
            this.options().column(column);
            
            var params = this.toJson();
            this.load(params);
        };
        
        grid.ListViewModel.prototype.isSortedAscending = function (column) {
            if (column == this.options().column() && !this.options().isDescending()) {
                return true;
            }
            return false;
        };

        grid.ListViewModel.prototype.isSortedDescending = function (column) {
            if (column == this.options().column() && this.options().isDescending()) {
                return true;
            }
            return false;
        };
        
        grid.ListViewModel.prototype.toJson = function () {
            var params = {
                SearchQuery: this.options().searchQuery(),
                Column: this.options().column(),
                Direction: this.options().isDescending() ? sortDirections.descending : sortDirections.ascending
            };
            
            return params;
        };

        grid.ListViewModel.prototype.load = function (params) {
            var indicatorId = 'koGridList',
                container = this.container,
                self = this,
                onComplete = function (json) {
                    if (container) {
                        container.hideLoading(indicatorId);
                    }

                    self.parseResults(json);
                };
            if (container) {
                container.showLoading(indicatorId);
            }
            
            $.ajax({
                type: 'POST',
                cache: false,
                url: this.loadUrl,
                data: params
            })
                .done(function (result) {
                    onComplete(result);
                })
                .fail(function (response) {
                    onComplete(bcms.parseFailedResponse(response));
                });
        };

        grid.ListViewModel.prototype.parseResults = function(json) {
            if (this.messagesContainer) {
                messages.refreshBox(this.messagesContainer, json);
            }
            
            if (json.Success) {
                this.items.removeAll();

                if (json.Data) {

                    // Map media items
                    if (json.Data.Items && json.Data.Items && $.isArray(json.Data.Items)) {
                        this.setItems(json.Data.Items);
                    }

                    // Map grid options
                    if (json.Data.GridOptions) {
                        this.setOptions(json.Data.GridOptions);
                    }
                }
            }
        };

        return grid.ListViewModel;
    })();

    /*
    * Item field view model
    */
    grid.FieldViewModel = function(id, field, parent) {
        var self = this;

        self.id = id;
        self.oldValue = '';
        self.field = field;
        self.parent = parent;

        self.field.subscribe(function () {
            var oldValue = self.field() ? self.field() : '';
            if (!self.parent.isActive() && oldValue != self.oldValue) {
                // console.log('Changing value of item ' + id + ' from "' + self.oldValue + '" to "' + oldValue + '"');
                self.oldValue = oldValue;
            }
        });

        self.hasChanges = function() {
            var oldValue = self.oldValue || '',
                newValue = self.field() || '';

            return oldValue != newValue;
        };

        self.restoreOldValue = function () {
            var oldValue = self.oldValue || '';
            
            self.field(oldValue);
        };

        self.isValid = function () {
            if (self.field.hasError && self.field.hasError()) {
                return false;
            }

            return true;
        };
    };
        
    /**
    * Grid list item view model
    */
    grid.ItemViewModel = (function () {

        grid.ItemViewModel = function(parent, item) {
            var self = this;

            self.parent = parent;

            self.id = ko.observable(item.Id);
            self.version = ko.observable(item.Version);
            self.isActive = ko.observable(item.IsActive || false);
            self.hasFocus = ko.observable(true);
            self.hasError = ko.observable(false);
            self.isSelected = false;
            self.registeredFields = [];
            
            self.onOpen = function (data, event) {
                bcms.stopEventPropagation(event);
                this.openItem();
            };

            self.onEdit = function (data, event) {
                bcms.stopEventPropagation(event);
                this.editItem();
            };

            self.onDelete = function (data, event) {
                bcms.stopEventPropagation(event);
                this.deleteItem();
            };

            self.onCancelEdit = function (data, event) {
                bcms.stopEventPropagation(event);
                this.cancelEditItem();
            };

            self.onSave = function (data, event) {
                bcms.stopEventPropagation(event);
                this.saveItem();
            };

            self.onStopEvent = function (data, event) {
                this.isSelected = true;
                bcms.stopEventPropagation(event);
            };

            self.onBlurField = function (data, event) {
                this.isSelected = false;
                this.blurField();
            };

            self.registerFields = function() {
                self.registeredFields = [];
                if (arguments.length > 0) {
                    for (var i = 0; i < arguments.length; i++) {
                        var field = new grid.FieldViewModel(i, arguments[i], self);
                        self.registeredFields.push(field);
                    }
                }
            };

            self.restoreOldValues = function () {
                for (var i = 0; i < this.registeredFields.length; i++) {
                    var field = this.registeredFields[i];
                    
                    field.restoreOldValue();
                }
            };

            self.isValid = function () {
                for (var i = 0; i < this.registeredFields.length; i++) {
                    var field = this.registeredFields[i];
                    
                    if (!field.isValid()) {
                        return false;
                    }
                }
                return true;
            };
        };

        grid.ItemViewModel.prototype.hasChanges = function () {
            for (var i = 0; i < this.registeredFields.length; i++) {
                var field = this.registeredFields[i];

                if (field.hasChanges()) {
                    return true;
                }
            }
            return false;
        };

        grid.ItemViewModel.prototype.blurField = function() {
            this.cancelOrSaveItem();
        };

        grid.ItemViewModel.prototype.openItem = function () {
            this.editItem();
        };

        grid.ItemViewModel.prototype.editItem = function () {
            this.isActive(true);
            this.hasFocus(true);
        };

        grid.ItemViewModel.prototype.cancelOrSaveItem = function () {
            var self = this;
            setTimeout(function () {
                if (!self.isSelected && self.isActive()) {
                    if (!self.id() && !self.hasChanges()) {
                        self.cancelEditItem();
                    } else {
                        self.saveItem();
                    }
                }
            }, 500);
        };

        grid.ItemViewModel.prototype.getDeleteParams = function() {
            return {
                Id: this.id(),
                Version: this.version()
            };
        };

        grid.ItemViewModel.prototype.deleteItem = function () {
            var self = this,
                url = self.parent.deleteUrl;

            if (!url) {
                console.log("Delete url is not specified");
                return;
            }

            var container = self.parent.container,
                items = self.parent.items,
                message = self.getDeleteConfirmationMessage(),
                params = self.getDeleteParams(),
                onDeleteCompleted = function (json) {
                    if (container != null) {
                        messages.refreshBox(container, json);
                    }
                    if (json.Success) {
                        items.remove(self);
                    }
                    confirmDialog.close();
                },
                confirmDialog = modal.confirm({
                    content: message || '',
                    onAccept: function() {
                        $.ajax({
                            type: 'POST',
                            url: url,
                            cache: false,
                            data: params
                        })
                            .done(function(json) {
                                onDeleteCompleted(json);
                            })
                            .fail(function(response) {
                                onDeleteCompleted(bcms.parseFailedResponse(response));
                            });
                        return false;
                    }
                });
        };

        grid.ItemViewModel.prototype.getSaveParams = function () {
            return {
                Id: this.id(),
                Version: this.version()
            };
        };

        grid.ItemViewModel.prototype.saveItem = function () {
            var self = this,
                url = self.parent.saveUrl,
                canSave = url && this.isActive() && this.hasChanges() && this.isValid(),
                removeFromList = this.isActive() && !this.hasChanges() && !this.id(),
                keepActive = !this.isValid();

            if (!url) {
                console.log("Save url is not specified");
            }

            if (canSave) {
                var onSaveCompleted = function(json) {
                        if (self.parent.container) {
                            messages.refreshBox(self.parent.container, json);
                        }

                        if (json.Success) {
                            if (json.Data) {
                                self.version(json.Data.Version);
                                self.id(json.Data.Id);
                            }
                            self.isActive(false);
                        }
                    },
                    params = self.getSaveParams();

                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'json',
                    cache: false,
                    data: params
                })
                    .done(function(json) {
                        onSaveCompleted(json);
                    })
                    .fail(function(response) {
                        onSaveCompleted(bcms.parseFailedResponse(response));
                    });
            } else {
                if (!keepActive) {
                    this.isActive(false);
                }
                
                if (removeFromList) {
                    this.parent.items.remove(this);
                }
            }
        };

        grid.ItemViewModel.prototype.cancelEditItem = function () {
            this.isActive(false);
            if (!this.id()) {
                this.parent.items.remove(this);
            } else {
                this.restoreOldValues();
            }
        };

        grid.ItemViewModel.prototype.getDeleteConfirmationMessage = function () {
            return '';
        };

        return grid.ItemViewModel;
    })();

    /**
    * Initializes knockout grid module.
    */
    grid.init = function () {
        console.log('Initializing bcms.ko.grid module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(grid.init);
    
    return grid;
});
