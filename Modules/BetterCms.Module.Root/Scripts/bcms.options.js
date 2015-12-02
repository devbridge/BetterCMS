/// <reference path="../../../../../Modules/BetterCms.Module.Root/Views/Shared/EditableGrid/Partial/BooleanCell.cshtml" />
/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.options', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.datepicker', 'bcms.antiXss'],
    function ($, bcms, ko, kogrid, datepicker, antiXss) {
        'use strict';

        var options = {},
            selectors = {
                datePickers: '.bcms-datepicker',
                datePickerTrigger: '.ui-datepicker-trigger',
                datePickerBox: '#ui-datepicker-div:first',
                focusedElements: ':focus',
                multioptionContainer: '#bcms-multi-opts'
            },
            links = {},
            globalization = {
                deleteOptionConfirmMessage: null,
                optionTypeText: null,
                optionTypeMultilineText: null,
                optionTypeInteger: null,
                optionTypeBoolean: null,
                optionTypeDateTime: null,
                optionTypeFloat: null,
                optionTypeCustom: null,
                optionTypeJavaScriptUrl: null,
                optionTypeCssUrl: null,
                datePickerTooltipTitle: null,
                optionValidationMessage: null,
                invariantLanguage: null
            },
            optionTypes = {
                textType: 1,
                integerType: 2,
                floatType: 3,
                dateTimeType: 4,
                boolType: 5,
                textMultilineType: 21,
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

            function OptionsListViewModel(container, items, customOptions, opts) {
                var self = this,
                    ci, cl, cOption;
                self.customOptions = addCustomOptions(customOptions);

                self.optionTypes = ko.observableArray();
                self.optionTypes.push({ id: optionTypes.textType, name: globalization.optionTypeText });
                self.optionTypes.push({ id: optionTypes.textMultilineType, name: globalization.optionTypeMultilineText });
                self.optionTypes.push({ id: optionTypes.integerType, name: globalization.optionTypeInteger });
                self.optionTypes.push({ id: optionTypes.floatType, name: globalization.optionTypeFloat });
                self.optionTypes.push({ id: optionTypes.dateTimeType, name: globalization.optionTypeDateTime });
                self.optionTypes.push({ id: optionTypes.boolType, name: globalization.optionTypeBoolean });
                self.optionTypes.push({ id: optionTypes.javaScriptUrlType, name: globalization.optionTypeJavaScriptUrl });
                self.optionTypes.push({ id: optionTypes.cssUrlType, name: globalization.optionTypeCssUrl });

                // Add custom options to types list
                for (ci = 0, cl = self.customOptions.length; ci < cl; ci++) {
                    cOption = {
                        id: optionTypes.customType + ':' + self.customOptions[ci].identifier,
                        name: self.customOptions[ci].title
                    };
                    self.optionTypes.push(cOption);
                }

                self.attachDatePickers = function () {
                    attachDatePickers(self);
                };

                self.serializeToObject = function () {
                    var allItems = self.items(),
                        result = [],
                        i;

                    for (i = 0; i < allItems.length; i++) {
                        var item = allItems[i];
                        if (item.isSelected == true && item.isActive()) {
                            item.onSave();
                        }
                        result.push(item.toJson());
                    }

                    return result;
                };

                self.showLanguages = ko.observable(opts.showLanguages);
                self.languages = ko.observableArray();
                self.languageId = ko.observable("");
                self.language = opts.showLanguages ? new LanguageViewModel(opts.languages, null, self) : null;
                self.translationsEnabled = opts.translationsEnabled;
                self.suspendAddItemSubscribe = true;
                self.selectedTypeId = ko.observable();
                self.isAddNewSelected = ko.observable(false);

                self.selectedTypeId.subscribe(function () {
                    if (!self.suspendAddItemSubscribe) {
                        self.isAddNewSelected(false);
                        _super.prototype.addNewItem.call(self);
                    }
                });

                _super.call(self, container, null, items, null);

                //  Override methods
                self.addNewItem = function () {
                    self.suspendAddItemSubscribe = true;
                    self.selectedTypeId(null);
                    self.suspendAddItemSubscribe = false;
                    self.isAddNewSelected(true);
                };
            };

            OptionsListViewModel.prototype.createItem = function (item) {
                var newItem = new OptionViewModel(this, item);

                return newItem;
            };

            OptionsListViewModel.prototype.onLanguageChanged = function (languageId) {
                if (languageId != this.languageId()) {
                    this.languageId(languageId);
                    this.changeLanguageForItems(this.items());
                }
            };

            OptionsListViewModel.prototype.changeLanguageForItems = function (items) {
                for (var i = 0; i < items.length; i++) {
                    items[i].activateTranslation(this.languageId());
                }
            };

            OptionsListViewModel.prototype.onAfterNewItemAdded = function (item) {
                if (item.canEditOption() !== false && item.key.domElement) {
                    $(item.key.domElement).focus();
                }
                item.calcType(this.selectedTypeId());
            };

            return OptionsListViewModel;

        })(kogrid.ListViewModel);

        var OptionValuesListViewModel = (function (_super) {
            bcms.extendsClass(OptionValuesListViewModel, _super);

            function OptionValuesListViewModel(container, items, customOptions, opts) {
                _super.call(this, container, items, customOptions, opts);

                var self = this;
                if (bcms.languageId && self.translationsEnabled) {
                    self.onLanguageChanged(bcms.languageId);
                }
                self.attachDatePickers = function () {
                    attachDatePickers(self);
                };

                self.toJson = function () {
                    var allItems = self.items(),
                        result = [],
                        i;

                    for (i = 0; i < allItems.length; i++) {
                        result.push(allItems[i].toJson());
                    }

                    return result;
                };

            };

            OptionValuesListViewModel.prototype.createItem = function (item) {
                var newItem = new OptionValueViewModel(this, item);

                return newItem;
            };

            OptionsListViewModel.prototype.onLanguageChanged = function (languageId) {
                if (languageId != this.languageId()) {
                    this.languageId(languageId);
                    this.changeLanguageForItems(this.items());
                }
            };

            OptionsListViewModel.prototype.changeLanguageForItems = function (items) {
                for (var i = 0; i < items.length; i++) {
                    items[i].activateTranslation(this.languageId());
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
                self.key = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name }, preventHtml: "" });
                self.defaultValueBinding = ko.observable().extend({ optionValue: { self: self } }).extend({ notify: 'always' });
                self.valueBinding = ko.observable().extend({ optionValue: { self: self } }).extend({ notify: 'always' });

                self.type = ko.observable();
                self.useDefaultValueBinding = ko.observable(-1);
                self.canEditOption = ko.observable(-1);
                self.canDeleteOption = true;
                self.translations = [];

                // Additional values
                self.value = ko.observable();
                self.valueHasFocus = ko.observable(false);
                self.valueTranslations = [];
                self.defaultValue = ko.observable();
                self.useDefaultValue = ko.observable(false);
                self.typeName = ko.observable();
                self.lastType = null;
                self.editableValue = self.getValueField();
                self.rowId = null;
                self.customType = ko.observable();
                self.customOptionTitle = ko.observable();
                self.customOptionTitleBinding = ko.observable();
                self.customOptionDefaultTitle = ko.observable();
                self.customOptionDefaultTitleBinding = ko.observable();
                self.calcType = ko.observable();
                self.translationsEnabled = parent.translationsEnabled;
                self.oldValues = {};

                // NOTE: useDefaultValueBinding should be registered before defaultValue and type
                // because in other case, when cancelling edit mode, it sets wrong values.
                self.registerFields(self.useDefaultValueBinding, self.key, self.defaultValueBinding, self.valueBinding,
                    self.type, self.customType, self.calcType, self.customOptionTitleBinding, self.customOptionDefaultTitleBinding, self.customOptionTitle, self.customOptionDefaultTitle);

                self.getOptionTypeName = function () {
                    var i,
                        type = self.calcType();
                    var optionTypesList = parent.optionTypes();
                    for (i = 0; i < optionTypesList.length; i++) {
                        if (optionTypesList[i].id == type) {
                            return optionTypesList[i].name;
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
                        self.customOptionTitleBinding('');
                        self.customOptionDefaultTitleBinding('');

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
                            var value = self.editableValue();
                            if (value && typeof value === "string") {
                                value = value.toLowerCase();
                            }
                            if (value !== 'true' && value !== true) {
                                self.editableValue(false);
                            } else {
                                self.editableValue(true);
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

                self.initDatePickers = function () {
                    self.parent.attachDatePickers();

                    var datePickerOpts = {
                        onSelect: function (newDate, datePicker) {
                            self.isSelected = true;
                            self.editableValue(newDate);

                            datePicker.input.focus();
                        },
                        onChangeMonthYear: function () {
                            self.isSelected = true;
                        },
                        onClose: function (selectedDate, datePicker) {
                        }
                    },
                        row = $('#' + self.rowId),
                        datePickerTrigger,
                        datePickers = row.find(selectors.datePickers);

                    if (!datePickers.hasClass(classes.hasDatePicker)) {
                        datePickers.initializeDatepicker(globalization.datePickerTooltipTitle, datePickerOpts);

                        datePickerTrigger = row.find(selectors.datePickerTrigger);

                        datePickerTrigger.on('click', function (event) {
                            self.valueHasFocus(true);
                        });
                        datePickerTrigger.on('focus', function (event) {
                            self.valueHasFocus(true);
                        });
                    }
                };

                self.preventFromSave = false;
                // Set values
                self.key(item.OptionKey);
                self.defaultValue(item.OptionDefaultValue);
                self.defaultValueBinding(item.OptionDefaultValue);
                self.valueBinding(item.OptionValue);
                self.value = ko.observable(item.OptionValue);
                self.type(item.Type);
                if (item.Type == optionTypes.customType) {
                    self.customType(item.CustomType);
                    self.customOptionDefaultTitleBinding(item.CustomOptionDefaultValueTitle);
                    self.customOptionDefaultTitle(item.CustomOptionDefaultValueTitle);
                    self.customOptionTitleBinding(item.CustomOptionValueTitle);
                    self.customOptionTitle(item.CustomOptionValueTitle);
                }
                self.calcType(self.getCalcType(self));
                self.canEditOption(item.CanEditOption !== false);
                self.useDefaultValueBinding(!self.canEditOption() && item.UseDefaultValue === true);
                self.useDefaultValue(!self.canEditOption() && item.UseDefaultValue === true);
                self.canDeleteOption = item.CanDeleteOption !== false;
                if (item.Translations != null) {
                    self.translations = item.Translations;
                }
                self.translationsEnabled = parent.showLanguages();

                if (item.ValueTranslations != null) {
                    self.valueTranslations = item.ValueTranslations;
                    self.valueTranslations.forEach(function (item) {
                        item.UseDefaultValue = false;
                    });
                }
                // Disable editing and deletion
                self.changeFieldsEditing();

                self.saveItem = function () {
                    _super.prototype.saveItem.call(self);
                    var newValue = self.defaultValueBinding();
                    if (newValue != null) {
                        newValue = newValue.toString();
                    }
                    var customOptionDefaultTitle = self.customOptionDefaultTitleBinding();
                    if (customOptionDefaultTitle != null) {
                        customOptionDefaultTitle = customOptionDefaultTitle.toString();
                    }

                    var languageId = parent.languageId();
                    if (languageId == "") {
                        self.defaultValue(newValue);
                        self.customOptionDefaultTitle(customOptionDefaultTitle);
                    }
                    else {
                        var translation = this.getTranslation(languageId);
                        if (translation == null) {
                            translation = { OptionValue: newValue, LanguageId: languageId, CustomOptionTitle: customOptionDefaultTitle };
                            this.translations.push(translation);
                        }
                        else {
                            translation.OptionValue = newValue;
                            translation.CustomOptionTitle = customOptionDefaultTitle;
                        }
                    }
                }

                self.getValueField().subscribe(function () {
                    // Closes expanded calendar, when user manually enters the value
                    var focusedElement = $(selectors.focusedElements);
                    if (focusedElement.length > 0 && $.isFunction(focusedElement.datepicker)) {
                        focusedElement.datepicker("hide");
                    }
                });

                self.onCustomOptionExecute = function (data, titleObservable, valueObservable) {
                    self.onItemSelect(self.valueHasFocus, data, null);

                    var customType = self.customType();
                    if (!customType) {
                        return;
                    }

                    for (ci = 0, cl = registeredCustomOptions.length; ci < cl; ci++) {
                        if (registeredCustomOptions[ci].identifier == customType) {
                            self.preventFromSave = true;

                            registeredCustomOptions[ci].onExecute(valueObservable, titleObservable, function () {
                                setTimeout(function () {
                                    self.preventFromSave = false;
                                }, 100);
                            });

                            return;
                        }
                    }
                };

                self.toJson = function () {
                    var result = {
                        CustomType: self.customType(),
                        OptionDefaultValue: self.defaultValue(),
                        OptionKey: self.key(),
                        Type: self.type(),
                        Translations: self.translations
                    };
                    if (result.Type == optionTypes.customType) {
                        result.CustomOption = { Identifier: self.customType() };
                    }

                    return result;
                };
            };

            OptionViewModel.prototype.getValueField = function () {
                return this.defaultValueBinding;
            };

            OptionViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteOptionConfirmMessage, antiXss.encodeHtml(this.key()));
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

            OptionViewModel.prototype.getTranslation = function (languageId) {
                var translationsArray = this.translations;
                if (translationsArray != null) {
                    for (var i = 0, len = translationsArray.length; i < len; i++) {
                        if (translationsArray[i].LanguageId == languageId) {
                            return translationsArray[i];
                        }
                    }
                }
                return null;
            }

            OptionViewModel.prototype.activateTranslation = function (languageId) {

                var defaultValue = this.ClearFixValue(this.defaultValue());

                var customOptionDefaultTitle = null;
                if (this.type() == optionTypes.customType) {
                    customOptionDefaultTitle = this.ClearFixValue(this.customOptionDefaultTitle());
                }
                if (languageId == "") {
                    this.defaultValueBinding(defaultValue);
                    this.customOptionDefaultTitleBinding(customOptionDefaultTitle);
                } else {
                    var translation = this.getTranslation(languageId);
                    if (translation == null) {
                        this.defaultValueBinding(defaultValue);
                        this.customOptionDefaultTitleBinding(customOptionDefaultTitle);
                    } else {
                        var translationValue = this.ClearFixValue(translation.OptionValue);
                        var translationCustomTitle = this.ClearFixValue(translation.CustomOptionTitle);
                        this.defaultValueBinding(translationValue);
                        this.customOptionDefaultTitleBinding(translationCustomTitle);
                    }
                }
            };

            OptionViewModel.prototype.ClearFixValue = function (value) {
                if (!value) {
                    return value;
                }
                if (this.type() == optionTypes.boolType) {
                    var lowerCaseValue = value.toLowerCase();
                    if (lowerCaseValue == "true") {
                        return true;
                    } else {
                        return false;
                    }
                }
                return value;
            }

            OptionViewModel.prototype.editItem = function () {
                _super.prototype.editItem.call(this);
                this.oldValues['key'] = this.key();
                this.oldValues['calcType'] = this.calcType();
                this.oldValues['customType'] = this.customType();
                this.oldValues['defaultValueBinding'] = this.defaultValueBinding();
                this.oldValues['customOptionDefaultTitleBinding'] = this.customOptionDefaultTitleBinding();
            };

            OptionViewModel.prototype.restoreOldValues = function () {
                this.useDefaultValueBinding(this.oldValues.useDefaultValueBinding);
                this.key(this.oldValues.key);
                this.defaultValueBinding(this.oldValues.defaultValueBinding);
                this.valueBinding(this.oldValues.valueBinding);
                this.customType(this.oldValues.customType);
                this.customOptionTitle(this.oldValues.customOptionTitle);
                this.customOptionDefaultTitleBinding(this.oldValues.customOptionDefaultTitleBinding);
            };

            OptionViewModel.prototype.onBodyClick = function (e) {
                if (!this.preventFromSave) {
                    _super.prototype.onBodyClick.call(this, e);
                }
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
                        self.useDefaultValueBinding(!self.useDefaultValueBinding());
                        bcms.stopEventPropagation(event);
                        if (self.useDefaultValueBinding.domElement) {
                            $(self.useDefaultValueBinding.domElement).focus();
                        }

                        return false;
                    }

                    return true;
                };

                self.useDefaultValueBinding.subscribe(function (newValue) {
                    var languageId = self.parent.languageId();
                    if (self.isActive()) {
                        if ((self.translationsEnabled && languageId == "") || !self.translationsEnabled) {
                            if (newValue) {
                                self.valueBinding(self.defaultValue());
                                self.customOptionTitleBinding(self.customOptionDefaultTitle());
                            } else {
                                var val = self.type() == optionTypes.boolType ? false : '';
                                self.valueBinding(val);
                                self.customOptionTitleBinding(val);
                            }
                        } else {
                            var translation = self.getTranslation(languageId);
                            if (newValue) {
                                var optionValue;
                                var customOptionValue;
                                if (translation != null) {
                                    optionValue = self.ClearFixValue(translation.OptionValue);
                                    customOptionValue = self.ClearFixValue(translation.CustomOptionTitle);
                                    self.valueBinding(optionValue);
                                    self.customOptionTitleBinding(customOptionValue);
                                } else {
                                    self.valueBinding(self.defaultValue());
                                    self.customOptionTitleBinding(self.customOptionDefaultTitle());
                                }
                                if (self.useDefaultValue() == false) {
                                    optionValue = self.ClearFixValue(self.value());
                                    customOptionValue = self.ClearFixValue(self.customOptionTitle());
                                    self.valueBinding(optionValue);
                                    self.customOptionTitleBinding(customOptionValue);
                                }
                            } else {
                                var val = self.type() == optionTypes.boolType ? false : '';
                                self.valueBinding(val);
                                self.customOptionTitleBinding(val);
                            }
                        }
                    }
                });

                self.toJson = function () {

                    var valueTranslationsFiltered = [];
                    self.valueTranslations.forEach(function (current) {
                        if (current.UseDefaultValue == false) {
                            valueTranslationsFiltered.push(current);
                        }
                    });
                    var result = {
                        OptionValue: self.value(),
                        UseDefaultValue: self.useDefaultValue(),
                        OptionDefaultValue: self.defaultValue(),
                        OptionKey: self.key(),
                        Type: self.type(),
                        ValueTranslations: valueTranslationsFiltered
                    };
                    if (result.Type == optionTypes.customType) {
                        result.CustomOption = { Identifier: self.customType() };
                    }

                    return result;
                };

                self.saveItem = function () {
                    _super.prototype.saveItem.call(self);
                    var useDefaultValue = self.useDefaultValueBinding();

                    var newValue = self.valueBinding();
                    if (newValue != null) {
                        newValue = newValue.toString();
                    }
                    var customOptionTitle = self.customOptionTitleBinding();
                    if (customOptionTitle != null) {
                        customOptionTitle = customOptionTitle.toString();
                    }

                    var languageId = parent.languageId();
                    if (languageId == "") {
                        self.value(newValue);
                        self.useDefaultValue(useDefaultValue);
                        self.customOptionTitle(customOptionTitle);
                    }
                    else {
                        var valueTranslation = this.getValueTranslation(languageId);
                        if (!useDefaultValue) {
                            if (valueTranslation == null) {
                                valueTranslation = { OptionValue: newValue, LanguageId: languageId, UseDefaultValue: useDefaultValue, CustomOptionTitle: customOptionTitle };
                                this.valueTranslations.push(valueTranslation);
                            } else {
                                valueTranslation.OptionValue = newValue;
                                valueTranslation.UseDefaultValue = useDefaultValue;
                                valueTranslation.CustomOptionTitle = customOptionTitle;
                            }
                        } else {
                            // remove the translation
                            if (valueTranslation != null) {
                                var index = self.valueTranslations.indexOf(valueTranslation);
                                if (index > -1) {
                                    self.valueTranslations.splice(index, 1);
                                }
                            }
                        }
                    }
                }
            }

            function changeFieldEditing(field, disable) {
                if (!field.editingIsDisabled) {
                    field.editingIsDisabled = ko.observable();
                }

                field.editingIsDisabled(disable);
            }

            OptionValueViewModel.prototype.getValueField = function () {
                return this.valueBinding;
            };

            OptionValueViewModel.prototype.changeFieldsEditing = function () {
                var disableEditing = this.canEditOption() === false;

                changeFieldEditing(this.key, disableEditing);
                changeFieldEditing(this.calcType, disableEditing);

                this.deletingIsDisabled(disableEditing);
            };

            OptionValueViewModel.prototype.editItem = function () {
                _super.prototype.editItem.call(this);

                this.oldValues['useDefaultValueBinding'] = this.useDefaultValueBinding();
                this.oldValues['valueBinding'] = this.valueBinding();
                this.oldValues['customOptionTitle'] = this.customOptionTitle();

                if (!this.editingIsDisabled() && this.useDefaultValueBinding() && this.useDefaultValueBinding.domElement) {
                    $(this.useDefaultValueBinding.domElement).focus();
                }
            };

            OptionValueViewModel.prototype.getValueTranslation = function (languageId) {
                var translationsArray = this.valueTranslations;
                if (translationsArray != null) {
                    for (var i = 0, len = translationsArray.length; i < len; i++) {
                        if (translationsArray[i].LanguageId == languageId) {
                            return translationsArray[i];
                        }
                    }
                }
                return null;
            }

            OptionValueViewModel.prototype.activateTranslation = function (languageId) {
                var defaultValue = this.ClearFixValue(this.defaultValue());
                var value = this.ClearFixValue(this.value());
                var customOptionTitle = null;
                var customOptionDefaultTitle = null;
                if (this.type() == optionTypes.customType) {
                    customOptionTitle = this.ClearFixValue(this.customOptionTitle());
                    customOptionDefaultTitle = this.ClearFixValue(this.customOptionDefaultTitle());
                }

                if (languageId == "") {
                    if (this.useDefaultValue() && value == null) {
                        this.valueBinding(defaultValue);
                        this.customOptionTitleBinding(customOptionDefaultTitle);
                    } else {
                        this.valueBinding(value);
                        this.customOptionTitleBinding(customOptionTitle);
                    }
                    this.useDefaultValueBinding(this.useDefaultValue());
                    this.customOptionDefaultTitleBinding(customOptionDefaultTitle);
                    this.defaultValueBinding(defaultValue);
                } else {
                    var translation = this.getTranslation(languageId);
                    var optionValue;
                    var customOptionTitleValue;
                    if (translation == null) {
                        this.defaultValueBinding(defaultValue);
                        this.customOptionDefaultTitleBinding(customOptionDefaultTitle);
                        this.valueBinding(defaultValue);
                        this.customOptionTitleBinding(customOptionDefaultTitle);
                    } else {
                        optionValue = this.ClearFixValue(translation.OptionValue);
                        customOptionTitleValue = this.ClearFixValue(translation.CustomOptionTitle);
                        this.defaultValueBinding(optionValue);
                        this.customOptionDefaultTitleBinding(customOptionTitleValue);
                        this.valueBinding(optionValue);
                        this.customOptionTitleBinding(customOptionTitleValue);
                    }

                    if (this.useDefaultValue() == false) {
                        this.defaultValueBinding(value);
                        this.customOptionDefaultTitleBinding(customOptionTitle);
                        this.valueBinding(value);
                        this.customOptionTitleBinding(customOptionTitle);
                    }

                    var valueTranslation = this.getValueTranslation(languageId);
                    if (valueTranslation != null) {
                        optionValue = this.ClearFixValue(valueTranslation.OptionValue);
                        customOptionTitleValue = this.ClearFixValue(valueTranslation.CustomOptionTitle);
                        this.valueBinding(optionValue);
                        this.customOptionTitleBinding(customOptionTitleValue);
                        this.useDefaultValueBinding(false);
                    } else {
                        this.useDefaultValueBinding(true);
                    }
                }
            };

            return OptionValueViewModel;
        })(OptionViewModel);

        /**
        * Language view model.
        */
        function LanguageViewModel(languages, languageId, optionsListViewModel) {
            var self = this,
                i, l;

            self.languageId = ko.observable(languageId);

            self.languages = [];
            self.languages.push({ key: '', value: globalization.invariantLanguage });
            for (i = 0, l = languages.length; i < l; i++) {
                self.languages.push({
                    key: languages[i].Key,
                    value: languages[i].Value
                });
            }

            self.languageId.subscribe(function (newValue) {
                if ($.isFunction(optionsListViewModel.onLanguageChanged)) {
                    optionsListViewModel.onLanguageChanged(newValue);
                }
            });

            return self;
        };

        /**
        * Creates options list view model
        */
        options.createOptionsViewModel = function (container, items, customOptions, showLanguages, languages) {
            return new OptionsListViewModel(container, items, customOptions, showLanguages, languages);
        };

        /**
        * Creates option values list view model
        */
        options.createOptionValuesViewModel = function (container, items, customOptions, showLanguages, languages) {
            return new OptionValuesListViewModel(container, items, customOptions, showLanguages, languages);
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
                    hasError = !self.useDefaultValueBinding() && newValue &&
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
