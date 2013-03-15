/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.template', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.datepicker', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'bcms.slides.jquery'],
    function ($, bcms, modal, datepicker, dynamicContent, siteSettings, messages, preview, grid, editor) {
        'use strict';

        var template = {},
            links = {
                loadSiteSettingsTemplateListUrl: null,
                loadRegisterTemplateDialogUrl: null,
                loadEditTemplateDialogUrl: null,
                deleteTemplateUrl: null,
                loadTemplateRegionDialogUrl: null
            },
            globalization = {
                createTemplateDialogTitle: null,
                editTemplateDialogTitle: null,
                deleteTemplateConfirmMessage: null,
                deleteRegionConfirmMessage: null,
                editTemplateRegionTitle: null,
                previewImageNotFoundMessage: null
            },
            selectors = {
                templatePreviewImageUrl: '#PreviewImageUrl',
                templatePreviewImage: '#bcms-template-preview-image',
                htmlContentTemplateRowTemplate: '#bcms-advanced-content-list-row-template',
                htmlContentTemplateRowTemplateFirstRow: 'tr:first',
                htmlContentTemplateTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                messagesContainer: "#bcms-edit-template-messages",

                templateSearchButton: '#bcms-template-search-btn',
                templateSearchField: '.bcms-search-query',

                templateRegisterButton: '#bcms-register-template-button',
                templateRowEditButtons: '.bcms-grid-item-edit-button',

                templatesRowDeleteButtons: '.bcms-grid-item-delete-button',
                templateParentRow: 'tr:first',
                templateNameCell: '.bcms-template-name',
                templateRowDeleteButtons: '.bcms-grid-item-delete-button',
                templateRowTemplate: '#bcms-template-list-row-template',
                templateRowTemplateFirstRow: 'tr:first',
                templateTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                templateInsertButtons: '.bcms-template-insert-button',

                addNewRegionButton: '#bcms-template-options-add-region',
                templatesListForm: '#bcms-templates-form',

                addOptionLink: '#bcms-add-option-button',
                optionsTable: '#bcms-options-grid'
            },
            classes = {
            };

        /**
        * Assign objects to module.
        */
        template.links = links;
        template.globalization = globalization;


        /**
        * Opens template edit dialog.
        */
        template.openEditTemplateDialog = function (templateId, onSaveCallback) {
            modal.open({
                title: globalization.editTemplateDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditTemplateDialogUrl, templateId), {
                        contentAvailable: initializeEditTemplateForm,

                        beforePost: function (form) {
                            editor.resetAutoGenerateNameId();
                            editor.setInputNames(form);
                        },

                        postSuccess: onSaveCallback
                    });
                }
            });
        };

        /**
        * Opens template create form from site settings template list
        */
        template.openRegisterTemplateDialog = function (onSaveCallback) {
            modal.open({
                title: globalization.createTemplateDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadRegisterTemplateDialogUrl, {
                        contentAvailable: initializeEditTemplateForm,

                        postSuccess: onSaveCallback
                    });
                }
            });
        };

        /**
        * Initializes template form
        */
        function initializeEditTemplateForm(dialog) {
            editor.initialize(dialog.container, {
                deleteRowMessageExtractor: function () {
                    return globalization.deleteRegionConfirmMessage;
                }
            });

            dialog.container.find(selectors.addOptionLink).on('click', function () {
                editor.addNewRow(dialog.container, $(selectors.optionsTable));
            });

            dialog.container.find(selectors.addNewRegionButton).on('click', function () {
                editor.addNewRow(dialog.container, $(selectors.optionsTable));
            });

            dialog.container.find(selectors.templatePreviewImage).error(function () {
                var image = dialog.container.find(selectors.templatePreviewImage);
                if (image.attr("src") != null && image.attr("src") != "") {
                    messages.box({ container: dialog.container.find(selectors.messagesContainer) }).addWarningMessage(globalization.previewImageNotFoundMessage);
                    image.hide();
                    image.removeAttr("src");
                }
            });

            dialog.container.find(selectors.templatePreviewImageUrl).blur(function () {
                var image = dialog.container.find(selectors.templatePreviewImage),
                    urlInput = dialog.container.find(selectors.templatePreviewImageUrl);

                if (urlInput.valid()) {
                    image.attr({ src: urlInput.val() });
                    image.show();
                } else {
                    image.hide();
                    image.removeAttr("src");
                }
            });
        };

        /*
        * Open a template edit dialog by the specified tempalte type.
        */
        template.editTemplate = function (templateId, onSaveCallback) {
            template.openEditTemplateDialog(templateId, onSaveCallback);

        };

        /**
        * Deletes template.
        */
        template.deleteTemplate = function (row, onDeleteCallback) {
            var templateId = row.data('id'),
                templateVersion = row.data('version'),
                templateName = row.find(selectors.templateNameCell).html(),
                url = $.format(links.deleteTemplateUrl, templateId, templateVersion),
                message = $.format(globalization.deleteTemplateConfirmMessage, templateName),
                onDeleteCompleted = function (json) {
                    messages.refreshBox(row, json);
                    try {
                        if (json.Success && $.isFunction(onDeleteCallback)) {
                            onDeleteCallback(json);
                        }
                    } finally {
                        confirmDialog.close();
                    }
                },
                confirmDialog = modal.confirm({
                    content: message,
                    onAccept: function () {
                        $.ajax({
                            type: 'POST',
                            url: url,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            cache: false
                        })
                        .done(function (json) {
                            onDeleteCompleted(json);
                        })
                        .fail(function (response) {
                            onDeleteCompleted(bcms.parseFailedResponse(response));
                        });
                        return false;
                    }
                });
        };

        /**
        * Opens site settings template list dialog
        */
        template.loadSiteSettingsTemplateList = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsTemplateListUrl, {
                contentAvailable: initializeTemplatesList
            });
        };

        /**
        * Initializes site settings template list and list items
        */
        function initializeTemplatesList() {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container,
                onTemplateCreated = function (json) {
                    if (json.Success && json.Data != null) {
                        var rowtemplate = $(selectors.templateRowTemplate),
                            newRow = $(rowtemplate.html()).find(selectors.templateRowTemplateFirstRow);
                        setTemplateFields(newRow, json);
                        messages.refreshBox(selectors.templatesListForm, json);
                        newRow.insertBefore($(selectors.templateTableFirstRow, container));
                        initializeTemplateListEvents(newRow);
                        grid.showHideEmptyRow(container);
                    }
                };

            var form = dialog.container.find(selectors.templatesListForm);
            grid.bindGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeTemplatesList();
            });

            form.on('submit', function (event) {
                event.preventDefault();
                searchTemplates(form);
                return false;
            });

            form.find(selectors.templateSearchButton).on('click', function () {
                searchTemplates(form, container);
            });

            container.find(selectors.templateRegisterButton).on('click', function () {
                template.openRegisterTemplateDialog(onTemplateCreated);
            });

            initializeTemplateListEvents(container);
        };

        /**
        * Search site settings template.
        */
        function searchTemplates(form, container) {
            grid.submitGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeTemplatesList();
                var searchInput = container.find(selectors.templateSearchField);
                var val = searchInput.val();
                searchInput.focus().val("");
                searchInput.val(val);
            });
        };

        /**
        * Initializes site settings template list items.
        */
        function initializeTemplateListEvents(container) {
            container.find(selectors.templateRowEditButtons).on('click', function () {
                editTemplate(container, $(this));
            });

            container.find(selectors.templatesRowDeleteButtons).on('click', function () {
                deleteTemplates(container, $(this));
            });
        };

        /**
        * Calls function, which opens dialog for a template editing.
        */
        function editTemplate(container, self) {
            var row = self.parents(selectors.templateParentRow),
                id = row.data('id');

            template.editTemplate(id, function (data) {
                if (data.Data != null) {
                    setTemplateFields(row, data);
                    grid.showHideEmptyRow(container);
                }
            });
        };

        /**
        * Deletes template from site settings template list.
        */
        function deleteTemplates(container, self) {
            var row = self.parents(selectors.templateParentRow);

            template.deleteTemplate(row, function (data) {
                messages.refreshBox(row, data);
                if (data.Success) {
                    row.remove();
                    grid.showHideEmptyRow(container);
                }
            });
        };

        /**
        * Set values, returned from server to row fields
        */
        function setTemplateFields(row, json) {
            row.data('id', json.Data.Id);
            row.data('version', json.Data.Version);
            row.find(selectors.templateNameCell).html(json.Data.TemplateName);
        };

        return template;
    });

