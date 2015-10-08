/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.user', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.role', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect'],
    function($, bcms, modal, siteSettings, dynamicContent, role, media, messages, grid, ko, redirect) {
        'use strict';

        var user = {},
            selectors = {
                siteSettingsUserCreateButton: "#bcms-create-user-button",
                usersForm: '#bcms-users-form',
                userForm: 'form:first',
                usersSearchButton: '#bcms-users-search-btn',
                usersSearchField: '.bcms-search-block input.bcms-editor-field-box',
                userCells: 'td',
                userEditButton: '.bcms-icn-edit',
                userRowDeleteButton: '.bcms-grid-item-delete-button',
                userParentRow: 'tr:first',
                userNameCell: '.bcms-user-name',
                userFullNameCell: '.bcms-user-fullName',
                userEmailCell: '.bcms-user-email',
                userRowTemplate: '#bcms-users-list-row-template',
                userTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                userRowTemplateFirstRow: 'tr:first',
                openEditUserProfile: '#bcms-user-profile'
            },
            links = {
                loadSiteSettingsUsersUrl: null,
                loadCreateUserUrl: null,
                loadEditUserUrl: null,
                deleteUserUrl: null
            },
            globalization = {
                usersListTabTitle: null,
                usersAddNewTitle: null,
                editUserTitle: null,
                deleteUserConfirmMessage: null
            },
            usersContainer = null,
            userViewModel = null;

        // Assign objects to module.
        user.links = links;
        user.globalization = globalization;
        user.selectors = selectors;

        /**
        * Submits users list seach/sort/paging form
        */
        function searchSiteSettingsUsers(form) {
            grid.submitGridForm(form, function (htmlContent) {
                usersContainer.html(htmlContent);
                initializeSiteSettingsUsersList();
            });
        }

        /**
        * Initailizes site settings users list
        */
        function initializeSiteSettingsUsersList() {
            var form = usersContainer.find(selectors.usersForm);

            grid.bindGridForm(form, function (htmlContent) {
                usersContainer.html(htmlContent);
                initializeSiteSettingsUsersList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsUsers(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.usersSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsUsers(form);
                },
            });

            form.find(selectors.usersSearchButton).on('click', function () {
                searchSiteSettingsUsers(form);
            });

            usersContainer.find(selectors.siteSettingsUserCreateButton).on('click', function () {
                createUser();
                
            });

            initializeSiteSettingsUsersListItem(usersContainer);
            
            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(selectors.usersSearchField), true);
        }

        /**
        * Initailizes site settings users list items
        */
        function initializeSiteSettingsUsersListItem(container) {
            container.find(selectors.userCells).on('click', function () {
                editUser($(this));
                return false;
            });

            container.find(selectors.userRowDeleteButton).on('click', function () {
                deleteUser($(this));
                return false;
            });
        }

        /**
        * Opens site settings users section's users and roles tabs
        */
        user.loadSiteSettingsUsers = function() {
            var tabs = [],
                onShow = function (container) {
                    var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
                    if (firstVisibleInputField) {
                        firstVisibleInputField.focus();
                    }
                },
                users = new siteSettings.TabViewModel(globalization.usersListTabTitle, links.loadSiteSettingsUsersUrl, function (container) {
                    usersContainer = container;

                    initializeSiteSettingsUsersList();
                }, onShow),
                roles = new siteSettings.TabViewModel(role.globalization.rolesListTabTitle, role.links.loadSiteSettingsRoleUrl, role.initializeRolesList, onShow);

            tabs.push(users);
            tabs.push(roles);
            
            siteSettings.initContentTabs(tabs);
        };

        /**
        * Opens dialog for creating a user.
        */
        function createUser() {
            var onSaveCallback = function (json) {
                messages.refreshBox(usersContainer, json);
                if (json.Success) {
                    var rowtemplate = $(selectors.userRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.userRowTemplateFirstRow);
                    
                    setUserFields(newRow, json.Data);
                    newRow.insertBefore(usersContainer.find(selectors.userTableFirstRow));
                    initializeSiteSettingsUsersListItem(newRow);
                    grid.showHideEmptyRow(usersContainer);
                }
            };
            
            openUserEditForm(globalization.usersAddNewTitle, links.loadCreateUserUrl, onSaveCallback);
        }

        /**
        * Opens dialog for editing a user.
        */
        function editUser(self) {
            var row = self.parents(selectors.userParentRow),
                id = row.find(selectors.userEditButton).data('id'),
                onSaveCallback = function(json) {
                    messages.refreshBox(usersContainer, json);
                    if (json.Success) {
                        setUserFields(row, json.Data);
                        grid.showHideEmptyRow(usersContainer);
                    }
                };

            openUserEditForm(globalization.editUserTitle, $.format(links.loadEditUserUrl, id), onSaveCallback);
        }
        
        /**
        * Open dialog for edit/create a user
        */
        function openUserEditForm(title, url, onSaveCallback) {
            modal.open({
                title: title,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: initializeEditUserForm,

                        postSuccess: onSaveCallback
                    });
                }
            });
        }

        function UserViewModel(imageData, rolesData, userData) {
            var self = this;

            self.image = ko.observable(new media.ImageSelectorViewModel(imageData));
            self.roles = new role.RolesAutocompleteListViewModel(rolesData);
            self.id = userData.Id;
            self.firstName = ko.observable(userData.FirstName);
            self.lastName = ko.observable(userData.LastName);
            self.userName = ko.observable(userData.UserName);
            self.userNameManuallyEdited = false;

            self.changeUserName = function() {
                if (bcms.isEmptyGuid(self.id) && !self.userNameManuallyEdited) {
                    var userName = '',
                        firstName = self.firstName(),
                        lastName = self.lastName();

                    if (firstName) {
                        userName += firstName;
                    }
                    if (firstName && lastName) {
                        userName += '.';
                    }
                    if (lastName) {
                        userName += lastName;
                    }
                    userName = userName.toLowerCase().replace(/ /g, '');

                    self.userName(userName);
                }
            };

            self.userNameOnKeyUp = function () {
                self.userNameManuallyEdited = true;
                return true;
            };
            
            self.firstName.subscribe(self.changeUserName);
            self.lastName.subscribe(self.changeUserName);

            return self;
        }

        /**
        * Initializes edit/create user form
        */
        function initializeEditUserForm(dialog, content) {

            var imageData = content.Data.Image,
                rolesData = content.Data.Roles,
                userData = content.Data,
                form = dialog.container.find(selectors.userForm);

            userViewModel = new UserViewModel(imageData, rolesData, userData);
            ko.applyBindings(userViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setUserFields(row, json) {
            row.find(selectors.userEditButton).data('id', json.Id);
            row.find(selectors.userRowDeleteButton).data('id', json.Id);
            row.find(selectors.userRowDeleteButton).data('version', json.Version);
            row.find(selectors.userNameCell).html(json.UserName);
            row.find(selectors.userFullNameCell).html(json.FullName);
            row.find(selectors.userEmailCell).html(json.Email);
        }

        /**
        * Deletes user from site settings users list.
        */
        function deleteUser(self) {
            var row = self.parents(selectors.userParentRow),
                id = self.data('id'),
                version = self.data('version'),
                name = row.find(selectors.userNameCell).html(),
                url = $.format(links.deleteUserUrl, id, version),
                message = $.format(globalization.deleteUserConfirmMessage, name),
                onDeleteCompleted = function(json) {
                    try {
                        messages.refreshBox(usersContainer, json);
                        if (json.Success) {
                            row.remove();
                            grid.showHideEmptyRow(usersContainer);
                        }
                    } finally {
                        confirmDialog.close();
                    }
                },
                confirmDialog = modal.confirm({
                    content: message,
                    onAccept: function() {
                        $.ajax({
                            type: 'POST',
                            url: url,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            cache: false
                        })
                            .done(function(json) {
                                onDeleteCompleted(json);
                            })
                            .fail(function(response) {
                                onDeleteCompleted(bcms.parseFailedResponse(response));
                            });
                        return false;
                    }
                });
        }

        /**
        * Initialize custom jQuery validators
        */
        function initializeCustomValidation() {
            $.validator.addMethod("jqpasswordvalidation", function (value, element, params) {
                if (value) {
                    var match = new RegExp(params.pattern).exec(value);
                    params.regex = true;
                    
                    var isMatch = (match && (match.index === 0) && (match[0].length === value.length));
                    if (!isMatch) {
                        return false;
                    }
                }

                params.regex = false;
                return !bcms.isEmptyGuid(userViewModel.id) || value;
            }, function (params) {
                if (params.regex) {
                    return params.patternmessage;
                }
                
                return params.message;
            });

            $.validator.unobtrusive.adapters.add("passwordvalidation", ['pattern', 'patternmessage'], function (opts) {
                opts.rules["jqpasswordvalidation"] = { message: opts.message, pattern: opts.params.pattern, patternmessage: opts.params.patternmessage };
            });
        }

        /**
        * Initialize edit user profile form url
        */
        function initializeUserEditProfileFormUrl() {
            $(selectors.openEditUserProfile).on('click', function () {
                var self = $(this),
                    url = self.data('url'),
                    onSaveCallback = function (json) {
                        messages.refreshBox(usersContainer, json);
                        if (json.Success) {
                            redirect.ReloadWithAlert();
                        }
                    };

                if (url) {
                    modal.open({
                        title: globalization.editUserProfileTitle,
                        onLoad: function (dialog) {
                            dynamicContent.bindDialog(dialog, url, {
                                contentAvailable: initializeEditUserForm,

                                postSuccess: onSaveCallback
                            });
                        }
                    });
                }
            });
        }

        /**
        * Initializes user module
        */
        user.init = function () {
            bcms.logger.debug('Initializing bcms.user module.');

            initializeCustomValidation();
            initializeUserEditProfileFormUrl();
        };

        bcms.registerInit(user.init);

        return user;
    });