/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */
bettercms.define('bcms.vimeo', ['bcms.jquery', 'bcms'],
    function($, bcms) {
        'use strict';

        var vimeo = {},
            selectors = {},
            links = {},
            globalization = {};

        /**
        * Assign objects to module.
        */
        vimeo.links = links;
        vimeo.globalization = globalization;
        vimeo.selectors = selectors;

        /**
        * Initializes vimeo module.
        */
        vimeo.init = function() {
            console.log('Initializing bcms.vimeo module.');
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(vimeo.init);

        return vimeo;
    });