/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */
bettercms.define('bcms.vimeo', ['bcms.jquery', 'bcms', 'bcms.media', 'bcms.vimeo.videos'],
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
            // TODO: implement choice to upload or select.
            videos.addUploadedVideo(rootFolderId, onSaveCallback);
        };

        /**
        * Initializes module.
        */
        module.init = function () {
            console.log('Initializing bcms.vimeo module.');

            media.videoProviderOptions.uploadMediaAction = uploadAction;
        };

        /**
        * Register initialization.
        */
        bcms.registerInit(module.init);

        return module;
    });