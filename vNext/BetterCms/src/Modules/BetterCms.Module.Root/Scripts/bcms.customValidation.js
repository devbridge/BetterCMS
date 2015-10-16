bettercms.define("bcms.customValidation", ['bcms.jquery', 'bcms'],
    function ($, bcms) {
        "use strict";

        var customValidation = {},
            globalization = {
                antiXssContainsHtmlError: null
            };

        /**
        * Assign objects to module.
        */
        customValidation.globalization = globalization;

        /**
        * Registers bcms customValidation module
        */
        customValidation.init = function () {
            bcms.logger.debug("Initializing bcms.customValidation module");

            $.validator.addMethod("jqdisallowhtml", function (value, element, params) {
                if (!value) {
                    return true;
                }
                var match = new RegExp(params.pattern).exec(value);
                return !match;
            }, function (params) {
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("disallowhtml", ["pattern"], function (opts) {
                opts.rules["jqdisallowhtml"] = { message: opts.message, pattern: opts.params.pattern };
            });

            $.validator.addMethod("jqdisallownonalphanumeric", function (value, element, params) {
                if (!value) {
                    return true;
                }
                var match = new RegExp(params.pattern).exec(value);
                return match;
            }, function (params) {
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("disallownonalphanumeric", ["pattern"], function (opts) {
                opts.rules["jqdisallownonalphanumeric"] = { message: opts.message, pattern: opts.params.pattern };
            });

            $.validator.addMethod("jqdisallownonactivedirectorynamecompliant", function (value, element, params) {
                if (!value) {
                    return true;
                }
                var match = new RegExp(params.pattern).exec(value);
                return match;
            }, function (params) {
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("disallownonactivedirectorynamecompliant", ["pattern"], function (opts) {
                opts.rules["jqdisallownonactivedirectorynamecompliant"] = { message: opts.message, pattern: opts.params.pattern };
            });
        };

        /**
        * Register initialization
        */
        bcms.registerInit(customValidation.init);

        return customValidation;
    });