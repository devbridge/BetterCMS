/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console, document */

define('bcms.tooltip', ['bcms.jquery', 'bcms'], function ($, bcms) {
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