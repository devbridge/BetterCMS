/*global define, console */

define('bcms.ko.extenders', ['bcms.jquery', 'bcms', 'knockout'], function ($, bcms, ko) {
    'use strict';

    ko.globalization = {
        maximumLengthMessage: null,
        requiredFieldMessage: null
    };

    ko.maxLength = {
        email: 400,
        name: 200,
        text: 2000,
        url: 850,
        uri: 2000,
    };

    /**
    * Extend knockout handlers: add Enter key press event handler
    */
    ko.bindingHandlers.enterPress = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var allBindings = allBindingsAccessor();
           
            bcms.preventInputFromSubmittingForm($(element), {
                preventedEnter: function () {
                    allBindings.enterPress.call(viewModel, element);
                }
            });
        }
    };
    
    /**
    * Extend knockout handlers: add Esc key press event handler
    */
    ko.bindingHandlers.escPress = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var allBindings = allBindingsAccessor();
            
            bcms.preventInputFromSubmittingForm($(element), {
                preventedEsc: function () {
                    allBindings.escPress.call(viewModel, element);
                }
            });
        }
    };

    /**
    * Extend knockout handlers: stop binding to child elements
    */
    ko.bindingHandlers.stopBindings = {
        init: function () {
            return { controlsDescendantBindings: true };
        }
    };

    /**
    * Knockout validation controller
    */
    function KnockoutValidator(target) {
        var self = this;

        self.rules = [];
        self.target = target;

        self.registerRule = function(ruleName) {
            var rule = getRule(ruleName);
            
            if (!rule) {
                self.rules[self.rules.length] = {
                    name: ruleName,
                    hasError: false,
                    message: ''
                };
            }
        };

        self.setError = function (ruleName, hasError, message) {
            var rule = getRule(ruleName);

            if (rule) {
                rule.hasError = hasError;
                rule.message = hasError ? message : '';

                self.target.hasError(hasAnyErrors());
                self.target.validationMessage(getValidationMessages());
            }
        };

        function hasAnyErrors() {
            for (var i = 0; i < self.rules.length; i++) {
                if (self.rules[i].hasError === true) {
                    return true;
                }
            }
            return false;
        }

        function getValidationMessages() {
            var messages = "";

            for (var i = 0; i < self.rules.length; i++) {
                if (self.rules[i].hasError === true && self.rules[i].message) {
                    if (messages) {
                        messages += "<br />";
                    }
                    messages += self.rules[i].message;
                }
            }

            return messages;
        }

        function getRule(ruleName) {
            var length = self.rules.length;
            if (!length) {
                return null;
            }

            for (var i = 0; i < length; i++) {
                if (self.rules[i].name == ruleName) {
                    return self.rules[i];
                }
            }
            return null;
        };
    }

    /**
    * Extend knockout: add required value validation
    */
    ko.extenders.required = function (target, overrideMessage) {
        var ruleName = 'required';
        return koValidationExtender(ruleName, target, function (newValue) {
            var hasError = (!newValue),
                message = hasError ? overrideMessage || ko.globalization.requiredFieldMessage : "";

            target.validator.setError(ruleName, hasError, message);
        });
    };

    /**
    * Extend knockout: add maximum length validation
    */
    ko.extenders.maxLength = function (target, options) {
        var ruleName = 'maxLength',
            maxLength = options.maxLength,
            message = options.message || ko.globalization.maximumLengthMessage;
        return koValidationExtender(ruleName, target, function (newValue) {
            var hasError = (newValue != null && newValue.length > maxLength),
                showMessage = hasError ? $.format(message, maxLength) : '';
            
            target.validator.setError(ruleName, hasError, showMessage);
        });
    };

    /**
    * Knockout validation extender
    */
    function koValidationExtender(ruleName, target, validate) {
        // add some sub-observables to our observable
        if (!target.hasError) {
            target.hasError = ko.observable(false);
            target.validationMessage = ko.observable();
            target.validator = new KnockoutValidator(target);
        }
        target.validator.registerRule(ruleName);

        // initial validation
        validate(target());

        // validate whenever the value changes
        target.subscribe(validate);

        // return the original observable
        return target;
    }

    return ko;
});