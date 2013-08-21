/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

bettercms.define('bcms.user', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.inlineEdit', 'bcms.dynamicContent', 'bcms.role', 'bcms.media', 'bcms.messages', 'bcms.grid'],
    function($, bcms, modal, siteSettings, editor, dynamicContent, role, media, messages, grid) {
        'use strict';

        var user = {},
            selectors = {
                siteSettingsUserCreateButton: "#bcms-create-user-button",
                usersForm: '#bcms-users-form',
                usersSearchButton: '#bcms-users-search-btn',
                usersSearchField: '.bcms-search-block input.bcms-editor-field-box',
                userUploadImageButton: "#bcms-open-uploader-button",
                userImageId: ".bcms-user-image-id",
                userImage: ".bcms-user-image-url",
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

            var onUserCreated = function (json) {
                if (json.Success && json.Data != null) {
                    var rowtemplate = $(selectors.userRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.userRowTemplateFirstRow);
                    setUserFields(newRow, json.Data);
                    newRow.insertBefore($(selectors.userTableFirstRow, usersContainer));
                    initializeSiteSettingsUsersListItem(newRow);
                    messages.refreshBox(usersContainer, json);
                    grid.showHideEmptyRow(usersContainer);
                }
            };

            usersContainer.find(selectors.siteSettingsUserCreateButton).on('click', function () {
                user.openCreateUserDialog(onUserCreated);
            });

            initializeSiteSettingsUsersListItem(usersContainer);
        }

        /**
        * Initailizes site settings users list items
        */
        function initializeSiteSettingsUsersListItem(container) {
            container.find(selectors.userCells).on('click', function () {
                editUser(container, $(this));
                return false;
            });

            container.find(selectors.userRowDeleteButton).on('click', function () {
                deleteUser(container, $(this));
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
                roles = new siteSettings.TabViewModel(role.globalization.rolesListTabTitle, role.links.loadSiteSettingsRoleUrl, role.initializeRoleListForm);

            tabs.push(users);
            tabs.push(roles);
            
            siteSettings.initContentTabs(tabs);
        };

        user.openCreateUserDialog = function(onSaveCallback) {
            modal.open({
                title: globalization.usersAddNewTitle,
                onLoad: function(childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadEditUserUrl, {
                        contentAvailable: initUserCreateEvents,

                        postSuccess: onSaveCallback
                    });
                }
            });
        };

        function initUserCreateEvents(dialog) {
            var onImageInsert = function(image) {
                dialog.container.find(selectors.userImageId).val(image.id());
                dialog.container.find(selectors.userImage).attr('src', image.thumbnailUrl());
            };
            
            dialog.container.find(selectors.userUploadImageButton).on('click', function() {
                media.openImageInsertDialog(onImageInsert);
            });
        }

        /**
        * Calls function, which opens dialog for a user editing.
        */
        function editUser(container, self) {
            var row = self.parents(selectors.userParentRow),
                id = row.find(selectors.userEditButton).data('id'),
                onSaveCallback = function(json) {
                    messages.refreshBox(container, json);
                    if (json.Success) {
                        setUserFields(row, json.Data);
                        grid.showHideEmptyRow(container);
                    }
                };

            modal.open({
                title: globalization.editUserTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditUserUrl, id), {
                        contentAvailable: initializeEditUserForm,

                        postSuccess: onSaveCallback
                    });
                }
            });
        }

        function initializeEditUserForm() {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container;

            var form = container.find(selectors.userForm);

            form.on('submit', function(event) {
                event.preventDefault();
                return false;
            });
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
        * Delete user from site settings users list.
        */
        function deleteUser(container, self) {
            var row = self.parents(selectors.userParentRow),
                id = self.data('id'),
                version = self.data('version'),
                name = row.find(selectors.userNameCell).html(),
                url = $.format(links.deleteUserUrl, id, version),
                message = $.format(globalization.deleteUserConfirmMessage, name),
                onDeleteCompleted = function(json) {
                    try {
                        messages.refreshBox(container, json);
                        if (json.Success) {
                            row.remove();
                            grid.showHideEmptyRow(container);
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