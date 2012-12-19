/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.authentication', ['jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var authentication = {},
        links = {
            logoutUrl: null
        },
        globalization = {
            confirmLogoutMessage: null
        };

    // Assign objects to module.
    authentication.links = links;
    authentication.globalization = globalization;
    
    authentication.logout = function () {
        // TODO: replace with internal BCMS confirmation popup.
        if (confirm(authentication.globalization.confirmLogoutMessage)) {
            bcms.redirect(links.logoutUrl);            
        }
    };
    
    authentication.init = function () {
        $(".bcms-sidemenu-header").find(".bcms-logout-btn").on("click", function (event) {
            event.stopPropagation();
            authentication.logout();
        });
    };

    bcms.registerInit(authentication.init);

    return authentication;
});
