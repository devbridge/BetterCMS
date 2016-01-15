/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.tooltip.js" company="Devbridge Group LLC">
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

bettercms.define('bcms.tooltip', ['bcms.jquery', 'bcms'], function ($, bcms) {
    'use strict';

    var tooltip = {},
        idMarker = 0,

    // Selectors used in the module to locate DOM elements.
        selectors = {
            message: 'p',
            close: '.bcms-tip-close',
            arrowContainer: '.bcms-tooltip-box'
        },

        arrowClasses = {
            topLeft: 'bcms-tooltip-box bcms-tooltip-tl',
            topRight: 'bcms-tooltip-box bcms-tooltip-tr',
            bottomLeft: 'bcms-tooltip-box bcms-tooltip-bl',
            bottomRight: 'bcms-tooltip-box bcms-tooltip-br'
        };

    tooltip.selectors = selectors;

    /**
    * Tooltip instance constructor:
    */
    function Tooltip(options) {

        var template = $('#bcms-tooltip-template').html(),
            container = $(template);

        this.options = $.extend({
            message: null,
            elementPositioning: null
        }, options);

        this.container = container;

        if (options.message) {
            this.setMessage(options.message);
        }
    }

    /**
    * Tooltip instance methods:
    */
    Tooltip.prototype = {

        create: function (insertTag) {
            var instance = this,
                container = instance.container,
                zindex = bcms.getHighestZindex() + 1;

            this.options.id = 'bcms-tooltip-' + idMarker++;

            container.find(selectors.close).on('click', function () {
                instance.hide();
            });
            
            $(document.body).bind('click', function (e) {
                if ($(e.target).closest(selectors.arrowContainer).length == 0) {
                    instance.hide();
                }
            });

            container
                .hide()
                .css('z-index', zindex)
                .attr('id', this.options.id)
                .insertAfter(insertTag);

            instance.handlePosition(insertTag);

            container.fadeIn('fast');
        },

        handlePosition: function (insertTag) {
            var element = this.options.elementPositioning,
                container = this.container,
                arrowContainer = this.container.find(selectors.arrowContainer),
                actualHeight = container.height(),
                scrollOffset = $(insertTag).scrollParent().offset() || { top: 0, left: 0 },
                top = element.position.top,
                left = element.position.left - element.width - 8,

                offset = insertTag.offset();

            arrowContainer.removeClass();

            if (offset.top - 15 - scrollOffset.top < actualHeight) {
                top = element.position.top + element.height + 15;
                arrowContainer.addClass(arrowClasses.topLeft);
            } else {
                top = element.position.top - actualHeight - element.height / 2 - 10;
                arrowContainer.addClass(arrowClasses.bottomLeft);
            }

            container.css({ top: top, left: left });
        },

        setMessage: function (text) {
            this.message = text;
            this.container.find(selectors.message).empty().append(text);
        },


        hide: function () {
            this.container.hide();
        },

        destroy: function () {
            this.container.find(selectors.close).off('click');
            this.container.remove();
        }
    };

    tooltip.hideVisible = function () {
        $(".bcms-tooltip-holder").hide();
    };

    tooltip.create = function (insertAfter, options) {
        var message = new Tooltip(options);
        message.create(insertAfter);
        return message;
    };

    tooltip.initialize = function () {
        $(document).on('click.bcms.tooltip', '.bcms-tooltip-mark', function () {
            var element = $(this),
                ref = element.data('ref'),
                popup;

            if (ref) {
                popup = $('#' + ref);
                if (popup.length > 0 && popup.is(':hidden')) {
                    tooltip.hideVisible();
                    popup.fadeIn('fast');
                } else {
                    popup.hide();
                }
            } else {
                var message = element.data('message');

                if (message) {
                    tooltip.hideVisible();
                    popup = tooltip.create(
                        element,
                        {
                            message: message,
                            elementPositioning:
                                {
                                    position: element.position(),
                                    width: element.width(),
                                    height: element.height()
                                }
                        }
                    );

                    element.data("ref", popup.options.id);
                }
            }
        });
    };

    bcms.registerInit(tooltip.initialize);

    return tooltip;
});