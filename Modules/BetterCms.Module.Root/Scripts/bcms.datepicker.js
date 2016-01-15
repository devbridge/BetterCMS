/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.datepicker.js" company="Devbridge Group LLC">
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
bettercms.define('bcms.datepicker', ['bcms.jquery', 'bcms', 'bcms.jquery.validate.unobtrusive'], function ($, bcms) {
    'use strict';

    var datepicker = {},
        links = {
            calendarImageUrl: null
        },
        globalization = {
            dateFormat: "mm/dd/yy",
            currentCulture: null
        };

    // Assign objects to module.
    datepicker.links = links;
    datepicker.globalization = globalization;

    datepicker.isDateValid = function (value) {
        var ok = true;
        try {
            $.datepicker.parseDate(globalization.dateFormat, value);
        } catch (err) {
            ok = false;
        }
        return ok;
    };
    
    datepicker.parseDate = function (value)
    {
        try {
            return $.datepicker.parseDate(globalization.dateFormat, value);
        } catch (err) {
            // Ignore errors.
        }
        return null;
    };

    datepicker.init = function () {
        bcms.logger.debug('Initializing bcms.datepicker module');
        bcms.logger.trace('    globalization.dateFormat = "' + globalization.dateFormat + '".');
        bcms.logger.trace('    globalization.currentCulture = "' + globalization.currentCulture + '".');
        
        $.validator.addMethod("jqdatevalidation", function(value, element, params) {
            if (element.value) {
                return datepicker.isDateValid(element.value);
            }

            return true;
        },
            function(params) {
                return params.message;
            });

        $.validator.unobtrusive.adapters.add("datevalidation", [], function (options) {
            options.rules["jqdatevalidation"] = { message: options.message };
        });
        
        $.validator.addMethod('date',
            function(value, element) {
                if (this.optional(element)) {
                    return true;
                }
                return datepicker.isDateValid(value);
            });

        $.fn.initializeDatepicker = function (tooltipTitle, opts) {
            var options = $.extend({
                showOn: 'button',
                buttonImage: links.calendarImageUrl,
                buttonImageOnly: true,
                dateFormat: globalization.dateFormat
            }, opts);

            $(this).datepicker(options);
            if (tooltipTitle == null) {
                tooltipTitle = "";
            }
            $(this).datepicker("option", "buttonText", tooltipTitle);
        };
    };

    bcms.registerInit(datepicker.init);

    return datepicker;
});