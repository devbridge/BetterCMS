/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.user', ['jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var user = {},
        
        links = {
              logoutUrl: 'unknown'
        },
        
        globalization = {
            confirmLogoutMessage: null
        };

    // Assign objects to module.
    user.links = links;
    user.globalization = globalization;

   
    user.init = function () {
    };

    bcms.registerInit(user.init);
    
    return user;
});
