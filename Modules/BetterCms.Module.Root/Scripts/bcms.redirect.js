/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.redirect.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.redirect', ['bcms.jquery', 'bcms', 'bcms.modal'], function ($, bcms, modal) {
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
            disableAccept: true
        });
    }

    return redirect;
});
