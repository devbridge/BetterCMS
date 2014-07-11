/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.dynamicContent', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.forms', 'bcms.messages'], function ($, bcms, modal, forms) {
    'use strict';

    var dynamicConent = {},
    // Classes that are used to maintain various UI states:
        classes = {

        },
    // Selectors used in the module to locate DOM elements:
        selectors = {
            ajaxForm: 'form.bcms-ajax-form'
        },

        links = {},

        globalization = {
            failedLoadDialogMessage: null,
            dialogLoadingCancelledMessage: null,
            forbiddenDialogMessage: null,
            unauthorizedDialogMessage: null
        },
        lastDialogId = null;

    /**
    /* Assign objects to module.
    */
    dynamicConent.selectors = selectors;
    dynamicConent.links = links;
    dynamicConent.globalization = globalization;

    /**
    * Sets dialog content from Url.
    */
    dynamicConent.setContentFromUrl = function (dialog, url, options) {
        
        function logError(error) {
            bcms.logger.error('Failed to load dialog content from ' + url + ' (' + error + ').');
        }

        var contentId = dialog.contentId || 0,
            currentDialogId = (new Date).getTime();

        options = $.extend({
            done: null,
            fail: function (failedDialog, message, request) {
                var html = '<div class="bcms-scroll-window">' +
                                '<div class="bcms-messages-type-2">' +
                                    '<ul class="bcms-error-messages">' +
                                        '<li>' +
                                            message +
                                        '</li>' +
                                    '</ul>' +
                                '</div>';
                            
                if (bcms.errorTrace && request) {
                    html = html + request.responseText;
                }
                
                html += '</div>';

                failedDialog.setContent(html);
                if (failedDialog.disableAcceptButton) {
                    failedDialog.disableAcceptButton();
                }
                if (failedDialog.disableExtraButtons) {
                    failedDialog.disableExtraButtons();
                }
            }
        }, options);

        lastDialogId = currentDialogId;
        dynamicConent.showLoading(dialog);

        $.ajax({
            type: "GET",
            url: url,
            cache: false
        })
        .done(function (content, status, response) {
            if (lastDialogId !== currentDialogId) {
                return;
            }
            
            if (response.getResponseHeader('Content-Type').indexOf('application/json') === 0 && content.Html) {
                if (content.Success) {
                    try {
                        dialog.setContent(content.Html, contentId);
                    } catch (exc) {
                        logError(exc.message);
                    } finally {
                        dynamicConent.hideLoading(dialog);
                    }
                } else {
                    if (dialog.close) {
                        dialog.close();
                    }
                    if (content.Messages && content.Messages.length > 0) {
                        modal.showMessages(content);
                    } else {
                        modal.alert({
                            content: globalization.failedLoadDialogMessage
                        });
                    }
                    dynamicConent.hideLoading(dialog);

                    return;
                }
            } else {
                try {
                    dialog.setContent(content, contentId);
                } catch (exc) {
                    logError(exc.message);
                } finally {
                    dynamicConent.hideLoading(dialog);
                } 
            }
           

            if ($.isFunction(options.done)) {
                options.done(content);
            }
        })
        .fail(function (response, status, error) {
            logError(error);

            dynamicConent.hideLoading(dialog);

            if ($.isFunction(options.fail)) {
                var errorMessage = globalization.failedLoadDialogMessage;
                if (error === "" && response.status === 0) {
                    errorMessage = globalization.dialogLoadingCancelledMessage;
                } else if (response.status === 401) {
                    errorMessage = globalization.unauthorizedDialogMessage;
                    try {
                        var redirectUrl = response.getResponseHeader(bcms.constants.loginRedirectHeader);
                        if (redirectUrl) {
                            window.location.href = redirectUrl;
                        }
                    } catch (err) {
                        bcms.logger.error(err);
                    }
                } else if (error == "Forbidden") {
                    errorMessage = globalization.forbiddenDialogMessage;
                }
                options.fail(dialog, errorMessage, response);
            }
        });
    };

    /**
    * Binds dialog forms to post data via Ajax.
    */
    dynamicConent.bindDialogAjaxPost = function (dialog, options) {

        var dialogForm = dialog.container.find(selectors.ajaxForm);

        dialogForm.each(function () {
            var form = $(this);
            forms.ajaxForm(form, {
                contentType: options.contentType || 'application/x-www-form-urlencoded',
                dataType: options.dataType || 'json',
                serialize: options.serialize || function (formToSerialize) {
                    return formToSerialize.serialize();
                },

                beforeSubmit: function () {
                    if ($.isFunction(options.beforeSubmit)) {
                        var result = options.beforeSubmit(form);
                        if (result !== false) {
                            dynamicConent.showLoading(dialog);
                        }
                        return result;
                    }
                    dynamicConent.showLoading(dialog);
                    return true;
                },

                success: function (json) {
                    if ($.isFunction(options.success)) {
                        if (options.success(json, dialog) !== false) {
                            dialog.accept();
                        }
                    } else {
                        dialog.accept();
                    }
                },

                error: function (json) {
                    if ($.isFunction(options.error)) {
                        options.error(json, dialog);
                    }
                },

                complete: function (json) {
                    dynamicConent.hideLoading(dialog);
                    if ($.isFunction(options.complete)) {
                        options.complete(json, dialog);
                    }
                }
            });
        });

        dialog.submitForm = function() {
            dialogForm.submit();
        };
        
        var oldOnAcceptClick = dialog.options.onAcceptClick;
        dialog.options.onAcceptClick = function () {
            if ($.isFunction(oldOnAcceptClick)) {
                oldOnAcceptClick(dialog);
            }

            dialog.submitForm();
            return false;
        };
    };

    /*
    * Loads content from remote Url to dialog. Allows to attach Ajax event to forms.
    */
    dynamicConent.bindDialog = function (dialog, url, options) {
        options = $.extend({
            contentAvailable: null,
            beforePost: null,
            postSuccess: null,
            postError: null,
            postComplete: null,

            formContentType: 'application/x-www-form-urlencoded',
            formDataType: 'json',
            formSerialize: function (form) {
                return form.serialize();
            }
        }, options);

        dynamicConent.setContentFromUrl(dialog, url, {
            done: function (content) {

                if ($.isFunction(options.contentAvailable)) {
                    options.contentAvailable(dialog, content);
                }

                dynamicConent.bindDialogAjaxPost(dialog, {
                    beforeSubmit: options.beforePost,
                    success: options.postSuccess,
                    error: options.postError,
                    complete: options.postComplete,

                    dataType: options.formDataType,
                    contentType: options.formContentType,
                    serialize: options.formSerialize
                });
            }
        });
    };

    dynamicConent.bindSiteSettings = function (siteSettings, url, options) {
        options = $.extend({
            contentAvailable: null
        }, options);

        siteSettings.contentId++;
        
        dynamicConent.setContentFromUrl(siteSettings, url, {
            done: function (content) {
                
                if ($.isFunction(options.contentAvailable)) {
                    options.contentAvailable(content);
                }
            }
        });
    };

    dynamicConent.getLoaderContainer = function (dialog) {
        if (dialog && dialog.getLoaderContainer) {
            return dialog.getLoaderContainer();
        }
        return null;
    };

    dynamicConent.showLoading = function (dialog) {
        var container = dynamicConent.getLoaderContainer(dialog);
        if (container) {
            container.showLoading('dynamicContent');
        }
    };
    
    dynamicConent.hideLoading = function (dialog) {
        var container = dynamicConent.getLoaderContainer(dialog);
        if (container) {
            container.hideLoading('dynamicContent');
        }
    };

    return dynamicConent;
});
