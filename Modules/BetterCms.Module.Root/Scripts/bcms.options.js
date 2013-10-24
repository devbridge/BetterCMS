/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.options', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.datepicker'],
    function ($, bcms, ko, kogrid, datepicker) {
        'use strict';

        var options = {},
            selectors = {
                datePickers: '.bcms-datepicker',
                datePickerTrigger: '.ui-datepicker-trigger',
                datePickerBox: '#ui-datepicker-div:first',
                focusedElements: ':focus'
            },
            links = {},
            globalization = {
                deleteOptionConfirmMessage: null,
                optionTypeText: null,
                optionTypeInteger: null,
                optionTypeBoolean: null,
                optionTypeDateTime: null,
                optionTypeFloat: null,
                optionTypeCustom: null,
                optionTypeJavaScriptUrl: null,
                optionTypeCssUrl: null,
                datePickerTooltipTitle: null,
                optionValidationMessage: null
            },
            optionTypes = {
                textType: 1,
                integerType: 2,
                floatType: 3,
                dateTimeType: 4,
                boolType: 5,
                javaScriptUrlType: 51,
                cssUrlType: 52,
                customType: 99
            },
            registeredCustomOptions = [],
            classes = {
                hasDatePicker: 'hasDatepicker'
            },
            rowId = 0;

        /**
        * Assign objects to module.
        */
        options.links = links;
        options.globalization = globalization;
        options.selectors = selectors;
        options.registerCustomOptions = registeredCustomOptions;

        function CustomOptionViewModel(title, identifier) {
            var self = this;

            self.title = title;
            self.identifier = identifier;

            return self;
        };
        
        function addCustomOptions(customOptions) {
            var list = [],
                i,
                l,
                option;

            if (!customOptions || !$.isArray(customOptions)) {
                return list;
            }

            for (i = 0, l = customOptions.length; i < l; i++) {
                option = customOptions[i];

                list.push(new CustomOptionViewModel(option.Title, option.Identifier));
            }

            return list;
        }

        /**
        * Options list view model
        */
        var OptionsListViewModel = (function (_super) {

            bcms.extendsClass(OptionsListViewModel, _super);

            function OptionsListViewModel(container, items, customOptions) {
                var self = this;
                self.customOptions = addCustomOptions(customOptions);

                _super.call(self, container, null, items, null);

                self.attachDatePickers = function() {
                    attachDatePickers(self);
                };
            };

            OptionsListViewModel.prototype.createItem = function (item) {
                var newItem = new OptionViewModel(this, item);
                
                return newItem;
            };

            return OptionsListViewModel;

        })(kogrid.ListViewModel);

        var OptionValuesListViewModel = (function (_super) {
            bcms.extendsClass(OptionValuesListViewModel, _super);
            
            function OptionValuesListViewModel(container, items, customOptions) {
                _super.call(this, container, items, customOptions);

                var self = this;

                self.attachDatePickers = function () {
                    attachDatePickers(self);
                };
            };

            OptionValuesListViewModel.prototype.createItem = function (item) {
                var newItem = new OptionValueViewModel(this, item);

                return newItem;
            };

            OptionValuesListViewModel.prototype.onAfterNewItemAdded = function (item) {
                if (item.canEditOption() !== false && item.key.domElement) {
                    $(item.key.domElement).focus();
                }
            };

            return OptionValuesListViewModel;
        })(OptionsListViewModel);

        function onDatePickerBoxFocused(self) {
            var i,
                item,
                len;

            for (i = 0, len = self.items().length; i < len; i++) {
                item = self.items()[i];

                if (item.isActive()) {
                    item.isSelected = true;
                }
            }
        }

        function attachDatePickers(self) {
            var datePickerBox = $(selectors.datePickerBox),
                dataKey = 'bcmsEventsAttached';

            if (!datePickerBox.data(dataKey)) {
                datePickerBox.data(dataKey, true);

                $(selectors.datePickerBox).on('click', function () {
                    onDatePickerBoxFocused(self);
                    return false;
                });

                $(selectors.datePickerBox).mousedown(function(event) {
                    bcms.stopEventPropagation(event);
                    return false;
                });
            }
        }

        /**
        * Option view model
        */
        var OptionViewModel = (function (_super) {
        
            bcms.extendsClass(OptionViewModel, _super);

            function OptionViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this,
                    ci, cl, cOption;

                // Main values
                self.key = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name } });
                self.defaultValue = ko.observable().extend({ optionValue: { self: self } }).extend({ notify: 'always' });
                self.value = ko.observable().extend({ optionValue: { self: self } }).extend({ notify: 'always' });
                self.type = ko.observable();
                self.useDefaultValue = ko.observable(-1);
                self.canEditOption = ko.observable(-1);
                self.canDeleteOption = true;

                // Additional values
                self.typeName = ko.observable();
                self.lastType = null;
                self.editableValue = self.getValueField();
                self.rowId = null;
                self.customType = ko.observable();
                self.customOptionTitle = ko.observable();
                self.customOptionDefaultTitle = ko.observable();
                self.calcType = ko.observable();

                self.optionTypes = [];
                self.optionTypes.push({ id: optionTypes.textType, name: globalization.optionTypeText });
                self.optionTypes.push({ id: optionTypes.integerType, name: globalization.optionTypeInteger });
                self.optionTypes.push({ id: optionTypes.floatType, name: globalization.optionTypeFloat });
                self.optionTypes.push({ id: optionTypes.dateTimeType, name: globalization.optionTypeDateTime });
                self.optionTypes.push({ id: optionTypes.boolType, name: globalization.optionTypeBoolean });
                self.optionTypes.push({ id: optionTypes.javaScriptUrlType, name: globalization.optionTypeJavaScriptUrl });
                self.optionTypes.push({ id: optionTypes.cssUrlType, name: globalization.optionTypeCssUrl });
                
                // Add custom options to types list
                for (ci = 0, cl = parent.customOptions.length; ci < cl; ci++) {
                    cOption = {
                        id: optionTypes.customType + ':' + parent.customOptions[ci].identifier,
                        name: parent.customOptions[ci].title
                    };
                    self.optionTypes.push(cOption);
                }

                // NOTE: useDefaultValue should be registered before defaultValue and type
                // because in other case, when cancelling edit mode, it sets wrong values.
                self.registerFields(self.useDefaultValue, self.key, self.value, self.defaultValue,
                    self.type, self.customType, self.calcType, self.customOptionTitle, self.customOptionDefaultTitle);

                self.getOptionTypeName = function() {
                    var i,
                        type = self.calcType();

                    for (i = 0; i < self.optionTypes.length; i++) {
                        if (self.optionTypes[i].id == type) {
                            return self.optionTypes[i].name;
                        }
                    }

                    return '';
                };

                self.getCalcType = function () {
                    var type = self.type(),
                        customType = self.customType(),
                        customOptionIdentifier = '';

                    if (type != optionTypes.customType) {
                        return type;
                    } else {
                        for (ci = 0, cl = self.parent.customOptions.length; ci < cl; ci++) {
                            if (self.parent.customOptions[ci].identifier == customType) {
                                customOptionIdentifier = self.parent.customOptions[ci].identifier;
                            }
                        }

                        return type + ':' + customOptionIdentifier;
                    }
                };

                self.calcType.subscribe(function (newType) {
                    var oldType = self.lastType,
                        typeValue = newType,
                        customType = '',
                        split;

                    if (newType && newType.indexOf && newType.indexOf(optionTypes.customType) === 0) {
                        // Entering custom mode
                        split = newType.indexOf(':');
                        typeValue = newType.substr(0, split);
                        customType = newType.substr(split + 1, newType.length - split);
                        
                        // Clearing old value
                        if (oldType) {
                            self.editableValue('');
                        }
                    } else {
                        self.customOptionTitle('');
                        self.customOptionDefaultTitle('');

                        // Leaving boolean mode
                        if (oldType == optionTypes.boolType) {
                            self.editableValue('');
                        }

                        // Leaving custom mode
                        if (oldType == optionTypes.customType) {
                            self.editableValue('');
                        }

                        // Entering boolean mode
                        if (newType == optionTypes.boolType) {
                            if (self.editableValue() !== 'true' && self.editableValue() !== true) {
                                self.editableValue(false);
                            }
                        }
                    }
                    
                    // Save new value
                    self.lastType = newType;

                    // Set type name
                    self.type(typeValue);
                    self.customType(customType);
                    self.typeName(self.getOptionTypeName());
                    
                    // Notify value to be re-validated
                    self.editableValue(self.editableValue());
                });

                self.initDatePickers = function() {
                    self.parent.attachDatePickers();

                    var datePickerOpts = {
                        onSelect: function (newDate, datePicker) {
                            self.isSelected = true;
                            self.editableValue(newDate);

                            datePicker.input.focus();
                        },
                        onChangeMonthYear: function() {
                            self.isSelected = true;
                        },
                        onClose: function (selectedDate, datePicker) {
                            setTimeout(function() {
                                var focusedElement = datePicker.input.parents('#' + self.rowId).find(selectors.focusedElements);
                                if (focusedElement.length === 0) {
                                    self.onBlurField();
                                }
                            }, 10);
                        }
                    },
                        row = $('#' + self.rowId),
                        datePickerTrigger,
                        datePickers = row.find(selectors.datePickers);

                    if (!datePickers.hasClass(classes.hasDatePicker)) {
                        datePickers.initializeDatepicker(globalization.datePickerTooltipTitle, datePickerOpts);

                        datePickerTrigger = row.find(selectors.datePickerTrigger);
                        datePickerTrigger.on('click', function (event) {
                            self.onItemSelect(self, event);
                        });
                        datePickerTrigger.on('focus', function (event) {
                            self.onItemSelect(self, event);
                        });
                    }
                };
                
                // Set values
                self.key(item.OptionKey);
                self.defaultValue(item.OptionDefaultValue);
                self.value(item.OptionValue);
                self.type(item.Type);
                if (item.Type == optionTypes.customType) {
                    self.customType(item.CustomType);
                    self.customOptionDefaultTitle(item.CustomOptionDefaultValueTitle);
                    self.customOptionTitle(item.CustomOptionValueTitle);
                }
                self.calcType(self.getCalcType(self));
                self.canEditOption(item.CanEditOption !== false);
                self.useDefaultValue(!self.canEditOption() && item.UseDefaultValue === true);
                self.canDeleteOption = item.CanDeleteOption !== false;

                // Disable editing and deletion
                self.changeFieldsEditing();
                
                self.useDefaultValue.subscribe(function (newValue) {
                    if (self.isActive()) {
                        if (newValue) {
                            self.value(self.defaultValue());
                            self.customOptionTitle(self.customOptionDefaultTitle());
                        } else {
                            self.value('');
                            self.customOptionTitle('');
                        }
                    }
                });

                self.getValueField().subscribe(function () {
                    // Closes expanded calendar, when user manually enters the value
                    var focusedElement = $(selectors.focusedElements);
                    if (focusedElement.length > 0 && $.isFunction(focusedElement.datepicker)) {
                        focusedElement.datepicker("hide");
                    }
                });

                self.onCustomOptionExecute = function (data, titleObservable, valueObservable) {
                    self.onItemSelect(data, null);

                    var customType = self.customType();
                    if (!customType) {
                        return;
                    }
                    
                    for (ci = 0, cl = registeredCustomOptions.length; ci < cl; ci++) {
                        if (registeredCustomOptions[ci].identifier == customType) {
                            registeredCustomOptions[ci].onExecute(valueObservable, titleObservable, self);
                            
                            return;
                        }
                    }
                };
            };

            OptionViewModel.prototype.getValueField = function() {
                return this.defaultValue;
            };

            OptionViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteOptionConfirmMessage, this.key());
            };

            OptionViewModel.prototype.changeFieldsEditing = function () {
                this.deletingIsDisabled(!this.canDeleteOption);
            };
            
            OptionViewModel.prototype.getRowId = function () {
                if (!this.rowId) {
                    this.rowId = 'bcms-option-row-' + rowId++;
                }
                return this.rowId;
            };

            return OptionViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Option value view model
        */
        var OptionValueViewModel = (function (_super) {
            bcms.extendsClass(OptionValueViewModel, _super);

            function OptionValueViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this;

                self.changeUseDefaultValue = function (data, event) {
                    if (self.isActive()) {
                        self.useDefaultValue(!self.useDefaultValue());
                        bcms.stopEventPropagation(event);
                        if (self.useDefaultValue.domElement) {
                            $(self.useDefaultValue.domElement).focus();
                        }
                        
                        return false;
                    }

                    return true;
                };
            }

            function changeFieldEditing(field, disable) {
                if (!field.editingIsDisabled) {
                    field.editingIsDisabled = ko.observable();
                }
                
                field.editingIsDisabled(disable);
            }

            OptionValueViewModel.prototype.getValueField = function () {
                return this.value;
            };

            OptionValueViewModel.prototype.changeFieldsEditing = function () {
                var disableEditing = this.canEditOption() === false;
                
                changeFieldEditing(this.key, disableEditing);
                changeFieldEditing(this.calcType, disableEditing);

                this.deletingIsDisabled(disableEditing);
            };

            OptionValueViewModel.prototype.editItem = function () {
                _super.prototype.editItem.call(this);

                if (!this.editingIsDisabled() && this.useDefaultValue() && this.useDefaultValue.domElement) {
                    $(this.useDefaultValue.domElement).focus();
                }
            };

            return OptionValueViewModel;
        })(OptionViewModel);

        /**
        * Creates options list view model
        */
        options.createOptionsViewModel = function(container, items, customOptions) {
            return new OptionsListViewModel(container, items, customOptions);
        };

        /**
        * Creates option values list view model
        */
        options.createOptionValuesViewModel = function (container, items, customOptions) {
            return new OptionValuesListViewModel(container, items, customOptions);
        };

        ko.extenders.optionValue = function (target, opts) {
            var ruleName = 'optionValue',
                self = opts.self;

            return ko.extenders.koValidationExtender(ruleName, target, function (newValue) {
                if (!self.type) {
                    return;
                }

                var type = self.type(),
                    mustBeNumber = type == optionTypes.floatType || type == optionTypes.integerType,
                    mustBeDate = type == optionTypes.dateTimeType,
                    hasError = !self.useDefaultValue() && newValue &&
                        ((mustBeNumber && isNaN(Number(newValue.replace(',', '.')))) 
                            || (mustBeDate && !datepicker.isDateValid(newValue))),
                    showMessage,
                    regExp;
                
                if (!hasError && newValue && type == optionTypes.integerType) {
                    regExp = new RegExp(/^-?\d*$/);
                    hasError = !regExp.test(newValue);
                }

                showMessage = hasError ? $.format(globalization.optionValidationMessage, self.key() || '', self.getOptionTypeName() || '') : '';

                target.validator.setError(ruleName, hasError, showMessage);
            });
        };

        /**
        * Registers custom option
        */
        options.registerCustomOption = function (identifier, onExecute) {
            registeredCustomOptions.push({
                identifier: identifier,
                onExecute: onExecute
            });
        };

        /**
        * Initializes bcms options module.
        */
        options.init = function () {
            bcms.logger.debug('Initializing bcms.options module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(options.init);

        return options;
});
