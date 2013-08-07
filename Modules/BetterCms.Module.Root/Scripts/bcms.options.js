/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

bettercms.define('bcms.options', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.ko.grid'],
    function ($, bcms, ko, kogrid) {
        'use strict';

        var options = {},
            selectors = {},
            links = {},
            globalization = {
                deleteOptionConfirmMessage: null,
                optionTypeText: null,
                optionTypeInteger: null,
                optionTypeBoolean: null,
                optionTypeDateTime: null,
                optionTypeFloat: null
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
                self.defaultValue = ko.observable();
                self.type = ko.observable();
                self.typeName = ko.observable();

                self.optionTypes = [];
                self.optionTypes.push({ id: optionTypes.textType, name: globalization.optionTypeText });
                self.optionTypes.push({ id: optionTypes.integerType, name: globalization.optionTypeInteger });
                self.optionTypes.push({ id: optionTypes.floatType, name: globalization.optionTypeFloat });
                self.optionTypes.push({ id: optionTypes.dateTimeType, name: globalization.optionTypeDateTime });
                self.optionTypes.push({ id: optionTypes.boolType, name: globalization.optionTypeBoolean });
            
                self.registerFields(self.key, self.defaultValue, self.type);

                self.type.subscribe(function (newValue) {
                    var i;
                    
                    for (i = 0; i < self.optionTypes.length; i++) {
                        if (self.optionTypes[i].id == newValue) {
                            self.typeName(self.optionTypes[i].name);

                            return;
                        }
                    }
                });

                self.key(item.OptionKey);
                self.defaultValue(item.OptionDefaultValue);
                self.type(item.Type);
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
