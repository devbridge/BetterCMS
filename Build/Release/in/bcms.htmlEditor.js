/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define */

define('bcms.htmlEditor', ['bcms.jquery', 'bcms', 'ckeditor'], function ($, bcms) {
    'use strict';

    var htmlEditor = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {},
        links = {},
        globalization = {},
        events = {
            insertImage: 'insertImage',
            insertFile: 'insertFile',
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
        CKEDITOR.replace(id, options);
        
        CKEDITOR.instances[id].InsertImageClicked = function(editor) {
            bcms.trigger(htmlEditor.events.insertImage, editor);
        };
        
        CKEDITOR.instances[id].InsertFileClicked = function (editor) {
            bcms.trigger(htmlEditor.events.insertFile, editor);
        };
    };

    htmlEditor.destroyAllHtmlEditorInstances = function () {
        for (name in CKEDITOR.instances) {
            CKEDITOR.instances[name].destroy();
        }
    };
    
    htmlEditor.destroyHtmlEditorInstance = function () {
        var editor = CKEDITOR.instances[htmlEditor.id];
        if (editor) {
            editor.destroy();
        }
    };

    htmlEditor.setSourceMode = function (id) {
        CKEDITOR.instances[id].on('instanceReady', function () {
            var instance = CKEDITOR.instances[id];
            instance.setMode('source');
        });
    };

    htmlEditor.updateEditorContent = function (textareaId) {
        var id = textareaId ? textareaId : htmlEditor.id;

        // Put content from HTML editor to textarea:
        var instance = CKEDITOR.instances[id],
        html = instance.getData();
        $('#' + id).val(html);
    };

    htmlEditor.insertImage = function(imageUrl) {
        alert('Insert image to html editor! (' + imageUrl +')');
    };

    /**
    * Initializes sidebar module.
    */
    htmlEditor.init = function () {
        console.log('Initializing bcms.htmlEditor module');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(htmlEditor.init);

    return htmlEditor;
});