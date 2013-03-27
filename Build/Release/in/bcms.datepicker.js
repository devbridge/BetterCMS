/*global define, console */

define('bcms.datepicker', ['bcms.jquery', 'bcms', 'bcms.jquery.validate.unobtrusive'], function ($, bcms) {
    'use strict';

    var datepicker = {},
        links = {
            calendarImageUrl: '/file/bcms-root/Content/Styles/images/icn-calendar.png'
        },
        globalization = {};

    // Assign objects to module.
    datepicker.links = links;
    datepicker.globalization = globalization;

    datepicker.init = function () {
        console.log('Initializing bcms.datepicker module');
        $.validator.addMethod("jqdatevalidation", function(value, element, params) {
        if (element.value) {
            return isValidDate(element.value);
        }

        return true;
    },
        function(params) {
            return params.message;
        });

    $.validator.unobtrusive.adapters.add("datevalidation", [], function(options) {
        options.rules["jqdatevalidation"] = { message: options.message };
    });

    function isValidDate(s) {
        if (s.search(/^\d{1,2}[\/|\-|\.|_]\d{1,2}[\/|\-|\.|_]\d{4}/g) != 0) {
            return false;
        }
        s = s.replace(/[\-|\.|_]/g, "/");

        var dt = new Date(Date.parse(s));
        var arrDateParts = s.split("/");
        return (
        dt.getMonth() == arrDateParts[0] - 1 &&
            dt.getDate() == arrDateParts[1] &&
                dt.getFullYear() == arrDateParts[2]
    );
    }

        $.fn.initializeDatepicker = function (tooltipTitle) {
            $(this).datepicker({
                showOn: 'button',
                buttonImage: links.calendarImageUrl,
                buttonImageOnly: true
            });
            if (tooltipTitle == null) {
                tooltipTitle = "";
            }
            $(this).datepicker("option", "buttonText", tooltipTitle);
        };
    };

    bcms.registerInit(datepicker.init);

    return datepicker;
});