/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.security.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.security', ['bcms.jquery', 'bcms', 'bcms.ko.extenders', 'bcms.messages', 'bcms.autocomplete'], function ($, bcms, ko, messages, autocomplete) {
    'use strict';

    var security = {},
        // Classes that are used to maintain various UI states:
        classes = {},
        // Selectors used in the module to locate DOM elements:
        selectors = {},
        links = {
            isAuthorized: null,
            rolesSuggestionServiceUrl: null,
            usersSuggestionServiceUrl: null
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
            bcms.logger.error('Error occurred while checking if role(s) is authorized.');
        }

        return false;
    }

    security.IsAuthorized = function (accessRoleArray) {
        for (var i = 0; i < accessRoleArray.length; i++) {
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

    var AccessControlViewModel = (function (_super) {
        bcms.extendsClass(AccessControlViewModel, _super);

        function AccessControlViewModel(identities, isRole, addMode, autoCompleteUrl, initialValues) {
            var options = {
                serviceUrl: autoCompleteUrl,
                width: '100%'
            };

            var self = this;
            self.identities = identities;
            self.isInAddMode = addMode;
            self.clickPlus = function() {
                if (self.isInAddMode() === (isRole ? 'role' : 'user') && !!self.newItem()) {
                    var name = self.newItem();
                    if (!name || self.newItem.hasError()) {
                        return;
                    }

                    self.isExpanded(false);
                    self.clearItem();
                    self.identities.push(new UserAccessViewModel({ Identity: name, IsForRole: isRole }));
                    self.isInAddMode('none');
                } else {
                    self.clearItem();
                    self.isExpanded(false);
                    if (self.isInAddMode() === (isRole ? 'role' : 'user')) {
                        self.isInAddMode('none');
                    } else {
                        self.isInAddMode(isRole ? 'role' : 'user');
                        setTimeout(function () {
                            self.isExpanded(true);
                        }, 50);
                    }
                }
            };
            
            _super.call(self, initialValues, options);
            
            self.items.subscribe(function (newValue) {
                self.clickPlus();
            });
        }

        return AccessControlViewModel;
    })(autocomplete.AutocompleteListViewModel);

    security.createUserAccessViewModel = function (accessList) {
        var roles = [],
            users = [];

        if (accessList) {
            $.each(accessList, function(i, item) {
                if (item.IsForRole) {
                    roles.push(item.Identity);
                } else {
                    users.push(item.Identity);
                }
            });
        }

        var identities = ko.observableArray(),
            addMode = ko.observable('none'),
            model = {
                UserAccessList: identities,
                userAccessControl: new AccessControlViewModel(identities, false, addMode, links.usersSuggestionServiceUrl, users),
                roleAccessControl: new AccessControlViewModel(identities, true, addMode, links.rolesSuggestionServiceUrl, roles),
                removeUser: function(userAccessViewModel) {
                    model.UserAccessList.remove(userAccessViewModel);
                },
                getPropertyIndexer: function(i, propName) {
                    return 'UserAccessList[' + i + '].' + propName;
                }                         
        };

        if (accessList) {
            $.each(accessList, function (i, item) {
                model.UserAccessList.push(new UserAccessViewModel(item));
            });
        }

        return model;
    };

    security.updateUserAccessViewModel = function (model, accessList) {
        var roles = [],
            users = [];

        model.userAccessControl.removeAll();
        model.roleAccessControl.removeAll();
        model.UserAccessList.removeAll();

        if (accessList) {
            $.each(accessList, function (i, item) {
                if (item.IsForRole) {
                    roles.push(item.Identity);
                } else {
                    users.push(item.Identity);
                }
            });
        }
        model.userAccessControl.applyItemList(users);
        model.roleAccessControl.applyItemList(roles);
        if (accessList) {
            $.each(accessList, function (i, item) {
                model.UserAccessList.push(new UserAccessViewModel(item));
            });
        }
        
        model.userAccessControl.clearItem();
        model.userAccessControl.isExpanded(false);
        model.userAccessControl.isInAddMode('none');
        model.roleAccessControl.clearItem();
        model.roleAccessControl.isExpanded(false);
        model.roleAccessControl.isInAddMode('none');
    };

    return security;
});
