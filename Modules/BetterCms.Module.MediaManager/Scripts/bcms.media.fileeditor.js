/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.media.fileeditor', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.forms', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.tags', 'bcms.security'],
    function ($, bcms, modal, siteSettings, forms, dynamicContent, ko, tags, security) {
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
                fileEditorUpdateFailureMessageMessage: null
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
                        contentAvailable: initFileEditorDialogEvents,
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
                        },
                        postError: function () {
                            modal.alert({
                                title: globalization.fileEditorUpdateFailureMessageTitle,
                                content: globalization.fileEditorUpdateFailureMessageMessage
                            });
                        }
                    });
                }
            });
        };

        /**
        * File edit form view model
        */
        function FileEditViewModel(tagsViewModel, image, accessControl) {
            var self = this;
            self.tags = tagsViewModel;
            self.image = ko.observable(new media.ImageSelectorViewModel(image));
            self.accessControl = accessControl;
        }

        /**
        * Initializes dialog events.
        */
        function initFileEditorDialogEvents(dialog, content) {

            var tagsViewModel = new tags.TagsListViewModel(content.Data.Tags),
                accessControl = security.createUserAccessViewModel(content.Data.UserAccessList),
                viewModel = new FileEditViewModel(tagsViewModel, content.Data.Image, accessControl);

            ko.applyBindings(viewModel, dialog.container.find(selectors.fileEditorForm).get(0));

            dialog.container.find(selectors.selectableInputs).on('click', function () {
                this.select();
            });
        }

        return editor;
    });