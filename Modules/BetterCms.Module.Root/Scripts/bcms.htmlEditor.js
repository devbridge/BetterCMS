/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.htmlEditor', ['bcms.jquery', 'bcms', 'ckeditor'], function ($, bcms) {
    'use strict';

    var htmlEditor = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {
            imageButtonContainer: '.cke_button:has(.cke_button__image_icon)'
        },
        links = {},
        globalization = {},
        events = {
            insertImage: 'insertImage',
            insertFile: 'insertFile',
            insertDynamicRegion: 'insertDynamicRegion',
        };

    // Assign objects to module
    htmlEditor.selectors = selectors;
    htmlEditor.links = links;
    htmlEditor.globalization = globalization;
    htmlEditor.events = events;

    /**
    * Html editor id
    */
    htmlEditor.id = null;

    htmlEditor.initializeHtmlEditor = function (id, options) {
        if (!CKEDITOR) {
            alert('Failed to load HTML editor.');
        }

        htmlEditor.id = id;

        // Load CKEditor:
        var instance = CKEDITOR.instances[id];
        if (instance) {
            instance.destroy(true);
        }
        
        if (window.location.href.slice(-1) === '#') {
            window.location.hash = '#-';
        }

        CKEDITOR.replace(id, options);

        CKEDITOR.instances[id].InsertImageClicked = function (editor) {
            bcms.trigger(htmlEditor.events.insertImage, editor);
        };
        
        CKEDITOR.instances[id].InsertFileClicked = function (editor) {
            bcms.trigger(htmlEditor.events.insertFile, editor);
        };
        
        CKEDITOR.instances[id].InsertDynamicRegion = function (editor) {
            bcms.trigger(htmlEditor.events.insertDynamicRegion, editor);
        };

        CKEDITOR.instances[id].addHtml = function (html) {
            var editor = this;

            if (editor.mode == 'source') {
                if (editor.aceEditor && $.isFunction(editor.aceEditor.insert)) {
                    editor.aceEditor.insert(html);
                } else {
                    editor.setData(editor.getData() + html);
                }
            } else {
                editor.insertHtml(html);
            }
        };

        // Hide native image button container
        CKEDITOR.instances[id].on('instanceReady', function () {
            $(selectors.imageButtonContainer).hide();
            var element = $('#' + id),
                isReadOnly = element.attr('readonly') === 'readonly' || element.attr('disabled') === 'disabled';
            CKEDITOR.instances[id].setReadOnly(isReadOnly);
        });
    };

    htmlEditor.destroyAllHtmlEditorInstances = function () {
        for (name in CKEDITOR.instances) {
            CKEDITOR.instances[name].destroy();
        }
        if (window.location.href.slice(-2) === '#-') {
            window.location.hash = '';
        }
    };
    
    htmlEditor.destroyHtmlEditorInstance = function () {
        var editor = CKEDITOR.instances[htmlEditor.id];
        if (editor) {
            editor.destroy();
        }
        if (window.location.href.slice(-2) === '#-') {
            window.location.hash = '';
        }
    };

    htmlEditor.setSourceMode = function (textareaId) {
        var id = textareaId ? textareaId : htmlEditor.id;

        CKEDITOR.instances[id].on('instanceReady', function () {
            var instance = CKEDITOR.instances[id];
            instance.setMode('source');
        });
    };
    
    htmlEditor.enableInsertDynamicRegion = function (textareaId) {
        var id = textareaId ? textareaId : htmlEditor.id;
        CKEDITOR.instances[id].DynamicRegionsEnabled = true;
    };
    
    htmlEditor.isSourceMode = function (textareaId) {
        var id = textareaId ? textareaId : htmlEditor.id;

        var instance = CKEDITOR.instances[id];
        return instance.mode === 'source';
    };

    htmlEditor.updateEditorContent = function (textareaId) {
        var id = textareaId ? textareaId : htmlEditor.id;

        // Put content from HTML editor to textarea:
        var instance = CKEDITOR.instances[id],
        html = instance.getData();
        $('#' + id).val(html);
    };

    /**
    * Initializes sidebar module.
    */
    htmlEditor.init = function () {
        bcms.logger.debug('Initializing bcms.htmlEditor module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(htmlEditor.init);

    return htmlEditor;
});