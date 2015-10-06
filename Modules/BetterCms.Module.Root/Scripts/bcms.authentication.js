/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

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
