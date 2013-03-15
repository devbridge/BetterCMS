/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console, document */

define('bcms.redirect', ['bcms.jquery', 'bcms', 'bcms.modal'], function ($, bcms, modal) {
    'use strict';

    var redirect = {},
        selectors = {},
        links = {},
        globalization = {
            reloadingPageTitle: null,
            reloadingPageMessage: null,
            redirectingPageTitle: null,
            redirectingPageMessage: null
        };

    /**
    * Assign objects to module.
    */
    redirect.selectors = selectors;
    redirect.links = links;
    redirect.globalization = globalization;

    /**
    * Shows alert and redirects a page to given url
    */
    redirect.RedirectWithAlert = function (url, options) {
        options = $.extend({
            title: globalization.redirectingPageTitle,
            message: globalization.redirectingPageMessage,
            timeout: 0
        }, options);

        showAlert(options);

        setTimeout(function() {
            window.location.href = url;
        }, options.timeout);
    };
    
    /**
    * Shows alert and reload a page
    */
    redirect.ReloadWithAlert = function (options) {
        options = $.extend({
            title: globalization.reloadingPageTitle,
            message: globalization.reloadingPageMessage,
            timeout: 0
        }, options);

        showAlert(options);

        setTimeout(bcms.reload, options.timeout);
    };

    /**
    * Function shows alert with information about redirecting/reloading page
    */
    function showAlert(options) {
        modal.info({
            title: options.title,
            content: options.message,
            disableCancel: true,
            disableAccept: true,
        });
    }

    return redirect;
});
