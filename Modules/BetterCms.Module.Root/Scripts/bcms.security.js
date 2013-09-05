/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.security', ['bcms.jquery', 'bcms.ko.extenders', 'bcms.messages'], function ($, ko, messages) {
    'use strict';

    var security = {},
        // Classes that are used to maintain various UI states:
        classes = {},
        // Selectors used in the module to locate DOM elements:
        selectors = {},
        links = {
            isAuthorized: null
        },
        globalization = {},
        authorizedFor = [];

    // Assign objects to module
    security.classes = classes;
    security.selectors = selectors;
    security.links = links;
    security.globalization = globalization;

    function isUserAuthorized(roles) {
        try {
            var url = $.format(links.isAuthorized, roles),
                response = $.parseJSON($.ajax({
                    type: 'POST',
                    url: url,
                    cache: false,
                    async: false
                }).responseText);
            if (response.Success === false || response.Success === true) {
                authorizedFor[roles] = response.Success;
                if (response.Success === true) {
                    return true;
                }
            }
        } catch (e) {
            console.log('Error occurred while checking if role(s) is authorized.');
        }

        return false;
    }

    security.IsAuthorized = function (accessRoleArray) {
        for (var i in accessRoleArray) {
            var role = accessRoleArray[i];
            var value = authorizedFor[role];
            if (value === true || value === false) {
                if (value === true) {
                    return true;
                }
            } else {
                if (isUserAuthorized(role)) {
                    return true;
                }
            }
        }
        return false;
    };

    /**
    * Extend knockout: add maximum length validation
    */
    ko.extenders.uniqueAccessRuleIdentity = function (target, options) {
        var ruleName = 'uniqueAccessRuleIdentity',
            message = options.message || 'Identity {0} exits!';
        
        return ko.extenders.koValidationExtender(ruleName, target, function (newValue) {
            var hasError = existIdentity(options.identities(), newValue, options.isRole),
                showMessage = hasError ? $.format(message, newValue) : '';

            target.validator.setError(ruleName, hasError, showMessage);
        });
    };
    
    function existIdentity(identityList, identityName, isRole) {
        var i,
            len = identityList.length,
            identityNameTrimmedUpper = $.trim(identityName).toUpperCase();
                
        for (i = 0; i < len; i++) {
            var item = identityList[i];
            if (item.IsForRole() === isRole && $.trim(item.Identity()).toUpperCase() == identityNameTrimmedUpper) {
                return true;
            }
        }

        return false;
    }            
    
    function UserAccessViewModel(item) {
        this.Identity = ko.observable(item.Identity);
        this.AccessLevel = ko.observable(item.AccessLevel || 3);
        this.IsForRole = ko.observable(item.IsForRole);
    }

    security.createUserAccessViewModel = function (accessList) {
        var messageBox =
                messages.box({
                container: $(".bcms-modal")
                }),
            identities = ko.observableArray(),
            model = {
                UserAccessList: identities,
                newUserName: ko.observable('').extend({ uniqueAccessRuleIdentity: { identities: identities, isRole: false } }),
                newRoleName: ko.observable('').extend({ uniqueAccessRuleIdentity: { identities: identities, isRole: true } }),
                isInAddUserMode: ko.observable(false),
                isInAddRoleMode: ko.observable(false),
                hasAddNameFocus: ko.observable('none'),

                clearNewUserInput: function () {
                    model.isInAddUserMode(false);
                },
                
                clearNewRoleInput : function() {
                    model.isInAddRoleMode(false);
                },
                
                gotoAddNewUser: function () {
                    if (model.isInAddUserMode() && !!model.newUserName()) {
                        model.addNewUser();
                    } else {
                        model.hasAddNameFocus('none');
                        model.isInAddRoleMode(false);
                        model.isInAddUserMode(!model.isInAddUserMode());
                        model.newUserName('');
                    
                        setTimeout(function () {
                            model.hasAddNameFocus('user');
                        }, 50);
                    }
                },
            
                gotoAddNewRole: function () {
                    if (model.isInAddRoleMode() && !!model.newRoleName()) {
                        model.addNewRole();
                    } else {
                        model.hasAddNameFocus('none');
                        model.isInAddUserMode(false);
                        model.isInAddRoleMode(!model.isInAddRoleMode());
                        model.newRoleName('');
                    
                        setTimeout(function() {
                            model.hasAddNameFocus('role');
                        }, 50);
                    }
                },
            
                addNewUser: function() {
                    var name = model.newUserName();
                    if (!name || model.newUserName.hasError()) {
                        return;
                    }
                
                    model.UserAccessList.push(new UserAccessViewModel({ Identity: name, IsForRole: false }));
                    model.newUserName('');
                    model.isInAddUserMode(false);
                    model.hasAddNameFocus('none');
                },
            
                addNewRole: function () {
                    var name = model.newRoleName();
                    
                    if (!name || model.newRoleName.hasError()) {
                        return;
                    }
                    
                    model.UserAccessList.push(new UserAccessViewModel({ Identity: name, IsForRole: true }));
                    model.newRoleName('');
                    model.isInAddRoleMode(false);
                    model.hasAddNameFocus('none');
                },
            
                removeUser: function(userAccessViewModel) {
                    model.UserAccessList.remove(userAccessViewModel);
                },
            
                getPropertyIndexer: function(i, propName) {
                    return 'UserAccessList[' + i + '].' + propName;
                }                         
        };

        $.each(accessList, function(i, item) {
            model.UserAccessList.push(new UserAccessViewModel(item));
        });

        return model;
    };

    return security;
});
