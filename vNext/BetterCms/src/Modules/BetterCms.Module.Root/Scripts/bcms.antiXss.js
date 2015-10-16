bettercms.define("bcms.antiXss", ['bcms'],
    function (bcms) {
        "use strict";

        var antiXss = {},
            globalization = {
                antiXssContainsHtmlError: null
            };

        /**
        * Assign objects to module.
        */
        antiXss.globalization = globalization;

        function MakeTextHtmlSafe(str) {
            return String(str)
                .replace(/&/g, "&amp;")
                .replace(/</g, "&lt;")
                .replace(/>/g, "&gt;")
                .replace(/"/g, "&quot;")
                .replace(/'/g, "&#039;")
                .replace(/\//g, "&#x2F;");
        }

        antiXss.encodeHtml = function(html) {
            return MakeTextHtmlSafe(html);
        }

        /**
        * Registers bcms antiXss module
        */
        antiXss.init = function() {
            bcms.logger.debug('Initializing bcms.antiXss module');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(antiXss.init);

        return antiXss;
    });