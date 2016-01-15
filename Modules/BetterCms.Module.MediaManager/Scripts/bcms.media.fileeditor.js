/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.media.fileeditor.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.media.fileeditor', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.tags', 'bcms.categories', 'bcms.security', 'bcms.media.upload'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, ko, tags, categories, security, mediaUpload) {
        'use strict';

        var editor = {},
            selectors = {
                fileToEdit: ".bcms-croped-block img",
                fileVersionField: "#image-version-field",
                fileCaption: "#Caption",
                fileFileName: "#image-file-name",
                fileFileSize: "#image-file-size",

                fileEditorForm: 'form:first',
                fileTitleEditInput: "#bcms-image-title-editor",

                selectableInputs: 'input.bcms-editor-selectable-field-box'
            },
            links = {
                fileEditorDialogUrl: null
            },
            globalization = {
                fileEditorDialogTitle: null,
                fileEditorUpdateFailureMessageTitle: null,
                fileEditorUpdateFailureMessageMessage: null,
                fileEditorHasChangesMessage: null
            },
            media = null;

        /**
        * Assign objects to module.
        */
        editor.links = links;
        editor.globalization = globalization;
        editor.SetMedia = function (mediaModule) {
            media = mediaModule;
        };

        /**
        * Called when editing is needed.
        */
        editor.onEditFile = function (fileId, callback) {
            editor.showFileEditorDialog(fileId, callback);
        };

        /**
        * Show file editor dialog.
        */
        editor.showFileEditorDialog = function (fileId, callback) {
            modal.open({
                title: globalization.fileEditorDialogTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.fileEditorDialogUrl, fileId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (childDialog, content) {
                            initFileEditorDialogEvents(childDialog, content, callback);
                        },
                        beforePost: function () {
                            var newVersion = $(selectors.fileToEdit).data('version');
                            if (newVersion > 0) {
                                $(selectors.fileVersionField).val(newVersion);
                            }
                        },
                        postSuccess: function (json) {
                            if (json.Success) {
                                callback(json.Data);
                            }
                            dialog.close();
                        }
                    });
                }
            });
        };

        /**
        * File edit form view model
        */
        function FileEditViewModel(dialog, data, onSaveCallback) {
            var self = this,
                tagsViewModel = new tags.TagsListViewModel(data.Tags),
                categoriesViewModel = new categories.CategoriesSelectListModel(data.Categories),
                accessControl = security.createUserAccessViewModel(data.UserAccessList),
                image = data.Image,
                userAccessList = accessControl.UserAccessList(),
                l = userAccessList.length,
                i;
            
            self.tags = tagsViewModel;
            self.categories = categoriesViewModel;
            self.image = ko.observable(new media.ImageSelectorViewModel(image));
            self.accessControl = accessControl;

            // Track form changes
            self.modelChanged = false;
            self.onValueChange = function () {
                self.modelChanged = true;
            };
            self.image().id.subscribe(self.onValueChange);
            tagsViewModel.items.subscribe(self.onValueChange);
            accessControl.UserAccessList.subscribe(self.onValueChange);
            for (i = 0; i < l; i++) {
                userAccessList[i].AccessLevel.subscribe(self.onValueChange);
            }

            self.reupload = function () {
                var reupload = function () {
                    mediaUpload.openReuploadFilesDialog(data.Id, data.FolderId || '', 4, function (filesData) {
                        editor.showFileEditorDialog(data.Id, onSaveCallback);

                        if (filesData && filesData.Data && filesData.Data.Medias && filesData.Data.Medias.length > 0) {
                            onSaveCallback(filesData.Data.Medias[0]);
                        }
                    }, function () {
                        editor.showFileEditorDialog(data.Id, onSaveCallback);
                    });
                };

                if (self.modelChanged) {
                    modal.confirmWithDecline({
                        content: globalization.fileEditorHasChangesMessage,
                        onAccept: function () {
                            if ($.isFunction(dialog.options.onAccept)) {
                                var delegate = dialog.options.onAccept;
                                dialog.options.onAccept = function (closingDialog) {
                                    delegate(closingDialog);
                                    reupload();
                                };
                            } else {
                                dialog.options.onAccept = function () {
                                    reupload();
                                };
                            }
                            dialog.container.find(selectors.fileEditorForm).submit();
                        },
                        onDecline: function () {
                            reupload();
                            dialog.close();
                            return true;
                        }
                    });
                } else {
                    dialog.close();
                    reupload();
                }
            };
        }

        /**
        * Initializes dialog events.
        */
        function initFileEditorDialogEvents(dialog, content, onSaveCallback) {

            var viewModel = new FileEditViewModel(dialog, content.Data, onSaveCallback);

            categories.initCategoriesSelect(viewModel.categories, content.Data.CategoriesLookupList, dialog.container);

            ko.applyBindings(viewModel, dialog.container.find(selectors.fileEditorForm).get(0));

            dialog.container.find(selectors.selectableInputs).on('click', function () {
                this.select();
            });

            
        }

        return editor;
    });