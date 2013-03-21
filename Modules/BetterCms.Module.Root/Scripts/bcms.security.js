/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.security', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var security = {},
        // Classes that are used to maintain various UI states:
        classes = {},
        // Selectors used in the module to locate DOM elements:
        selectors = {},
        links = {},
        globalization = {};

    // Assign objects to module
    security.classes = classes;
    security.selectors = selectors;
    security.links = links;
    security.globalization = globalization;

    security.IsAuthorized = function (accessRoleArray) {
        if (bcms.securityProfile && bcms.securityProfile.CurrentUserRoles) {
            if (accessRoleArray == null || accessRoleArray == {}) {
                return true;
            }

            // Check if user is in full access role.
            if (bcms.securityProfile.FullAccessRoles && bcms.securityProfile.FullAccessRoles != "") {
                if (anyInArray(roleStringToArray(bcms.securityProfile.FullAccessRoles), bcms.securityProfile.CurrentUserRoles)) {
                    return true;
                }
            }
            
            // Check if user is in role.
            if (bcms.securityProfile.CustomRoles) {
                // Use role mapping.
                for (var i in accessRoleArray) {
                    var customRoles = bcms.securityProfile.CustomRoles[accessRoleArray[i]];
                    if (customRoles != null && customRoles != "") {
                        var customRoleArray = roleStringToArray(customRoles);
                        if (anyInArray(customRoleArray, bcms.securityProfile.CurrentUserRoles)) {
                            return true;
                        }
                    } else {
                        if ($.inArray(accessRoleArray[i], bcms.securityProfile.CurrentUserRoles) != -1) {
                            return true;
                        }
                    }
                }
            } else {
                // Do not use mapping.
                if (anyInArray(accessRoleArray, bcms.securityProfile.CurrentUserRoles)) {
                    return true;
                }
            }
            
            return false;
        }

        console.log("No security data provided for GUI.");
        return true; // This means that security data is not provided for GUI - no GUI hiding will be performed.
    };

    function anyInArray(sourceArray, lookInArray) {
        for (var i in sourceArray) {
            if ($.inArray(sourceArray[i], lookInArray) != -1) {
                return true;
            }
        }
        return false;
    };

    function roleStringToArray(roleString) {
        var roles = roleString.split(',');
        for (var i in roles) {
            roles[i] = $.trim(roles[i]);
        }
        return roles;
    };
    
    return security;
});
