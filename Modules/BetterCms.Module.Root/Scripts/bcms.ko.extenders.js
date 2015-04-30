/*global bettercms */

bettercms.define('bcms.ko.extenders', ['bcms.jquery', 'bcms', 'knockout', 'bcms.antiXss'], function ($, bcms, ko, antiXss) {
    'use strict';

    ko.globalization = {
        maximumLengthMessage: null,
        requiredFieldMessage: null,
        invalidEmailMessage: null,
        invalidKeyMessage: null,
        nonAlphanumericMessage: null,
        activeDirectoryCompliantMessage : null
    },

    ko.maxLength = {
        email: 400,
        name: 200,
        text: 2000,
        url: 850,
        uri: 2000
    };

    /**
    * Extend knockout handlers: add Enter key press event handler
    */
    ko.bindingHandlers.enterPress = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var allBindings = allBindingsAccessor();
           
            bcms.preventInputFromSubmittingForm($(element), {
                preventedEnter: function (el, e) {
                    allBindings.enterPress.call(viewModel, viewModel, e);
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
                preventedEsc: function (el, e) {
                    allBindings.escPress.call(viewModel, viewModel, e);
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
    * Extend knockout handlers: add value with boolean value fix.
    */
    ko.bindingHandlers.valueBinder = {
        update: function (element, valueAccessor) {
            if ($.isFunction(valueAccessor)) {
                var observable = valueAccessor();
                if ($.isFunction(observable)) {
                    var value = observable();
                    if (typeof value === "boolean") {
                        $(element).val(value === true ? "true" : "false");

                        return;
                    }
                }
            }

            ko.bindingHandlers.value.update(element, valueAccessor);
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
        return ko.extenders.koValidationExtender(ruleName, target, function (newValue) {
            newValue = $.trim(newValue);

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
        return ko.extenders.koValidationExtender(ruleName, target, function (newValue) {
            var hasError = (newValue != null && newValue.length > maxLength),
                showMessage = hasError ? $.format(message, maxLength) : '';
            
            target.validator.setError(ruleName, hasError, showMessage);
        });
    };

    /**
    * Extend knockout: add validation against HTML
    */
    ko.extenders.preventHtml = function (target, options) {
        options = $.extend({
            pattern: "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>",
            message: ko.globalization.invalidKeyMessage
    }, options);
        return ko.extenders.doNotMatchRegularExpression(target, options);
    };

    /**
    * Extend knockout: add validation against non-alphanumeric
    */
    ko.extenders.preventNonAlphanumeric = function (target, options) {
        options = $.extend({
            pattern: "^[a-zA-Z0-9]*$",
            message: ko.globalization.nonAlphanumericMessage
        }, options);
        return ko.extenders.regularExpression(target, options);
    };

    /**
    * Extend knockout: add validation against non-username-compliant
    */
    ko.extenders.activeDirectoryCompliant = function (target, options) {
        options = $.extend({
            pattern: /[\\\/\"\[\]\:\;\|\=\,\+\*\?\<\>\%]/,
            message: ko.globalization.activeDirectoryCompliantMessage,
            isconstructedregex: true
        }, options);
        return ko.extenders.doNotMatchRegularExpression(target, options);
    };

    /**
    * Extend knockout: add regular expression validation
    */
    ko.extenders.email = function (target, options) {
        options = $.extend({
            pattern: '^[a-zA-Z0-9.!#$%&\'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)+$',
            message: ko.globalization.invalidEmailMessage
        }, options);
        return ko.extenders.regularExpression(target, options);
    };

    var baseRegularExpressionExtender = function (target, options, ruleName, globalValidationMessage, shouldMatch) {
        var pattern = options.pattern || '',
            message = options.message || globalValidationMessage;
        return ko.extenders.koValidationExtender(ruleName, target, function (newValue) {
            var hasError;

            var regExp;

            if (options.isconstructedregex === true && pattern) {
                regExp = pattern;
            } else {
                regExp = new RegExp(pattern, "i");
            }

            // if we're validating that the input SHOULD match the regexp
            if (shouldMatch) {
                hasError = (newValue != null && pattern && !newValue.match(regExp));
            }
            // if we're validating that the input SHOULD NOT match the regexp
            else {
                hasError = (newValue != null && pattern && newValue.match(regExp) != null);
            }

            var showMessage = hasError ? $.format(message, pattern) : '';
            target.validator.setError(ruleName, hasError, showMessage);
        });
    }

    /**
    * Extend knockout: add regular expression validation
    */
    ko.extenders.regularExpression = function (target, options) {
        return baseRegularExpressionExtender(target, options, 'regularExpression', ko.globalization.regularExpressionMessage, true);
    };

    /*
    ** Extend knockout: add 'does not' match regular expression validation
    */

    ko.extenders.doNotMatchRegularExpression = function (target, options) {
        return baseRegularExpressionExtender(target, options, 'doNotMatchRegularExpression', ko.globalization.regularExpressionMessage, false);
    };

    /**
    * Knockout validation extender
    */
    ko.extenders.koValidationExtender = function(ruleName, target, validate) {
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
    };

    ko.PagingViewModel = (function () {
        function PagingViewModel(pageSize, pageNumber, totalCount, onOpenPage) {
            var self = this;

            self.totalPagingLinks = 5;
            self.activePagePosition = 2;

            self.pageSize = 0;
            self.pageNumber = ko.observable(1);
            self.totalPages = ko.observable(1);
            self.pagingUpperBound = ko.observable(1);
            self.pagingLowerBound = ko.observable(1);
            self.totalCount = 0;

            self.pages = ko.computed(function () {
                var pages = [];
                for (var i = self.pagingLowerBound(); i <= self.pagingUpperBound(); i++) {
                    pages.push(i);
                }
                return pages;
            });

            self.openPage = function (pageNr) {
                self.pageNumber(pageNr);

                if ($.isFunction(onOpenPage)) {
                    onOpenPage(pageNr);
                }
            };

            self.setPaging = function (newPageSize, newPageNumber, newTotalCount) {
                self.totalCount = newTotalCount > 0 ? newTotalCount : 1;
                
                if (newPageSize > 0) {
                    if (newPageNumber <= 0) {
                        newPageNumber = 1;
                    }
                    if (newTotalCount < 0) {
                        newTotalCount = 0;
                    }
                    var totalPages = parseInt(Math.ceil(newTotalCount / newPageSize));
                    totalPages = totalPages > 0 ? totalPages : 1;

                    self.pageSize = newPageSize;
                    self.pageNumber(newPageNumber);
                    self.totalPages(totalPages);
                    self.totalCount = newTotalCount;

                    // lower bound
                    var pagingLowerBound = newPageNumber - self.activePagePosition;
                    if (pagingLowerBound < 1) {
                        pagingLowerBound = 1;
                    }

                    // upper bound
                    var pagingUpperBound = pagingLowerBound + self.totalPagingLinks;
                    if (pagingUpperBound > totalPages) {
                        pagingUpperBound = totalPages;
                    }

                    // lower bound correction
                    if (pagingUpperBound - pagingLowerBound < self.totalPagingLinks) {
                        pagingLowerBound = pagingUpperBound - self.totalPagingLinks;
                        if (pagingLowerBound < 1) {
                            pagingLowerBound = 1;
                        }
                    }

                    self.pagingLowerBound(pagingLowerBound);
                    self.pagingUpperBound(pagingUpperBound);
                }
            };
            
            self.setPaging(pageSize, pageNumber, totalCount);
        }

        return PagingViewModel;
    })();

    return ko;
});