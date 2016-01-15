/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.pages.template.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.pages.template', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.datepicker', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages',
        'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'bcms.options', 'bcms.ko.extenders', 'bcms.pages.masterpage', 'bcms.pages', 'bcms.antiXss', 'bcms.pages.properties'],
    function ($, bcms, modal, datepicker, dynamicContent, siteSettings, messages, preview, grid, editor, options, ko, masterpage, pages, antiXss, pageProperties) {
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
                templateNoImagePreview: '#bcms-template-no-preview',
                htmlContentTemplateRowTemplate: '#bcms-advanced-content-list-row-template',
                htmlContentTemplateRowTemplateFirstRow: 'tr:first',
                htmlContentTemplateTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                messagesContainer: "#bcms-edit-template-messages",
                siteSettingsTemplatesListForm:"#bcms-templates-form",
                templateSearchButton: '#bcms-template-search-btn',
                templateSearchField: '.bcms-search-query',

                templateRegisterButton: '#bcms-register-template-button',
                templateCreateButton: '#bcms-create-page-button',
                templateRowEditButtons: '.bcms-grid-item-edit-button',

                templatesRowDeleteButtons: '.bcms-grid-item-delete-button',
                templatesRowDeleteMessage: '.bcms-grid-item-message',
                templatesRowUsageLinks: '.bcms-action-usage',
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
                
                optionsTab: '#bcms-tab-3',
                
                siteSettingsButtonOpener: ".bcms-btn-opener",
                siteSettingsButtonHolder: ".bcms-btn-opener-holder"
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
                var image = dialog.container.find(selectors.templatePreviewImage),
                    noPreviewGrid = dialog.container.find(selectors.templateNoImagePreview);
                noPreviewGrid.show();
                if (image.attr("src") != null && image.attr("src") != "") {
                    messages.box({ container: dialog.container.find(selectors.messagesContainer) }).addWarningMessage(globalization.previewImageNotFoundMessage);
                    image.hide();
                    image.removeAttr("src");
                }
            });

            dialog.container.find(selectors.templatePreviewImageUrl).blur(function () {
                var image = dialog.container.find(selectors.templatePreviewImage),
                    urlInput = dialog.container.find(selectors.templatePreviewImageUrl),
                    noPreviewGrid = dialog.container.find(selectors.templateNoImagePreview);

                if (urlInput.valid()) {
                    image.attr({ src: urlInput.val() });
                    noPreviewGrid.hide();
                    image.show();
                } else {
                    image.hide();
                    noPreviewGrid.show();
                    image.removeAttr("src");
                }
            });
            
            // IE fix: by default, while loading, picture is hidden
            var previewImage = dialog.container.find(selectors.templatePreviewImage),
                noPreviewGrid = dialog.container.find(selectors.templateNoImagePreview);
            if (previewImage.attr('src')) {
                noPreviewGrid.hide();
                previewImage.show();
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
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsTemplateListUrl, {
                contentAvailable: initializeTemplatesList
            });
        };

        /**
        * Initializes site settings template list and list items
        */
        function initializeTemplatesList(isSearchResult) {
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
                var parent = $(this).parent();
                if (!parent.hasClass('bcms-active-search')) {
                    form.find(selectors.templateSearchField).prop('disabled', false);
                    parent.addClass('bcms-active-search');
                    form.find(selectors.templateSearchField).focus();
                } else {
                    form.find(selectors.templateSearchField).prop('disabled', true);
                    parent.removeClass('bcms-active-search');
                    form.find(selectors.templateSearchField).val('');
                }
            });

            form.find(selectors.siteSettingsButtonOpener).on('click', function (event) {
                bcms.stopEventPropagation(event);
                var holder = form.find(selectors.siteSettingsButtonHolder);
                if (!holder.hasClass('bcms-opened')) {
                    holder.addClass('bcms-opened');
                } else {
                    holder.removeClass('bcms-opened');
                }
            });

            bcms.on(bcms.events.bodyClick, function (event) {
                var holder = form.find(selectors.siteSettingsButtonHolder);
                if (holder.hasClass('bcms-opened')) {
                    holder.removeClass('bcms-opened');
                }
            });

            container.find(selectors.templateRegisterButton).on('click', function () {
                template.openRegisterTemplateDialog(onTemplateCreated);
            });

            if (isSearchResult === true) {
                form.find(selectors.templateSearchButton).parent().addClass('bcms-active-search');
            } else {
                form.find(selectors.templateSearchField).prop('disabled', true);
            }

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
                initializeTemplatesList(true);
            });
        };

        /**
        * Initializes site settings template list items.
        */
        function initializeTemplateListEvents(container) {
            container.find(selectors.templateRowEditButtons).on('click', function (event) {
                var url = $(this).data('url');
                if (url) {
                    bcms.stopEventPropagation(event);
                    window.open(url);
                } else {
                    editTemplate(container, $(this));
                }
            });

            container.find(selectors.templatesRowDeleteButtons).on('click', function () {
                deleteTemplates(container, $(this));
            });

            container.find(selectors.templatesRowUsageLinks).on('click', function (e) {
                bcms.stopEventPropagation(e);

                filterPagesByTemplate($(this));
            });

            container.find(selectors.templateCreateButton).on('click', function () {
                addMasterPage(container);
            });
        };

        /**
        * Calls function, which opens dialog for a template editing.
        */
        function editTemplate(container, self) {
            var row = self.parents(selectors.templateParentRow),
                id = row.data('id'),
                isMasterPage = row.data('ismasterpage');

            if (isMasterPage == 0) {

                template.editTemplate(id, function(data) {
                    if (data.Data != null) {
                        setTemplateFields(row, data);
                        grid.showHideEmptyRow(container);
                    }
                });

            } else {
                editMasterPage(row, container, function (data) {
                    if (data.Data != null) {
                        setMasterPageFields(row, data);
                        grid.showHideEmptyRow(container);
                    }
                });
            }
        };

        /**
        * Deletes template from site settings template list.
        */
        function deleteTemplates(container, self) {
            var row = self.parents(selectors.templateParentRow),
                isMasterPage = row.data('ismasterpage');

            if (isMasterPage == 1) {
                deleteMasterPage(row, container);
            } else {
                template.deleteTemplate(row, function(data) {
                    messages.refreshBox(row, data);
                    if (data.Success) {
                        row.remove();
                        grid.showHideEmptyRow(container);
                    }
                });
            }
        };

        /*
        * Opens pages list, filtered by template
        */
        function filterPagesByTemplate(self) {
            var row = self.parents(selectors.templateParentRow),
                id = row.data('id'),
                isMasterPage = row.data('ismasterpage');

            if (isMasterPage == 1) {
                filterPagesByMasterPage(row);
            } else {
                pages.openPageSelectDialog({
                    params: {
                        Layout: 'l-' + id
                    },
                    canBeSelected: false,
                    title: pages.globalization.pagesListTitle,
                    disableAccept: true
                });
            }
        }


        function addMasterPage(container) {
            var onCreated = function(json) {
                if (json.Data != null) {
                    var rowtemplate = $(selectors.templateRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.templateRowTemplateFirstRow);
                    setMasterPageFields(newRow, json);
                    messages.refreshBox(selectors.templatesListForm, json);
                    newRow.insertBefore($(selectors.templateTableFirstRow, container));
                    initializeTemplateListEvents(newRow);
                    grid.showHideEmptyRow(container);
                }
            };
            pages.openCreatePageDialog(onCreated, true);
        };

        function editMasterPage(self, container, postSuccess) {
            var id = self.data('id');

            pageProperties.openEditPageDialog(id, postSuccess, globalization.editMasterPagePropertiesModalTitle);
        };

        function filterPagesByMasterPage(self) {
            var id = self.data('id');

            pages.openPageSelectDialog({
                params: {
                    Layout: 'm-' + id
                },
                canBeSelected: false,
                title: pages.globalization.pagesListTitle,
                disableAccept: true
            });
        }

        function deleteMasterPage(self, container) {
            var id = self.data('id');

            pages.deletePage(id, function (json) {
                messages.refreshBox(self, json);
                if (json.Success) {
                    self.remove();
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
            row.data('ismasterpage', 0);
            row.find(selectors.templateNameCell).html(antiXss.encodeHtml(json.Data.TemplateName));
        };

        function setMasterPageFields(row, json) {
            row.data('id', json.Data.PageId);
            row.data('version', json.Data.Version);
            row.data('ismasterpage', 1);
            row.find(selectors.templateNameCell).html(antiXss.encodeHtml(json.Data.Title));
            row.find(selectors.templateNameCell).data('url',json.Data.PageUrl);
        }

        return template;
    });

