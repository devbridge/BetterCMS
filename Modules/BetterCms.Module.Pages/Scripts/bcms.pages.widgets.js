/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.widgets', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.datepicker', 'bcms.htmlEditor',
        'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.grid',
        'bcms.slides.jquery', 'bcms.redirect', 'bcms.pages.history', 'bcms.security', 'bcms.options', 'bcms.ko.extenders', 'bcms.codeEditor',
        'bcms.pages', 'bcms.categories', 'bcms.forms', 'bcms.ko.grid', 'bcms.antiXss'],
    function ($, bcms, modal, datepicker, htmlEditor, dynamicContent, siteSettings, messages, preview, grid, slides, redirect, contentHistory, security, options, ko, codeEditor, pages, categories, forms, kogrid, antiXss) {
        'use strict';

        var widgets = {},
            links = {
                loadSiteSettingsWidgetListUrl: null,
                loadCreateHtmlContentWidgetDialogUrl: null,
                loadEditHtmlContentWidgetDialogUrl: null,
                loadCreateServerControlWidgetDialogUrl: null,
                loadEditServerControlWidgetDialogUrl: null,
                deleteWidgetUrl: null,
                loadPageContentOptionsDialogUrl: null,
                loadChildContentOptionsDialogUrl: null,
                getContentTypeUrl: null,
                getWidgetUsagesUrl: null
            },
            globalization = {
                createHtmlContentWidgetDialogTitle: null,
                editAdvancedContentDialogTitle: null,
                createWidgetDialogTitle: null,
                editWidgetDialogTitle: null,
                deleteWidgetConfirmMessage: null,
                editPageWidgetOptionsTitle: null,
                previewImageNotFoundMessage: null,
                widgetStatusPublished: null,
                widgetStatusDraft: null,
                widgetStatusPublishedWithDraft: null,
                deletingMessage: null,
                widgetUsageTitle: null,
                editChildWidgetOptionsTitle: null,
                editChildWidgetOptionsCloseButtonTitle: null,
                widgetUsagesDialogTitle: null,
                widgetUsagesType_Page: null,
                widgetUsagesType_HtmlWidget: null,
                widgetUsagesType_MasterPage: null,
                invariantLanguage: null
            },
            selectors = {
                desirableStatus: '#bcmsWidgetDesirableStatus',
                destroyDraftVersionLink: '.bcms-messages-draft-destroy',
                contentId: '#bcmsContentId',
                contentVersion: '#bcmsContentVersion',

                messagesContainer: "#bcms-edit-widget-messages",

                widgetPreviewImageUrl: '#PreviewImageUrl',
                widgetPreviewImage: '#bcms-widget-preview-image',
                widgetPreviewPageContentId: '.bcms-preview-page-content-id',
                htmlContentWidgetContentHtmlEditor: '.bcms-advanced-contenthtml',

                htmlContentWidgetRowTemplate: '#bcms-advanced-content-list-row-template',
                htmlContentWidgetRowTemplateFirstRow: 'tr:first',
                htmlContentWidgetTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                widgetsSearchButton: '#bcms-widget-search-btn',
                widgetsSearchField: '.bcms-search-query',
                widgetCreateButton: '#bcms-create-widget-button',
                widgetRegisterButton: '#bcms-register-widget-button',
                widgetRowEditButton: '.bcms-grid-item-edit-button',
                widgetRowHistoryButton: '.bcms-grid-item-history-button',
                widgetsRowDeleteButtons: '.bcms-grid-item-delete-button',
                widgetsRowDeleteMessage: '.bcms-grid-item-message',
                widgetsRowDeleteElementsToHide: '.bcms-grid-item-delete-button, .bcms-grid-item-edit-button, bcms-widget-status, .bcms-widget-status > div, .bcms-grid-item-history-button',
                widgetParentRow: 'tr:first',
                widgetNameCell: '.bcms-widget-name',
                widgetCategoryNameCell: '.bcms-category-name',
                widgetStatusCell: '.bcms-widget-status',
                widgetRowDeleteButtons: '.bcms-grid-item-delete-button',
                widgetRowTemplate: '#bcms-widget-list-row-template',
                widgetRowCells: 'td',
                widgetRowTemplateFirstRow: 'tr:first',
                widgetsTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                widgetInsertButtons: '.bcms-widget-insert-button',
                widgetRowUsageLinks: '.bcms-template-usage',

                siteSettingsWidgetsListForm: '#bcms-widgets-form',

                widgetTab: '#bcms-tab-1',
                optionsTab: '#bcms-tab-2',
                htmlWidgetJsCssTabOpener: '.bcms-tab-item[data-name="#bcms-tab-3"]',
                pageContentOptionsForm: '#bcms-options-form',

                editInSourceModeHiddenField: '#bcms-edit-in-source-mode',

                widgetUsagesGrid: '#bcms-widget-usages-grid',
                userConfirmationHiddenField: '#bcms-user-confirmed-region-deletion'
            },
            classes = {
                regionAdvancedContent: 'bcms-content-advanced',
                regionWidget: 'bcms-content-widget',
                draftStatus: 'bcms-icn-draft',
                publishStatus: 'bcms-icn-published',
                draftPublStatus: 'bcms-icn-pubdraft'
            },
            contentTypes = {
                htmlWidget: 'html-widget',
                serverWidget: 'server-widget'
            },
            widgetTypes = {
                htmlWidget: 'HtmlContent',
                serverWidget: 'ServerControl'
            },
            widgetUsageTypes = {
                page: 1,
                masterPage: 2,
                htmlWidget: 3
            },
            widgetTypeMappings = {};

        widgetTypeMappings[widgetTypes.htmlWidget] = contentTypes.htmlWidget;
        widgetTypeMappings[widgetTypes.serverWidget] = contentTypes.serverWidget;

        /**
        * Assign objects to module.
        */
        widgets.links = links;
        widgets.globalization = globalization;


        /**
        * Opens dialog with a create html content widget form.
        */
        widgets.openCreateHtmlContentWidgetDialog = function (postSuccess, availablePreviewOnPageContentId) {
            var optionsViewModel,
                editorId;

            modal.edit({
                isPreviewAvailable: availablePreviewOnPageContentId != null,
                title: globalization.createHtmlContentWidgetDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadCreateHtmlContentWidgetDialogUrl, {
                        contentAvailable: function (dialog, content) {
                            editorId = dialog.container.find(selectors.htmlContentWidgetContentHtmlEditor).attr('id');

                            var editInSourceMode = false;
                            if (content && content.Data && content.Data.EditInSourceMode) {
                                editInSourceMode = true;
                            }
                            optionsViewModel = initializeEditHtmlContentWidgetForm(dialog, availablePreviewOnPageContentId, postSuccess, editInSourceMode, content, editorId, false);
                        },

                        beforePost: function () {
                            var editInSourceMode = htmlEditor.isSourceMode(editorId);
                            childDialog.container.find(selectors.editInSourceModeHiddenField).val(editInSourceMode);

                            return optionsViewModel.isValid(true);
                        },

                        postSuccess: postSuccess,

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, editorId, optionsViewModel);
                        },
                        formContentType: 'application/json; charset=utf-8'
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyHtmlEditorInstance(editorId);
                },
                onClose: function () {
                    htmlEditor.destroyHtmlEditorInstance(editorId);
                }
            });
        };

        /**
        * Opens dialog with an edit html content widget form.
        */
        widgets.openEditHtmlContentWidgetDialog = function (id, postSuccess, availablePreviewOnPageContentId, onCloseCallback, includeChildRegions) {
            var optionsViewModel,
                editorId;

            includeChildRegions = (includeChildRegions === true) ? 1 : 0;

            modal.edit({
                isPreviewAvailable: availablePreviewOnPageContentId != null,
                title: globalization.editAdvancedContentDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditHtmlContentWidgetDialogUrl, id), {
                        contentAvailable: function (dialog, content) {
                            editorId = dialog.container.find(selectors.htmlContentWidgetContentHtmlEditor).attr('id');

                            var editInSourceMode = false;
                            if (content && content.Data && content.Data.EditInSourceMode) {
                                editInSourceMode = true;
                            }
                            optionsViewModel = initializeEditHtmlContentWidgetForm(dialog, availablePreviewOnPageContentId, postSuccess, editInSourceMode, content, editorId, includeChildRegions);
                        },

                        beforePost: function () {
                            var editInSourceMode = htmlEditor.isSourceMode(editorId);
                            childDialog.container.find(selectors.editInSourceModeHiddenField).val(editInSourceMode);

                            return optionsViewModel.isValid(true);
                        },

                        postSuccess: postSuccess,

                        postError: function (json) {
                            if (json.Data && json.Data.ConfirmationMessage) {
                                modal.confirm({
                                    content: json.Data.ConfirmationMessage,
                                    onAccept: function () {
                                        childDialog.container.find(selectors.userConfirmationHiddenField).val(true);
                                        childDialog.submitForm();

                                        return true;
                                    }
                                });
                            }
                        },

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, editorId, optionsViewModel, function (data) {
                                if (includeChildRegions) {
                                    data.IncludeChildRegions = true;
                                }
                            });
                        },
                        formContentType: 'application/json; charset=utf-8'
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyHtmlEditorInstance(editorId);
                },
                onClose: function () {
                    htmlEditor.destroyHtmlEditorInstance(editorId);

                    if ($.isFunction(onCloseCallback)) {
                        onCloseCallback();
                    }
                }
            });
        };

        /**
        * Opens ServerControlWidget edit dialog.
        */
        widgets.openEditServerControlWidgetDialog = function (widgetId, onSaveCallback, availablePreviewOnPageContentId, onCloseCallback) {
            var optionsViewModel;

            modal.edit({
                isPreviewAvailable: availablePreviewOnPageContentId != null,
                disableSaveDraft: true,
                title: globalization.editWidgetDialogTitle,
                onClose: onCloseCallback,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditServerControlWidgetDialogUrl, widgetId), {
                        contentAvailable: function (dialog, content) {
                            optionsViewModel = initializeEditServerControlWidgetForm(dialog, availablePreviewOnPageContentId, onSaveCallback, content.Data);
                        },

                        beforePost: function () {
                            return optionsViewModel.isValid(true);
                        },

                        postSuccess: onSaveCallback,

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, null, optionsViewModel);
                        },
                        formContentType: 'application/json; charset=utf-8'
                    });
                }
            });
        };

        /**
        * Opens widget create form from site settings widgets list
        */
        widgets.openCreateServerControlWidgetDialog = function (onSaveCallback, availablePreviewOnPageContentId) {
            var optionsViewModel;

            modal.edit({
                isPreviewAvailable: availablePreviewOnPageContentId != null,
                disableSaveDraft: true,
                title: globalization.createWidgetDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadCreateServerControlWidgetDialogUrl, {
                        contentAvailable: function (dialog, content) {
                            optionsViewModel = initializeEditServerControlWidgetForm(dialog, availablePreviewOnPageContentId, onSaveCallback, content.Data);
                        },

                        beforePost: function () {
                            return optionsViewModel.isValid(true);
                        },

                        postSuccess: onSaveCallback,

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, null, optionsViewModel);
                        },
                        formContentType: 'application/json; charset=utf-8'
                    });
                }
            });
        };

        /**
        * Initializes 'Edit Html Content Widget' dialog form.
        */

        function WidgetEditViewModel(data) {
            var self = this,
                categorieslist = data.Categories;
            self.categories = new categories.CategoriesListViewModel(categorieslist, data.CategoriesFilterKey);
        }

        function initializeEditHtmlContentWidgetForm(dialog, availablePreviewOnPageContentId, onSaveCallback, editInSourceMode, content, editorId, includeChildRegions) {
            var optionsContainer = dialog.container.find(selectors.optionsTab),
                widgetEditContainer = dialog.container.find(selectors.widgetTab),
                data = content.Data || {},
                widgetOptions = data.Options,
                customOptions = data.CustomOptions,
                showLanguages = data.ShowLanguages,
                languages = data.Languages,
                optionListViewModel = options.createOptionsViewModel(optionsContainer, widgetOptions, customOptions, showLanguages, languages),
                widgetEditViewModel = new WidgetEditViewModel(data),
                codeEditorInitialized = false;


            ko.applyBindings(optionListViewModel, optionsContainer.get(0));
            ko.applyBindings(widgetEditViewModel, widgetEditContainer.get(0));

            if (availablePreviewOnPageContentId !== null) {
                dialog.container.find(selectors.widgetPreviewPageContentId).val(availablePreviewOnPageContentId);
            }

            dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
                var contentId = dialog.container.find(selectors.contentId).val(),
                    contentVersion = dialog.container.find(selectors.contentVersion).val();

                contentHistory.destroyDraftVersion(contentId, contentVersion, includeChildRegions, dialog.container, function (publishedId, json) {
                    dialog.close();

                    var onCloseCallback = function () {
                        onWidgetCloseCallback(onSaveCallback, json);
                    };

                    widgets.openEditHtmlContentWidgetDialog(publishedId, onSaveCallback, availablePreviewOnPageContentId, onCloseCallback);
                });
            });

            var editorHeight = modal.maximizeChildHeight(dialog.container.find("#" + editorId), dialog);

            htmlEditor.initializeHtmlEditor(editorId, data.Id, {
                cmsEditorType: htmlEditor.cmsEditorTypes.widget,
                height: editorHeight
            }, editInSourceMode);
            htmlEditor.enableInsertDynamicRegion(editorId, false, data.LastDynamicRegionNumber);

            dialog.container.find(selectors.htmlWidgetJsCssTabOpener).on('click', function () {
                if (!codeEditorInitialized) {
                    codeEditor.initialize(dialog.container, dialog, {
                        cmsEditorType: htmlEditor.cmsEditorTypes.widget
                    });
                    codeEditorInitialized = true;
                }
            });

            return optionListViewModel;
        };

        /**
        * Initializes widget form
        */
        function initializeEditServerControlWidgetForm(dialog, availablePreviewOnPageContentId, onSaveCallback, data) {
            if (availablePreviewOnPageContentId !== null) {
                dialog.container.find(selectors.widgetPreviewPageContentId).val(availablePreviewOnPageContentId);
            }

            var optionsContainer = dialog.container.find(selectors.optionsTab),
                widgetEditContainer = dialog.container.find(selectors.widgetTab),
                widgetOptions = data != null ? data.Options : null,
                customOptions = data != null ? data.CustomOptions : null,
                showLanguages = data != null ? data.ShowLanguages : null,
                languages = data != null ? data.Languages : null,
                optionListViewModel = options.createOptionsViewModel(optionsContainer, widgetOptions, customOptions, showLanguages, languages),
                widgetEditViewModel = new WidgetEditViewModel(data);

            ko.applyBindings(optionListViewModel, optionsContainer.get(0));
            ko.applyBindings(widgetEditViewModel, widgetEditContainer.get(0));

            dialog.container.find(selectors.widgetPreviewImage).error(function () {
                var image = dialog.container.find(selectors.widgetPreviewImage);
                if (image.attr("src") != null && image.attr("src") != "") {
                    messages.box({ container: dialog.container.find(selectors.messagesContainer) }).addWarningMessage(globalization.previewImageNotFoundMessage);
                    image.parent().hide();
                    image.removeAttr("src");
                }
            });

            dialog.container.find(selectors.widgetPreviewImageUrl).blur(function () {
                var image = dialog.container.find(selectors.widgetPreviewImage),
                    urlInput = dialog.container.find(selectors.widgetPreviewImageUrl);

                if (urlInput.valid()) {
                    image.attr({ src: urlInput.val() });
                    image.parent().show();
                } else {
                    image.hide();
                    image.parent().removeAttr("src");
                }
            });

            dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
                var contentId = dialog.container.find(selectors.contentId).val(),
                    contentVersion = dialog.container.find(selectors.contentVersion).val();

                contentHistory.destroyDraftVersion(contentId, contentVersion, false, dialog.container, function (publishedId, json) {
                    dialog.close();

                    var onCloseCallback = function () {
                        onWidgetCloseCallback(onSaveCallback, json);
                    };

                    widgets.openEditServerControlWidgetDialog(publishedId, onSaveCallback, availablePreviewOnPageContentId, onCloseCallback);
                });
            });

            // IE fix: by default, while loading, picture is hidden
            var previewImage = dialog.container.find(selectors.widgetPreviewImage);
            if (previewImage.attr('src')) {
                previewImage.parent().show();
            }


            return optionListViewModel;
        };

        /*
        * Open a widget edit dialog by the specified widget type.
        */
        widgets.editWidget = function (widgetId, widgetType, onSaveCallback, previewAvailableOnPageContentId) {
            if (widgetType === widgetTypes.serverWidget) {
                widgets.openEditServerControlWidgetDialog(widgetId, onSaveCallback, previewAvailableOnPageContentId);
            }
            else if (widgetType === widgetTypes.htmlWidget) {
                widgets.openEditHtmlContentWidgetDialog(widgetId, onSaveCallback, previewAvailableOnPageContentId);
            } else {
                throw new Error($.format('A widget type "{0}" is unknown and edit action is imposible.', widgetType));
            }
        };

        /**
        * Deletes widget.
        */
        widgets.deleteWidget = function (widgetId, widgetVersion, widgetName, onBeforeDelete, onDeleteCallback, onErrorCallback) {
            var url = $.format(links.deleteWidgetUrl, widgetId, widgetVersion),
                message = $.format(globalization.deleteWidgetConfirmMessage, widgetName),

                onDeleteCompleted = function (json) {
                    if (json.Success && $.isFunction(onDeleteCallback)) {
                        onDeleteCallback(json);
                    } else if (!json.Success && $.isFunction(onErrorCallback)) {
                        onErrorCallback(json);
                    }
                };

            modal.confirm({
                content: message,
                onAccept: function () {
                    onBeforeDelete();

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
        * Opens dialog for editing widget options 
        */
        widgets.configureWidget = function (id, onSaveCallback, opts) {
            var optionListViewModel;

            opts = $.extend({
                title: globalization.editPageWidgetOptionsTitle,
                url: $.format(links.loadPageContentOptionsDialogUrl, id),
                disableAccept: false,
                cancelTitle: '',
                onCloseClick: null,
                optionListViewModel: null
            }, opts);

            modal.open({
                title: opts.title,
                disableAccept: opts.disableAccept,
                cancelTitle: opts.cancelTitle,
                onLoad: function (dialog) {
                    var url = opts.url;
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog, content) {
                            var optionsContainer = contentDialog.container.find(selectors.pageContentOptionsForm);

                            optionListViewModel = opts.optionListViewModel
                                || options.createOptionValuesViewModel(optionsContainer, content.Data.OptionValues, content.Data.CustomOptions, content.Data.ShowLanguages, content.Data.Languages);

                            ko.applyBindings(optionListViewModel, optionsContainer.get(0));
                        },

                        beforePost: function () {
                            return optionListViewModel.isValid(true);
                        },

                        postSuccess: function (json) {
                            if ($.isFunction(onSaveCallback)) {
                                onSaveCallback(json);
                            }
                        }
                    });
                },

                onCloseClick: function () {
                    if (opts.onCloseClick && $.isFunction(opts.onCloseClick)) {
                        return opts.onCloseClick(optionListViewModel);
                    }

                    return true;
                }
            });
        };

        /**
        * Opens site settings widgets list dialog
        */
        widgets.loadSiteSettingsWidgetList = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsWidgetListUrl, {
                contentAvailable: initializeSiteSettingsWidgetsList
            });
        };

        /**
        * Initializes site settings widgets list and list items
        */
        function initializeSiteSettingsWidgetsList() {
            var dialog = siteSettings.getModalDialog(),
                container = dialog.container,
                onWidgetCreated = function (json) {
                    if (json.Success && json.Data != null) {
                        var template = $(selectors.widgetRowTemplate),
                            newRow = $(template.html()).find(selectors.widgetRowTemplateFirstRow);
                        setWidgetFields(newRow, json);
                        messages.refreshBox(container, json);
                        newRow.insertBefore($(selectors.widgetsTableFirstRow, container));
                        initializeSiteSettingsWidgetListEvents(newRow);
                        grid.showHideEmptyRow(container);
                    }
                };

            var form = dialog.container.find(selectors.siteSettingsWidgetsListForm);
            grid.bindGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeSiteSettingsWidgetsList(data);
            });

            form.on('submit', function (event) {
                event.preventDefault();
                searchSiteSettingsWidgets(form);
                return false;
            });

            form.find(selectors.widgetsSearchButton).on('click', function () {
                searchSiteSettingsWidgets(form);
            });

            container.find(selectors.widgetCreateButton).on('click', function () {
                widgets.openCreateHtmlContentWidgetDialog(onWidgetCreated, null);
            });

            container.find(selectors.widgetRegisterButton).on('click', function () {
                widgets.openCreateServerControlWidgetDialog(onWidgetCreated);
            });

            initializeSiteSettingsWidgetListEvents(container);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(dialog.container.find(selectors.widgetsSearchField), true);
        };

        /**
        * Search site settings widgets.
        */
        function searchSiteSettingsWidgets(form) {
            grid.submitGridForm(form, function (data) {
                siteSettings.setContent(data);
                initializeSiteSettingsWidgetsList();
            });
        };

        /**
        * Initializes site settings widgets list items.
        */
        function initializeSiteSettingsWidgetListEvents(container) {
            container.find(selectors.widgetRowCells).on('click', function () {
                var editButton = $(this).parents(selectors.widgetParentRow).find(selectors.widgetRowEditButton);
                editSiteSettingsWidget(container, editButton);
            });

            container.find(selectors.widgetRowHistoryButton).on('click', function (event) {
                bcms.stopEventPropagation(event);

                var historyButton = $(this);
                var contentId = historyButton.parents(selectors.widgetParentRow).data('id');
                contentHistory.openPageContentHistoryDialog(contentId, null);
            });

            container.find(selectors.widgetsRowDeleteButtons).on('click', function (event) {
                bcms.stopEventPropagation(event);
                deleteSiteSettingsWidget(container, $(this));
            });

            container.find(selectors.widgetRowUsageLinks).on('click', function (event) {
                bcms.stopEventPropagation(event);

                findWidgetUsages($(this));
            });
        };

        /*
        * Opens pages and widgets list, filtered by using widget
        */
        function findWidgetUsages(self) {
            var widgetId = self.parents(selectors.widgetParentRow).data('originalId'),
                url = $.format(links.getWidgetUsagesUrl, widgetId);

            modal.open({
                title: globalization.widgetUsagesDialogTitle,
                disableAccept: true,
                onLoad: function (dialog) {
                    dynamicContent.setContentFromUrl(dialog, url, {
                        done: function (json) {

                            var data = (json.Success == true) ? json.Data : null,
                                opts = {
                                    createItem: function (listModel, item) {
                                        var newItem = new kogrid.ItemViewModel(listModel, item);

                                        newItem.title = ko.observable(item.Title);
                                        newItem.url = ko.observable(item.Url);
                                        newItem.typeTitle = ko.observable();
                                        newItem.type = ko.observable(item.Type);

                                        if (item.Type == widgetUsageTypes.page) {
                                            newItem.typeTitle(globalization.widgetUsagesType_Page);
                                        } else if (item.Type == widgetUsageTypes.masterPage) {
                                            newItem.typeTitle(globalization.widgetUsagesType_MasterPage);
                                        } else if (item.Type == widgetUsageTypes.htmlWidget) {
                                            newItem.typeTitle(globalization.widgetUsagesType_HtmlWidget);
                                            newItem.deletingIsDisabled = true;
                                        }

                                        newItem.editItem = function () {
                                            if (this.type() == widgetUsageTypes.page || this.type() == widgetUsageTypes.masterPage) {
                                                pages.openEditPageDialog(this.id(), function (pageData) {
                                                    newItem.url(pageData.Data.PageUrl);
                                                    newItem.title(antiXss.encodeHtml(pageData.Data.Title));
                                                });
                                            } else if (item.Type == widgetUsageTypes.htmlWidget) {
                                                widgets.openEditHtmlContentWidgetDialog(this.id(), function (widgetData) {
                                                    newItem.title(widgetData.Data.WidgetName);
                                                });
                                            }
                                        };

                                        newItem.openItem = function () {
                                            if (this.url()) {
                                                window.open(this.url());
                                            } else {
                                                this.editItem();
                                            }
                                        };

                                        return newItem;
                                    }
                                },
                                container = dialog.container.find(selectors.widgetUsagesGrid),
                                usagesViewModel = new kogrid.ListViewModel(container, url, data.Items, data.GridOptions, opts);

                            ko.applyBindings(usagesViewModel, container.get(0));
                        }
                    });
                }
            });
        }

        /**
        * Calls function, which opens dialog for a widget editing.
        */
        function editSiteSettingsWidget(container, self) {
            var row = self.parents(selectors.widgetParentRow),
                id = row.data('id'),
                widgetType = row.data('type');

            widgets.editWidget(id, widgetType, function (data) {
                if (data.Data != null) {
                    setWidgetFields(row, data);
                    grid.showHideEmptyRow(container);
                }
            }, null);
        };

        /**
        * Deletes widget from site settings widgets list.
        */
        function deleteSiteSettingsWidget(container, self) {
            var row = self.parents(selectors.widgetParentRow),
                widgetId = row.data('originalId'),
                widgetVersion = row.data('originalVersion'),
                widgetName = row.find(selectors.widgetNameCell).html(),
                messageDiv = row.find(selectors.widgetsRowDeleteMessage),
                elementsToHide = row.find(selectors.widgetsRowDeleteElementsToHide),
                onComplete = function (data) {
                    messageDiv.html('');
                    messageDiv.hide();
                    elementsToHide.show();

                    messages.refreshBox(row, data);
                };

            widgets.deleteWidget(widgetId, widgetVersion, widgetName,
                function () {
                    elementsToHide.hide();
                    messageDiv.show();
                    messageDiv.html(globalization.deletingMessage);
                },
                function (data) {
                    onComplete(data);
                    if (data.Success) {
                        row.remove();
                        grid.showHideEmptyRow(container);
                    }
                },
                onComplete
            );
        };

        /**
        * Set values, returned from server to row fields
        */
        function setWidgetFields(row, json) {
            row.data('id', json.Data.Id);
            row.data('originalId', json.Data.OriginalId);
            row.data('version', json.Data.Version);
            row.data('originalVersion', json.Data.OriginalVersion);
            row.find(selectors.widgetNameCell).html(antiXss.encodeHtml(json.Data.WidgetName));
            row.find(selectors.widgetCategoryNameCell).html(antiXss.encodeHtml(json.Data.CategoryName));

            // Set widget type, if it's set
            if (json.Data.WidgetType) {
                row.data('type', json.Data.WidgetType);
            }

            var status = '';
            var statusContainer = $("<div></div>");
            if (json.Data.IsPublished && json.Data.HasDraft) {
                statusContainer.addClass(classes.draftPublStatus);
                status = globalization.widgetStatusPublishedWithDraft;
            } else if (json.Data.IsPublished) {
                statusContainer.addClass(classes.publishStatus);
                status = globalization.widgetStatusPublished;
            } else if (json.Data.HasDraft) {
                statusContainer.addClass(classes.draftStatus);
                status = globalization.widgetStatusDraft;
            }
            statusContainer.html(status);
            row.find(selectors.widgetStatusCell).html(statusContainer);
        };

        /**
        * Called when content view model is created
        */
        function onContentModelCreated(contentViewModel) {
            var contentId = contentViewModel.contentId,
                pageContentId = contentViewModel.pageContentId,
                onAfterSuccessCallback,
                onSave = function (json) {
                    var result = json != null ? json.Data : null;
                    if (result && result.DesirableStatus === bcms.contentStatus.preview) {
                        try {
                            preview.previewPageContent(bcms.pageId, result.PreviewOnPageContentId);
                        } finally {
                            return false;
                        }
                    } else {
                        if ($.isFunction(onAfterSuccessCallback)) {
                            if (json) {
                                json.Data.ContentType = widgetTypeMappings[json.Data.WidgetType];
                            }
                            onAfterSuccessCallback(json);
                        } else {
                            redirect.ReloadWithAlert();
                        }
                    }
                    return true;
                };

            if (contentViewModel.contentType == contentTypes.serverWidget) {
                // Edit
                contentViewModel.onEditContent = function (onSuccess) {
                    onAfterSuccessCallback = onSuccess;
                    widgets.openEditServerControlWidgetDialog(contentId, onSave, pageContentId);
                };

                // Configure
                contentViewModel.onConfigureContent = function (onSuccess) {
                    widgets.configureWidget(pageContentId, function (json) {
                        if ($.isFunction(onSuccess)) {
                            onSuccess(json);
                        } else {
                            redirect.ReloadWithAlert();
                        }
                    });
                };

                if (!security.IsAuthorized(["BcmsAdministration"])) {
                    contentViewModel.visibleButtons.history = false;
                    contentViewModel.visibleButtons.edit = false;
                }
                if (!security.IsAuthorized(["BcmsEditContent"])) {
                    contentViewModel.visibleButtons.configure = false;
                    contentViewModel.visibleButtons["delete"] = false;
                }
            } else if (contentViewModel.contentType == contentTypes.htmlWidget) {
                // Edit
                contentViewModel.onEditContent = function (onSuccess, includeChildRegions) {
                    onAfterSuccessCallback = onSuccess;
                    widgets.openEditHtmlContentWidgetDialog(contentId, onSave, pageContentId, null, includeChildRegions);
                };

                // Configure
                contentViewModel.onConfigureContent = function (onSuccess) {
                    widgets.configureWidget(pageContentId, function (json) {
                        if ($.isFunction(onSuccess)) {
                            onSuccess(json);
                        } else {
                            redirect.ReloadWithAlert();
                        }
                    });
                };

                if (!security.IsAuthorized(["BcmsAdministration"])) {
                    contentViewModel.visibleButtons.history = false;
                    contentViewModel.visibleButtons.edit = false;
                }
                if (!security.IsAuthorized(["BcmsEditContent"])) {
                    contentViewModel.visibleButtons.configure = false;
                    contentViewModel.visibleButtons["delete"] = false;
                }
            }
        }

        /**
        * Called when editing page content
        */
        function onWidgetCloseCallback(onSaveCallback, json) {
            if ($.isFunction(onSaveCallback)) {
                onSaveCallback(json);
            }
        }

        /**
        * Called when editing child widget options (called from HTML editor)
        */
        function onEditChildWidgetOptions(data) {
            var assignmentId = data.assignmentId,
                widgetId = data.widgetId,
                contentId = data.contentId,
                onCloseClick = data.onCloseClick,
                optionListViewModel = data.optionListViewModel;

            if (!assignmentId && !widgetId) {
                bcms.logger.error("Cannot open child widget options modal window. assignmentId or widgetId should be set.");
                return;
            }

            widgets.configureWidget('', function () {
                // Do nothing - just close modal and that's it
            }, {
                title: globalization.editChildWidgetOptionsTitle,
                url: $.format(links.loadChildContentOptionsDialogUrl, contentId, assignmentId, widgetId, !optionListViewModel),
                disableAccept: true,
                cancelTitle: globalization.editChildWidgetOptionsCloseButtonTitle,
                onCloseClick: onCloseClick,
                optionListViewModel: optionListViewModel
            });
        }

        /**
        * Called when editing widget options
        */
        function onEditWidget(data) {
            var contentId = data.contentId,
                url = $.format(links.getContentTypeUrl, contentId),
                onCompleted = function (json) {
                    if (json.Success && json.Data && json.Data.Id && json.Data.Type) {
                        widgets.editWidget(json.Data.Id, json.Data.Type);
                    } else {
                        bcms.logger.error("Failed to load child widget type.");
                    }
                };;

            $.ajax({
                type: 'GET',
                url: url,
                cache: false
            })
                .done(function (json) {
                    onCompleted(json);
                })
                .fail(function (response) {
                    onCompleted(bcms.parseFailedResponse(response));
                });
        }

        /*
         * Returns child content options view models
         */
        function getChildContentOptions(htmlEditorId) {
            var editorInstance = htmlEditor.getInstance(htmlEditorId);

            if (editorInstance) {
                return editorInstance.childWidgetOptions;
            }

            return null;
        }

        /**
         * Serializes content edit form with child widget options
         */
        widgets.serializeFormWithChildWidgetOptions = function (form, htmlEditorId, optionsViewModel, onBeforeStringify) {
            var serializedForm = forms.serializeToObject(form, true),
                childOptions = htmlEditorId != null ? getChildContentOptions(htmlEditorId) : null,
                childContentOptionValues = [],
                i, j, needFix, model;

            // MVC's double checkbox fix - take only first item of an array
            for (i in serializedForm) {
                if ($.isArray(serializedForm[i]) && serializedForm[i].length == 2) {
                    needFix = true;
                    for (j = 0; j < 2; j++) {
                        if (serializedForm[i][j] !== "true" && serializedForm[i][j] !== "false") {
                            needFix = false;
                            continue;
                        }
                    }

                    if (needFix) {
                        serializedForm[i] = serializedForm[i][0];
                    }
                }
            }

            if (optionsViewModel != null && optionsViewModel.serializeToObject != null) {
                serializedForm.Options = optionsViewModel.serializeToObject();
            }

            if (childOptions) {
                for (i in childOptions) {
                    childContentOptionValues.push({
                        OptionValuesContainerId: i,
                        OptionValues: childOptions[i].toJson()
                    });
                }
            }


            if ($.isFunction(onBeforeStringify)) {
                onBeforeStringify(serializedForm, childContentOptionValues);
            }

            model = {
                content: serializedForm,
                childContentOptionValues: childContentOptionValues
            };

            return JSON.stringify(model);
        };

        /**
        * Initializes widgets module.
        */
        widgets.init = function () {
            bcms.logger.debug('Initializing bcms.pages.widgets module.');
        };

        /**
        * Subscribe to events
        */
        bcms.on(bcms.events.contentModelCreated, onContentModelCreated);
        bcms.on(htmlEditor.events.editChildWidgetOptions, onEditChildWidgetOptions);
        bcms.on(htmlEditor.events.editWidget, onEditWidget);

        /**
        * Register initialization
        */
        bcms.registerInit(widgets.init);

        return widgets;
    });