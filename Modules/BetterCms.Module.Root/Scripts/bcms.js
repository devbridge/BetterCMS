/*global bettercms */

bettercms.define('bcms', ['bcms.jquery','bcms.store'], function ($, store) {
    'use strict';

    var app = {
        logger: null
    },
        callbacks = [],
        initialized = false,
        // Selectors used in the module to locate DOM elements:
        selectors = {
            zIndexLayers: '.bcms-layer, .cke_maximized',
            browserInfo: '#bcms-browser-info',
            browserInfoClose: '.bcms-msg-message-close'
        },
        events = {
            editModeOff: 'editModeOff',
            editModeOn: 'editModeOn',
            addPageContent: 'addPageContent',
            sortPageContent: 'sortPageContent',
            contentModelCreated: 'contentModelCreated',
            pageCreated: 'pageCreated',
            editContentsTree: 'editContentsTree',
            insertWidget: 'insertWidget',
            bodyClick: 'bodyClick'
        },
        eventListeners = {},
        contentStatus = {
            published: 3,
            draft: 2,
            preview: 1
        },
        globalization = {
            sessionHasExpired: null,
            failedToProcessRequest: null
        },
        keys = {
            loggerLevel: 'bcms.loggerLevel',
        },
        constants = {
            emptyGuid: '00000000-0000-0000-0000-000000000000',
            loginRedirectHeader: 'Bcms-Redirect-To'
        },
        errorTrace = !!true;

    /**
    * Exposes reference to events:
    */
    app.events = events;

    /**
    * Exposes reference to globalization:
    */
    app.globalization = globalization;

    /**
    * Exposes reference to constants:
    */
    app.constants = constants;

    /**
    * Current page id.
    */
    app.pageId = null;

    /**
    * Current page language.
    */
    app.language = null;

    /**
    * Indicates if error trace output is enabled.
    */
    app.errorTrace = errorTrace;

    /**
    * Contains available content statuses.
    */
    app.contentStatus = contentStatus;

    app.previewWindow = '__bcmsPreview';

    /**
    * Model for logging messages to console
    */
    function loggerModel() {
        var self = this;

        self.levels = {
            off: -1,
            fatal: 10,
            error: 20,
            warn: 30,
            info: 40,
            debug: 50,
            trace: 60
        };

        self.info = function (message) {
            self.log(message, self.levels.info);
        };

        self.warn = function (message) {
            self.log(message, self.levels.warn);
        };

        self.debug = function (message) {
            self.log(message, self.levels.debug);
        };

        self.trace = function (message) {
            self.log(message, self.levels.trace);
        };

        self.fatal = function (message) {
            self.log(message, self.levels.fatal);
        };

        self.error = function (message) {
            self.log(message, self.levels.error);
        };

        self.log = function (message, level) {
            var maxLevel = self.getMaxLevel();
            if (level <= maxLevel) {
                if (level <= self.levels.error && $.isFunction(console.error)) {
                    console.error(message);
                } else if (level > self.levels.error && level <= self.levels.warn && $.isFunction(console.warn)) {
                    console.warn(message);
                } else if (level > self.levels.warn && level <= self.levels.info && $.isFunction(console.info)) {
                    console.info(message);
                } else {
                    console.log(message);
                }
            }
        };

        self.getMaxLevel = function () {
            var level = store.get(keys.loggerLevel);

            if (!level) {
                level = self.levels.info;
                store.set(keys.loggerLevel, level);
            }

            return level;
        };

        return self;
    }

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

        app.logger.debug('Initializing bcms.js');

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
            app.logger.error('Event must be type of string');
            return;
        }

        if (!$.isFunction(fn)) {
            app.logger.error('Event callback must be type of function');
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

        $(selectors.zIndexLayers).each(function () {
            var indexCurrent = parseInt($(this).css("z-index"), 10);
            if (indexCurrent > indexHighest) {
                indexHighest = indexCurrent;
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
            if ((code === 13 && $.isFunction(options.preventedEnter)) || (code === 27 && $.isFunction(options.preventedEsc))) {
                e.returnValue = false;
                if (e.stopPropagation) {
                    e.stopPropagation();
                    e.preventDefault();
                }

                if (code === 13 && $.isFunction(options.preventedEnter)) {
                    options.preventedEnter($(this), e);
                } else if (code === 27 && $.isFunction(options.preventedEsc)) {
                    options.preventedEsc($(this), e);
                }

                return false;
            }

            return true;
        });
    };

    app.parseFailedResponse = function (response) {
        var success = 200,
            contentType = response.getResponseHeader('Content-Type'),
            isJson = contentType && contentType.indexOf('application/json') !== -1,
            message;

        // If response status is success and content type is not JSON
        // assume that it was redirected to login page:
        message = response.status === success && !isJson
            ? globalization.sessionHasExpired
            : $.format(globalization.failedToProcessRequest, response.status, response.statusText);

        return {
            Success: false,
            Messages: [message]
        };
    };

    /**
    * Helper method for JavaScript classes inheritance
    */
    app.extendsClass = function (d, b) {

        function __() { this.constructor = d; }

        __.prototype = b.prototype;
        d.prototype = new __();
    };

    /**
    * Stops specified event propagation
    */
    app.stopEventPropagation = function (event) {
        if (event != null) {
            event.cancelBubble = true;
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
    app.updateFormValidator = function (form) {
        if (form && $.validator && $.validator.unobtrusive) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }
    };

    /**
    * Helper method, tests, if given Guid is empty guid
    */
    app.isEmptyGuid = function (guid) {
        return guid === constants.emptyGuid || guid === '00000000000000000000000000000000';
    };

    function handleUnauthorizedAjaxError(event, xhr) {
        if (xhr && xhr.status === 401) {
            try {
                var redirectUrl = xhr.getResponseHeader(app.constants.loginRedirectHeader);
                if (redirectUrl) {
                    window.location.href = redirectUrl;
                }
            } catch (err) {
                app.logger.error(err);
            }
        }
    }

    /**
    * Creates new Guid
    */
    app.createGuid = function() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16).toUpperCase();
        });
    };

    /**
    * Returns string 1 or 0, converted from boolean
    */
    app.boolAsString = function (boolValue) {
        if (boolValue) {
            return "1";
        } else {
            return "0";
        }
    };

    /*
    * Helper methods for filter and loop through an array 
    */
    app.asEnumerable = function (arr) {
        var i,
            l = arr.length,
            forEach = function (callBack) {
                for (i = 0; i < l; i++) {
                    callBack(arr[i]);
                }

                return app.asEnumerable(arr);
            };

        return {
            where: function (whereClause) {
                var filtered = [];

                forEach(function(x) {
                    if (whereClause(x)) {
                        filtered.push(x);
                    }
                });

                return app.asEnumerable(filtered);
            },

            toArray: function() {
                return arr;
            },

            forEach: forEach
        };
    };

    /**
    * Initiliazes web page: checks browser version
    */
    function globalInit() {
        // Check browser version
        if ($.browser.msie && parseInt($.browser.version, 10) <= 7) {
            var browserInfo = $(selectors.browserInfo);

            browserInfo.find(selectors.browserInfoClose).on('click', function () {
                browserInfo.hide();
            });
            browserInfo.css('display', 'block');
        }

        $(document).on('click', function () {
            app.trigger(app.events.bodyClick, $(this));
        });

        // Handle unauthorized ajax errors
        $(document).ajaxError(handleUnauthorizedAjaxError);
    }

    // Init logger
    app.logger = new loggerModel();

    /**
    * Register initialization
    */
    app.registerInit(globalInit);

    return app;
});