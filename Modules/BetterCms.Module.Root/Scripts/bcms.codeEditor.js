/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.codeEditor', ['bcms.jquery', 'bcms', 'bcms.htmlEditor', 'ace'], function ($, bcms, htmlEditor) {
    'use strict';

    var codeEditor = {},
        selectors = {
            inputFieldForJS: '.bcms-code-field-javascript',
            inputFieldForCSS: '.bcms-code-field-css',
            alCodeEditors: '.bcms-code-field-javascript, .bcms-code-field-css'
        },
        links = {},
        globalization = {},
        events = {},
        ckEditorConfig = {
            extraPlugins: 'cms-imagemanager,cms-filemanager,cms-modelvalues,aceeditor,cms-option',

            toolbar: [
                ['CmsImageManager', 'CmsFileManager', 'Image'],
                ['CmsOption', 'CmsModelValues'],
                ['Maximize']
            ],

            codeEditorMode: true
        };

    // Assign objects to module
    codeEditor.selectors = selectors;
    codeEditor.links = links;
    codeEditor.globalization = globalization;
    codeEditor.events = events;

    function initCkEditor(textarea, mode) {
        var configuration = $.extend({
                    aceEditorOptions: {
                        mode: mode
                    }
                },
                ckEditorConfig),
            id = textarea.attr('id'),
            isInitialized = textarea.data('isInitialized');

        if (!isInitialized) {
            console.log('Init tab');
            htmlEditor.initializeHtmlEditor(id, null, configuration, true);
            textarea.data('isInitialized', true);
        } else {
            console.log('Skipping Init tab');
        }
    }

    codeEditor.initialize = function (container) {
        if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
            bcms.logger.info('ACE editor is enabled only on IE versions > 8.');
            return;
        }

        if (!ace) {
            bcms.logger.error('Failed to load ACE editor.');
            return;
        }

        container.find(selectors.inputFieldForJS).each(function () {
            initCkEditor($(this), "ace/mode/javascript");
        });

        container.find(selectors.inputFieldForCSS).each(function () {
            initCkEditor($(this), "ace/mode/css");
        });
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