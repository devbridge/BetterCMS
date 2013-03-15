/*global define, console */

define('bcms', ['bcms.jquery'], function ($) {
    'use strict';

    var app = {},
        callbacks = [],
        initialized = false,

    // Selectors used in the module to locate DOM elements:
        selectors = {
            zIndexContainers: 'body div:not(div div)'
        },

        events = {
            editModeOff: 'editModeOff',
            editModeOn: 'editModeOn',
            addPageContent: 'addPageContent',
            sortPageContent: 'sortPageContent',
            createContentOverlay: 'createContentOverlay',
            pageCreated: 'pageCreated',
        },

        eventListeners = {},

        contentStatus = {
             published: 3,
             draft: 2,
             preview: 1
         },
    
        errorTrace = !!true;

    /**
    * Exposes reference to events:
    */
    app.events = events;

    /**
    * Current page id.
    */
    app.pageId = null;

    /**
    * Indicates if error trace output is enabled.
    */
    app.errorTrace = errorTrace;

    /**
    * Contains available content statuses.
    */
    app.contentStatus = contentStatus;
    
    /**
    */
    app.previewWindow = '__bcmsPreview';
    
    /**
    * Indicates if edit mode is ON:
    */
    app.editModeIsOn = function () {
        return $('html').hasClass('bcms-on');
    };

    /**
    * Registers callback that would be executed during init
    */
    app.registerInit = function (callback) {
        callbacks.push(callback);
    };

    /**
    * Initializes CMS when it loads for the first time
    */
    app.init = function () {

        if (initialized) {
            return;
        }

        initialized = true;

        console.log('Initializing better CMS');

        $.each(callbacks, function (i, fn) {
            fn();
        });

        callbacks = [];
    };

    /**
    * Add an event handler
    */
    app.on = function (event, fn) {
        if (typeof event !== 'string') {
            console.error('Event must be type of string');
            return;
        }

        if (!$.isFunction(fn)) {
            console.error('Event callback must be type of function');
            return;
        }

        (eventListeners[event] = eventListeners[event] || []).push(fn);
    };

    /**
    * Remove an event handler
    */
    app.off = function (event, fn) {
        var listeners = (eventListeners[event] = eventListeners[event] || []),
            index = $.inArray(fn, listeners);

        // Remove specified listener if found:
        if (index !== -1) {
            listeners.splice(index, 1);
        }

        // If no function was passed remove all event listeners:
        if (!fn) {
            eventListeners[event] = [];
        }
    };

    /**
    * Trigger an event. All registered handlers will be executed.
    */
    app.trigger = function (event, data) {
        var countSuccess = 0;
        $.each(eventListeners[event] || [], function (i, fn) {
            try {
                if (data) {
                    fn(data);
                } else {
                    fn();
                }
                countSuccess++;
            } catch (e) {
                $.error(e.message);
            }
        });
        return countSuccess;
    };

    /**
    * Redirect document to a given location.
    */
    app.redirect = function (url) {
        document.location.href = url;
    };

    /**
    * Reloads document.
    */
    app.reload = function () {
        location.reload(true);
    };

    /**
    * Returns highest zIndex of body children.
    */
    app.getHighestZindex = function () {
        var indexHighest = 0;

        $(selectors.zIndexContainers).each(function () {
            if (!$(this).hasClass('cke_panel')) {   //ckeditor bug fix
                var indexCurrent = parseInt($(this).css("z-index"), 10);
                if (indexCurrent > indexHighest) {
                    indexHighest = indexCurrent;
                }
            }
        });

        return indexHighest;
    };

    /**
    * Prevents input element from submitting form via Enter key.
    */
    app.preventInputFromSubmittingForm = function (inputElement, options) {
        options = $.extend({
            preventedEnter: null,
            preventedEsc: null
        }, options);

        $(inputElement).on('keydown', function (e) {
            e = e || event;
            var code = e.keyCode || event.which || event.charCode || 0;
            if (code === 13 || code === 27) {
                e.returnValue = false;
                if (e.stopPropagation) {
                    e.stopPropagation();
                    e.preventDefault();
                }

                if (code === 13 && $.isFunction(options.preventedEnter)) {
                    options.preventedEnter($(this));
                } else if (code === 27 && $.isFunction(options.preventedEsc)) {
                    options.preventedEsc($(this));
                }

                return false;
            }

            return true;
        });
    };

    app.parseFailedResponse = function (response) {
        var success = 200,
            contentType = response.getResponseHeader('Content-Type'),
            isJson = contentType.indexOf('application/json') !== -1,
            message;

        // If response status is success and content type is not JSON
        // assume that it was redirected to login page:
        message = response.status === success && !isJson
            ? 'Your session has expired. Please login to continue.'
            : 'Failed to process request. Response status: ' + response.status + ' ' + response.statusText;

        return {
            Success: false,
            Messages: [message]
        };
    };

    /**
    * Helper method for JavaScript classes inheritance
    */
    app.extendsClass = function(d, b) {

        function __() { this.constructor = d; }

        __.prototype = b.prototype;
        d.prototype = new __();
    };

    /**
    * Stops specified event propagation
    */
    app.stopEventPropagation = function (event) {
        if (event != null) {
            event.returnValue = false;
            if (event.stopPropagation) {
                event.stopPropagation();
                event.preventDefault();
            }
        }
    };

    /**
    * Recreates form's uobtrusive validator
    */
    app.updateFormValidator = function(form) {
        if (form && $.validator && $.validator.unobtrusive) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }
    };

    return app;
});