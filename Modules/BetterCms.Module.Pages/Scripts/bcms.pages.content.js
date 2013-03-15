/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */

define('bcms.pages.content', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.content', 'bcms.pages.widgets', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'bcms.slides.jquery', 'bcms.redirect', 'bcms.pages.history'],
    function($, bcms, modal, content, widgets, datepicker, htmlEditor, dynamicContent, siteSettings, messages, preview, grid, editor, slides, redirect, history) {
        'use strict';

        var pagesContent = {},
            selectors = {
                sliderBoxes: '.bcms-slider-box',
                sliderContainer: 'bcms-slides-container',

                contentId: '#bcmsContentId',
                pageContentId: '#bcmsPageContentId',
                contentFormRegionId: '#bcmsContentToRegionId',
                desirableStatus: '#bcmsContentDesirableStatus',
                dataPickers: '.bcms-datepicker',
                htmlEditor: 'bcms-contenthtml',
                destroyDraftVersionLink: '.bcms-messages-draft-destroy',

                widgetsSearchButton: '#bcms-advanced-content-search-btn',
                widgetsSearchInput: '#bcms-advanced-content-search',
                widgetsContainer: '#bcms-advanced-contents-container',
                widgetCreateButton: '#bcms-create-advanced-content-button',
                widgetRegisterButton: '#bcms-registeradvanced-content-button',
                widgetInsertButtons: '.bcms-content-insert-button',
                widgetDeleteButtons: '.bcms-content-delete-button',
                widgetEditButtons: '.bcms-content-edit-button',
                widgetContainerBlock: '.bcms-preview-block',
                widgetCategory: '.bcms-category',
                widgetName: '.bcms-title-holder > .bcms-content-titles',
                widgetIFramePreview: '.bcms-preview-box:has(iframe) .bcms-zoom-overlay',
                widgetImagePreview: '.bcms-preview-box:not(:has(iframe)) .bcms-zoom-overlay',

                widgetsContent: '.bcms-widgets',

                enableCustomJs: '#bcms-enable-custom-js',
                enableCustomCss: '#bcms-enable-custom-css',
                customJsContainer: '#bcms-custom-js-container',
                customCssContainer: '#bcms-custom-css-container'
            },
            classes = {
                sliderPrev: 'bcms-slider-prev',
                sliderNext: 'bcms-slider-next'
            },
            links = {
                loadWidgetsUrl: null,
                loadAddNewHtmlContentDialogUrl: null,
                insertContentToPageUrl: null,
                deletePageContentUrl: null,
                editPageContentUrl: null,
                sortPageContentUrl: null,
                previewPageUrl: null
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
                sortingPageContentMessage: null,
                errorTitle: null,
                insertingWidgetInfoMessage: null,
                insertingWidgetInfoHeader: null,
                insertingWidgetErrorMessage: null,
                datePickerTooltipTitle: null
            },
            contentTypes = {
                htmlContent: 'html-content'
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
            
            modal.edit({
                title: globalization.addNewContentDialogTitle,
              
                onLoad: function(dialog) {
                    var url = $.format(links.loadAddNewHtmlContentDialogUrl, bcms.pageId, regionId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: pagesContent.initializeAddNewContentForm,

                        beforePost: function() {
                                htmlEditor.updateEditorContent(selectors.htmlEditor);   
                        },

                        postSuccess: function (json) {                            
                            if (json.Data.DesirableStatus == bcms.contentStatus.preview) {
                                try {
                                    var result = json.Data;
                                    $(selectors.contentId).val(result.ContentId);
                                    $(selectors.pageContentId).val(result.PageContentId);
                                    preview.previewPageContent(result.PageId, result.PageContentId);
                                } finally  {
                                    return false;
                                }                                 
                            } else {
                                redirect.ReloadWithAlert();
                            }                            
                        }
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                },
                onClose: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                }
            });
        };

        /**
        * Save content order after sorting.
        */
        pagesContent.onSortPageContent = function (model) {
            if (model.data.pageContents.length < 2) {
                return; // Sorting is needed for more than one item.
            }

            var info = modal.info({
                content: globalization.sortingPageContentMessage,
                disableCancel: true,
                disableAccept: true,
            });

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
                success: function (json) {
                    info.close();
                    if (json.Success) {
                        redirect.ReloadWithAlert();
                    } else {
                        if (json.Messages && json.Messages.length > 0) {
                            modal.showMessages(json);
                        } else {
                            alertOnError();
                        }
                    }
                },
                error: function () {
                    info.close();
                    alertOnError();
                }
            });
        };
        
        /**
        * Initializes content dialog form.
        */
        pagesContent.initializeAddNewContentForm = function (dialog) {
            dialog.container.find(selectors.dataPickers).initializeDatepicker(globalization.datePickerTooltipTitle);

            dialog.container.find(selectors.widgetsSearchButton).on('click', function () {
                pagesContent.updateWidgetCategoryList(dialog);
                
            });

            dialog.container.find(selectors.widgetCreateButton).on('click', function () {
                widgets.openCreateHtmlContentWidgetDialog(function (json) {
                    htmlEditor.updateEditorContent(selectors.htmlEditor);
                    // Reload search results after category was created.
                    pagesContent.updateWidgetCategoryList(dialog);
                    messages.refreshBox(dialog.container, json);
                }, null);
            });

            dialog.container.find(selectors.widgetRegisterButton).on('click', function () {
                widgets.openCreateServerControlWidgetDialog(function (json) {
                    pagesContent.updateWidgetCategoryList(dialog);
                    messages.refreshBox(dialog.container, json);
                }, null);
            });

            bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.widgetsSearchInput), {
                preventedEnter: function () {                    
                    pagesContent.updateWidgetCategoryList(dialog);
                }
            });

            pagesContent.initializeWidgets(dialog.container, dialog);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);

            pagesContent.initializeCustomTextArea(dialog);
        };
        
        /**
        * Initializes content edit dialog form.
        */
        pagesContent.initializeEditContentForm = function (dialog) {
            dialog.container.find(selectors.dataPickers).initializeDatepicker();
            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);
            pagesContent.initializeCustomTextArea(dialog);
            
            dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
                var contentId = dialog.container.find(selectors.contentId).val(),
                    pageContentId = dialog.container.find(selectors.pageContentId).val();
                
                history.destroyDraftVersion(contentId, dialog.container, function () {
                    dialog.close();
                    pagesContent.editPageContent(pageContentId, function () {
                        redirect.ReloadWithAlert();
                    });
                });
            });
        };

         /**
        * Initializes custom css and js text fields.
        */
        pagesContent.initializeCustomTextArea = function(dialog) {
            dialog.container.find(selectors.enableCustomCss).on('change', function() {
                showHideCustomCssText(dialog);
            });

            dialog.container.find(selectors.enableCustomJs).on('change', function() {
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
                dialog.container.find(selectors.widgetsSearchInput).focus();
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

                widgets.deleteWidget(widgetId, widgetVersion, widgetName,
                    function(data) {
                        messages.refreshBox(widgetContainer, data);
                        pagesContent.updateWidgetCategoryList(dialog);
                    },
                    function(data) {
                        messages.refreshBox(widgetContainer, data);
                    });
            });

            container.find(selectors.widgetEditButtons).on('click', function () {
                var self = $(this),
                    widgetContainer = self.parents(selectors.widgetContainerBlock),
                    widgetId = widgetContainer.data('id'),
                    widgetType = widgetContainer.data('type');               
                
                widgets.editWidget(widgetId, widgetType, function(data) {
                    messages.refreshBox(widgetContainer, data);
                    pagesContent.updateWidgetCategoryList(dialog);
                },
                null);
            });

            preview.initialize(container.find(selectors.widgetsContainer), selectors.widgetIFramePreview);
            
            // Add preview for widget with images (unbind click for iframe preview)
            dialog.container.find(selectors.widgetImagePreview).unbind('click');
            dialog.container.find(selectors.widgetImagePreview).on('click', function () {
                var self = $(this),
                    url = self.data('previewUrl'),
                    alt = self.data('previewTitle');

                modal.imagePreview(url, alt);
            });
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
                        redirect.ReloadWithAlert();
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
        * Called when content overlay is created
        */
        function onCreateContentOverlay(contentViewModel) {
            var contentId = contentViewModel.contentId,
                pageContentId = contentViewModel.pageContentId,
                contentVersion = contentViewModel.contentVersion,
                pageContentVersion = contentViewModel.pageContentVersion;

            if (contentViewModel.contentType == contentTypes.htmlContent) {
                contentViewModel.removeConfigureButton();

                // Edit content
                contentViewModel.onEditContent = function() {
                    pagesContent.editPageContent(pageContentId);
                };
            }
            
            // Delete content
            contentViewModel.onDeleteContent = function () {
                pagesContent.removeContentFromPage(pageContentId, pageContentVersion, contentVersion);
            };
            
            // Content history
            contentViewModel.onContentHistory = function() {
                history.openPageContentHistoryDialog(contentId, pageContentId);
            };

            // Change draft icon
            if (contentViewModel.draft) {
                contentViewModel.addDraftIcon();
            }
        };

        /**
        * Opens dialog for editing page regular content  
        */
        pagesContent.editPageContent = function (contentId, onCloseClick) {
            modal.edit({
                title: globalization.editContentDialogTitle,
                onCloseClick: onCloseClick,
                onLoad: function (dialog) {
                    var url = $.format(links.editPageContentUrl, contentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: pagesContent.initializeEditContentForm,

                        beforePost: function () {
                            htmlEditor.updateEditorContent();
                        },

                        postSuccess: function (json) {
                            if (json.Data.DesirableStatus == bcms.contentStatus.preview) {
                                try {                                    
                                    preview.previewPageContent(bcms.pageId, json.Data.PageContentId);
                                } finally {
                                    return false;
                                }
                            } else {
                                redirect.ReloadWithAlert();
                            }
                        }
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                },
                onClose: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
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
                            redirect.ReloadWithAlert({
                                title: globalization.deleteContentSuccessMessageTitle,
                                message: globalization.deleteContentSuccessMessageMessage,
                                timeout: 1500
                            });
                        }
                        else {
                            if (json.Messages && json.Messages.length > 0) {
                                modal.showMessages(json);
                            } else {
                                modal.alert({
                                    title: globalization.deleteContentFailureMessageTitle,
                                    content: globalization.deleteContentFailureMessageMessage,
                                });
                            }
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
        };

        /**
        * Subscribe to events
        */
        bcms.on(bcms.events.addPageContent, pagesContent.onAddNewContent);
        bcms.on(bcms.events.sortPageContent, pagesContent.onSortPageContent);
        bcms.on(bcms.events.createContentOverlay, onCreateContentOverlay);

        /**
        * Register initialization
        */
        bcms.registerInit(pagesContent.init);

        return pagesContent;
    });
