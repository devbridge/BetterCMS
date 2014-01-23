/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.pages.content', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.content', 'bcms.pages.widgets', 'bcms.datepicker', 'bcms.htmlEditor', 'bcms.dynamicContent', 'bcms.siteSettings', 'bcms.messages', 'bcms.preview', 'bcms.grid', 'bcms.inlineEdit', 'bcms.slides.jquery', 'bcms.redirect', 'bcms.pages.history', 'bcms.security', 'bcms.codeEditor', 'bcms.forms'],
    function ($, bcms, modal, content, widgets, datepicker, htmlEditor, dynamicContent, siteSettings, messages, preview, grid, editor, slides, redirect, history, security, codeEditor, forms) {
        'use strict';

        var pagesContent = {},
            selectors = {
                sliderBoxes: '.bcms-slider-box',
                sliderContainer: 'bcms-slides-container',

                contentId: '#bcmsContentId',
                contentVersion: '#bcmsPageContentVersion',
                pageContentId: '#bcmsPageContentId',
                contentFormRegionId: '#bcmsContentToRegionId',
                desirableStatus: '#bcmsContentDesirableStatus',
                dataPickers: '.bcms-datepicker',
                htmlEditor: 'bcms-contenthtml',
                destroyDraftVersionLink: '.bcms-messages-draft-destroy',
                pageContentUserConfirmationHiddenField: '#bcms-user-confirmed-region-deletion',

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
                widgetIFramePreview: ".bcms-preview-box[data-as-image='False'] .bcms-zoom-overlay",
                widgetImagePreview: ".bcms-preview-box[data-as-image='True'] .bcms-zoom-overlay",
                anyTab: '.bcms-tab',

                widgetsContent: '.bcms-widgets',

                enableCustomJs: '#bcms-enable-custom-js',
                enableCustomCss: '#bcms-enable-custom-css',
                customJsContainer: '#bcms-custom-js-container',
                customCssContainer: '#bcms-custom-css-container',
                aceEditorContainer: '.bcms-editor-field-area-container:first',
                
                editInSourceModeHiddenField: '#bcms-edit-in-source-mode',
                firstForm: 'form:first',
                datePickers: 'input.bcms-datepicker'
            },
            classes = {
                sliderPrev: 'bcms-slider-prev',
                sliderNext: 'bcms-slider-next',
                inactive: 'bcms-inactive'
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
                disableSaveAndPublish: !security.IsAuthorized(["BcmsPublishContent"]),
                onLoad: function (dialog) {
                    var url = $.format(links.loadAddNewHtmlContentDialogUrl, bcms.pageId, regionId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog, data) {
                            var editInSourceMode = false,
                                enableInsertDynamicRegion = false;
                            if (data && data.Data) {
                                if (data.Data.EditInSourceMode) {
                                    editInSourceMode = true;
                                }
                                if (data.Data.EnableInsertDynamicRegion) {
                                    enableInsertDynamicRegion = true;
                                }
                            }
                            pagesContent.initializeAddNewContentForm(contentDialog, editInSourceMode, enableInsertDynamicRegion);
                        },

                        beforePost: function() {
                            htmlEditor.updateEditorContent(selectors.htmlEditor);

                            var editInSourceMode = htmlEditor.isSourceMode(selectors.htmlEditor);
                            dialog.container.find(selectors.editInSourceModeHiddenField).val(editInSourceMode);
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
            console.log(model);

            var info = modal.info({
                content: globalization.sortingPageContentMessage,
                disableCancel: true,
                disableAccept: true
            });

            var url = links.sortPageContentUrl,
               alertOnError = function () {
                   modal.alert({
                       title: globalization.sortPageContentFailureMessageTitle,
                       content: globalization.sortPageContentFailureMessageMessage
                   });
               },
               dataToSend = JSON.stringify({
                   PageId: bcms.pageId,
                   PageContents: model
               });

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
        pagesContent.initializeAddNewContentForm = function (dialog, editInSourceMode, enableInsertDynamicRegion) {
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
            
            dialog.container.find(selectors.anyTab).click(function () {
                setTimeout(function() {
                    dialog.setFocus();
                }, 100);
            });

            pagesContent.initializeWidgets(dialog.container, dialog);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);
            if (editInSourceMode) {
                htmlEditor.setSourceMode(selectors.htmlEditor);
            }
            if (enableInsertDynamicRegion) {
                htmlEditor.enableInsertDynamicRegion(selectors.htmlEditor);
            }

            pagesContent.initializeCustomTextArea(dialog);

            codeEditor.initialize(dialog.container);
        };
        
        /**
        * Initializes content edit dialog form.
        */
        pagesContent.initializeEditContentForm = function (dialog, editInSourceMode, enableInsertDynamicRegion) {
            var canEdit = security.IsAuthorized(["BcmsEditContent"]),
                canPublish = security.IsAuthorized(["BcmsPublishContent"]),
                form = dialog.container.find(selectors.firstForm);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditor);

            if (editInSourceMode) {
                htmlEditor.setSourceMode(selectors.htmlEditor);
            }
            if (enableInsertDynamicRegion) {
                htmlEditor.enableInsertDynamicRegion(selectors.htmlEditor);
            }

            pagesContent.initializeCustomTextArea(dialog);
            
            codeEditor.initialize(dialog.container);

            dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
                var contentId = dialog.container.find(selectors.contentId).val(),
                    pageContentId = dialog.container.find(selectors.pageContentId).val(),
                    contentVersion = dialog.container.find(selectors.contentVersion).val();

                history.destroyDraftVersion(contentId, contentVersion, dialog.container, function () {
                    dialog.close();
                    pagesContent.editPageContent(pageContentId, function () {
                        redirect.ReloadWithAlert();
                    });
                });
            });
            
            // User with only BcmsPublishContent but without BcmsEditContent can only publish and change publish dates
            if (form.data('readonly') !== true && canPublish && !canEdit) {
                form.addClass(classes.inactive);
                forms.setFieldsReadOnly(form);

                // Enable date pickers for editing
                $.each(form.find(selectors.datePickers), function () {
                    var self = $(this);

                    self.removeAttr('readonly');
                    self.parent('div').css('z-index', bcms.getHighestZindex() + 1);
                });
            }

            dialog.container.find(selectors.dataPickers).initializeDatepicker();
        };

         /**
        * Initializes custom css and js text fields.
        */
        pagesContent.initializeCustomTextArea = function(dialog) {
            dialog.container.find(selectors.enableCustomCss).on('change', function() {
                showHideCustomCssText(dialog, true);
            });

            dialog.container.find(selectors.enableCustomJs).on('change', function() {
                showHideCustomJsText(dialog, true);
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
                    widgetId = widgetContainer.data('originalId'),
                    widgetVersion = widgetContainer.data('originalVersion'),
                    widgetName = widgetContainer.find(selectors.widgetName).text(),
                    onComplete = function (data) {
                        messages.refreshBox(widgetContainer, data);
                        widgetContainer.hideLoading();
                    };

                widgets.deleteWidget(widgetId, widgetVersion, widgetName,
                    function() {
                        widgetContainer.showLoading();
                    },
                    function(data) {
                        onComplete(data);
                        pagesContent.updateWidgetCategoryList(dialog);
                    },
                    onComplete);
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
                contentId = widgetContainer.data('originalId'),
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
                        disableAccept: true
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
            var updateSlide = function (slideBox, currentSlideNumber) {
                var currentSlide = slideBox.find(".bcms-slides-single-slide").get([currentSlideNumber - 1]);
                $(currentSlide).find('.bcms-preview-box').each(function () {
                    var previewBox = $(this),
                        data = previewBox.data();
                    if (!data.isLoaded) {
                        if (data.asImage === "True") {
                            previewBox.find('div:first').append($.format("<img src=\"{0}\" alt=\"{1}\" />",
                                data.previewUrl, data.title));
                        } else {
                            previewBox.find('div:first').append($.format("<iframe class=\"{0}\" width=\"{1}\" height=\"{2}\" scrolling=\"no\" border=\"0\" frameborder=\"0\" src=\"{3}\" style=\"background-color:white;\"/>",
                                data.frameCssClass, data.width, data.height, data.previewUrl));
                        }
                        previewBox.data("isLoaded", true);
                    }
                });
            };
            container.find(selectors.sliderBoxes).each(function () {
                var slideBox = $(this);
                slideBox.slides({
                    container: selectors.sliderContainer,
                    generateNextPrev: true,
                    generatePagination: false,
                    prev: classes.sliderPrev,
                    next: classes.sliderNext,
                    slidesLoaded: function () {
                        updateSlide(slideBox, 1);
                    },
                    animationStart: function (currentSlideNumber) {
                    },
                    animationComplete: function (currentSlideNumber) {
                        updateSlide(slideBox, currentSlideNumber);
                    }
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
                
                if (!security.IsAuthorized(["BcmsEditContent", "BcmsPublishContent"])) {
                    contentViewModel.removeHistoryButton();
                    contentViewModel.removeEditButton();
                }
                
                if (!security.IsAuthorized(["BcmsEditContent"])) {
                    contentViewModel.removeDeleteButton();
                }
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
            var canEdit = security.IsAuthorized(["BcmsEditContent"]);
            modal.edit({
                title: globalization.editContentDialogTitle,
                disableSaveDraft: !canEdit,
                isPreviewAvailable: canEdit,
                disableSaveAndPublish: !security.IsAuthorized(["BcmsPublishContent"]),
                onCloseClick: onCloseClick,
                onLoad: function (dialog) {
                    var url = $.format(links.editPageContentUrl, contentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog, data) {
                            var editInSourceMode = false,
                                enableInsertDynamicRegion = false;
                            if (data && data.Data) {
                                if (data.Data.EditInSourceMode) {
                                    editInSourceMode = true;
                                }
                                if (data.Data.EnableInsertDynamicRegion) {
                                    enableInsertDynamicRegion = true;
                                }
                            }
                            pagesContent.initializeEditContentForm(contentDialog, editInSourceMode, enableInsertDynamicRegion);
                        },

                        beforePost: function () {
                            htmlEditor.updateEditorContent();
                            
                            var editInSourceMode = htmlEditor.isSourceMode(selectors.htmlEditor);
                            dialog.container.find(selectors.editInSourceModeHiddenField).val(editInSourceMode);
                        },

                        postError: function(json) {
                            if (json.Data && json.Data.ConfirmationMessage) {
                                modal.confirm({
                                    content: json.Data.ConfirmationMessage,
                                    onAccept: function () {
                                        dialog.container.find(selectors.pageContentUserConfirmationHiddenField).val(true);
                                        dialog.submitForm();
                                        return true;
                                    }
                                });
                            }
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
            var createUrl = function(isUserConfirmed) {
                    return $.format(links.deletePageContentUrl, pageContentId, pageContentVersion, contentVersion, isUserConfirmed);
                },
                getUrl = function() {
                    return createUrl(false);
                },
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
                            if (json.Data && json.Data.ConfirmationMessage) {
                                modal.confirm({
                                    content: json.Data.ConfirmationMessage,
                                    onAccept: function () {
                                        getUrl = function() {
                                            return createUrl(true);
                                        };
                                        confirmDialog.accept();
                                        return true;
                                    }
                                });
                            } else if (json.Messages && json.Messages.length > 0) {
                                modal.showMessages(json);
                            } else {
                                modal.alert({
                                    title: globalization.deleteContentFailureMessageTitle,
                                    content: globalization.deleteContentFailureMessageMessage
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
                            url: getUrl(),
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
        * Function tries to resolve ace editor container ithin given container and focuses the editor
        */
        function focusAceEditor(container) {
            var aceEditor = container.find(selectors.aceEditorContainer).data('aceEditor');
            if (aceEditor != null) {
                aceEditor.focus();
            }
        }
        
        /**
        * IE11 fix: recall resize method after editors initialization
        */
        function resizeAceEditor(container) {
            setTimeout(function () {
                var aceEditor = container.find(selectors.aceEditorContainer).data('aceEditor');
                if (aceEditor && $.isFunction(aceEditor.resize)) {
                    aceEditor.resize(true);
                    aceEditor.renderer.updateFull();
                }
            }, 20);
        }

        /**
        * Shows/hides custom css field in a content edit form
        */
        function showHideCustomCssText(dialog, focus) {
            var customCssContainer = dialog.container.find(selectors.customCssContainer);

            if (dialog.container.find(selectors.enableCustomCss).attr('checked')) {
                customCssContainer.show();
                resizeAceEditor(customCssContainer);
                if (focus) {
                    focusAceEditor(customCssContainer);
                }
            } else {
                customCssContainer.hide();
            }
        };

        function showHideCustomJsText(dialog, focus) {
            var customJsContainer = dialog.container.find(selectors.customJsContainer);
            
            if (dialog.container.find(selectors.enableCustomJs).attr('checked')) {
                customJsContainer.show();
                resizeAceEditor(customJsContainer);
                if (focus) {
                    focusAceEditor(customJsContainer);
                }
            } else {
                customJsContainer.hide();
            }
        };

        /**
        * Creates new Guid
        */
        function createGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16).toUpperCase();
            });
        }

        /**
        * Inserts dynamic region 
        */
        function onDynamicRegionInsert(htmlContentEditor) {
            if (htmlContentEditor != null) {
                var guid = createGuid(),
                    html;
                
                if (htmlContentEditor.mode == 'source') {
                    html = '<div>{{DYNAMIC_REGION:' + guid + '}}</div>';
                    htmlContentEditor.addHtml(html);
                } else {
                    // Create fake CKEditor object with real object representation (inversion of code in /[BetterCms.Module.Root]/Scripts/ckeditor/plugins/cms-dynamicregion/plugin.js).
                    // NOTE: EDITOR.createFakeParserElement(...) functionality does not work...
                    html = '<div class="bcms-draggable-region" data-cke-realelement="%3Cdiv%3E%7B%7BDYNAMIC_REGION%3A'
                        + guid
                        + '%7D%7D%3C%2Fdiv%3E" data-cke-real-node-type="1" title="Dynamic Region" data-cke-real-element-type="cmsdynamicregion" isregion="true">Content to add</div>';
                    var re = CKEDITOR.dom.element.createFromHtml(html, htmlContentEditor.document);
                    htmlContentEditor.insertElement(re);
                }
            }
        }

        /**
        * Initializes page module.
        */
        pagesContent.init = function() {
            bcms.logger.debug('Initializing bcms.pages.content module.');
        };

        /**
        * Subscribe to events
        */
        bcms.on(bcms.events.addPageContent, pagesContent.onAddNewContent);
        bcms.on(bcms.events.sortPageContent, pagesContent.onSortPageContent);
        bcms.on(bcms.events.createContentOverlay, onCreateContentOverlay);
        bcms.on(htmlEditor.events.insertDynamicRegion, onDynamicRegionInsert);

        /**
        * Register initialization
        */
        bcms.registerInit(pagesContent.init);

        return pagesContent;
    });
