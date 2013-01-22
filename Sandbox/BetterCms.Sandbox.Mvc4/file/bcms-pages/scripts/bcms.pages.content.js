/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.content', ['jquery', 'bcms', 'bcms.modal', 'bcms.content', 'bcms.pages.widgets', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.pages.history', 'bcms.grid', 'bcms.inlineEdit', 'slides.jquery'],
    function($, bcms, modal, content, widgets, datepicker, htmlEditor, dynamicContent, siteSettings, messages, preview, history, grid, editor) {
        'use strict';

        var pagesContent = {},
            selectors = {
                sliderBoxes: '.bcms-slider-box',
                sliderContainer: 'bcms-slides-container',

                contentId: '#bcmsContentId',
                pageContentId: '#bcmsPageContentId',
                contentFormRegionId: '#bcmsAddContentToRegionId',
                desirableStatus: '#bcmsAddContentDesirableStatus',
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
                
                enableCustomJs: '#bcms-enable-custom-js',
                enableCustomCss: '#bcms-enable-custom-css',
                customJsContainer: '#bcms-custom-js-container',
                customCssContainer: '#bcms-custom-css-container'
            },
            
            classes = {
                sliderPrev: 'bcms-slider-prev',
                sliderNext: 'bcms-slider-next',
                regionContent: 'bcms-content-regular',
                regionAdvancedContent: 'bcms-content-advanced',
                regionWidget: 'bcms-content-widget',
                grayButton: 'bcms-btn-small bcms-btn-gray'
            },
            
            links = {
                loadWidgetsUrl: null,
                loadAddNewHtmlContentDialogUrl: null,
                insertContentToPageUrl: null,
                deletePageContentUrl: null,
                editPageContentUrl: null,
                sortPageContentUrl: null               
            },

            globalization = {
                saveDraft: null,
                saveAndPublish: null,
                preview: null,
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
                errorTitle: null,                
                insertingWidgetInfoMessage: null,
                insertingWidgetInfoHeader: null,
                insertingWidgetErrorMessage: null                
            },
            
            contentStatus = {
                published: 'Published',
                draft: 'Draft',
                preview: 'Preview'
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
            var extraButtons = [],
                changeContentDesirableStatus = function(dialog, status) {
                    dialog.container.find(selectors.desirableStatus).val(status);                    
                },
                saveAndPublishButton = new modal.button(globalization.saveAndPublish, classes.grayButton, 2, function(dialog) {
                    changeContentDesirableStatus(dialog, contentStatus.published);
                    dialog.submitForm();
                }),
                saveDraftButton = new modal.button(globalization.preview, classes.grayButton, 3, function(dialog) {
                    changeContentDesirableStatus(dialog, contentStatus.preview);
                    dialog.submitForm();
                });
            
            extraButtons.push(saveDraftButton);            
            extraButtons.push(saveAndPublishButton);
            
            modal.open({
                buttons: extraButtons,
                acceptTitle: globalization.saveDraft,
                title: globalization.addNewContentDialogTitle,
                onAcceptClick: function (dialog) {
                    changeContentDesirableStatus(dialog, contentStatus.draft);
                },
                onLoad: function(dialog) {
                    var url = $.format(links.loadAddNewHtmlContentDialogUrl, bcms.pageId, regionId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: pagesContent.initializeAddNewContentForm,

                        beforePost: function() {
                            htmlEditor.updateEditorContent();
                        },

                        postSuccess: function (json) {                            
                            if (json.Data.DesirableStatus === contentStatus.preview) {
                                try {                                    
                                    $(selectors.contentId).val(json.Data.ContentId);
                                    $(selectors.pageContentId).val(json.Data.PageContentId);
                                    preview.previewPageContent(json.Data.PageContentId);
                                } finally  {
                                    return false;
                                }                                 
                            } else {
                                bcms.reload();
                            }                            
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

            pagesContent.initializeCustomTextArea(dialog);
        };
        
        /**
        * Initializes content edit dialog form.
        */
        pagesContent.initializeEditContentForm = function (dialog) {
            dialog.container.find(selectors.dataPickers).initializeDatepicker();
            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);
            pagesContent.initializeCustomTextArea(dialog);
        };

         /**
        * Initializes custom css and js text fields.
        */
        pagesContent.initializeCustomTextArea = function(dialog) {
            dialog.container.find(selectors.enableCustomCss).on('click', function() {
                showHideCustomCssText(dialog);
            });

            dialog.container.find(selectors.enableCustomJs).on('click', function() {
                showHideCustomJsText(dialog);
            });
            showHideCustomCssText(dialog);
            showHideCustomJsText(dialog);
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
                overlay.find(selectors.overlayConfigure).hide();
            } else if (element.hasClass(classes.regionWidget)) {
            } else {
                overlay.find(selectors.overlayEdit).hide();
                overlay.find(selectors.overlayConfigure).hide();
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
                contentId = element.data('contentId'),
                contentVersion = element.data('contentVersion'),
                pageContentId = element.data('pageContentId'),
                pageContentVersion = element.data('pageContentVersion');

            history.openPageContentHistoryDialog(contentId, contentVersion, pageContentId, pageContentVersion);
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
        pagesContent.editPageContent = function (contentId) {
            var extraButtons = [],
              changeContentDesirableStatus = function (dialog, status) {
                  dialog.container.find(selectors.desirableStatus).val(status);
              },
              saveAndPublishButton = new modal.button(globalization.saveAndPublish, classes.grayButton, 2, function (dialog) {
                  changeContentDesirableStatus(dialog, contentStatus.published);
                  dialog.submitForm();
              }),
              saveDraftButton = new modal.button(globalization.preview, classes.grayButton, 3, function (dialog) {
                  changeContentDesirableStatus(dialog, contentStatus.preview);
                  dialog.submitForm();
              });

            extraButtons.push(saveDraftButton);
            extraButtons.push(saveAndPublishButton);
            
            modal.open({
                buttons: extraButtons,
                acceptTitle: globalization.saveDraft,
                onAcceptClick: function (dialog) {
                    changeContentDesirableStatus(dialog, contentStatus.draft);
                },
                title: globalization.editContentDialogTitle,
                onLoad: function (dialog) {
                    var url = $.format(links.editPageContentUrl, contentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: pagesContent.initializeEditContentForm,

                        beforePost: function () {
                            htmlEditor.updateEditorContent();
                        },

                        postSuccess: function (json) {
                            if (json.Data.DesirableStatus === contentStatus.preview) {
                                try {                                    
                                    preview.previewPageContent(json.Data.PageContentId);
                                } finally {
                                    return false;
                                }
                            } else {
                                bcms.reload();
                            }
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
        * Shows/hides custom css field in a content edit form
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
