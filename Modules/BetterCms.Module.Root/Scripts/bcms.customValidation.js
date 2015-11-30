/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.customValidation.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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