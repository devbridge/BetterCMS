/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.ko.grid', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.messages', 'bcms.modal'],
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
        staticDomId = 1,
        saveTimers = [];

    /**
    * Assign objects to module.
    */
    grid.links = links;
    grid.globalization = globalization;
    grid.selectors = selectors;

    grid.PagingViewModel = (function () {
        function PagingViewModel(json) {
            var self = this;

            self.pageSize = 0;
            self.pageNumber = ko.observable(1);
            self.totalPages = ko.observable(1);
            self.pagingUpperBound = null;
            self.pagingLowerBound = null;

            self.totalPagingLinks = 5;
            self.activePagePosition = 2;

            if (json && json.PageSize > 0) {
                var pageSize = json.PageSize,
                    pageNumber = json.PageNumber > 0 ? json.PageNumber : 1,
                    totalPages = parseInt(Math.ceil(json.TotalCount / pageSize));

                self.pageSize = pageSize;
                self.pageNumber(pageNumber);
                self.totalPages(totalPages);
                
                // lower bound
                self.pagingLowerBound = pageNumber - self.activePagePosition;
                if (self.pagingLowerBound < 1) {
                    self.pagingLowerBound = 1;
                }

                // upper bound
                self.pagingUpperBound = self.pagingLowerBound + self.totalPagingLinks;
                if (self.pagingUpperBound > totalPages) {
                    self.pagingUpperBound = totalPages;
                }
                
                // lower bound correction
                if (self.pagingUpperBound - self.pagingLowerBound < self.totalPagingLinks) {
                    self.pagingLowerBound = self.pagingUpperBound - self.totalPagingLinks;
                    if (self.pagingLowerBound < 1) {
                        self.pagingLowerBound = 1;
                    }
                }
            }

            self.pages = ko.computed(function () {
                var pages = [];
                for (var i = self.pagingLowerBound; i <= self.pagingUpperBound; i++) {
                    pages.push(i);
                }
                return pages;
            });
        }

        return PagingViewModel;
    })();

    /**
    * Grid options view model
    */
    grid.OptionsViewModel = (function() {
        function OptionsViewModel(options) {
            var self = this;

            self.searchQuery = ko.observable(options.SearchQuery || '');
            self.column = ko.observable(options.Column);
            self.isDescending = ko.observable(options.Direction == sortDirections.descending);
            self.paging = new grid.PagingViewModel(options);
            
            self.hasFocus = ko.observable(false);
        }

        return OptionsViewModel;
    })();

    /**
    * Grid list view model
    */
    grid.ListViewModel = (function() {

        grid.ListViewModel = function (container, loadUrl, items, gridOptions, totalItemsCount) {
            var self = this;

            self.loadUrl = loadUrl;
            self.saveUrl = null;
            self.deleteUrl = null;
            self.items = ko.observableArray();
            self.options = ko.observable();
            self.container = container;

            self.rowAdded = false;
            
            self.messagesDomId = function () {
                var id = staticDomId++;
                return 'bcms-editable-grid-messages-' + id;
            };
            
            self.setOptions = function (newGridOptions) {
                if (!newGridOptions) {
                    return;
                }
                var options = new grid.OptionsViewModel(newGridOptions);
                self.options(options);
            };

            self.setItems = function (itemsList) {
                if (!itemsList) {
                    return;
                }
                for (var i = 0; i < itemsList.length; i++) {
                    self.setItem(itemsList[i]);
                }
            };

            self.setItem = function (item) {
                var itemViewModel = self.createItem(item);

                self.items.push(itemViewModel);
            };

            self.addNewItem = function () {
                if (!self.rowAdded) {
                    self.rowAdded = true;

                    var newItem = self.createItem({
                        IsActive: true,
                        IsNew: true
                    });
                    self.items.unshift(newItem);
                    self.isSelected = true;
                }
            };
            
            self.searchItems = function () {
                self.options().paging.pageNumber(1);

                var params = self.toJson();
                self.load(params);
            };

            self.sortItems = function (column) {
                var columnBefore = self.options().column(),
                        wasDescending = self.options().isDescending();
                if (columnBefore == column) {
                    self.options().isDescending(!wasDescending);
                } else {
                    self.options().isDescending(false);
                }
                self.options().column(column);

                var params = self.toJson();
                self.load(params);
            };

            self.openPage = function(pageNr) {
                self.options().paging.pageNumber(pageNr);
                
                var params = self.toJson();
                self.load(params);
            };

            self.isSortedAscending = function (column) {
                if (column == self.options().column() && !self.options().isDescending()) {
                    return true;
                }
                return false;
            };

            self.isSortedDescending = function (column) {
                if (column == self.options().column() && self.options().isDescending()) {
                    return true;
                }
                return false;
            };

            self.toJson = function () {
                var params = {
                    SearchQuery: self.options().searchQuery(),
                    Column: self.options().column(),
                    Direction: self.options().isDescending() ? sortDirections.descending : sortDirections.ascending,
                    PageSize: self.options().paging.pageSize,
                    PageNumber: self.options().paging.pageNumber()
                };

                return params;
            };

            self.load = function (params) {
                var indicatorId = 'koGridList',
                    spinContainer = self.container,
                    onComplete = function (json) {
                        if (spinContainer) {
                            spinContainer.hideLoading(indicatorId);
                        }

                        self.parseResults(json);
                    };
                if (spinContainer) {
                    spinContainer.showLoading(indicatorId);
                }

                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: self.loadUrl,
                    data: params
                })
                    .done(function (result) {
                        onComplete(result);
                    })
                    .fail(function (response) {
                        onComplete(bcms.parseFailedResponse(response));
                    });
            };

            self.parseResults = function (json) {
                if (self.container) {
                    messages.refreshBox(self.container, json);
                }

                if (json.Success) {
                    self.items.removeAll();

                    if (json.Data) {

                        // Map media items
                        if (json.Data.Items && json.Data.Items && $.isArray(json.Data.Items)) {
                            self.setItems(json.Data.Items);
                        }

                        // Map grid options
                        if (json.Data.GridOptions) {
                            self.setOptions(json.Data.GridOptions);
                        }
                    }
                }
            };
            
            // Set items
            if (items && $.isArray(items)) {
                self.setItems(items);
            }

            // Set options
            if (gridOptions) {
                self.setOptions(gridOptions, totalItemsCount);
            }
        };

        grid.ListViewModel.prototype.createItem = function(item) {
            var newItem = new grid.ItemViewModel(this, item);
            return newItem;
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
            self.saving = ko.observable(false);
            self.deleting = ko.observable(false);
            self.hasFocus = ko.observable(true);
            self.hasError = ko.observable(false);
            self.isSelected = false;
            self.isNew = ko.observable(item.IsNew || false);
            self.registeredFields = [];
            self.savePressed = false;

            self.hasFocus.subscribe(function(value) {
                self.isSelected = value;
            });

            self.onOpen = function (data, event) {
                bcms.stopEventPropagation(event);
                self.openItem();
            };

            self.onEdit = function (data, event) {
                bcms.stopEventPropagation(event);
                self.editItem();
            };

            self.onDelete = function (data, event) {
                bcms.stopEventPropagation(event);
                self.deleteItem();
            };

            self.onCancelEdit = function (data, event) {
                bcms.stopEventPropagation(event);
                self.cancelEditItem();
            };

            self.onSave = function (data, event) {
                bcms.stopEventPropagation(event);
                self.savePressed = true;
                self.saveItem();
            };

            self.onStopEvent = function (data, event) {
                self.isSelected = true;
                bcms.stopEventPropagation(event);
            };

            self.onBlurField = function (data, event) {
                self.isSelected = false;
                self.blurField();
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
                for (var i = 0; i < self.registeredFields.length; i++) {
                    var field = self.registeredFields[i];

                    field.restoreOldValue();
                }
            };

            self.isValid = function () {
                for (var i = 0; i < self.registeredFields.length; i++) {
                    var field = self.registeredFields[i];
                    
                    if (!field.isValid()) {
                        return false;
                    }
                }
                return true;
            };

            self.loseFocus = function () {
                // Set false and then back true, else value will not change
                self.parent.options().hasFocus(false);
                self.parent.options().hasFocus(true);
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

        grid.ItemViewModel.prototype.blurField = function () {
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
            var self = this,
                saveTimer = setTimeout(function () {
                    removeSaveTimer(self);
                    if (!self.isSelected && self.isActive()) {
                        if (!self.id() && !self.hasChanges()) {
                            self.cancelEditItem();
                        } else {
                            self.saveItem();
                        }
                    }
                }, 500);

            saveTimers.push({id: self.id, timer: saveTimer});
        };

        function removeSaveTimer(self, clear) {
            for (var i = 0; i < saveTimers.length; i ++) {
                if (self.id == saveTimers[i].id) {
                    if (clear) {
                        clearTimeout(saveTimers[i].timer);
                    }
                    saveTimers.splice(i, 1);
                }
            }
        }

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
                onDeleteCompleted = function(json) {
                    if (container != null) {
                        messages.refreshBox(container, json);
                    }
                    if (json.Success) {
                        items.remove(self);
                    }
                    self.deleting(false);
                },
                confirmDialog = modal.confirm({
                    content: message || '',
                    onAccept: function () {
                        confirmDialog.close();
                        self.deleting(true);
                        $.ajax({
                            type: 'POST',
                            url: url,
                            cache: false,
                            data: params
                        })
                        .done(function (json) {
                            onDeleteCompleted(json);
                            self.onAfterItemDeleted(json);
                        })
                        .fail(function (response) {
                            var result = bcms.parseFailedResponse(response);
                            onDeleteCompleted(result);
                            self.onAfterItemDeleted(result);
                        });
                        return true;
                    },
                    onClose: function () {
                        self.deleting(false);
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
            var self = this;
            self.hasFocus(false);
            self.saving(true);
            removeSaveTimer(self, true);
            
            var url = self.parent.saveUrl,
                canSave = url && this.isActive() && this.hasChanges() && this.isValid(),
                removeFromList = this.isActive() && !this.hasChanges() && this.isValid() && !this.id(),
                keepActive = !this.isValid(),
                keepFocus = !canSave && !removeFromList && keepActive && self.savePressed;

            self.savePressed = false;

            // Mark item as no new anymore, if trying to save
            self.isNew(false);
            self.parent.rowAdded = false;

            if (!url) {
                console.log("Save url is not specified");
            }

            if (canSave) {
                var params = self.getSaveParams();
                $.ajax({
                    url: url,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    cache: false,
                    data: JSON.stringify(params)
                })
                    .done(function(json) {
                        self.onAfterItemSaved(json);
                        self.saving(false);
                    })
                    .fail(function(response) {
                        self.onAfterItemSaved(bcms.parseFailedResponse(response));
                        self.saving(false);
                    });
            } else {
                if (!keepActive) {
                    this.isActive(false);
                    this.loseFocus();
                }
                
                if (removeFromList) {
                    this.parent.items.remove(this);
                }

                self.saving(false);
            }
            
            if (keepFocus) {
                this.hasFocus(true);
            }
        };

        grid.ItemViewModel.prototype.onAfterItemSaved = function (json) {
            var self = this;

            if (self.parent.container) {
                messages.refreshBox(self.parent.container, json);
            }

            if (json.Success) {
                self.isSelected = false;
                if (json.Data) {
                    self.version(json.Data.Version);
                    self.id(json.Data.Id);
                }
                self.isActive(false);
                self.loseFocus();
                
                for (var i = 0; i < this.registeredFields.length; i++) {
                    var field = this.registeredFields[i];

                    field.oldValue = field.field() || '';
                }
            } else {
                self.hasFocus(true);
                self.isSelected = true;
            }
        };

        grid.ItemViewModel.prototype.onAfterItemDeleted = function (json) {
        };

        grid.ItemViewModel.prototype.cancelEditItem = function () {
            if (!this.id()) {
                this.parent.items.remove(this);
                this.parent.rowAdded = false;
            } else {
                this.restoreOldValues();
            }
            this.isActive(false);
            this.loseFocus();
        };

        grid.ItemViewModel.prototype.getDeleteConfirmationMessage = function () {
            return '';
        };

        grid.ItemViewModel.prototype.getRowId = function () {
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
