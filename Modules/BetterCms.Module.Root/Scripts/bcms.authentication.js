/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.authentication.js" company="Devbridge Group LLC">
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
bettercms.define('bcms.authentication', ['bcms.jquery', 'bcms', 'bcms.modal'],
    function ($, bcms, modal) {
    'use strict';

    var authentication = {},
        selectors = {
            sideManuHeader: ".bcms-sidemenu-header",
            logoutButton: ".bcms-btn-logout",
            viewAsPublicLink: ".bcms-as-public"
        },
        links = {
            logoutUrl: null
        },
        globalization = {
            confirmLogoutMessage: null
        },
        getParams = {
            viewAsAnonymous: 'bcms-view-page-as-anonymous'
        };

    // Assign objects to module.
    authentication.links = links;
    authentication.selectors = selectors;
    authentication.globalization = globalization;
    
    authentication.logout = function () {
        modal.confirm({
            content: authentication.globalization.confirmLogoutMessage,
            onAccept: function () {
                bcms.redirect(links.logoutUrl);
            }
        });
    };

    function viewPageAsAnonymous() {
        var url = window.location.href,
            hash = window.location.hash;

        if (!hash && url.lastIndexOf('#') == url.length - 1) {
            hash = '#';
        }

        if (hash) {
            url = url.substring(0, url.lastIndexOf(hash));
        }

        url = url + (url.indexOf("?") > 0 ? "&" : "?") + getParams.viewAsAnonymous + "=1";

        if (hash) {
            url = url + hash;
        }

        window.open(url);
    }

    authentication.init = function () {
        $(selectors.sideManuHeader).find(selectors.logoutButton).on("click", function (event) {
            event.stopPropagation();
            authentication.logout();
        });

        $(selectors.viewAsPublicLink).on('click', function (event) {
            bcms.stopEventPropagation(event);
            viewPageAsAnonymous();
        });
    };

    bcms.registerInit(authentication.init);

    return authentication;
});
