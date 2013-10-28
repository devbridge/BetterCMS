/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.codeEditor', ['bcms.jquery', 'bcms', 'ace'], function ($, bcms) {
    'use strict';

    var codeEditor = {},
        selectors = {},
        links = {},
        globalization = {},
        events = {};

    // Assign objects to module
    codeEditor.selectors = selectors;
    codeEditor.links = links;
    codeEditor.globalization = globalization;
    codeEditor.events = events;

    codeEditor.initialize = function() {

    };

    /**
    * Initializes module.
    */
    codeEditor.init = function () {
        bcms.logger.debug('Initializing bcms.codeEditor module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(codeEditor.init);

    return codeEditor;
});