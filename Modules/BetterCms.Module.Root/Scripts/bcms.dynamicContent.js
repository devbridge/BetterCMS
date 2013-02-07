/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.dynamicContent', ['jquery', 'bcms', 'bcms.modal', 'bcms.forms', 'bcms.messages'], function ($, bcms, modal, forms) {
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
            failedLoadDialogMessage: 'Failed to load dialog. Internal server error. Please try again later.'
        };

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
        var contentId = dialog.contentId || 0;

        options = $.extend({
            done: null,
            fail: function (failedDialog, message, request) {
                var html = '<div class="bcms-messages-type-2">' +
                                '<ul class="bcms-error-messages">' +
                                    '<li>' +
                                        message +
                                    '</li>' +
                                '</ul>' +
                            '</div>';

                if (bcms.errorTrace && request) {
                    html = html + request.responseText;
                }

                failedDialog.setContent(html);
                failedDialog.disableAccept();
            }
        }, options);

        dynamicConent.showLoading(dialog);

        $.ajax({
            type: "GET",
            url: url,
            cache: false
        })
        .done(function (content, status, response) {
            if (response.getResponseHeader('Content-Type').indexOf('application/json') === 0 && content.Html) {
                dialog.setContent(content.Html, contentId);
            } else {
                dialog.setContent(content, contentId);
            }

            dynamicConent.hideLoading(dialog);

            if ($.isFunction(options.done)) {
                options.done(content);
            }
        })
        .fail(function (request, status, error) {
            console.log('Failed to load dialog content from ' + url + ' (' + error + ').');

            dynamicConent.hideLoading(dialog);

            if ($.isFunction(options.fail)) {
                options.fail(dialog, globalization.failedLoadDialogMessage, request);
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
                beforeSubmit: function () {
                    if ($.isFunction(options.beforeSubmit)) {
                        options.beforeSubmit(form);
                    }
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
            postComplete: null
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
                    complete: options.postComplete
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
