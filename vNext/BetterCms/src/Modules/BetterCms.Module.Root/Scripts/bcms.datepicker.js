/*global bettercms */

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