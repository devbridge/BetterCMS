/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */
bettercms.define('bcms.viddler', ['bcms.jquery', 'bcms', 'bcms.media', 'bcms.viddler.videos'],
    function ($, bcms, media, videos) {
        'use strict';

        var module = {},
            selectors = {},
            links = {},
            globalization = {};

        /**
        * Assign objects to module.
        */
        module.links = links;
        module.globalization = globalization;
        module.selectors = selectors;

        function uploadAction(rootFolderId, onSaveCallback, reuploadMediaId) {
            videos.uploadVideo(rootFolderId, onSaveCallback);
        };

        /**
        * Initializes module.
        */
        module.init = function () {
            console.log('Initializing bcms.viddler module.');

            media.videoProviderOptions.uploadMediaAction = uploadAction;
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(module.init);

        return module;
    });