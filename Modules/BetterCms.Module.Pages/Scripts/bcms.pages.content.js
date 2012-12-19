/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.content', ['jquery', 'bcms', 'bcms.modal', 'bcms.content', 'bcms.pages.widgets', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'slides.jquery'],
    function($, bcms, modal, content, widgets, datepicker, htmlEditor, dynamicContent, siteSettings, messages, preview, grid, editor) {
        'use strict';

        var pagesContent = {},
            links = {
                loadWidgetsUrl: null,                
                loadAddNewHtmlContentDialogUrl: null,                
                insertContentToPageUrl: null,
                deletePageContentUrl: null,
                editPageContentUrl: null,
                sortPageContentUrl: null,
            },
            globalization = {
                addNewContentDialogTitle: null,
                editContentDialogTitle: null,
                deleteContentConfirmationTitle: null,
                deleteContentConfirmationMessage: null,
                deleteContentSuccessMessageTitle: null,
                deleteContentSuccessMessageMessage: null,
                deleteContentFailureMessageTitle: null,
                deleteContentFailureMessageMessage: null,
                sortPageContentFailureMessageTitle: null,
                sortPageContentFailureMessageMessage: null,
            },
            selectors = {
                sliderBoxes: '.bcms-slider-box',
                sliderContainer: 'bcms-slides-container',
                
                contentFormRegionId: '#AddContentToRegionId',
                
                dataPickers: '.bcms-datepicker',
                htmlEditor: 'bcms-contenthtml',
                
                widgetsSearchButton: '#bcms-advanced-content-search-btn',
                widgetsSearchInput: '#bcms-advanced-content-search',
                widgetsContainer: '#bcms-advanced-contents-container',
                widgetCreateButton: '#bcms-create-advanced-content-button',
                widgetInsertButtons: '.bcms-content-insert-button',
                widgetDeleteButtons: '.bcms-content-delete-button',
                widgetEditButtons: '.bcms-content-edit-button',
                widgetContainerBlock: '.bcms-preview-block',
                widgetCategory: '.bcms-category',
                widgetName: '.bcms-title-holder > .bcms-content-titles',
                
                widgetsContent: '.bcms-widgets',
                
                overlayEdit: '.bcms-content-edit',
                overlayConfigure: '.bcms-content-configure',
                overlay: '.bcms-content-overlay',
                
                insertingWidgetInfoMessage: null,
                insertingWidgetInfoHeader: null,
                insertingWidgetErrorMessage: null,
                errorTitle: null,
            },
            classes = {
                sliderPrev: 'bcms-slider-prev',
                sliderNext: 'bcms-slider-next',
                regionContent: 'bcms-content-regular',
                regionAdvancedContent: 'bcms-content-advanced',
                regionWidget: 'bcms-content-widget'
            };

        /**
        * Assign objects to module.
        */
        pagesContent.links = links;
        pagesContent.globalization = globalization;
        
        /**
        * Open dialog with add new content form
        */
        pagesContent.onAddNewContent = function(regionId) {
            modal.open({
                title: globalization.addNewContentDialogTitle,
                onLoad: function(dialog) {
                    var url = $.format(links.loadAddNewHtmlContentDialogUrl, bcms.pageId, regionId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: pagesContent.initializeAddNewContentForm,

                        beforePost: function() {
                            htmlEditor.updateEditorContent();
                        },

                        postSuccess: function() {
                            bcms.reload();
                        }
                    });
                }
            });
        };

        /**
        * Save content order after sorting.
        */
        pagesContent.onSortPageContent = function (model) {
            var url = links.sortPageContentUrl,
                alertOnError = function() {
                    modal.alert({
                        title: globalization.sortPageContentFailureMessageTitle,
                        content: globalization.sortPageContentFailureMessageMessage,
                    });
                },
                dataToSend = JSON.stringify(model.data);
            
            $.ajax({
                type: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                cache: false,
                data: dataToSend,
                success: function(json) {
                    if (json.Success && json.Data != null) {
                        content.updateRegionContentVersions(model.region, json.Data.UpdatedPageContents);
                    } else {
                        alertOnError();
                    }
                },
                error: function () {
                    alertOnError();
                }
            });
        };
        
        /**
        * Initializes content dialog form.
        */
        pagesContent.initializeAddNewContentForm = function (dialog) {
            dialog.container.find(selectors.dataPickers).initializeDatepicker();

            dialog.container.find(selectors.widgetsSearchButton).on('click', function () {
                pagesContent.updateWidgetCategoryList(dialog);
            });

            dialog.container.find(selectors.widgetCreateButton).on('click', function () {
                widgets.openCreateHtmlContentWidgetDialog(function () {
                    // Reload search results after category was created.
                    pagesContent.updateWidgetCategoryList(dialog);
                });
            });

            bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.widgetsSearchInput), {
                preventedEnter: function () {                    
                    pagesContent.updateWidgetCategoryList(dialog);
                }
            });

            pagesContent.initializeWidgets(dialog.container, dialog);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);

            preview.initialize(dialog.container.find(selectors.widgetsContent));
        };
        
        /**
        * Initializes content edit dialog form.
        */
        pagesContent.initializeEditContentForm = function (dialog) {
            dialog.container.find(selectors.dataPickers).initializeDatepicker();
            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);
        };

        /**
        * Reloads widget category list.
        */
        pagesContent.updateWidgetCategoryList = function (dialog) {            
            $.ajax({
                url: $.format(links.loadWidgetsUrl, dialog.container.find(selectors.widgetsSearchInput).val())
            }).done(function (data) {            
                dialog.container.find(selectors.widgetsContainer).html(data);

                pagesContent.initializeWidgets(dialog.container, dialog);
            });
        };
        
        /**
        * Initializes widget categories list with sliders.
        */
        pagesContent.initializeWidgets = function (container, dialog) {

            pagesContent.initializeSliders(container);

            container.find(selectors.widgetInsertButtons).on('click', function () {
                pagesContent.insertWidget(this, dialog);
            });

            container.find(selectors.widgetDeleteButtons).on('click', function () {
                var self = $(this),
                    widgetContainer = self.parents(selectors.widgetContainerBlock),
                    widgetId = widgetContainer.data('id'),
                    widgetVersion = widgetContainer.data('version'),
                    widgetName = widgetContainer.find(selectors.widgetName).text();
                                        
                widgets.deleteWidget(widgetId, widgetVersion, widgetName, function (data) {
                    messages.refreshBox(dialog.container, data);
                    pagesContent.updateWidgetCategoryList(dialog);
                });
            });

            container.find(selectors.widgetEditButtons).on('click', function () {
                var self = $(this),
                    widgetContainer = self.parents(selectors.widgetContainerBlock),
                    widgetId = widgetContainer.data('id'),
                    widgetType = widgetContainer.data('type');
                
                widgets.editWidget(widgetId, widgetType, function(data) {
                    messages.refreshBox(dialog.container, data);
                    pagesContent.updateWidgetCategoryList(dialog);
                });
            });

            preview.initialize(container.find(selectors.widgetsContainer));
        };
              
        /**
        * Inserts widget to CMS page
        */
        pagesContent.insertWidget = function (self, dialog) {
            var regionId = dialog.container.find(selectors.contentFormRegionId).val(),
                widgetContainer = $(self).parents(selectors.widgetContainerBlock),
                contentId = widgetContainer.data('id'),
                url = $.format(links.insertContentToPageUrl, bcms.pageId, contentId, regionId);

            $.ajax({
                type: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                cache: false,
                beforeSend: function () {
                    dialog.close();

                    dialog = modal.info({
                        title: globalization.insertingWidgetInfoHeader,
                        content: globalization.insertingWidgetInfoMessage,
                        disableCancel: true,
                        disableAccept: true,
                    });
                },

                success: function (data) {
                    dialog.close();
                    if (data && data.Success) {
                        bcms.reload();
                    } else {
                        modal.alert({
                            title: globalization.errorTitle,
                            content: data.Messages ? data.Messages[0] : globalization.insertingWidgetErrorMessage
                        });
                    }
                },

                error: function () {
                    dialog.close();
                    modal.alert({
                        title: globalization.errorTitle,
                        content: globalization.insertingWidgetErrorMessage
                    });
                }
            });
        };

        /**
        * Initializes a content sliders.
        */
        pagesContent.initializeSliders = function (container) {
            container.find(selectors.sliderBoxes).each(function () {
                $(this).slides({
                    container: selectors.sliderContainer,
                    generateNextPrev: true,
                    generatePagination: false,
                    prev: classes.sliderPrev,
                    next: classes.sliderNext
                });
            });
        };
               
        /**
        * Called when edit overlay is shown
        */
        pagesContent.onShowOverlay = function(sender) {
            var element = $(sender),
                overlay = $(selectors.overlay);

            if (element.hasClass(classes.regionContent)) {
                overlay.find(selectors.overlayConfigure).hide();
            } else if (element.hasClass(classes.regionAdvancedContent)) {
                overlay.find(selectors.overlayEdit).hide();
                overlay.find(selectors.overlayConfigure).hide();
            } else if (element.hasClass(classes.regionWidget)) {
                overlay.find(selectors.overlayEdit).hide();
            }            
        };

        /**
        * Called when edit overlay is hidden
        */
        pagesContent.onHideOverlay = function() {
            var overlay = $(selectors.overlay);

            overlay.find(selectors.overlayEdit).show();
            overlay.find(selectors.overlayConfigure).show();
        };

        /**
        * Called when editing page content
        */
        pagesContent.onEditContent = function(sender) {
            var element = $(sender),
                contentId = element.data('pageContentId');

            if (element.hasClass(classes.regionContent)) {
                pagesContent.editPageContent(contentId);
            }
            ;
        };

        /**
        * Called when deleting page content
        */
        pagesContent.onDeleteContent = function(sender) {
            var element = $(sender),
                pageContentId = element.data('pageContentId'),
                pageContentVersion = element.data('pageContentVersion'),
                contentVersion = element.data('contentVersion');
            pagesContent.removeContentFromPage(pageContentId, pageContentVersion, contentVersion);
        };

        /**
        * Called when showing content history
        */
        pagesContent.onContentHistory = function(sender) {
            var element = $(sender),
                contentId = element.data('id');

            if (element.hasClass(classes.regionContent)) {
                pagesContent.showContentHistory(contentId);
            } else if (element.hasClass(classes.regionAdvancedContent)) {
                pagesContent.showHtmlContentWidgetHistory(contentId);
            } else if (element.hasClass(classes.regionWidget)) {
                pagesContent.showServerControlWidgetHistory(contentId);
            }
            ;
        };

        /**
        * Called when configuring page content
        */
        pagesContent.onConfigureContent = function(sender) {
            var element = $(sender),
                pageContentId = element.data('pageContentId');

            if (element.hasClass(classes.regionWidget)) {
                widgets.configureWidget(pageContentId, function () {
                    bcms.reload();
                });
            }            
        };

        /**
        * Opens dialog for editing page regular content  
        */
        pagesContent.editPageContent = function(contentId) {
            modal.open({
                title: globalization.editContentDialogTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.editPageContentUrl, contentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: pagesContent.initializeEditContentForm,

                        beforePost: function () {
                            htmlEditor.updateEditorContent();
                        },

                        postSuccess: function () {
                            bcms.reload();
                        },
                    });
                }
            });
        };

        /**
        * Removes regular content from page.
        */
        pagesContent.removeContentFromPage = function (pageContentId, pageContentVersion, contentVersion) {
            var url = $.format(links.deletePageContentUrl, pageContentId, pageContentVersion, contentVersion),
                onDeleteCompleted = function (json) {
                    try {
                        if (json.Success) {
                            modal.info({
                                title: globalization.deleteContentSuccessMessageTitle,
                                content: globalization.deleteContentSuccessMessageMessage,
                                disableCancel: true,
                                disableAccept: true,
                                onLoad: function () {
                                    setTimeout(bcms.reload, 1500);
                                }
                            });
                        }
                        else {
                            modal.alert({
                                title: globalization.deleteContentFailureMessageTitle,
                                content: globalization.deleteContentFailureMessageMessage,
                            });
                        }
                    } finally {
                        confirmDialog.close();
                    }
                },
                confirmDialog = modal.confirm({
                    title: globalization.deleteContentConfirmationTitle,
                    content: globalization.deleteContentConfirmationMessage,
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
        * Opens dialog with regular html content history.
        * TODO: need implementation
        */
        pagesContent.showContentHistory = function(contentId) {
            alert('Show regular content history ' + contentId);
        };

        /**
        * Opens dialog with html content widget history.
        * TODO: need implementation
        */
        pagesContent.showHtmlContentWidgetHistory = function(contentId) {
            alert('Show widget history ' + contentId);
        };

        /**
        * Opens dialog with server control widget history.
        * TODO: need implementation
        */
        pagesContent.showServerControlWidgetHistory = function(contentId) {
            alert('Show widget history ' + contentId);
        };

        /**
        * Initializes page module.
        */
        pagesContent.init = function() {
            console.log('Initializing bcms.pages.content module.');

            /**
            * Subscribe to events
            */
            bcms.on(bcms.events.addPageContent, pagesContent.onAddNewContent);
            bcms.on(bcms.events.sortPageContent, pagesContent.onSortPageContent);
            bcms.on(bcms.events.showOverlay, pagesContent.onShowOverlay);
            bcms.on(bcms.events.hideOverlay, pagesContent.onHideOverlay);
            bcms.on(bcms.events.editContent, pagesContent.onEditContent);
            bcms.on(bcms.events.deleteContent, pagesContent.onDeleteContent);
            bcms.on(bcms.events.contentHistory, pagesContent.onContentHistory);
            bcms.on(bcms.events.configureContent, pagesContent.onConfigureContent);
        };

        /**
        * Register initialization
        */
        bcms.registerInit(pagesContent.init);

        return pagesContent;
    });
