/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.widgets', ['jquery', 'bcms', 'bcms.modal', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'slides.jquery', 'bcms.redirect'],
    function($, bcms, modal, datepicker, htmlEditor, dynamicContent, siteSettings, messages, preview, grid, editor, slides, redirect) {
        'use strict';

        var widgets = {},
            links = {
                loadSiteSettingsWidgetListUrl: null,                
                loadCreateHtmlContentWidgetDialogUrl: null,
                loadEditHtmlContentWidgetDialogUrl: null,
                loadCreateServerControlWidgetDialogUrl: null,
                loadEditServerControlWidgetDialogUrl: null,                               
                deleteWidgetUrl: null,
                loadPageContentOptionsDialogUrl: null
            },
            globalization = {
                createHtmlContentWidgetDialogTitle: null,
                editAdvancedContentDialogTitle: null,
                createWidgetDialogTitle: null,
                editWidgetDialogTitle: null,
                deleteWidgetConfirmMessage: null,
                deleteOptionConfirmMessage: null,
                editPageWidgetOptionsTitle: null,
                previewImageNotFoundMessage: null
            },
            selectors = {                                
                enableCustomCss: '#bcms-enable-custom-css',
                customCssContainer: '#bcms-custom-css-container',
                enableCustomJs: '#bcms-enable-custom-js',
                customJsContainer: '#bcms-custom-js-container',
                enableCustomHtml: '#bcms-enable-custom-html',
                customHtmlContainer: '#bcms-custom-html-container',

                messagesContainer: "#bcms-edit-widget-messages",

                widgetPreviewImageUrl: '#PreviewImageUrl',
                widgetPreviewImage: '#bcms-widget-preview-image',
                
                htmlContentWidgetContentHtmlEditor: 'bcms-advanced-contenthtml',

                htmlContentWidgetRowTemplate: '#bcms-advanced-content-list-row-template',
                htmlContentWidgetRowTemplateFirstRow: 'tr:first',
                htmlContentWidgetTableFirstRow: 'table.bcms-tables > tbody > tr:first',

                widgetsSearchButton: '#bcms-widget-search-btn',
                widgetCreateButton: '#bcms-create-widget-button',
                widgetRegisterButton: '#bcms-register-widget-button',
                widgetRowEditButtons: '.bcms-grid-item-edit-button',
                widgetsRowDeleteButtons: '.bcms-grid-item-delete-button',
                widgetParentRow: 'tr:first',
                widgetNameCell: '.bcms-widget-name',
                widgetCategoryNameCell: '.bcms-category-name',
                widgetRowDeleteButtons: '.bcms-grid-item-delete-button',
                widgetRowTemplate: '#bcms-widget-list-row-template',
                widgetRowCells: 'td',
                widgetRowTemplateFirstRow: 'tr:first',
                widgetsTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                widgetInsertButtons: '.bcms-widget-insert-button',

                siteSettingsWidgetsListForm: '#bcms-widgets-form',

                addOptionLink: '#bcms-add-option-button',
                optionsTable: '#bcms-options-grid'
            },
            classes = {
                regionAdvancedContent: 'bcms-content-advanced',
                regionWidget: 'bcms-content-widget',
            };

        /**
        * Assign objects to module.
        */
        widgets.links = links;
        widgets.globalization = globalization;


        /**
        * Opens dialog with a create html content widget form.
        */
        widgets.openCreateHtmlContentWidgetDialog = function(postSuccess) {
            modal.open({
                title: globalization.createHtmlContentWidgetDialogTitle,
                onLoad: function(childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadCreateHtmlContentWidgetDialogUrl, {
                        contentAvailable: initializeEditHtmlContentWidgetForm,

                        beforePost: function() {
                            htmlEditor.updateEditorContent(selectors.htmlContentWidgetContentHtmlEditor);
                        },

                        postSuccess: postSuccess
                    });
                }
            });
        };

        /**
        * Opens dialog with an edit html content widget form.
        */
        widgets.openEditHtmlContentWidgetDialog = function(id, postSuccess) {
            modal.open({
                title: globalization.editAdvancedContentDialogTitle,
                onLoad: function(childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditHtmlContentWidgetDialogUrl, id), {
                        contentAvailable: initializeEditHtmlContentWidgetForm,

                        beforePost: function() {
                            htmlEditor.updateEditorContent(selectors.htmlContentWidgetContentHtmlEditor);
                        },

                        postSuccess: postSuccess
                    });
                }
            });
        };

        /**
       * Opens ServerControlWidget edit dialog.
       */
        widgets.openEditServerControlWidgetDialog = function(widgetId, onSaveCallback) {
            modal.open({
                title: globalization.editWidgetDialogTitle,
                onLoad: function(childDialog) {
                    dynamicContent.bindDialog(childDialog, $.format(links.loadEditServerControlWidgetDialogUrl, widgetId), {
                        contentAvailable: initializeEditServerControlWidgetForm,

                        beforePost: function(form) {
                            editor.resetAutoGenerateNameId();
                            editor.setInputNames(form);
                        },

                        postSuccess: onSaveCallback
                    });
                }
            });
        };
        
        /**
        * Opens widget create form from site settings widgets list
        */
        widgets.openCreateServerControlWidgetDialog = function (onSaveCallback) {
            modal.open({
                title: globalization.createWidgetDialogTitle,
                onLoad: function (childDialog) {
                    dynamicContent.bindDialog(childDialog, links.loadCreateServerControlWidgetDialogUrl, {
                        contentAvailable: initializeEditServerControlWidgetForm,

                        postSuccess: onSaveCallback
                    });
                }
            });
        };
        
        /**
       * Initializes 'Edit Html Content Widget' dialog form.
       */
        function initializeEditHtmlContentWidgetForm(dialog) {
            dialog.container.find(selectors.enableCustomCss).on('click', function () {
                showHideCustomCssText(dialog);
            });

            dialog.container.find(selectors.enableCustomJs).on('click', function () {
                showHideCustomJsText(dialog);
            });

            dialog.container.find(selectors.enableCustomHtml).on('click', function () {
                showHideCustomHtmlText(dialog);
            }); 
            
            htmlEditor.initializeHtmlEditor(selectors.htmlContentWidgetContentHtmlEditor);
            htmlEditor.setSourceMode(selectors.htmlContentWidgetContentHtmlEditor);               
            
            showHideCustomCssText(dialog);
            showHideCustomJsText(dialog);
            showHideCustomHtmlText(dialog);
        };
       
        /**
        * Initializes widget form
        */
        function initializeEditServerControlWidgetForm(dialog) {
            editor.initialize(dialog.container, {
                deleteRowMessageExtractor: function () {
                    return globalization.deleteOptionConfirmMessage;
                }
            });

            dialog.container.find(selectors.addOptionLink).on('click', function () {
                editor.addNewRow(dialog.container, $(selectors.optionsTable));
            });

            dialog.container.find(selectors.widgetPreviewImage).error(function() {
                var image = dialog.container.find(selectors.widgetPreviewImage);
                if (image.attr("src") != null && image.attr("src") != "") {
                    messages.box({ container: dialog.container.find(selectors.messagesContainer) }).addWarningMessage(globalization.previewImageNotFoundMessage);
                    image.hide();
                    image.removeAttr("src");
                }
            });
            
            dialog.container.find(selectors.widgetPreviewImageUrl).blur(function() {
                var image = dialog.container.find(selectors.widgetPreviewImage),
                    urlInput = dialog.container.find(selectors.widgetPreviewImageUrl);

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
        * Open a widget edit dialog by the specified widget type.
        */
        widgets.editWidget = function (widgetId, widgetType, onSaveCallback) {
            if (widgetType === 'ServerControl') {
                widgets.openEditServerControlWidgetDialog(widgetId, onSaveCallback);
            }
            else if (widgetType === 'HtmlContent') {
                widgets.openEditHtmlContentWidgetDialog(widgetId, onSaveCallback);
            } else {
                throw new Error($.format('A widget type "{0}" is unknown and edit action is imposible.', widgetType));
            }                                
        };

        /**
        * Deletes widget.
        */
        widgets.deleteWidget = function(widgetId, widgetVersion, widgetName, onDeleteCallback) {
            var url = $.format(links.deleteWidgetUrl, widgetId, widgetVersion),
                message = $.format(globalization.deleteWidgetConfirmMessage, widgetName),
                onDeleteCompleted = function(json) {
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
        };
        
        /**
        * Opens dialog for editing widget options 
        */
        widgets.configureWidget = function (pageContentId, onSaveCallback) {
            modal.open({
                title: globalization.editPageWidgetOptionsTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.loadPageContentOptionsDialogUrl, pageContentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog) {
                            editor.initialize(contentDialog.container, {});
                        },

                        postSuccess: function () {
                            if ($.isFunction(onSaveCallback)) {
                                onSaveCallback();
                            }
                        }
                    });
                }
            });
        };
        
        /**
        * Opens site settings widgets list dialog
        */
        widgets.loadSiteSettingsWidgetList = function() {
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
                onWidgetCreated = function(json) {
                    if (json.Success && json.Data != null) {
                        var template = $(selectors.widgetRowTemplate),
                            newRow = $(template.html()).find(selectors.widgetRowTemplateFirstRow);
                        setWidgetFields(newRow, json);
                        newRow.insertBefore($(selectors.widgetsTableFirstRow, container));
                        initializeSiteSettingsWidgetListEvents(newRow);
                        grid.showHideEmptyRow(container);
                    }
                };

            var form = dialog.container.find(selectors.siteSettingsWidgetsListForm);
            grid.bindGridForm(form, function(data) {
                siteSettings.setContent(data);
                widgets.initializeSiteSettingsWidgetsList(data);
            });

            form.on('submit', function(event) {
                event.preventDefault();
                searchSiteSettingsWidgets(form);
                return false;
            });

            form.find(selectors.widgetsSearchButton).on('click', function() {
                searchSiteSettingsWidgets(form);
            });

            container.find(selectors.widgetCreateButton).on('click', function () {
                widgets.openCreateHtmlContentWidgetDialog(onWidgetCreated);
            });

            container.find(selectors.widgetRegisterButton).on('click', function () {
                widgets.openCreateServerControlWidgetDialog(onWidgetCreated);
            });
            
            initializeSiteSettingsWidgetListEvents(container);
        };

        /**
        * Search site settings widgets.
        */
        function searchSiteSettingsWidgets(form) {
            grid.submitGridForm(form, function(data) {
                siteSettings.setContent(data);
                initializeSiteSettingsWidgetsList();
            });
        };

        /**
        * Initializes site settings widgets list items.
        */
        function initializeSiteSettingsWidgetListEvents(container) {
            container.find(selectors.widgetRowCells).on('click', function () {
                var editButton = $(this).parents(selectors.widgetParentRow).find(selectors.widgetRowEditButtons);
                editSiteSettingsWidget(container, editButton);
            });

            container.find(selectors.widgetsRowDeleteButtons).on('click', function (event) {
                bcms.stopEventPropagation(event);
                deleteSiteSettingsWidget(container, $(this));
            });
        };

        /**
        * Calls function, which opens dialog for a widget editing.
        */
        function editSiteSettingsWidget(container, self) {
            var row = self.parents(selectors.widgetParentRow),
                id = row.data('id'),
                widgetType = row.data('type');

            widgets.editWidget(id, widgetType, function(data) {
                if (data.Data != null) {
                    setWidgetFields(row, data);
                    grid.showHideEmptyRow(container);
                }
            });           
        };                     

        /**
        * Deletes widget from site settings widgets list.
        */
        function deleteSiteSettingsWidget(container, self) {
            var row = self.parents(selectors.widgetParentRow),
                id = row.data('id'),
                version = row.data('version'),
                name = row.find(selectors.widgetNameCell).html();

            widgets.deleteWidget(id, version, name, function (data) {
                messages.refreshBox(container, data);
                if (data.Success) {
                    row.remove();
                    grid.showHideEmptyRow(container);
                }
            });
        };
        
        /**
        * Set values, returned from server to row fields
        */
        function setWidgetFields(row, json) {
            row.data('id', json.Data.Id);
            row.data('version', json.Data.Version);
            row.data('type', json.Data.WidgetType);
            row.find(selectors.widgetNameCell).html(json.Data.WidgetName);
            row.find(selectors.widgetCategoryNameCell).html(json.Data.CategoryName);
        };
        
        /**
        * Shows/hides custom css field in a html content widget edit form
        */
        function showHideCustomCssText(dialog) {
            if (dialog.container.find(selectors.enableCustomCss).attr('checked')) {
                dialog.container.find(selectors.customCssContainer).show();
            } else {
                dialog.container.find(selectors.customCssContainer).hide();
            }
        };

        function showHideCustomJsText(dialog) {
            if (dialog.container.find(selectors.enableCustomJs).attr('checked')) {
                dialog.container.find(selectors.customJsContainer).show();
            } else {
                dialog.container.find(selectors.customJsContainer).hide();
            }
        };

        function showHideCustomHtmlText(dialog) {
            if (dialog.container.find(selectors.enableCustomHtml).attr('checked')) {
                dialog.container.find(selectors.customHtmlContainer).show();                
            } else {
                dialog.container.find(selectors.customHtmlContainer).hide();
            }
        };
        
        /**
        * Called when editing page content
        */
        function onEditContent(sender) {
            var element = $(sender),
                contentId = element.data('contentId'),
                onSuccess = function () {
                    redirect.ReloadWithAlert();
                };

            if (element.hasClass(classes.regionWidget)) {
                widgets.openEditServerControlWidgetDialog(contentId, onSuccess);
            } else if (element.hasClass(classes.regionAdvancedContent)) {
                widgets.openEditHtmlContentWidgetDialog(contentId, onSuccess);
            }
        }

        /**
        * Initializes widgets module.
        */
        widgets.init = function () {
            console.log('Initializing bcms.pages.widgets module.');

            /**
            * Subscribe to events
            */
            bcms.on(bcms.events.editContent, onEditContent);
        };

        /**
        * Register initialization
        */
        bcms.registerInit(widgets.init);

        return widgets;
    });

