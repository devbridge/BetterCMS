/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.inlineEdit', ['bcms.jquery', 'bcms', 'bcms.messages', 'bcms.modal', 'bcms.grid'], function ($, bcms, messages, modal, grid) {
    'use strict';

    var editor = {
        // Prevents to add two or more new rows
        rowAdded: false,

        // Editor options
        options: {},
        
        // Auto generated name sequence id
        autoGenerateNameId: 0
    },
        selectors = {},
        defaultSelectors = {
            row: 'tr',
            editableRows: 'tr:visible:has("td")',
            rowCells: 'td',
            firstRow: 'tr:first',
            firstCell: 'td:first',
            firstForm: 'form:first',
            firstEditableInput: 'input[type="text"]:first',
            editableInput: 'input[type="text"]',
            fieldInputs: 'td > .bcms-input-box > input.bcms-editor-field-box',
            allInputs: 'td input',
            fieldValues: '.bcms-grid-item-info',
            deleteRowLink: 'a.bcms-icn-delete',
            rowMessage: '.bcms-grid-item-message',
            saveRowLink: '.bcms-btn-small',
            cancelLink: 'a.bcms-btn-links-small',
            editRowLink: '.bcms-grid-item-edit-button',
            firstTable: 'table.bcms-tables:first',
            templateFirstRow: 'tr:first',
            template: '#bcms-editable-row-template',
            fieldEditableValue: 'input[type="text"]:first',
            fieldHiddenValue: 'input[type="hidden"]:first',
            fieldVisibleValue: '.bcms-grid-item-info:first',
            prependNewRowTo: 'tbody',
            firstInvalidField: '.bcms-input-validation-error:first'
        },
        links = {
            
        },
        globalization = {
            messageSaving: null,
            messageDeleting: null,
            confirmDeleteMessage: null
        };

    /**
    * Assign objects to module.
    */
    editor.links = links;
    editor.globalization = globalization;
    editor.selectors = selectors;
    editor.defaultSelectors = defaultSelectors;
    
    /**
    * Checks if row is in edit mode.
    */
    editor.isRowInEditMode = function(row) {
        var activeElement = $(document.activeElement);

        // Checking if display != 'none', because when tab changes, ':visible' always returns false
        return row.css('display') != 'none'
            && !row.data('saving')
            && activeElement.parents(selectors.firstRow).get(0) != row.get(0);
    };
    
    /**
    * Initializes inline editor
    */
    editor.initialize = function (container, opts, selectorOpts) {
        this.options = $.extend({
            saveUrl: null,
            deleteUrl: null,
            newRowAdder: null,
            onSaveSuccess: function () { },
            rowDataExtractor: function () { return {}; },
            deleteRowMessageExtractor: function () { return ""; },
            switchRowToEdit: this.switchRowToEdit,
            switchRowToView: this.switchRowToView,
            showHideEmptyRow: grid.showHideEmptyRow,
            formContainer: container,
            form: container.find(selectors.firstForm)
        }, opts);

        this.initializeSelectors(selectorOpts);
        
        this.rowAdded = false;

        this.initRowEvents(container);

        this.resetAutoGenerateNameId();
        
        this.setInputNames(container);
    };

    /**
    * Setup selectors
    */
    editor.initializeSelectors = function (opts) {
        selectors = $.extend({}, defaultSelectors);
        if (opts) {
            selectors = $.extend({}, selectors, opts);
        }
    };

    /**
    * Selects / unselects row
    */
    function setRowSelectedStatus(row, isSelected) {
        if (isSelected) {
            var rows = row.parents(selectors.firstTable).find(selectors.editableRows),
                id = row.find(selectors.editRowLink).data('id');
            rows.each(function () {
                var self = $(this);
                if (id != self.find(selectors.editRowLink).data('id')) {
                    self.data('isSelected', false);
                }
            });
        }

        row.data('isSelected', isSelected);
    }

    /**
    * Initializes rows events
    */
    editor.initRowEvents = function (initContainer) {
        var self = this;

        initContainer.find(selectors.rowCells).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            var row = $(this).parents(selectors.firstRow);
            setRowSelectedStatus(row, true);
            self.editRow(row);
        });

        initContainer.find(selectors.saveRowLink).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            var row = $(this).parents(selectors.firstRow);
            setRowSelectedStatus(row, true);
            row.data('savePressed', true);
            self.saveRow(row);
        });

        initContainer.find(selectors.cancelLink).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            var row = $(this).parents(selectors.firstRow);
            setRowSelectedStatus(row, true);
            self.cancelRowEdit(row);
        });
        
        initContainer.find(selectors.deleteRowLink).on('click', function (event) {
            bcms.stopEventPropagation(event);
            
            var row = $(this).parents(selectors.firstRow);
            setRowSelectedStatus(row, true);
            self.deleteRow(row);
        });

        bcms.preventInputFromSubmittingForm(initContainer.find(selectors.fieldInputs), {
            preventedEnter: function (which) {
                var row = which.parents(selectors.firstRow);

                self.saveRow(row);
            },
            preventedEsc: function (which) {
                var row = which.parents(selectors.firstRow);
                self.cancelRowEdit(row);
            }
        });
    };

    /**
    * Checks if row is valid
    */
    editor.isRowValid = function(row) {
        var valid = true,
            firstInput = null;

        row.find(selectors.fieldInputs).each(function () {
            var input = $(this);

            if (!input.valid()) {
                if (!firstInput) {
                    firstInput = input;
                }
                valid = false;
            }
        });

        if (!valid && firstInput && row.data('savePressed')) {
            firstInput.focus();
            setRowSelectedStatus(row, false);
            row.data('savePressed', false);
        }

        return valid;
    };

    /**
    * Checks, if any value has changed during inline edit
    */
    editor.hasAnyValueChanged = function (row) {
        var changed = false;

        row.find(selectors.fieldInputs).each(function () {
            var input = $(this),
                cell = input.parents(selectors.firstCell),
                hidden = cell.find(selectors.fieldHiddenValue);
            
            if (input.val() != hidden.val()) {
                changed = true;
                return false;
            }

            return true;
        });
        
        return changed;
    };

    /**
    * Cancels row editing and restores old values
    */
    editor.cancelRowEdit = function (row) {
        if (row.data('new')) {
            row.find(selectors.editableInput).blur();
            row.remove();
            this.options.showHideEmptyRow(this.options.formContainer);
            this.rowAdded = false;
        } else {
            row.find(selectors.fieldInputs).each(function () {
                var input = $(this),
                    cell = input.parents(selectors.firstCell),
                    hidden = cell.find(selectors.fieldHiddenValue);

                input.val(hidden.val());
                //input.valid();
            });
            
            this.options.switchRowToView(row);
        }
    };

    /**
    * Bind input fields to blur event
    */
    editor.bindBlurEvents = function (row) {
        var self = this;

        row.find(selectors.fieldInputs).bind('blur', function (event) {
            self.onRowInputBlur(event, row);
        });
    };

    /**
    * Bind input fields from blur event
    */
    editor.unbindBlurEvents = function(row) {
        row.find(selectors.fieldInputs).unbind('blur');
    };

    /**
    * Function is called when focus lost, when inline editing mode is on
    */
    editor.onRowInputBlur = function (event, row) {
        var self = this;
        row.data('blurred', true);
        
        setTimeout(function () {
            var unbind = false;

            if (self.isRowInEditMode(row)) {
               
                var valueChanged = self.hasAnyValueChanged(row);
                
                if (valueChanged) {
                    if (self.isRowValid(row) && row.data('blurred')) {
                        self.saveRow(row);
                        unbind = true;
                    }
                } else {
                    if (!row.data('new') || !row.data('isSelected')) {
                        self.cancelRowEdit(row);
                        unbind = true;
                    }
                }
                
                setRowSelectedStatus(row, false);
            }

            if (unbind) {
                // Unbind, and later bind  back again
                self.unbindBlurEvents(row);
            }
            
            row.data('blurred', false);
        }, 500);
    };

    /**
    * Add new row to table and enter inline edit mode
    */
    editor.addNewRow = function (container, table) {
        if (!this.rowAdded) {
            var template = container.find(selectors.template),
                newRow = $(template.html()).find(selectors.templateFirstRow);
            
            if (!table) {
                table = container.find(selectors.firstTable);
            }

            newRow.data('new', true);
            this.setRowInputNames(newRow);

            if ($.isFunction(this.options.newRowAdder)) {
                this.options.newRowAdder(newRow, table);
            } else {
                newRow.prependTo(table.find(selectors.prependNewRowTo));
            }

            this.options.switchRowToEdit(newRow);

            bcms.updateFormValidator(this.options.form);

            this.initRowEvents(newRow, container);

            this.bindBlurEvents(newRow);

            this.rowAdded = true;

            this.options.showHideEmptyRow(container);
        }
    };


    editor.showRowLoading = function(tr, messageToShow) {
        $(tr).children().hide();
        var columnCount = $(tr).children().length;
        var td = $("<td>", { "colspan": columnCount });
        td.html(messageToShow);
        $(tr).append(td);
    };
    
    editor.hideRowLoading = function(tr) {
        $($(tr).children().get($(tr).children().length - 1)).remove();
        $(tr).children().show();
    };

    /**
    * Saves the row
    */
    editor.saveRow = function (row) {
        var self = this;

        if (self.isRowValid(row)) {

            //
            // If saveUrl is set, saving by AJAX. Else just setting visible values from editable fields.
            //
            if (self.options.saveUrl) {
                var rowData = self.options.rowDataExtractor(row),
                    saveLink = row.find(selectors.saveRowLink),
                    cancelLink = row.find(selectors.cancelLink),
                    url = self.options.saveUrl,
                    onComplete = function(json) {
                        messages.refreshBox(row, json);
                        self.hideRowLoading(row);
                        row.data('saving', false);
                        row.data('blurred', false);
                        if (json.Success) {
                            saveLink.hide();
                            cancelLink.hide();
                            if (json.Data) {
                                if (json.Data.Version) {
                                    row.find(selectors.deleteRowLink).data('version', json.Data.Version);
                                }
                                if (json.Data.Id) {
                                    row.find(selectors.deleteRowLink).data('id', json.Data.Id);
                                    row.find(selectors.editRowLink).data('id', json.Data.Id);   
                                }
                                self.rowAdded = false;
                                row.data('new', false);
                            }
                            if ($.isFunction(self.options.onSaveSuccess)) {
                                self.options.onSaveSuccess(row, json);
                            }
                            self.options.switchRowToView(row);
                        } else {
                            // Bind back to item blur event
                            self.bindBlurEvents(row);
                        }
                    };

                row.data('saving', true);
                self.showRowLoading(row, globalization.messageSaving);

                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: url,
                    data: rowData
                })
                    .done(function(result) {
                        onComplete(result);
                    })
                    .fail(function(response) {
                        onComplete(bcms.parseFailedResponse(response));
                    });
            } else {
                self.rowAdded = false;
                row.data('new', false);

                row.find(selectors.fieldInputs).each(function () {
                    var input = $(this),
                        cell = input.parents(selectors.firstCell),
                        hidden = cell.find(selectors.fieldHiddenValue),
                        visibleValue = cell.find(selectors.fieldVisibleValue);

                    hidden.val(input.val());
                    visibleValue.html(input.val());

                    return true;
                });

                self.options.switchRowToView(row);
            }
        }

        return true;
    };

    /**
    * Enters row inline edit mode.
    */
    editor.editRow = function (row) {
        this.bindBlurEvents(row);

        this.options.switchRowToEdit(row);
    };

    /**
    * Deletes the row
    */
    editor.deleteRow = function (row) {
        var rowData = this.options.rowDataExtractor(row),
            message,
            self = this;

        if ($.isFunction(this.options.deleteRowMessageExtractor)) {
            message = this.options.deleteRowMessageExtractor(rowData);
        } else {
            message = globalization.confirmDeleteMessage;
        }

        modal.confirm({
            content: message,
            onAccept: function () {
                if (self.options.deleteUrl) {
                    var url = self.options.deleteUrl,
                        deleteLink = row.find(selectors.deleteRowLink),
                        onComplete = function(json) {
                            messages.refreshBox(row, json);
                            self.hideRowLoading(row);
                            if (json.Success) {
                                row.remove();
                                self.options.showHideEmptyRow(self.options.formContainer);
                            } else {
                                deleteLink.show();
                            }
                        };

                    deleteLink.hide();
                    self.showRowLoading(row, globalization.messageDeleting);
                    $.ajax({
                        type: 'POST',
                        cache: false,
                        url: url,
                        data: rowData
                    })
                        .done(function(result) {
                            onComplete(result);
                        })
                        .fail(function(response) {
                            onComplete(bcms.parseFailedResponse(response));
                        });
                } else {
                    row.remove();
                    self.options.showHideEmptyRow(self.options.formContainer);
                }
            }
        });
    };

    /**
    * Enters row inline editing mode
    */
    editor.switchRowToEdit = function (row) {
        row.addClass("bcms-table-row-active");
        
        row.find(selectors.fieldValues).hide();
        row.find(selectors.fieldInputs).show();
        
        if (!row.find(selectors.editableInput).eq(1).is(":focus")) {
            row.find(selectors.firstEditableInput).focus();    
        }

        row.find(selectors.deleteRowLink).hide();
        row.find(selectors.saveRowLink).show();
        row.find(selectors.cancelLink).show();
        row.find(selectors.rowMessage).hide();
    };

    /**
    * Cancels row inline editing mode
    */
    editor.switchRowToView = function (row) {
        row.removeClass("bcms-table-row-active");
        
        row.find(selectors.fieldValues).show();
        row.find(selectors.fieldInputs).blur().hide();
        
        row.find(selectors.deleteRowLink).show();
        row.find(selectors.saveRowLink).hide();
        row.find(selectors.cancelLink).hide();
        row.find(selectors.rowMessage).hide();
    };

    /**
    * Resets auto generated name sequence number
    */
    editor.resetAutoGenerateNameId = function () {
        this.autoGenerateNameId = 0;
    };

    /**
    * Add names for each given container row inputs, if name patterns are set
    */
    editor.setInputNames = function (container) {
        var self = this;

        container.find(selectors.row).each(function () {
            var row = $(this);

            self.setRowInputNames(row);
        });
        
        bcms.updateFormValidator(self.options.form);
    };

    /**
    * Add names for row inputs, if name patterns are set
    */
    editor.setRowInputNames = function (row) {
        var counterSet = false,
            index = null,
            self = this;
        row.find(selectors.allInputs).each(function () {
            var input = $(this),
                pattern = input.data('namePattern');
            if (pattern) {
                if (!counterSet) {
                    counterSet = true;
                    index = self.autoGenerateNameId++;
                }
                var name = $.format(pattern, index);
                input.attr('name', name);
                
                var validator = input.parents(selectors.firstCell).find("span.bcms-field-validation-valid, span.bcms-field-validation-error");
                validator.data('valmsg-for', name);
                validator.attr('data-valmsg-for', name);
            }
        });
    };

    /**
    * Initializes inline edit module.
    */
    editor.init = function () {
        bcms.logger.debug('Initializing bcms.inlineEdit module.');
    };
    
    /**
    * Register initialization
    */
    bcms.registerInit(editor.init);
    
    return editor;
});
