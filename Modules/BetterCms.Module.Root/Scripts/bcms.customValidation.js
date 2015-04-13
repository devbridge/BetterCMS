﻿bettercms.define("bcms.customValidation", ['bcms.jquery', 'bcms'],
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
                    return false;
                }
                var match = new RegExp(params.pattern).exec(value);
                return !match;
            }, function (params) {
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("disallowhtml", ["pattern"], function (opts) {
                opts.rules["jqdisallowhtml"] = { message: opts.message, pattern: opts.params.pattern };
            });
        };

        /**
        * Register initialization
        */
        bcms.registerInit(customValidation.init);

        return customValidation;
    });