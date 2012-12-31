/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console, document */

define('bcms.modal', ['jquery', 'bcms', 'bcms.tabs'], function ($, bcms, tabs) {
    'use strict';

    var modal = {},
        modalStack = [],
        noScrollClass = 'bcms-noscroll',
        resizeTimer,

        selectors = {
            title: '.bcms-modal-title, .bcms-error-frame h4, .bcms-popinfo-frame h4',
            accept: '.bcms-modal-accept, .bcms-popinfo-frame .bcms-success-buttons-holder .bcms-btn-main',
            cancel: '.bcms-modal-cancel, .bcms-popinfo-frame .bcms-success-buttons-holder .bcms-btn-links-main',
            close: '.bcms-modal-close, .bcms-modal-cancel, .bcms-error-frame .bcms-error-close, .bcms-success-buttons-holder .bcms-btn-links-main, .bcms-popinfo-frame .bcms-btn-close',
            body: '.bcms-modal-body, .bcms-error-frame, .bcms-popinfo-frame',
            content: '.bcms-modal-content-padded, .bcms-error-frame p, .bcms-popinfo-frame p',
            scrollWindow: '.bcms-scroll-window',
            loader: '.bcms-loader',

            // selectors for calculation of modal window size
            elemOuter: '.bcms-modal-body',
            elemHeader: '.bcms-modal-header',
            elemFooter: '.bcms-modal-footer',
            elemTabsHeader: '.bcms-tab-header',
            elemContent: '.bcms-scroll-window'
        },

        links = {},

        globalization = {
        };

    /**
    /* Assign objects to module.
    */
    modal.selectors = selectors;
    modal.links = links;
    modal.globalization = globalization;

    /**
    * Returns count of currently open modal windows.
    */
    modal.getCount = function () {
        return modalStack.length;
    };

    /**
    * Returns last modal window instance of stack.
    */
    modal.last = function () {
        if (modalStack.length > 0) {
            return modalStack[modalStack.length - 1];
        }
        return null;
    };

    /**
    * ModalWindow instance constructor:
    */
    // ReSharper disable InconsistentNaming
    function ModalWindow(options) {
        // ReSharper restore InconsistentNaming        

        this.options = $.extend({
            templateId: 'bcms-modal-template',
            title: null,
            acceptTitle: null,
            cancelTitle: null,
            content: null,
            onLoad: null,
            onShow: null,
            onAcceptClick: null,
            onAccept: null,
            onCloseClick: null,
            onClose: null,
            cssClass: null,
            disableAnimation: true,
            disableMaxHeight: false,
            disableAccept: false,
            disableCancel: false,
            autoFocus: true
        }, options);

        var template = $('#' + this.options.templateId).html();

        this.container = $(template);

        if (this.options.title) {
            this.setTitle(options.title);
        }

        if (this.options.acceptTitle) {
            this.setAcceptTitle(options.acceptTitle);
        }

        if (this.options.cancelTitle) {
            this.setCancelTitle(options.cancelTitle);
        }

        if (this.options.content) {
            this.setContent(options.content);
        }

        if (this.options.cssClass) {
            this.setCssClass(options.cssClass);
        }

        if (this.options.disableAccept) {
            this.disableAccept();
        }

        if (this.options.disableCancel) {
            this.disableCancel();
        }

        if ($.isFunction(this.options.onLoad)) {
            this.options.onLoad(this);
        }
    }

    /**
    * ModalWindow instance methods:
    */
    ModalWindow.prototype = {
        /**
        * Open modal window over all other windows.
        */
        open: function (disableAnimation) {
            var instance = this,
                container = instance.container,
                zindex = bcms.getHighestZindex() + 1;

            this.options.id = zindex;

            modalStack.push(this);

            container.find(selectors.close).on('click', function () {
                instance.closeClick();
            });

            container.find(selectors.accept).on('click', function () {
                instance.acceptClick();
            });

            $('html').addClass(noScrollClass);

            container.hide().css('z-index', zindex).appendTo('body');

            if (disableAnimation || this.options.disableAnimation) {
                container.show();
            } else {                
                container.fadeIn('fast');
            }

            this.maximizeHeight();

            if ($.isFunction(this.options.onShow)) {
                this.options.onShow();
            }
        },

        /*
        * Executes accept button click logic.
        */
        acceptClick: function () {
            if (this.onAction(this.options.onAcceptClick) === true) {
                this.accept();
            }
        },

        /*
        * Executes accept action logic.
        */
        accept: function () {
            if (this.onAction(this.options.onAccept) === true) {
                this.destroy();
            }
        },

        /*
        * Triggers close button click logic.
        */
        closeClick: function () {
            if (this.onAction(this.options.onCloseClick) === true) {
                this.close();
            }
        },

        /**
        * Executes cancel action logic.
        */
        close: function () {
            if (this.onAction(this.options.onClose) === true) {
                this.destroy();
            }
        },

        /**
        * Updates modal window title.
        */
        setTitle: function (title) {
            this.title = title;
            this.container.find(selectors.title).empty().append(title);
        },

        /**
        * Updates modal window accept button title.
        */
        setAcceptTitle: function (title) {
            this.acceptTitle = title;
            this.container.find(selectors.accept).empty().append(title);
        },

        /**
        * Updates modal window accept button title.
        */
        setCancelTitle: function (title) {
            this.cancelTitle = title;
            this.container.find(selectors.cancel).empty().append(title);
        },

        /**
        * Updates modal window content.
        */
        setContent: function (content) {
            this.content = content;
            this.container
                .find(selectors.content)
                .empty()
                .append(content);

            this.container.find(selectors.loader).remove();

            if ($.validator && $.validator.unobtrusive) {
                $.validator.unobtrusive.parse(this.container);
            }

            this.maximizeHeight();

            tabs.initTabPanel(this.container);

            if (this.options.autoFocus) {
                this.setFocus();
            }
        },

        /**
        * Sets focus on the first visible input element or on the dialog close element.
        */
        setFocus: function () {
            var focustElement = this.container.find('input:visible:first');

            if (focustElement.length === 0) {
                focustElement = this.container.find(selectors.close).first();
            }

            if (focustElement.length > 0) {
                if (focustElement.is('input')) {
                    setTimeout(function () {
                        focustElement.focus();
                    }, 750);
                } else {
                    focustElement.focus();
                }
            }
        },

        /**
        * Sets modal dialog CSS class.
        */
        setCssClass: function (cssClass) {
            var body = this.container.find(selectors.body);

            if (this.cssClass) {
                body.removeClass(this.cssClass);
            }

            this.cssClass = cssClass;
            body.addClass(this.cssClass);
        },

        /**
        * Executes given action.
        */
        onAction: function (actionDelegate) {
            try {
                if ($.isFunction(actionDelegate)) {
                    return actionDelegate(this) !== false;
                }
            } catch (ex) {
                console.log('Failed to execute action delegate. ' + ex.message);
                return false;
            }

            return true;
        },

        /**
        * Removes dialog from DOM.
        */
        destroy: function () {
            this.container.find(selectors.accept).off('click');
            this.container.find(selectors.close).off('click');

            var index = $.inArray(this, modalStack);
            if (index !== -1) {
                modalStack.splice(index, 1);
            }

            if (modalStack.length === 0) {
                $('html').removeClass(noScrollClass);
            }

            if (this.options.disableAnimation) {
                this.container.hide().remove();
            } else {
                this.container.hide(200, function() {
                    $(this).remove();
                });
            }
        },

        /**
        * Disables accept function on this dialog.
        */
        disableAccept: function () {
            this.container.find(selectors.accept).off('click').hide();
        },

        /**
        * Disables cancel function on this dialog.
        */
        disableCancel: function () {
            this.container.find(selectors.close).off('click').hide();
        },

        /**
        * Maximizes dialog height to document height.
        */
        maximizeHeight: function () {
            if (this.disableMaxHeight) {
                return;
            }

            console.log("Maximize height for " + this.title + " modal window.");

            var viewportHeight = $(window).height(),
                elemOuter = this.container.find(selectors.elemOuter),
                elemHeader = this.container.find(selectors.elemHeader),
                elemFooter = this.container.find(selectors.elemFooter),
                elemTabsHeader = this.container.find(selectors.elemTabsHeader),
                elemContent = this.container.find(selectors.elemContent);

            elemOuter.css({ 'height': viewportHeight + 'px' });

            if (elemTabsHeader) {
                elemContent.css({ 'height': (viewportHeight - (elemHeader.outerHeight() + elemFooter.outerHeight() + elemTabsHeader.outerHeight())) + 'px' });
            } else {
                elemContent.css({ 'height': (viewportHeight - (elemHeader.outerHeight() + elemFooter.outerHeight())) + 'px' });
            }
        }
    };

    /**
    * Binds to a window resize event.
    */
    function addGlobalResizeModalEvent() {
        $(window).on('resize.bcms.modal', function () {
            clearTimeout(resizeTimer);

            resizeTimer = setTimeout(function () {
                var topModal = modal.last();
                if (topModal) {
                    topModal.maximizeHeight();
                }

                if (modal.getCount() < 1) {
                    removeGlobalEvents();
                }
            }, 200);
        });
    };

    /**
    * Checks if current element can handle key press events.
    */
    function canHandleKeyPress() {
        var element = $(document.activeElement);
        return element.is('body') || element.is('div') || element.is('input') || element.is('select') || element.is(selectors.close);
    }

    /**
    *  Binds to a document key press events.
    */
    function addGlobalKeyPressEvent() {
        $(document).on('keydown.bcms.modal', function (e) {
            var topModal = modal.last();
            if (topModal) {
                // If Esc pressed and cancel action is not disabled in the modal dialog.
                if (e.keyCode === 27 && !topModal.options.disableCancel) {
                    if (canHandleKeyPress()) {
                        e.preventDefault();
                        topModal.closeClick();
                    }
                }
                    // If Enter pressed and accept action is not disabled in the modal dialog.
                else if (e.keyCode === 13 && !topModal.options.disableAccept) {
                    if (canHandleKeyPress()) {
                        e.preventDefault();
                        topModal.acceptClick();
                    }
                }
            }

            if (modal.getCount() < 1) {
                removeGlobalEvents();
            }
        });
    }

    /**
    * Removes global modal window events if no modals exists.
    */
    function removeGlobalEvents() {
        if (modal.getCount() === 0) {
            $(document).off('.bcms.modal');
        }
    }

    /**
    * Creates and opens modal windows
    */
    modal.open = function (options) {
        var modalWindow = new ModalWindow(options);
        modalWindow.open();

        if (modal.getCount() === 1) {
            addGlobalKeyPressEvent();
            addGlobalResizeModalEvent();
        }

        return modalWindow;
    };

    modal.alert = function (options) {
        options = $.extend({
            disableAccept: true
        }, options);

        options.templateId = 'bcms-modal-alert-template';
        options.disableAnimation = true;

        return modal.open(options);
    };

    modal.confirm = function (options) {
        options = $.extend({}, options);

        options.templateId = 'bcms-modal-confirm-template';
        options.disableAnimation = true;

        return modal.open(options);
    };
    
    modal.info = function (options) {
        options = $.extend({}, options);

        options.templateId = 'bcms-modal-info-template';
        options.disableAnimation = true;
        options.disableCancel = true;
        return modal.open(options);
    };

    return modal;

});
