/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.options', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.ko.grid', 'bcms.datepicker'],
    function ($, bcms, ko, kogrid, datePicker) {
        'use strict';

        var options = {},
            selectors = {
                datePickers: '.bcms-datepicker',
                datePickerBoxes: '.ui-datepicker-trigger, .ui-datepicker'
            },
            links = {},
            globalization = {
                deleteOptionConfirmMessage: null,
                optionTypeText: null,
                optionTypeInteger: null,
                optionTypeBoolean: null,
                optionTypeDateTime: null,
                optionTypeFloat: null,
                datePickerTooltipTitle: null,
                optionValidationMessage: null
            },
            optionTypes = {
                textType: 1,
                integerType: 2,
                floatType: 3,
                dateTimeType: 4,
                boolType: 5
            };

        /**
        * Assign objects to module.
        */
        options.links = links;
        options.globalization = globalization;
        options.selectors = selectors;

        /**
        * Options list view model
        */
        var OptionsListViewModel = (function (_super) {

            bcms.extendsClass(OptionsListViewModel, _super);

            function OptionsListViewModel(container, items) {
                _super.call(this, container, null, items, null);
            };

            OptionsListViewModel.prototype.createItem = function (item) {
                var newItem = new OptionViewModel(this, item);
                return newItem;
            };

            return OptionsListViewModel;

        })(kogrid.ListViewModel);

        /**
        * Option view model
        */
        var OptionViewModel = (function (_super) {
        
            bcms.extendsClass(OptionViewModel, _super);

            function OptionViewModel(parent, item) {
                _super.call(this, parent, item);

                var self = this;

                self.key = ko.observable().extend({ required: "", maxLength: { maxLength: ko.maxLength.name } });
                self.defaultValue = ko.observable().extend({ optionValue: { self: self } }).extend({ notify: 'always' });
                self.type = ko.observable();
                self.typeName = ko.observable();
                self.lastType = null;

                self.optionTypes = [];
                self.optionTypes.push({ id: optionTypes.textType, name: globalization.optionTypeText });
                self.optionTypes.push({ id: optionTypes.integerType, name: globalization.optionTypeInteger });
                self.optionTypes.push({ id: optionTypes.floatType, name: globalization.optionTypeFloat });
                self.optionTypes.push({ id: optionTypes.dateTimeType, name: globalization.optionTypeDateTime });
                self.optionTypes.push({ id: optionTypes.boolType, name: globalization.optionTypeBoolean });
            
                self.registerFields(self.key, self.defaultValue, self.type);

                self.getOptionTypeName = function() {
                    var i,
                        type = self.type();

                    for (i = 0; i < self.optionTypes.length; i++) {
                        if (self.optionTypes[i].id == type) {
                            return self.optionTypes[i].name;
                        }
                    }

                    return '';
                };

                self.type.subscribe(function (newType) {
                    var oldType = self.lastType;

                    // Entering boolean mode
                    if (oldType == optionTypes.boolType) {
                        self.defaultValue('');
                    }
                    
                    // Leaving boolean mode
                    if (newType == optionTypes.boolType) {
                        if (self.defaultValue() !== 'true' && self.defaultValue() !== true) {
                            self.defaultValue(false);
                        }
                    }

                    // Save new value
                    self.lastType = newType;

                    // Set type name
                    self.typeName(self.getOptionTypeName());
                    
                    // Notify value to be re-validated
                    self.defaultValue(self.defaultValue());
                });

                self.key(item.OptionKey);
                self.defaultValue(item.OptionDefaultValue);
                self.type(item.Type);

                self.initDatePickers = function () {
                    var datePickerOpts = {
                        onSelect: function (newDate) {
                            self.isSelected = true;
                            self.defaultValue(newDate);
                        }
                    };

                    $(selectors.datePickers).initializeDatepicker(globalization.datePickerTooltipTitle, datePickerOpts);
                    $(selectors.datePickerBoxes).on('click', self.onItemSelect);
                    $(selectors.datePickerBoxes).on('blur', self.onBlurField);
                };
            };

            OptionViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteOptionConfirmMessage, this.key());
            };

            return OptionViewModel;

        })(kogrid.ItemViewModel);

        /**
        * Creates options list view model
        */
        options.createOptionsViewModel = function(container, items) {
            return new OptionsListViewModel(container, items);
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
                    hasError = mustBeNumber && newValue && isNaN(Number(newValue)),
                    showMessage,
                    regExp;
                
                if (!hasError && type == optionTypes.integerType) {
                    regExp = new RegExp(/^-?\d*$/);
                    hasError = !regExp.test(newValue);
                }

                showMessage = hasError ? $.format(globalization.optionValidationMessage, self.key(), self.getOptionTypeName()) : '';

                target.validator.setError(ruleName, hasError, showMessage);
            });
        };

        /**
        * Initializes bcms options module.
        */
        options.init = function () {
            console.log('Initializing bcms.options module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(options.init);

        return options;
});
