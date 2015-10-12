/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms, document */

bettercms.define('bcms.modal', ['bcms.jquery', 'bcms', 'bcms.tabs', 'bcms.ko.extenders', 'bcms.forms'], function ($, bcms, tabs, ko, forms) {
    'use strict';

    var modal = {},
        modalStack = [],
        noScrollClass = 'bcms-noscroll',
        resizeTimer,

        selectors = {
            title: '.bcms-modal-title, .bcms-error-frame h4, .bcms-popinfo-frame h4',
            accept: '.bcms-modal-accept, .bcms-popinfo-frame .bcms-success-buttons-holder .bcms-btn-main',
            cancel: '.bcms-modal-cancel, .bcms-popinfo-frame .bcms-success-buttons-holder .bcms-btn-links-main',
            close: '.bcms-modal-close, .bcms-modal-cancel, .bcms-error-frame .bcms-error-close, .bcms-success-buttons-holder .bcms-btn-links-main, .bcms-popinfo-frame .bcms-btn-close, .bcms-preview-image-border .bcms-btn-close',
            focusElements: '[tabindex=-1], .bcms-modal-close, .bcms-modal-cancel, .bcms-error-frame .bcms-error-close, .bcms-success-buttons-holder .bcms-btn-links-main, .bcms-popinfo-frame .bcms-btn-close, .bcms-preview-image-border .bcms-btn-close',
            body: '.bcms-modal-body, .bcms-error-frame, .bcms-popinfo-frame',
            content: '.bcms-modal-content-padded, .bcms-error-frame p, .bcms-popinfo-frame p',
            loader: '.bcms-loader',
            scrollWindow: '.bcms-scroll-window',
            previewImage: '.bcms-preview-image-frame img',
            previewImageContainer: '.bcms-preview-image-border',
            previewFailure: '.bcms-grid-image-holder',
            footer: '.bcms-success-buttons-holder, .bcms-modal-footer',
            desirableStatus: '.bcms-content-desirable-status',
            popinfoFrame: '.bcms-popinfo-frame',
            errorFrame: '.bcms-error-frame',
            loaderContainer: '.bcms-modal-content',

            // selectors for calculation of modal window size
            elemOuter: '.bcms-modal-body',
            elemHeader: '.bcms-modal-header',
            elemFooter: '.bcms-modal-footer',
            elemTabsHeader: '.bcms-tab-header',
            elemContent: '.bcms-scroll-window',
            readonly: '[data-readonly=true]'
        },

        classes = {
            saveButton: 'bcms-btn-small bcms-modal-accept',
            cancelButton: 'bcms-btn-links-small bcms-modal-cancel',
            grayButton: 'bcms-btn-small bcms-btn-gray',
            inactive: 'bcms-inactive'
        },

        links = {},

        globalization = {
            save: null,
            cancel: null,
            ok: null,
            saveDraft: null,
            saveAndPublish: null,
            preview: null
        },

        isGlobalKeyPressEventAttached = false;

    /**
    /* Assign objects to module.
    */
    modal.selectors = selectors;
    modal.links = links;
    modal.globalization = globalization;
    modal.classes = classes;
    var isResized = false;
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
    * Button view model.
    */
    function ButtonViewModel(title, css, order, onClickCallback) {
        var self = this;

        self.dialog = null;
        self.order = order || 0;
        self.title = ko.observable(title || '');
        self.css = ko.observable(css || '');
        self.disabled = ko.observable(false);
        self.click = function () {
            if (onClickCallback && $.isFunction(onClickCallback)) {
                onClickCallback(self.dialog, self);
            }
        };
    }

    /**
    * Modal window view model.
    */
    function ModalWindowViewModel() {
        var self = this;
        self.buttons = ko.observableArray();
    }

    modal.button = ButtonViewModel;

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
            acceptCss: null,
            disableAccept: false,
            onAcceptClick: null,

            cancelTitle: null,
            cancelCss: null,
            disableCancel: false,
            onCloseClick: null,

            content: null,
            onLoad: null,
            onShow: null,
            onAccept: null,
            onClose: null,
            cssClass: null,
            disableAnimation: true,
            disableMaxHeight: false,
            autoFocus: true,
            buttons: []
        }, options);

        var template = $('#' + this.options.templateId).html(),
            model = new ModalWindowViewModel(this);

        this.container = $(template);
        this.model = model;
        
        if (bcms.language) {
            this.container.attr('lang', bcms.language);
        }

        /* Accept action button:*/
        var acceptButton = new ButtonViewModel();
        acceptButton.order = 1;

        if (this.options.acceptTitle) {
            acceptButton.title(this.options.acceptTitle);
        } else {
            acceptButton.title(globalization.save);
        }

        if (this.options.acceptCss) {
            acceptButton.css(this.options.acceptCss);
        } else {
            acceptButton.css(classes.saveButton);
        }

        if (this.options.disableAccept) {
            acceptButton.disabled(!!this.options.disableAccept);
        }

        this.disableAcceptButton = function () {
            acceptButton.disabled(true);
        };

        this.disableExtraButtons = function () {
            for (var j in options.buttons) {
                options.buttons[j].disabled(true);
            }
        };

        /* Cancel action button: */
        var cancelButton = new ButtonViewModel();
        cancelButton.order = 10;

        if (this.options.cancelTitle) {
            cancelButton.title(this.options.cancelTitle);
        } else {
            cancelButton.title(globalization.cancel);
        }

        if (this.options.cancelCss) {
            cancelButton.css(this.options.cancelCss);
        } else {
            cancelButton.css(classes.cancelButton);
        }

        if (this.options.disableCancel) {
            cancelButton.disabled(!!this.options.disableCancel);
        }

        /* Content: */
        if (this.options.title) {
            this.setTitle(this.options.title);
        }

        if (this.options.content) {
            this.setContent(this.options.content);
        }

        if (this.options.cssClass) {
            this.setCssClass(this.options.cssClass);
        }

        model.buttons.push(acceptButton);
        model.buttons.push(cancelButton);

        if (options.buttons && options.buttons.length > 0) {
            for (var i = 0; i < options.buttons.length; i++) {
                var button = options.buttons[i];
                button.dialog = this;
                if (!button.css()) {
                    button.css(classes.grayButton);
                }
                model.buttons.push(button);
            }
        }
        model.buttons.sort(function (left, right) {
            return left.order > right.order ? 1 : -1;
        });

        if ($.isFunction(this.options.onLoad)) {
            this.options.onLoad(this);
        }

        this.getLoaderContainer = function () {
            return this.container.find(selectors.loaderContainer);
        };
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
                model = instance.model,
                zindex = bcms.getHighestZindex() + 1,
                footerDom = container.find(selectors.footer).get(0);

            this.options.id = zindex;

            if (footerDom) {
                ko.applyBindings(model, footerDom);
            }

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
            if (this.container.find('form').data('readonly') !== true) {
                if (this.onAction(this.options.onAcceptClick) === true) {
                    return this.accept();
                }
                return false;
            }

            return false;
        },

        /*
        * Executes accept action logic.
        */
        accept: function () {
            if (this.onAction(this.options.onAccept) === true) {
                this.destroy();
                return true;
            }
            return false;
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
                this.changeFirstModalWindowSize();
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
        * Updates modal window content.
        */
        setContent: function (content) {
            try {
                this.container
                    .find(selectors.content)
                    .empty()
                    .append(content);
            } catch (exc) {
                throw exc;
            } finally {
                this.container.find(selectors.loader).remove();

                // Check for readonly mode.
                this.container.find(selectors.readonly).addClass(classes.inactive);
                var form = this.container.find('form');
                if (form.data('readonly') === true) {
                    forms.setFieldsReadOnly(form);

                    this.disableAcceptButton();
                    this.disableExtraButtons();
                }

                if ($.validator && $.validator.unobtrusive) {
                    $.validator.unobtrusive.parse(this.container);
                }

                this.maximizeHeight();

                tabs.initTabPanel(this.container);

                forms.bindCheckboxes(this.container);

                if (this.options.autoFocus) {
                    this.setFocus();
                }
            }
        },

        /**
        * Sets focus on the first visible input element or on the dialog close element.
        */
        setFocus: function () {
            if (this.container.find('form').data('readonly') === true) {
                return;
            }

            var focustElement = this.container.find('input:visible:first');

            if (focustElement.length === 0) {
                focustElement = this.container.find(selectors.focusElements).first();
            }

            if (focustElement.length > 0) {
                if (focustElement.is('input')) {
                    setTimeout(function () {
                        focustElement.focus();
                    }, 100);
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
                bcms.logger.error('Failed to execute action delegate. ' + ex.message);
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
                this.container.hide(200, function () {
                    $(this).remove();
                });
            }

            if (modal.getCount() < 1) {
                removeGlobalEvents();
            }
        },

        /**
        * Maximizes dialog height to document height.
        */
        maximizeHeight: function () {
            if (this.disableMaxHeight) {
                return;
            }

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
        },

        changeFirstModalWindowSize: function () {
            var dialog = modal.last();
            if (dialog && isResized) {
                dialog.maximizeHeight();
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
                    isResized = true;
                }
            }, 200);
        });
    };

    /**
    * Checks if current element can handle key press events.
    */
    function canHandleKeyPress() {
        var element = $(document.activeElement);
        return element.is('body') || element.is('div') || element.is('input') || element.is(selectors.close);
    }

    /**
    *  Binds to a document key press events.
    */
    function addGlobalKeyPressEvent() {
        var lastEnterModal,
            lastEscModal;
        $(document).on('keydown.bcms.modal', function (e) {
            var topModal = modal.last();
            if (topModal) {
                // If Esc pressed and cancel action is not disabled in the modal dialog.
                if (e.keyCode === 27 && !topModal.options.disableCancel) {
                    if (canHandleKeyPress()) {
                        e.preventDefault();
                        if (topModal != lastEscModal) {
                            lastEscModal = topModal;
                            topModal.closeClick();
                        }
                    }
                }
                    // If Enter pressed and accept action is not disabled in the modal dialog.
                else if (e.keyCode === 13 && !topModal.options.disableAccept) {
                    if (canHandleKeyPress()) {
                        e.preventDefault();
                        if (topModal != lastEnterModal) {
                            if (topModal.acceptClick()) {
                                lastEnterModal = topModal;
                            }
                        }
                    }
                }
            }
        });
    }

    /**
    * Removes global modal window events if no modals exists.
    */
    function removeGlobalEvents() {
        isGlobalKeyPressEventAttached = false;
        $(document).off('.bcms.modal');
    }

    /**
    * Creates and opens modal windows
    */
    modal.open = function (options) {
        var modalWindow = new ModalWindow(options);
        modalWindow.open();

        if (!isGlobalKeyPressEventAttached) {
            addGlobalKeyPressEvent();
            addGlobalResizeModalEvent();
            isGlobalKeyPressEventAttached = true;
        }

        return modalWindow;
    };

    modal.edit = function (options) {
        options = $.extend({
            isPreviewAvailable: true,
            onSaveAndPublishClick: null,
            onPreviewClick: null,
            disableSaveDraft: false,
            disableSaveAndPublish: false,
            onSaveDraftClick: null
        }, options);

        var extraButtons = [],
            changeContentDesirableStatus = function (dialog, status) {
                var desirableStatus = dialog.container.find(selectors.desirableStatus);
                if (desirableStatus.length > 0) {
                    desirableStatus.val(status);
                } else {
                    throw new Error($.format('Dialog {0} should contain hidden input for a desirable status.', dialog.title));
                }
            },
            saveAndPublishAction = function (dialog) {
                if ($.isFunction(options.onSaveAndPublishClick)) {
                    if (options.onSaveAndPublishClick(dialog) !== false) {
                        changeContentDesirableStatus(dialog, bcms.contentStatus.published);
                        dialog.submitForm();
                    }
                } else {
                    changeContentDesirableStatus(dialog, bcms.contentStatus.published);
                    dialog.submitForm();
                }
            },
            saveDraftAction = function (dialog) {
                if ($.isFunction(options.onSaveDraftClick)) {
                    if (options.onSaveDraftClick(dialog) !== false) {
                        changeContentDesirableStatus(dialog, bcms.contentStatus.draft);
                        dialog.submitForm();
                    }
                } else {
                    changeContentDesirableStatus(dialog, bcms.contentStatus.draft);
                    dialog.submitForm();
                }
            };

        if (!options.disableSaveDraft && !options.disableSaveAndPublish) {
            var saveAndPublishButton = new ButtonViewModel(globalization.saveAndPublish, classes.grayButton, 2, saveAndPublishAction);
            extraButtons.push(saveAndPublishButton);
        }

        if (!!options.isPreviewAvailable) {
            var previewButton = new ButtonViewModel(globalization.preview, classes.grayButton, 3, function (dialog) {
                if ($.isFunction(options.onPreviewClick)) {
                    if (options.onPreviewClick(dialog) !== false) {
                        changeContentDesirableStatus(dialog, bcms.contentStatus.preview);
                        dialog.submitForm();
                    }
                } else {
                    changeContentDesirableStatus(dialog, bcms.contentStatus.preview);
                    dialog.submitForm();
                }
            });
            extraButtons.push(previewButton);
        }

        options.buttons = extraButtons;


        if (options.disableSaveDraft && !options.disableSaveAndPublish) {
            options.acceptTitle = globalization.saveAndPublish;
            options.onAcceptClick = saveAndPublishAction;
        } else if (!options.disableSaveDraft) {
            options.acceptTitle = globalization.saveDraft;
            options.onAcceptClick = saveDraftAction;
        } else {
            options.disableAccept = true;
        }

        modal.open(options);
    };

    modal.alert = function (options) {
        options = $.extend({
            disableAccept: true
        }, options);

        options.templateId = 'bcms-modal-alert-template';
        options.disableAnimation = true;

        var dialog = modal.open(options);
        // Hack for IE.
        dialog.container.find(selectors.errorFrame).get(0).focus();
        return dialog;
    };

    modal.confirm = function (options) {
        options = $.extend({
            acceptTitle: globalization.ok,
            cancelTitle: globalization.cancel
        }, options);

        options.templateId = 'bcms-modal-confirm-template';
        options.disableAnimation = true;

        var dialog = modal.open(options);
        // Hack for IE.
        dialog.container.find(selectors.popinfoFrame).get(0).focus();
        return dialog;
    };

    modal.confirmWithDecline = function (options) {
        options = $.extend({
            acceptTitle: globalization.yes,
            cancelTitle: globalization.cancel,
            declineTitle: globalization.no,
            onDecline: function () { return true; },
        }, options);
        if (!$.isArray(options.buttons)) {
            options.buttons = [];
        }
        options.buttons.push(new ButtonViewModel(options.declineTitle, classes.grayButton, 2, function (confirmDialog) {
            var canClose = true;
            
            if ($.isFunction(options.onDecline)) {
                canClose = options.onDecline();
            }
            
            if (canClose) {
                confirmDialog.destroy();
            }
        }));

        options.templateId = 'bcms-modal-confirm-template';
        options.disableAnimation = true;

        var dialog = modal.open(options);
        // Hack for IE.
        dialog.container.find(selectors.popinfoFrame).get(0).focus();
        return dialog;
    };

    modal.info = function (options) {
        options = $.extend({
            acceptTitle: globalization.ok
        }, options);

        options.templateId = 'bcms-modal-info-template';
        options.disableAnimation = true;
        options.disableCancel = true;

        var dialog = modal.open(options);
        // Hack for IE.
        dialog.container.find(selectors.popinfoFrame).get(0).focus();
        return dialog;
    };

    modal.imagePreview = function (src, alt, options) {
        options = $.extend({}, options);

        options.templateId = 'bcms-image-preview-template';
        options.disableAnimation = true;

        var dialog = modal.open(options),
            img = dialog.container.find(selectors.previewImage),
            imgLoaded = false;

        img.attr('alt', alt || '');

        img.on('load', function () {
            imgLoaded = true;

            var imgContainer = dialog.container.find(selectors.previewImageContainer),
                width = img.width(),
                visibleWidth = $(window).width() - 150,
                margin,
                previewFailure = imgContainer.find(selectors.previewFailure);

            previewFailure.hide();

            if (width > visibleWidth) {
                width = visibleWidth;
            }

            imgContainer.css('width', width + 'px');
            img.css('width', '100%');

            margin = (width + 50) / -2;
            imgContainer.css('margin-left', margin + 'px');

            imgContainer.find(selectors.loader).hide();
            img.show();
        });

        img.on('error', function () {
            // IE && other browsers compatibility fix: checking if image is not loaded yet
            if (!imgLoaded) {
                var imgContainer = dialog.container.find(selectors.previewImageContainer),
                    previewFailure = imgContainer.find(selectors.previewFailure);

                imgContainer.find(selectors.loader).hide();
                previewFailure.show();
            }
        });
        
        img.attr('src', src + (src.indexOf('?') != -1 ? '&' : '?') + (new Date()).getTime());

        return dialog;
    };

    modal.showMessages = function (json) {
        if (json.Messages) {
            var content = "";

            for (var i = 0; i < json.Messages.length; i++) {
                if (content) {
                    content += "<br />";
                }
                content += json.Messages[i];
            }

            if (json.Success === true) {
                modal.info({
                    content: content,
                    disableAccept: true
                });
            } else {
                modal.alert({
                    content: content
                });
            }
        }
    };

    /**
    * Maximizes dialog's provided child/children height up to maximum visible value.
    */
    modal.maximizeChildHeight = function (obj, dialog) {
        if (obj.length === 0) {
            return 0;
        }

        var objects = $.isArray(obj) ? obj : new Array(obj),
            contentContainer = dialog.container.find(selectors.scrollWindow).first(),
            containerHeight = contentContainer.outerHeight(),
            objectsHeight = 0,
            childrenHeight = 0,
            newHeight = 0,
            addHeight;

        $.each(objects, function() {
            objectsHeight += $(this).outerHeight();
            newHeight += $(this).height();
        });

        if (contentContainer.length === 0 || objectsHeight >= containerHeight) {
            return newHeight;
        }

        $.each(contentContainer.children(), function () {
            var child = $(this);
//            console.log(child);
//            console.log("Height: %s, isVisible: %s", child.outerHeight(), child.is(":visible"));

            childrenHeight += child.outerHeight();
        });
        // TODO: find a better solution for icon toolbars calculation!
        childrenHeight += objects.length * 60;

        addHeight = containerHeight - childrenHeight;
//        console.log('Container: %s, Objects(%s): %s, Other objects: %s, All children height: %s, Available height: %s',
//            containerHeight, objects.length, objectsHeight, childrenHeight - objectsHeight, childrenHeight, addHeight);

        if (objects.length > 1) {
            addHeight = Math.floor(addHeight / objects.length);
            //console.log('Available height for each object: %s', addHeight);
        }

        newHeight = 0;
        if (addHeight > 0) {
            $.each(objects, function () {
                var newObjHeight = $(this).height() + addHeight;
                newHeight += newObjHeight;
               
                //console.log("Changing textarea's height from %s to %s", $(this).height(), newObjHeight);
                $(this).height(newObjHeight + 'px');
            });
        }

        return newHeight;
    };

    return modal;

});
