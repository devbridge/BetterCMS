/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.messages', ['jquery', 'bcms', 'bcms.modal'], function ($, bcms, modal) {
    'use strict';

    var messages = {},

    // Classes that are used to maintain various UI states:
        classes = {
        },

    // Selectors used in the module to locate DOM elements:
        selectors = {
            messagesBox: '#bcms-messages-box',
            messages: '.bcms-messages:first',
            success: '.bcms-success-messages:first',
            info: '.bcms-info-messages:first',
            warn: '.bcms-warning-messages:first',
            error: '.bcms-error-messages:first'
        },

        links = {},
        globalization = {};

    // Assign objects to module
    messages.classes = classes;
    messages.selectors = selectors;
    messages.links = links;
    messages.globalization = globalization;

    /**
    * Messages box instance constructor.
    */

    function MessagesBox(options) {
        var container = null;

        this.options = $.extend({
            container: null,
            containerId: null,
            messageBoxId: null,
            enableAutoHide: true,
            autoHideTimeout: 10000
        }, options);

        if (options.container) {
            var parentContainer = options.container;
            do {
                if (options.container instanceof $) {
                    container = parentContainer.find(selectors.messages);
                } else {
                    container = $(parentContainer).find(selectors.messages);
                }
                if (container.length === 0) {
                    parentContainer = parentContainer.parent();
                }
            } while (container.length === 0 && parentContainer.length !== 0);
        } else if (options.containerId) {
            container = $('#' + options.containerId).find(selectors.messages);
        } else if (options.messageBoxId) {
            container = $('#' + options.messageBoxId);
        } else {
            container = $(selectors.messages);
        }

        this.container = container;
    }

    /**
    * Messages instance methods.
    */
    MessagesBox.prototype = {
        addSuccessMessage: function (message) {
            this.addMessage(selectors.success, message);
            return this;
        },

        addInfoMessage: function (message) {
            this.addMessage(selectors.info, message);
            return this;
        },

        addWarningMessage: function (message) {
            this.addMessage(selectors.warn, message);
            return this;
        },

        addErrorMessage: function (message) {
            this.addMessage(selectors.error, message);
            return this;
        },

        clearMessages: function () {
            this.addMessage(selectors.success, null);
            this.addMessage(selectors.info, null);
            this.addMessage(selectors.warn, null);
            this.addMessage(selectors.error, null);
            return this;
        },

        addMessage: function (messageTypeSelector, message) {
            var options = this.options;

            var messagesOfTypeContainer = this.container.find(messageTypeSelector);

            if (message) {
                messagesOfTypeContainer.fadeIn();
                var element = $('<li>' + message + '</li>');
                messagesOfTypeContainer.append(element);

                if (options.enableAutoHide) {
                    setTimeout(function () {
                        element.animate({
                            'line-height': '1px',                            
                            opacity: 0.2
                        }, 350,
                            function () {
                                var parent = $(this).parent('ul:first');
                                $(this).remove();
                                if (parent.find('li').length === 0) {
                                    parent.fadeOut('fast', function() { $(this).hide(); });
                                }
                            });
                    }, options.autoHideTimeout);
                }
            } else {
                messagesOfTypeContainer.hide();
                messagesOfTypeContainer.empty();
            }
        }
    };

    messages.box = function (options) {
        var messagesBox = new MessagesBox(options);
        return messagesBox;
    };

    messages.refreshBox = function (container, json) {
        var messagesBox = new MessagesBox({
            container: container
        });

        if (json.Messages) {
            messagesBox.clearMessages();
            for (var i = 0; i < json.Messages.length; i++) {
                if (json.Success) {
                    messagesBox.addSuccessMessage(json.Messages[i]);
                } else {
                    messagesBox.addErrorMessage(json.Messages[i]);
                }
            }
        }

        return messagesBox;
    };

    messages.showMessages = function (json) {
        if (json.Messages) {
            var content = "";

            for (var i = 0; i < json.Messages.length; i++) {
                if (content) {
                    content += "<br />";
                }
                content += json.Messages[i];
            }

            modal.alert({
                content: content
            });
        }
    };

    return messages;
});
