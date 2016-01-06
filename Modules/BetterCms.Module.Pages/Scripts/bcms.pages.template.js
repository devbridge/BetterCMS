/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.template', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.datepicker', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages',
        'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'bcms.slides.jquery', 'bcms.options', 'bcms.ko.extenders', 'bcms.pages.masterpage', 'bcms.pages', 'bcms.antiXss'],
    function ($, bcms, modal, datepicker, dynamicContent, siteSettings, messages, preview, grid, editor, slides, options, ko, masterpage, pages, antiXss) {
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
                previewImageNotFoundMessage: null,
                deletingMessage: null,
                templatesTabTitle: null,
            },
            selectors = {
                templatePreviewImageUrl: '#PreviewImageUrl',
                templatePreviewImage: '#bcms-template-preview-image',
                htmlContentTemplateRowTemplate: '#bcms-advanced-content-list-row-template',
                htmlContentTemplateRowTemplateFirstRow: 'tr:first',
                htmlContentTemplateTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                messagesContainer: "#bcms-edit-template-messages",
                siteSettingsTemplatesListForm:"#bcms-templates-form",
                templateSearchButton: '#bcms-template-search-btn',
                templateSearchField: '.bcms-search-query',

                templateRegisterButton: '#bcms-register-template-button',
                templateRowEditButtons: '.bcms-grid-item-edit-button',

                templatesRowDeleteButtons: '.bcms-grid-item-delete-button',
                templatesRowDeleteMessage: '.bcms-grid-item-message',
                templatesRowUsageLinks: '.bcms-template-usage',
                templatesRowDeleteElementsToHide: '.bcms-grid-item-delete-button, .bcms-grid-item-edit-button',
                templateParentRow: 'tr:first',
                templateNameCell: '.bcms-template-name',
                templateRowTemplate: '#bcms-template-list-row-template',
                templateRowTemplateFirstRow: 'tr:first',
                templateTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                templateInsertButtons: '.bcms-template-insert-button',

                addNewRegionButton: '#bcms-add-region-button',
                templatesListForm: '#bcms-templates-form',
                templateEditForm: '#bcms-template-form',

                regionsTable: '#bcms-regions-grid',
                regionsTab: '#bcms-tab-2',
                
                optionsTab: '#bcms-tab-3'
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
            var optionsViewModel;

            modal.open({
                title: globalization.editTemplateDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditTemplateDialogUrl, templateId), {
                        contentAvailable: function (dialog, content) {
                            optionsViewModel = initializeEditTemplateForm(dialog, content);
                        },

                        beforePost: function (form) {
                            editor.resetAutoGenerateNameId();
                            editor.setInputNames(form.find(selectors.regionsTab));

                            return optionsViewModel.isValid(true);
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
            var optionsViewModel;

            modal.open({
                title: globalization.createTemplateDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadRegisterTemplateDialogUrl, {
                        contentAvailable: function(dialog, content) {
                            optionsViewModel = initializeEditTemplateForm(dialog, content);
                        },

                        beforePost: function (form) {
                            editor.resetAutoGenerateNameId();
                            editor.setInputNames(form.find(selectors.regionsTab));
                            
                            return optionsViewModel.isValid(true);
                        },

                        postSuccess: onSaveCallback
                    });
                }
            });
        };

        /**
        * Initializes template form
        */
        function initializeEditTemplateForm(dialog, content) {
            var regionsContainer = dialog.container.find(selectors.regionsTab),
                optionsContainer = dialog.container.find(selectors.optionsTab),
                form = dialog.container.find(selectors.templateEditForm),
                templateOptions = content && content.Data ? content.Data.Options : null,
                customOptions = content && content.Data ? content.Data.CustomOptions : null,
                optionListViewModel = options.createOptionsViewModel(optionsContainer, templateOptions, customOptions);

            // Initialize regions tab
            editor.initialize(regionsContainer, {
                deleteRowMessageExtractor: function() {
                    return globalization.deleteRegionConfirmMessage;
                },
                form: form
            });
            
            regionsContainer.find(selectors.addNewRegionButton).on('click', function () {
                editor.addNewRow(regionsContainer, $(selectors.regionsTable));
            });

            // Init options tab
            ko.applyBindings(optionListViewModel, optionsContainer.get(0));

            dialog.container.find(selectors.templatePreviewImage).error(function () {
                var image = dialog.container.find(selectors.templatePreviewImage);
                if (image.attr("src") != null && image.attr("src") != "") {
                    messages.box({ container: dialog.container.find(selectors.messagesContainer) }).addWarningMessage(globalization.previewImageNotFoundMessage);
                    image.parent().hide();
                    image.removeAttr("src");
                }
            });

            dialog.container.find(selectors.templatePreviewImageUrl).blur(function () {
                var image = dialog.container.find(selectors.templatePreviewImage),
                    urlInput = dialog.container.find(selectors.templatePreviewImageUrl);

                if (urlInput.valid()) {
                    image.attr({ src: urlInput.val() });
                    image.parent().show();
                } else {
                    image.hide();
                    image.parent().removeAttr("src");
                }
            });
            
            // IE fix: by default, while loading, picture is hidden
            var previewImage = dialog.container.find(selectors.templatePreviewImage);
            if (previewImage.attr('src')) {
                previewImage.parent().show();
            }

            return optionListViewModel;
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
                messageDiv = row.find(selectors.templatesRowDeleteMessage),
                elementsToHide = row.find(selectors.templatesRowDeleteElementsToHide),
                onDeleteCompleted = function(json) {
                    messages.refreshBox(row, json);
                    messageDiv.html('');
                    messageDiv.hide();
                    elementsToHide.show();
                    if (json.Success && $.isFunction(onDeleteCallback)) {
                        onDeleteCallback(json);
                    }
                };
            
                modal.confirm({
                    content: message,
                    onAccept: function () {
                        elementsToHide.hide();
                        messageDiv.show();
                        messageDiv.html(globalization.deletingMessage);

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
                    }
                });
        };

        /**
        * Opens site settings template list dialog
        */
        template.loadSiteSettingsTemplateList = function () {
            var tabs = [],
                onShow = function (container) {
                    var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
                    if (firstVisibleInputField) {
                        firstVisibleInputField.focus();
                    }
                };
            var templates = new siteSettings.TabViewModel(globalization.templatesTabTitle, links.loadSiteSettingsTemplateListUrl, initializeTemplatesList, onShow);
            tabs.push(templates);
            var masterPages = new siteSettings.TabViewModel(masterpage.globalization.masterPagesTabTitle, masterpage.links.loadMasterPagesListUrl, masterpage.initializeMasterPagesList, onShow);
            tabs.push(masterPages);
            siteSettings.initContentTabs(tabs);
        };

        /**
        * Initializes site settings template list and list items
        */
        function initializeTemplatesList() {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container.find(selectors.siteSettingsTemplatesListForm),
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
            grid.bindGridForm(form, function (html) {
                container.html(html);
                initializeTemplatesList();
            });

            form.on('submit', function (event) {
                event.preventDefault();
                searchTemplates(form, container);
                return false;
            });

            form.find(selectors.templateSearchButton).on('click', function () {
                searchTemplates(form, container);
            });

            container.find(selectors.templateRegisterButton).on('click', function () {
                template.openRegisterTemplateDialog(onTemplateCreated);
            });

            initializeTemplateListEvents(container);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(dialog.container.find(selectors.templateSearchField), true);
        };

        /**
        * Search site settings template.
        */
        function searchTemplates(form, container) {
            grid.submitGridForm(form, function (htmlContent) {
                container.html(htmlContent);
                initializeTemplatesList();
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

            container.find(selectors.templatesRowUsageLinks).on('click', function (e) {
                bcms.stopEventPropagation(e);

                filterPagesByTemplate($(this));
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

        /*
        * Opens pages list, filtered by template
        */
        function filterPagesByTemplate(self) {
            var id = self.data('id');

            pages.openPageSelectDialog({
                params: {
                    Layout: 'l-' + id
                },
                canBeSelected: false,
                title: pages.globalization.pagesListTitle,
                disableAccept: true
            });
        }

        /**
        * Set values, returned from server to row fields
        */
        function setTemplateFields(row, json) {
            row.data('id', json.Data.Id);
            row.data('version', json.Data.Version);
            row.find(selectors.templateNameCell).html(antiXss.encodeHtml(json.Data.TemplateName));
        };

        return template;
    });

