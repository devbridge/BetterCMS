/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

bettercms.define('bcms.user', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.role', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders'],
    function($, bcms, modal, siteSettings, dynamicContent, role, media, messages, grid, ko) {
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
                userRowTemplate: '#bcms-users-list-row-template',
                userTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                userRowTemplateFirstRow: 'tr:first'
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
            usersContainer = null;

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

                var searchInput = usersContainer.find(selectors.usersSearchField);
                grid.focusSearchInput(searchInput);
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

            form.find(selectors.usersSearchField).keypress(function (event) {
                if (event.which == 13) {
                    bcms.stopEventPropagation(event);
                    searchSiteSettingsUsers(form);
                }
            });

            form.find(selectors.usersSearchButton).on('click', function () {
                searchSiteSettingsUsers(form);
            });

            usersContainer.find(selectors.siteSettingsUserCreateButton).on('click', function () {
                createUser();
                
            });

            initializeSiteSettingsUsersListItem(usersContainer);
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
                users = new siteSettings.TabViewModel(globalization.usersListTabTitle, links.loadSiteSettingsUsersUrl, function(container) {
                    usersContainer = container;

                    initializeSiteSettingsUsersList();
                }),
                roles = new siteSettings.TabViewModel(role.globalization.rolesListTabTitle, role.links.loadSiteSettingsRoleUrl, role.initializeRolesList);

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

        /**
        * Initializes edit/create user form
        */
        function initializeEditUserForm(dialog, content) {

            var imageData = content.Data.Image,
                rolesData = content.Data.Roles,
                userViewModel = {
                    image: ko.observable(new media.ImageSelectorViewModel(imageData)),
                    roles: new role.RolesAutocompleteListViewModel(rolesData)
                },
                form = dialog.container.find(selectors.userForm);

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
        * Initializes user module
        */
        user.init = function() {
        };

        bcms.registerInit(user.init);

        return user;
    });