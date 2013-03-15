/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.authentication', ['bcms.jquery', 'bcms', 'bcms.modal'],
    function ($, bcms, modal) {
    'use strict';

    var authentication = {},
        selectors = {
            sideManuHeader: ".bcms-sidemenu-header",
            logoutButton: ".bcms-logout-btn"
        },
        links = {
            logoutUrl: null
        },
        globalization = {
            confirmLogoutMessage: null
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
    
    authentication.init = function () {
        $(selectors.sideManuHeader).find(selectors.logoutButton).on("click", function (event) {
            event.stopPropagation();
            authentication.logout();
        });
    };

    bcms.registerInit(authentication.init);

    return authentication;
});
