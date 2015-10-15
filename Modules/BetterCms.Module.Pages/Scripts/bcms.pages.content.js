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
                parentPageContentId: '#bcmsParentPageContentId',
                contentFormRegionId: '#bcmsContentToRegionId',
                desirableStatus: '#bcmsContentDesirableStatus',
                dataPickers: '.bcms-datepicker',
                htmlEditor: '.bcms-contenthtml',
                destroyDraftVersionLink: '.bcms-messages-draft-destroy',
                pageContentUserConfirmationHiddenField: '#bcms-user-confirmed-region-deletion',
                htmlContentJsCssTabOpener: '.bcms-tab-item[data-name="#bcms-tab-2"]',

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
                anyTab: '.bcms-tab-item',

                widgetsContent: '.bcms-widgets',

                editInSourceModeHiddenField: '#bcms-edit-in-source-mode',
                contentTextModeHiddenField: '#bcms-content-text-mode',
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
                previewPageUrl: null,
                selectWidgetUrl: null
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
                datePickerTooltipTitle: null,
                selectWidgetDialogTitle: null
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
        pagesContent.onAddNewContent = function (data) {
            var editorId,
                contentTextMode = data.contentTextMode,
                regionViewModel = data.regionViewModel,
                includeChildRegions = bcms.boolAsString(data.includeChildRegions),
                onSuccess = data.onSuccess || function () {
                    redirect.ReloadWithAlert();
                };

            modal.edit({
                title: globalization.addNewContentDialogTitle,
                disableSaveAndPublish: !security.IsAuthorized(["BcmsPublishContent"]),
                onLoad: function (dialog) {
                    var url = $.format(links.loadAddNewHtmlContentDialogUrl, bcms.pageId, regionViewModel.id, regionViewModel.parentPageContentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog, data) {
                            var editInSourceMode = false,
                                enableInsertDynamicRegion = false;

                            editorId = dialog.container.find(selectors.htmlEditor).attr('id');
                            
                            if (data && data.Data) {
                                if (data.Data.EditInSourceMode) {
                                    editInSourceMode = true;
                                }
                                if (data.Data.EnableInsertDynamicRegion) {
                                    enableInsertDynamicRegion = true;
                                }
                            }

                            var settings = {
                                dialog: contentDialog, 
                                editInSourceMode: editInSourceMode, 
                                enableInsertDynamicRegion: enableInsertDynamicRegion, 
                                editorId: editorId,
                                data: data.Data,
                                onSuccess: onSuccess,
                                includeChildRegions: includeChildRegions,
                                contentTextMode: contentTextMode
                            };
                            pagesContent.initializeAddNewContentForm(settings);
                        },

                        beforePost: function () {
                            var editInSourceMode = htmlEditor.isSourceMode(editorId, contentTextMode);
                            dialog.container.find(selectors.editInSourceModeHiddenField).val(editInSourceMode);
                            dialog.container.find(selectors.contentTextModeHiddenField).val(contentTextMode);

                            return true;
                        },

                        postSuccess: function (json) {
                            if (json.Data.DesirableStatus == bcms.contentStatus.preview) {
                                try {
                                    var result = json.Data;
                                    $(selectors.contentId).val(result.ContentId);
                                    $(selectors.pageContentId).val(result.PageContentId);
                                    preview.previewPageContent(result.PageId, result.PageContentId);
                                } finally {
                                    return false;
                                }
                            } else {
                                if ($.isFunction(onSuccess)) {
                                    onSuccess(json);
                                }
                            }

                            return true;
                        },

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, editorId, null, function (serializedData) {
                                if (includeChildRegions) {
                                    serializedData.IncludeChildRegions = true;
                                }
                            });
                        },
                        formContentType: 'application/json; charset=UTF-8'
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
        pagesContent.onSortPageContent = function (data) {
            var models = data.models,
                onSuccess = data.onSuccess,
                info = modal.info({
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
                   PageContents: models
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
                        if ($.isFunction(onSuccess)) {
                            onSuccess(json.Data.PageContents);
                        }
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

        function initializeWidgetsTab(dialog, onInsert) {
            dialog.container.find(selectors.widgetsSearchButton).on('click', function () {
                pagesContent.updateWidgetCategoryList(dialog, onInsert);
            });

            dialog.container.find(selectors.widgetCreateButton).on('click', function () {
                widgets.openCreateHtmlContentWidgetDialog(function (json) {
                    // Reload search results after category was created.
                    pagesContent.updateWidgetCategoryList(dialog, onInsert);
                    messages.refreshBox(dialog.container, json);
                }, null);
            });

            dialog.container.find(selectors.widgetRegisterButton).on('click', function () {
                widgets.openCreateServerControlWidgetDialog(function (json) {
                    pagesContent.updateWidgetCategoryList(dialog, onInsert);
                    messages.refreshBox(dialog.container, json);
                }, null);
            });

            bcms.preventInputFromSubmittingForm(dialog.container.find(selectors.widgetsSearchInput), {
                preventedEnter: function () {
                    pagesContent.updateWidgetCategoryList(dialog, onInsert);
                }
            });
        }

        /**
        * Initializes content dialog form.
        */
        pagesContent.initializeAddNewContentForm = function (settings) {

            var codeEditorInitialized = false;
            settings = $.extend({
                enableInsertDynamicRegion: false,
                editorId: null,
                dialog: null,
                data: {},
                onSuccess: function () { },
                includeChildRegions: false,
                contentTextMode: content.contentTextModes.html
            }, settings);

            settings.dialog.container.find(selectors.dataPickers).initializeDatepicker(globalization.datePickerTooltipTitle);

            settings.dialog.container.find(selectors.anyTab).click(function () {
                setTimeout(function () {
                    settings.dialog.setFocus();
                }, 100);
            });

            settings.dialog.container.find(selectors.htmlContentJsCssTabOpener).on('click', function () {
                if (!codeEditorInitialized) {
                    codeEditor.initialize(settings.dialog.container, settings.dialog);
                    codeEditorInitialized = true;
                }
            });

            var editorHeight = modal.maximizeChildHeight(settings.dialog.container.find("#" + settings.editorId), settings.dialog);

            if (settings.contentTextMode == content.contentTextModes.markdown) {
                htmlEditor.initializeMarkdownEditor(settings.editorId, '', {});
            }

            if (settings.contentTextMode == content.contentTextModes.simpleText) {
                htmlEditor.initializeMarkdownEditor(settings.editorId, '', { hideIcons: true });
            }

            if (settings.contentTextMode == content.contentTextModes.html) {
                htmlEditor.initializeHtmlEditor(settings.editorId, '', {
                    height: editorHeight
                }, settings.editInSourceMode);

                if (settings.enableInsertDynamicRegion) {
                    htmlEditor.enableInsertDynamicRegion(settings.editorId, true, settings.data.LastDynamicRegionNumber);
                }
            }
        };

        /**
        * Initializes content edit dialog form.
        */
        pagesContent.initializeEditContentForm = function (settings) {
            var codeEditorInitialized = false;

            settings = $.extend({
                dialog: null,
                editInSourceMode: false,
                enableInsertDynamicRegion: false,
                data: {},
                editorId: null, 
                includeChildRegions: false, 
                onSuccess: function() {},
                contentTextMode: content.contentTextModes.html
            }, settings);

            var canEdit = security.IsAuthorized(["BcmsEditContent"]),
                canPublish = security.IsAuthorized(["BcmsPublishContent"]),
                form = settings.dialog.container.find(selectors.firstForm);

            settings.dialog.container.find(selectors.htmlContentJsCssTabOpener).on('click', function () {
                if (!codeEditorInitialized) {
                    codeEditor.initialize(settings.dialog.container, settings.dialog);
                    codeEditorInitialized = true;
                }
            });

            var editorHeight = modal.maximizeChildHeight(settings.dialog.container.find("#" + settings.editorId), settings.dialog);

            if (settings.contentTextMode == content.contentTextModes.markdown) {
                htmlEditor.initializeMarkdownEditor(settings.editorId, settings.data.ContentId, {});
            }

            if (settings.contentTextMode == content.contentTextModes.simpleText) {
                htmlEditor.initializeMarkdownEditor(settings.editorId, settings.data.ContentId, { hideIcons: true });
            }
             
            if (settings.contentTextMode == content.contentTextModes.html) {
                htmlEditor.initializeHtmlEditor(settings.editorId, settings.data.ContentId, {
                    height: editorHeight
                }, settings.editInSourceMode);

                if (settings.enableInsertDynamicRegion) {
                    htmlEditor.enableInsertDynamicRegion(settings.editorId, true, settings.data.LastDynamicRegionNumber);
                }
            }

            settings.dialog.container.find(selectors.destroyDraftVersionLink).on('click', function () {
                var contentId = settings.dialog.container.find(selectors.contentId).val(),
                    pageContentId = settings.dialog.container.find(selectors.pageContentId).val(),
                    contentVersion = settings.dialog.container.find(selectors.contentVersion).val();

                history.destroyDraftVersion(contentId, contentVersion, settings.includeChildRegions, settings.dialog.container, function (publishedId, json) {
                    settings.dialog.close();

                    pagesContent.editPageContent(pageContentId, {
                        onCloseClick: function () {
                            // If is set what to do on success, do it, otherwise - reload the page
                            if ($.isFunction(settings.onSuccess)) {
                                settings.onSuccess(json);
                            } else {
                                redirect.ReloadWithAlert();
                            }
                        },
                        onSuccess: settings.onSuccess,
                        includeChildRegions: settings.includeChildRegions
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

            settings.dialog.container.find(selectors.dataPickers).initializeDatepicker();
        };

        /**
        * Reloads widget category list.
        */
        pagesContent.updateWidgetCategoryList = function (dialog, onInsert) {
            $.ajax({
                url: $.format(links.loadWidgetsUrl, dialog.container.find(selectors.widgetsSearchInput).val()),
                cache: false
            }).done(function (data) {
                dialog.container.find(selectors.widgetsContainer).html(data);

                initializeWidgets(dialog.container, dialog, onInsert);
                dialog.container.find(selectors.widgetsSearchInput).focus();
            });
        };

        /**
        * Initializes widget categories list with sliders.
        */
        function initializeWidgets(container, dialog, onInsert) {

            pagesContent.initializeSliders(container);

            container.find(selectors.widgetInsertButtons).on('click', onInsert);

            container.find(selectors.widgetDeleteButtons).on('click', function () {
                var self = $(this),
                    widgetContainer = self.parents(selectors.widgetContainerBlock),
                    widgetId = widgetContainer.data('originalId'),
                    widgetVersion = widgetContainer.data('originalVersion'),
                    widgetName = widgetContainer.find(selectors.widgetName).text(),
                    onComplete = function (data) {
                        messages.refreshBox(modal.last().container || widgetContainer, data);
                        widgetContainer.hideLoading();
                    };

                widgets.deleteWidget(widgetId, widgetVersion, widgetName,
                    function () {
                        widgetContainer.showLoading();
                    },
                    function (data) {
                        onComplete(data);
                        pagesContent.updateWidgetCategoryList(dialog, onInsert);
                    },
                    onComplete);
            });

            container.find(selectors.widgetEditButtons).on('click', function () {
                var self = $(this),
                    widgetContainer = self.parents(selectors.widgetContainerBlock),
                    widgetId = widgetContainer.data('id'),
                    widgetType = widgetContainer.data('type');

                widgets.editWidget(widgetId, widgetType, function (data) {
                    messages.refreshBox(widgetContainer, data);
                    pagesContent.updateWidgetCategoryList(dialog, onInsert);
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
        pagesContent.insertWidget = function (opts) {
            if (!opts.regionViewModel) {
                bcms.logger.error('Failed to insert the widget to the page. Missing region info.');
                return;
            }

            if (!opts.widget) {
                bcms.logger.error('Failed to insert the widget to the page. Missing widget info.');
                return;
            }

            opts = $.extend({
                onSuccess: function() {
                    redirect.ReloadWithAlert();
                }
            }, opts);

            console.log(opts);

            var url = $.format(links.insertContentToPageUrl,
                    bcms.pageId,
                    opts.widget.id,
                    opts.regionViewModel.id,
                    opts.regionViewModel.parentPageContentId,
                    opts.includeChildRegions || false);

            $.ajax({
                type: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                cache: false,
                beforeSend: function () {
                    if (opts.widget.dialog) {
                        opts.widget.dialog.close();
                    }

                    opts.widget.dialog = modal.info({
                        title: globalization.insertingWidgetInfoHeader,
                        content: globalization.insertingWidgetInfoMessage,
                        disableCancel: true,
                        disableAccept: true
                    });
                },

                success: function (data) {
                    if (opts.widget.dialog) {
                        opts.widget.dialog.close();
                    }

                    if (data && data.Success) {
                        opts.onSuccess(data);
                    } else {
                        modal.alert({
                            title: globalization.errorTitle,
                            content: data.Messages ? data.Messages[0] : globalization.insertingWidgetErrorMessage
                        });
                    }
                },

                error: function () {
                    if (opts.widget.dialog) {
                        opts.widget.dialog.close();
                    }

                    modal.alert({
                        title: globalization.errorTitle,
                        content: globalization.insertingWidgetErrorMessage
                    });
                }
            });

            return false;
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
        * Called when content view model is created
        */
        function onContentModelCreated(contentViewModel) {
            var contentId = contentViewModel.contentId,
                pageContentId = contentViewModel.pageContentId;

            if (contentViewModel.contentType == contentTypes.htmlContent) {
                // Edit content
                contentViewModel.onEditContent = function (onSuccess, includeChildRegions) {
                    pagesContent.editPageContent(pageContentId, {
                        onSuccess: onSuccess,
                        includeChildRegions: includeChildRegions
                    });
                };

                contentViewModel.visibleButtons.configure = false;

                if (!security.IsAuthorized(["BcmsEditContent", "BcmsPublishContent"])) {
                    contentViewModel.visibleButtons.history = false;
                    contentViewModel.visibleButtons.edit = false;
                }

                if (!security.IsAuthorized(["BcmsEditContent"])) {
                    contentViewModel.visibleButtons["delete"] = false;
                }
            }

            // Delete content
            contentViewModel.onDeleteContent = function (onDeleteSuccess) {
                pagesContent.removeContentFromPage(contentViewModel.pageContentId,
                    contentViewModel.pageContentVersion,
                    contentViewModel.title,
                    contentViewModel.contentVersion,
                    onDeleteSuccess);
            };

            // Content history
            contentViewModel.onContentHistory = function (onContentRestore, includeChildRegions) {
                history.openPageContentHistoryDialog(contentId, pageContentId, {
                    onContentRestore: onContentRestore,
                    includeChildRegions: includeChildRegions
                });
            };

            // Change draft icon
            if (contentViewModel.draft) {
                contentViewModel.visibleButtons.draft = true;
            }
        }

        /**
        * Opens dialog for editing page regular content  
        */
        pagesContent.editPageContent = function (contentId, opts) {
            opts = $.extend({
                onCloseClick: null,
                onSuccess: null,
                includeChildRegions: false
            }, opts);

            var canEdit = security.IsAuthorized(["BcmsEditContent"]),
                contentTextMode = content.contentTextModes.html,
                onCloseClick = opts.onCloseClick,
                onSuccess = opts.onSuccess,
                includeChildRegions = (opts.includeChildRegions === true),
                editorId;

            modal.edit({
                title: globalization.editContentDialogTitle,
                disableSaveDraft: !canEdit,
                isPreviewAvailable: canEdit,
                disableSaveAndPublish: !security.IsAuthorized(["BcmsPublishContent"]),
                onCloseClick: onCloseClick,
                onLoad: function (dialog) {
                    var url = $.format(links.editPageContentUrl, contentId);
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog, json) {
                            var editInSourceMode = false,
                                enableInsertDynamicRegion = false;

                            editorId = dialog.container.find(selectors.htmlEditor).attr('id');

                            if (json && json.Data) {
                                if (json.Data.EditInSourceMode) {
                                    editInSourceMode = true;
                                }
                                if (json.Data.ContentTextMode) {
                                    contentTextMode = json.Data.ContentTextMode;
                                }
                                if (json.Data.EnableInsertDynamicRegion) {
                                    enableInsertDynamicRegion = true;
                                }
                            }

                            pagesContent.initializeEditContentForm({
                                dialog: contentDialog,
                                editInSourceMode: editInSourceMode,
                                enableInsertDynamicRegion: enableInsertDynamicRegion,
                                data: json.Data,
                                editorId: editorId,
                                includeChildRegions: includeChildRegions,
                                onSuccess: onSuccess,
                                contentTextMode: contentTextMode
                            });
                        },

                        beforePost: function () {
                            var editInSourceMode = htmlEditor.isSourceMode(editorId, contentTextMode);
                            dialog.container.find(selectors.editInSourceModeHiddenField).val(editInSourceMode);

                            return true;
                        },

                        postError: function (json) {
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
                                if ($.isFunction(onSuccess)) {
                                    onSuccess(json);
                                } else {
                                    redirect.ReloadWithAlert();
                                }
                            }

                            return true;
                        },

                        formSerialize: function (form) {
                            return widgets.serializeFormWithChildWidgetOptions(form, editorId, null, function (serializedData) {
                                if (includeChildRegions) {
                                    serializedData.IncludeChildRegions = true;
                                }
                            });
                        },
                        formContentType: 'application/json; charset=utf-8'
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
        pagesContent.removeContentFromPage = function (pageContentId, pageContentVersion, contentName, contentVersion, onDeleteSuccess) {
            if (!onDeleteSuccess) {
                onDeleteSuccess = function () {
                    redirect.ReloadWithAlert({
                        title: globalization.deleteContentSuccessMessageTitle,
                        message: globalization.deleteContentSuccessMessageMessage,
                        timeout: 1500
                    });
                };
            }

            var createUrl = function (isUserConfirmed) {
                return $.format(links.deletePageContentUrl, pageContentId, pageContentVersion, contentVersion, isUserConfirmed);
            },
                getUrl = function () {
                    return createUrl(false);
                },
                onDeleteCompleted = function (json) {
                    try {
                        if (json.Success) {
                            onDeleteSuccess(json);
                        }
                        else {
                            if (json.Data && json.Data.ConfirmationMessage) {
                                modal.confirm({
                                    content: json.Data.ConfirmationMessage,
                                    onAccept: function () {
                                        getUrl = function () {
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
                    content: $.format(globalization.deleteContentConfirmationMessage, contentName),
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
        * Inserts dynamic region 
        */
        function onDynamicRegionInsert(htmlContentEditor) {
            if (htmlContentEditor != null) {
                var last = htmlContentEditor.LastDynamicRegionNumber || 0,
                    isMasterPage = htmlContentEditor.IsMasterPage === true,
                    regionIdentifier,
                    html;

                // If widget
                if (!isMasterPage) {
                    last++;
                    regionIdentifier = 'WidgetRegion' + last + '_' + bcms.createGuid().replace('-', '');
                } else {
                    last++;
                    if (last == 1) {
                        regionIdentifier = 'CMSMainContent';
                    } else {
                        regionIdentifier = 'ContentRegion' + last;
                    }
                }
                htmlContentEditor.LastDynamicRegionNumber = last;

                if (htmlContentEditor.mode == 'source') {
                    html = '<div>{{DYNAMIC_REGION:' + regionIdentifier + '}}</div>';
                    htmlContentEditor.addHtml(html);
                } else {
                    // Create fake CKEditor object with real object representation (inversion of code in /[BetterCms.Module.Root]/Scripts/ckeditor/plugins/cms-dynamicregion/plugin.js).
                    // NOTE: EDITOR.createFakeParserElement(...) functionality does not work...
                    html = '<div class="bcms-draggable-region" data-cke-realelement="%3Cdiv%3E%7B%7BDYNAMIC_REGION%3A'
                        + regionIdentifier
                        + '%7D%7D%3C%2Fdiv%3E" data-cke-real-node-type="1" title="Dynamic Region" data-cke-real-element-type="cmsdynamicregion" isregion="true">Content to add</div>';
                    var re = CKEDITOR.dom.element.createFromHtml(html, htmlContentEditor.document);
                    htmlContentEditor.insertElement(re);
                }
            }
        }

        /**
        * Inserts child widget to the content
        */
        function onWidgetInsert(opts) {
            if (!$.isFunction(opts.onInsert) && !opts.regionViewModel) {
                return;
            }

            if (!$.isFunction(opts.onInsert)) {
                opts.onInsert = function (widget) {
                    opts.widget = widget;
                    pagesContent.insertWidget(opts);
                }
            }

            var dialog,
                onInsert = function () {
                    var widgetContainer = $(this).parents(selectors.widgetContainerBlock),
                        contentId = widgetContainer.data('originalId').toUpperCase(),
                        title = widgetContainer.find(selectors.widgetName).text(),
                        html = '<widget data-id="' + contentId + '" data-assign-id="' + bcms.createGuid() + '">' + title + '</widget>';

                    var result = opts.onInsert({
                        id: contentId,
                        title: title,
                        html: html,
                        dialog: dialog
                    });

                    if (result !== false) {
                        dialog.close();
                    }
                };

            dialog = modal.open({
                title: globalization.selectWidgetDialogTitle,
                disableAccept: true,
                onLoad: function (dialog) {
                    var url = links.selectWidgetUrl;
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: function (contentDialog) {
                            initializeWidgetsTab(contentDialog, onInsert);
                            initializeWidgets(contentDialog.container, contentDialog, onInsert);
                        }
                    });
                }
            });
        }

        /**
        * Initializes page module.
        */
        pagesContent.init = function () {
            bcms.logger.debug('Initializing bcms.pages.content module.');
        };

        /**
        * Subscribe to events
        */
        bcms.on(bcms.events.addPageContent, pagesContent.onAddNewContent);
        bcms.on(bcms.events.sortPageContent, pagesContent.onSortPageContent);
        bcms.on(bcms.events.contentModelCreated, onContentModelCreated);
        bcms.on(htmlEditor.events.insertDynamicRegion, onDynamicRegionInsert);
        bcms.on(bcms.events.insertWidget, onWidgetInsert);

        /**
        * Register initialization
        */
        bcms.registerInit(pagesContent.init);

        return pagesContent;
    });
