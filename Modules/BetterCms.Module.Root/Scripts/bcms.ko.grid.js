/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.ko.grid', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.messages', 'bcms.modal', 'bcms.tabs'],
    function ($, bcms, ko, messages, modal, tabs) {
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

    /**
    * Grid options view model
    */
    grid.OptionsViewModel = (function() {
        function OptionsViewModel(options, onOpenPage) {
            var self = this;

            self.searchQuery = ko.observable('');
            self.column = ko.observable();
            self.isDescending = ko.observable(false);

            self.setOptions = function(newOptions) {
                self.searchQuery(newOptions.SearchQuery || '');
                self.column(options.Column);
                self.isDescending(options.Direction == sortDirections.descending);

                self.paging = new ko.PagingViewModel(options.PageSize, options.PageNumber, options.TotalCount, onOpenPage);
            };
            
            self.hasFocus = ko.observable(true);
            
            if (options) {
                self.setOptions(options);

                self.hasPaging = true;
                self.paging = new ko.PagingViewModel(options.PageSize, options.PageNumber, options.TotalCount, onOpenPage);
            } else {
                self.hasPaging = false;
            }
        }

        return OptionsViewModel;
    })();

    /**
    * Grid list view model
    */
    grid.ListViewModel = (function() {

        grid.ListViewModel = function (container, loadUrl, items, gridOptions, opts) {
            var self = this;

            self.opts = $.extend({
                createItem: null
            }, opts);

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
                var options = new grid.OptionsViewModel(newGridOptions, self.openPage);
                self.options(options);
            };

            self.openPage = function() {
                var params = self.toJson();
                self.load(params);
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

            self.isValid = function(setFocus) {
                for (var i = 0; i < self.items().length; i++) {
                    if (!self.items()[i].isValid(setFocus)) {
                        return false;
                    }
                }
                
                return true;
            };
            
            self.getSelectedItem = function () {
                var i, l, listItem;

                for (i = 0, l = self.items().length; i < l; i++) {
                    listItem = self.items()[i];

                    if (listItem.isSelectedForInsert()) {
                        return listItem;
                    }
                }

                return null;
            };

            // Set items
            if (items && $.isArray(items)) {
                self.setItems(items);
            }

            // Set options
            self.setOptions(gridOptions);
        };

        grid.ListViewModel.prototype.addNewItem = function () {
            if (!this.rowAdded) {
                this.rowAdded = true;

                var newItem = this.createItem({
                    IsActive: true,
                    IsNew: true
                });
                this.items.unshift(newItem);
                this.isSelected = true;
                bcms.logger.trace('ko.grid.addNewItem: isSelected = true');

                this.onAfterNewItemAdded(newItem);
            }
        };

        grid.ListViewModel.prototype.createItem = function (item) {
            if (this.opts && this.opts.createItem && $.isFunction(this.opts.createItem)) {
                return this.opts.createItem(this, item);
            }

            var newItem = new grid.ItemViewModel(this, item);
            return newItem;
        };

        grid.ListViewModel.prototype.onAfterNewItemAdded = function (item) {
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
            var fieldValue = self.field(),
                oldValue = fieldValue || typeof fieldValue == "boolean" ? fieldValue : '';

            if (!self.parent.isActive() && oldValue !== self.oldValue) {
                self.oldValue = oldValue;
            }
        });

        self.hasChanges = function() {
            var oldValue = self.oldValue || typeof self.oldValue == "boolean" ? self.oldValue : '',
                fieldValue = self.field(),
                newValue = fieldValue || typeof fieldValue == "boolean" ? fieldValue : '';

            return oldValue != newValue;
        };

        self.restoreOldValue = function () {
            var oldValue = self.oldValue || typeof self.oldValue == "boolean" ? self.oldValue : '';

            self.field(oldValue);
        };

        self.isValid = function (setFocus) {
            if (self.field.hasError && self.field.hasError()) {
                if (setFocus) {
                    if (self.field.domElement) {
                        var domElement = $(self.field.domElement);
                        if (domElement) {
                            var tabPanel = tabs.getTabPanelOfElement(domElement);
                            if (tabPanel) {
                                tabPanel.selectTabOfElement(domElement);
                            }
                            domElement.focus();
                        }
                    }
                }
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
            bcms.logger.trace('ko.grid.ItemViewModel.constructor: isSelected = false');
            self.registeredFields = [];
            self.savePressed = false;
            self.deletingIsDisabled = ko.observable(false);
            self.editingIsDisabled = ko.observable(false);
            self.isSelectedForInsert = ko.observable(false);

            // Indicates, if item is still in add new phase
            self.isNew = ko.observable(item.IsNew || false);
            
            // Indicates, if item was added as new and has changed without saving to DB.
            self.wasSaved = !self.isNew();

            self.hasFocus.subscribe(function(value) {
                self.isSelected = value;
                bcms.logger.trace('ko.grid.hasfocus.subscribe: isSelected = value [' + value + ']');
            });

            self.onOpen = function (data, event) {
                self.isSelected = true;
                bcms.logger.trace('ko.grid.onOpen: isSelected = true');
                if (!this.isActive()) {
                    bcms.stopEventPropagation(event);
                    self.openItem();
                }

                return true;
            };

            self.onEdit = function (data, event) {
                self.isSelected = true;
                bcms.logger.trace('ko.grid.onEdit: isSelected = true');
                bcms.stopEventPropagation(event);
                self.editItem();
            };

            self.onDelete = function (data, event) {
                self.isSelected = true;
                bcms.logger.trace('ko.grid.onDelete: isSelected = true');
                bcms.stopEventPropagation(event);
                self.deleteItem();
            };

            self.onStopEvent = function (data, event) {
                self.isSelected = true;
                bcms.logger.trace('ko.grid.onStopEvent: isSelected = true');
                bcms.stopEventPropagation(event);
            };

            self.onItemSelect = function (data, event) {
                self.isSelected = true;
                bcms.logger.trace('ko.grid.onItemSelect: isSelected = true');
                
                return true;
            };

            self.onBlurField = function (data, event) {
                self.isSelected = false;
                bcms.logger.trace('ko.grid.onBlurField: iSelected = false');
                self.blurField();
            };

            self.onBlurFieldDelayed = function (data, event) {
                window.setTimeout(function () {
                    self.onBlurField(data, event);
                }, 250);
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

            self.isValid = function (setFocus) {
                for (var i = 0; i < self.registeredFields.length; i++) {
                    var field = self.registeredFields[i];
                    
                    if (!field.isValid(setFocus)) {
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

            self.hiddenFieldName = function (pattern, index) {
                return $.format(pattern, index);
            };

            self.initInput = function (element, field) {
                if (element && field && $.isFunction(field)) {
                    field.domElement = element;
                }
            };

            self.selectForInsert = function () {
                var i, l, listItem;

                for (i = 0, l = self.parent.items().length; i < l; i++) {
                    listItem = self.parent.items()[i];
                    
                    if (listItem == self) {
                        self.isSelectedForInsert(true);
                    } else if (listItem.isSelectedForInsert()) {
                        listItem.isSelectedForInsert(false);
                    }
                }
            };

            self.getCroppedText = function(text) {
                var length = this.getCroppedTextLength();

                if (text && text.length > length) {
                    text = text.substring(0, 250) + this.getCroppedTextSuffix();
                }

                return text;
            }
        };

        grid.ItemViewModel.prototype.getCroppedTextSuffix = function () {
            return ' ...';
        };

        grid.ItemViewModel.prototype.getCroppedTextLength = function () {
            return 250;
        };

        grid.ItemViewModel.prototype.onCancelEdit = function (data, event) {
            this.isSelected = true;
            bcms.logger.trace('ko.grid.onCancelEdit: isSelected = true');
            bcms.stopEventPropagation(event);
            this.cancelEditItem();
        };

        grid.ItemViewModel.prototype.onSave = function (data, event) {
            bcms.stopEventPropagation(event);
            this.hasFocus(!this.hasFocus(!this.hasFocus())); // Hack for IE.
            this.savePressed = true;
            this.saveItem();
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
            if (!this.editingIsDisabled()) {
                this.isActive(true);
                this.hasFocus(true);
            }
        };

        grid.ItemViewModel.prototype.cancelOrSaveItem = function () {
            var self = this,
                saveTimer = setTimeout(function () {
                    removeSaveTimer(self);
                    if (!self.isSelected && self.isActive()) {
                        if (self.isNew() && !self.hasChanges()) {
                            self.cancelEditItem();
                        } else {
                            self.saveItem();
                        }
                    }
                }, 100);

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
                    onAccept: function() {
                        confirmDialog.close();
                        self.deleting(true);
                        
                        if (url) {
                            // Call ajax method, if URL is specified
                            $.ajax({
                                type: 'POST',
                                url: url,
                                cache: false,
                                data: params
                            })
                                .done(function(json) {
                                    onDeleteCompleted(json);
                                    self.onAfterItemDeleted(json);
                                })
                                .fail(function(response) {
                                    var result = bcms.parseFailedResponse(response);
                                    onDeleteCompleted(result);
                                    self.onAfterItemDeleted(result);
                                });
                        } else {
                            // Remove from list if URL is not specified
                            var json = {
                                Success: true
                            };
                            onDeleteCompleted(json);
                            self.onAfterItemDeleted(json);
                        }
                        return true;
                    },
                    onClose: function() {
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
            if (self.saving() === true) {
                return;
            }
            
            self.hasFocus(false);
            self.saving(true);
            removeSaveTimer(self, true);
            
            var url = self.parent.saveUrl,
                canSave = this.isActive() && this.hasChanges() && this.isValid(),
                removeFromList = this.isActive() && !this.hasChanges() && this.isValid() && this.isNew(),
                keepActive = !this.isValid(),
                keepFocus = !canSave && !removeFromList && keepActive && self.savePressed;

            self.savePressed = false;

            // Mark item as no new anymore, if trying to save
            self.isNew(false);
            self.parent.rowAdded = false;

            if (canSave) {
                var params = self.getSaveParams();
                if (url) {
                    bcms.logger.trace('ko.grid.saveItem: starting to save item');
                    
                    // Save to server if URL is specified
                    $.ajax({
                        url: url,
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        cache: false,
                        data: JSON.stringify(params)
                    })
                        .done(function (json) {
                            bcms.logger.trace('ko.grid.saveItem: save is done');
                            self.onAfterItemSaved(json);
                            self.saving(false);

                            if (json.Success) {
                                self.wasSaved = true;
                            } else {
                                self.parent.rowAdded = true;
                            }
                        })
                        .fail(function (response) {
                            bcms.logger.trace('ko.grid.saveItem: save has failed');
                            self.onAfterItemSaved(bcms.parseFailedResponse(response));
                            self.saving(false);
                        });
                } else {
                    // Save locally if URL is NOT specified
                    var result = {
                        Success: true
                    };
                    self.onAfterItemSaved(result);
                    self.saving(false);
                    self.wasSaved = true;
                }
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
                bcms.logger.trace('ko.grid.onAfterItemSaved.onSuccess: isSelected = false');
                if (json.Data) {
                    self.version(json.Data.Version);
                    self.id(json.Data.Id);
                }
                self.isActive(false);
                self.loseFocus();
                
                for (var i = 0; i < this.registeredFields.length; i++) {
                    var field = this.registeredFields[i],
                        fieldValue = field.field();

                    field.oldValue = fieldValue || typeof fieldValue == "boolean" ? fieldValue : '';
                }
            } else {
                self.hasFocus(true);
                self.isSelected = true;
                bcms.logger.trace('ko.grid.onAfterItemSaved.onFailed: isSelected = true');
            }
        };

        grid.ItemViewModel.prototype.onAfterItemDeleted = function (json) {
        };

        grid.ItemViewModel.prototype.cancelEditItem = function () {
            if (this.isNew() || !this.wasSaved) {
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
        bcms.logger.debug('Initializing bcms.ko.grid module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(grid.init);
    
    return grid;
});
