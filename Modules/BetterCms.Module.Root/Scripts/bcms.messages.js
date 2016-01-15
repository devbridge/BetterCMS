/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.messages.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

bettercms.define('bcms.messages', ['bcms.jquery'], function ($) {
    'use strict';

    var messages = {},

    // Classes that are used to maintain various UI states:
        classes = {
        },

    // Selectors used in the module to locate DOM elements:
        selectors = {
            messagesBox: '#bcms-messages-box',
            messages: '.bcms-js-messages:not(.bcms-custom-messages):first',
            success: '.bcms-success-messages:first',
            info: '.bcms-info-messages:first',
            warn: '.bcms-warning-messages:first',
            error: '.bcms-error-messages:first',
            scrollable: '.bcms-scroll-window'
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
                    container = parentContainer.find(selectors.messages).first();
                } else {
                    container = $(parentContainer).find(selectors.messages).first();
                }
                if (container.length === 0) {
                    parentContainer = parentContainer.parent();
                }
            } while (container.length === 0 && parentContainer.length !== 0);
        } else if (options.containerId) {
            container = $('#' + options.containerId).find(selectors.messages).first();
        } else if (options.messageBoxId) {
            container = $('#' + options.messageBoxId);
        } else {
            container = $(selectors.messages).first();
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
                var closeButton = '<a class="bcms-messages-close bcms-js-btn-close">Close</a>';
                var element = $('<li>' + closeButton + message + '</li>');
                element.find('.bcms-js-btn-close').on('click', function () {
                    messagesOfTypeContainer.hide();
                });
                messagesOfTypeContainer.append(element);

                if (options.enableAutoHide) {
                    setTimeout(function () {
                        element.animate({
                            'line-height': '1px'
                        }, 200,
                            function () {
                                var parent = $(this).parent('ul:first');
                                $(this).remove();
                                if (parent.find('li').length === 0) {
                                    parent.fadeOut(100, function () { $(this).hide(); });
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

        messagesBox.clearMessages();
        
        if (json.Messages) {
            var i = 0;
            for (i = 0; i < json.Messages.length; i++) {
                if (json.Success) {
                    messagesBox.addSuccessMessage(json.Messages[i]);
                } else {
                    messagesBox.addErrorMessage(json.Messages[i]);
                }
            }
            if (i > 0) {
                $(messagesBox.container).closest(selectors.scrollable).scrollTop(0);
            }
        }

        return messagesBox;
    };

    return messages;
});
