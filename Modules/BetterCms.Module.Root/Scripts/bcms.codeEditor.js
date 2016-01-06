/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.codeEditor', ['bcms.jquery', 'bcms', 'bcms.htmlEditor', 'bcms.modal', 'ace'], function ($, bcms, htmlEditor, modal) {
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

            codeEditorMode: true,
            toolbarCanCollapse: false
        };

    // Assign objects to module
    codeEditor.selectors = selectors;
    codeEditor.links = links;
    codeEditor.globalization = globalization;
    codeEditor.events = events;

    function initCkEditor(textarea, mode, options) {
        var configuration = $.extend({
                    aceEditorOptions: {
                        mode: mode
                    },
                    height: textarea.outerHeight()
                },
                ckEditorConfig, options),
            id = textarea.attr('id'),
            isInitialized = textarea.data('isInitialized');

        if (!isInitialized) {
            htmlEditor.initializeHtmlEditor(id, null, configuration, true);
            textarea.data('isInitialized', true);
        }
    }

    codeEditor.initialize = function (container, dialog, options) {
        if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
            bcms.logger.info('ACE editor is enabled only on IE versions > 8.');
            return;
        }

        if (!ace) {
            bcms.logger.error('Failed to load ACE editor.');
            return;
        }

        var jsObjects = [],
            cssObjects = [];
        container.find(selectors.inputFieldForJS).each(function () {
            jsObjects.push($(this));
        });
        container.find(selectors.inputFieldForCSS).each(function () {
            cssObjects.push($(this));
        });

        modal.maximizeChildHeight($.merge(jsObjects, cssObjects), dialog);

        $.each(jsObjects, function() {
            initCkEditor($(this), "ace/mode/javascript", options);
        });
        $.each(cssObjects, function () {
            initCkEditor($(this), "ace/mode/css", options);
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