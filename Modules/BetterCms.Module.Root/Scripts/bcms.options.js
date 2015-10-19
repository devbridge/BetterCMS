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

            function OptionsListViewModel(container, items, customOptions, showLanguages, languages) {
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
                        result.push(allItems[i].toJson());
                    }

                    return result;
                };

                self.showLanguages = ko.observable(showLanguages);
                self.languages = ko.observableArray();
                self.languageId = ko.observable("");
                self.language = showLanguages ? new LanguageViewModel(languages, null, self) : null;
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

            function OptionValuesListViewModel(container, items, customOptions, showLanguages, languages) {
                _super.call(this, container, items, customOptions, showLanguages, languages);

                var self = this;

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

            //            OptionValuesListViewModel.prototype.onAfterNewItemAdded = function (item) {
            //                if (item.canEditOption() !== false && item.key.domElement) {
            //                    $(item.key.domElement).focus();
            //                }
            //            };

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

                $(selectors.datePickerBox).mousedown(function (event) {
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

                self.valueTranslations = [];
                self.defaultValue = ko.observable();
                self.useDefaultValue = ko.observable(false);
                self.typeName = ko.observable();
                self.lastType = null;
                self.editableValue = self.getValueField();
                self.rowId = null;
                self.customType = ko.observable();
                self.customOptionTitle = ko.observable();
                self.customOptionDefaultTitle = ko.observable();
                self.calcType = ko.observable();
                self.translationsEnabled = false;

                // NOTE: useDefaultValueBinding should be registered before defaultValue and type
                // because in other case, when cancelling edit mode, it sets wrong values.
                self.registerFields(self.useDefaultValueBinding, self.key, self.defaultValueBinding, self.valueBinding,
                    self.type, self.customType, self.calcType, self.customOptionTitle, self.customOptionDefaultTitle);

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
                            setTimeout(function () {
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
                self.defaultValueBinding(item.OptionDefaultValue);
                self.valueBinding(item.OptionValue);
                self.value = ko.observable(item.OptionValue);
                self.type(item.Type);
                if (item.Type == optionTypes.customType) {
                    self.customType(item.CustomType);
                    self.customOptionDefaultTitle(item.CustomOptionDefaultValueTitle);
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
                if (self.translationsEnabled) {
                    if (item.UseDefaultValue && item.OptionValue == null) {
                        self.valueBinding(item.OptionDefaultValue);
                    }
                }
                if (item.ValueTranslations != null) {
                    self.valueTranslations = item.ValueTranslations;
                    self.valueTranslations.forEach(function (item) {
                        item.UseDefaultValue = false;
                    });
                }
                // Disable editing and deletion
                self.changeFieldsEditing();

                self.useDefaultValueBinding.subscribe(function (newValue) {
                    if (self.isActive()) {
                        if ((self.translationsEnabled && self.languageId == "") || !self.translationsEnabled) {
                            if (newValue) {
                                self.value(self.defaultValue());
                                self.valueBinding(self.defaultValue());
                                self.customOptionTitle(self.customOptionDefaultTitle());
                            } else {
                                self.value('');
                                self.valueBinding('');
                                self.customOptionTitle('');
                            }
                        } else {
                            var valueTranslation = self.getValueTranslation(self.languageId);
                            var translation = self.getTranslation(self.languageId);
                            if (newValue) {
                                // remove translation if true
                                if (valueTranslation != null) {
                                    var index = self.valueTranslations.indexOf(valueTranslation);
                                    if (index > -1) {
                                        self.valueTranslations.splice(index, 1);
                                    }
                                }

                                if (translation != null) {
                                    self.valueBinding(translation.OptionValue);
                                } else {
                                    self.valueBinding(self.defaultValue());
                                }
                            } else {
                                self.valueBinding('');
                            }
                        }
                    }
                });

                self.saveItem = function () {
                    _super.prototype.saveItem.call(self);
                    var newValue = "";
                    if (self.type() != optionTypes.customType) {
                        newValue = self.defaultValueBinding();
                    } else {
                        newValue = self.customOptionDefaultTitle();
                    }
                    if (!self.translationsEnabled) {
                        self.defaultValue(newValue);
                        return;
                    }
                    var languageId = parent.languageId();
                    if (languageId == "") {
                        self.defaultValue(newValue);
                    }
                    else {
                        var translation = this.getTranslation(languageId);
                        if (translation == null) {
                            translation = { OptionValue: newValue, LanguageId: languageId };
                            this.translations.push(translation);
                        }
                        else {
                            translation.OptionValue = newValue;
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

                if (languageId == "") {
                    this.defaultValueBinding(defaultValue);
                    this.customOptionDefaultTitle(defaultValue);
                } else {
                    var translation = this.getTranslation(languageId);
                    if (translation == null) {
                        this.defaultValueBinding(defaultValue);
                        this.customOptionDefaultTitle(defaultValue);
                    } else {
                        var translationValue = this.ClearFixValue(translation.OptionValue);
                        this.defaultValueBinding(translationValue);
                        this.customOptionDefaultTitle(translationValue);
                    }
                }
            };

            OptionViewModel.prototype.ClearFixValue = function (value) {
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

                    var newValue = "";
                    if (self.type() != optionTypes.customType) {
                        newValue = self.valueBinding();
                    } else {
                        newValue = self.customOptionTitle();
                    }

                    if (!self.translationsEnabled) {
                        self.defaultValue(newValue);
                        self.useDefaultValue(useDefaultValue);
                        return;
                    }
                    var languageId = parent.languageId();
                    if (languageId == "") {
                        self.value(newValue);
                        self.useDefaultValue(useDefaultValue);
                    }
                    else {
                        var valueTranslation = this.getValueTranslation(languageId);
                        if (valueTranslation == null) {
                            valueTranslation = { OptionValue: newValue, LanguageId: languageId, UseDefaultValue: useDefaultValue };
                            this.valueTranslations.push(valueTranslation);
                        }
                        else {
                            valueTranslation.OptionValue = newValue;
                            valueTranslation.UseDefaultValue = useDefaultValue;
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
                if (languageId == "") {
                    if (this.useDefaultValue() && value == null) {
                        this.valueBinding(defaultValue);
                        this.customOptionTitle(defaultValue);
                    } else {
                        this.value();
                    }
                    this.useDefaultValueBinding(this.useDefaultValue());
                    this.defaultValueBinding(defaultValue);
                } else {
                    var valueTranslation = this.getValueTranslation(languageId);
                    var optionValue;
                    if (valueTranslation == null) {
                        this.valueBinding(defaultValue);
                        this.customOptionTitle(defaultValue);
                        this.useDefaultValueBinding(this.useDefaultValue());
                    } else {
                        optionValue = this.ClearFixValue(valueTranslation.OptionValue);
                        this.valueBinding(optionValue);
                        this.customOptionTitle(optionValue);
                        this.useDefaultValueBinding(optionValue);
                    }
                    var translation = this.getTranslation(languageId);
                    if (translation == null) {
                        this.defaultValueBinding(defaultValue);
                        this.customOptionDefaultTitle(defaultValue);
                    } else {
                        optionValue = this.ClearFixValue(translation.OptionValue);
                        this.defaultValueBinding(optionValue);
                        this.customOptionDefaultTitle(optionValue);
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
