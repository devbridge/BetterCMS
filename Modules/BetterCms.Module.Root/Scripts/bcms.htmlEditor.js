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
            insertWidget: 'insertWidget',
            editChildWidgetOptions: 'editChildWidgetOptions',
            editWidget: 'editWidget'
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

    htmlEditor.initializeHtmlEditor = function (id, editingContentId, options, startInSourceMode) {
        var editMode = startInSourceMode;
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

        if (editMode === true) {
            var text = $("#" + id).val();
            $("#" + id).val("");
        }

        CKEDITOR.replace(id, options);
        CKEDITOR.instances[id].contentId = editingContentId;

        CKEDITOR.instances[id].InsertImageClicked = function (editor) {
            bcms.trigger(htmlEditor.events.insertImage, editor);
        };

        CKEDITOR.instances[id].InsertFileClicked = function (editor) {
            bcms.trigger(htmlEditor.events.insertFile, editor);
        };

        CKEDITOR.instances[id].InsertDynamicRegion = function (editor) {
            bcms.trigger(htmlEditor.events.insertDynamicRegion, editor);
        };

        CKEDITOR.instances[id].InsertWidget = function (editor) {
            bcms.trigger(htmlEditor.events.insertWidget, {
                editor: editor,
                editorId: id
            });
        };

        CKEDITOR.instances[id].EditChildWidgetOptions = function (editor, widgetId, assignmentId, contentId, optionListViewModel, onCloseClick) {
            bcms.trigger(htmlEditor.events.editChildWidgetOptions, {
                editor: editor,
                widgetId: widgetId,
                assignmentId: assignmentId,
                contentId: contentId,
                onCloseClick: onCloseClick,
                optionListViewModel: optionListViewModel
            });
        };

        CKEDITOR.instances[id].EditWidget = function (editor, contentId) {
            bcms.trigger(htmlEditor.events.editWidget, {
                editor: editor,
                contentId: contentId
            });
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
                isReadOnly = element.attr('readonly') === 'readonly' || element.attr('disabled') === 'disabled',
                instance = CKEDITOR.instances[id];
            instance.setReadOnly(isReadOnly);
            if (editMode === true) {
                instance.setMode('source');
                instance.addHtml(text);
            }
        });
    };

    function closeMaximizedMode(instance) {
        if (instance && instance.commands && instance.commands.maximize && instance.commands.maximize.state == 1) {
            instance.execCommand('maximize');
        }
    }

    htmlEditor.destroyAllHtmlEditorInstances = function () {
        var instance;
        for (name in CKEDITOR.instances) {
            instance = CKEDITOR.instances[name];
            closeMaximizedMode(instance);
            instance.destroy();
        }
        if (window.location.href.slice(-2) === '#-') {
            window.location.hash = '';
        }
    };

    htmlEditor.destroyHtmlEditorInstance = function (textareaId) {
        var editor = htmlEditor.getInstance(textareaId);
        if (editor) {
            closeMaximizedMode(editor);
            editor.destroy();
        }
        if (window.location.href.slice(-2) === '#-') {
            window.location.hash = '';
        }
    };

    htmlEditor.setSourceMode = function (textareaId) {
        var instance = htmlEditor.getInstance(textareaId);

        instance.on('instanceReady', function () {
            instance.setMode('source');
        });
    };

    htmlEditor.enableInsertDynamicRegion = function (textareaId, isMasterPage, lastDynamicRegionNumber) {
        var instance = htmlEditor.getInstance(textareaId);

        instance.DynamicRegionsEnabled = true;
        instance.LastDynamicRegionNumber = lastDynamicRegionNumber;
        instance.IsMasterPage = isMasterPage;
    };

    htmlEditor.isSourceMode = function (textareaId) {
        var instance = htmlEditor.getInstance(textareaId);

        return instance.mode === 'source';
    };

    htmlEditor.updateEditorContent = function (textareaId) {
        // Put content from HTML editor to textarea:
        var instance = htmlEditor.getInstance(textareaId),
            id = textareaId || htmlEditor.id,
            html = instance.getData();

        $('#' + id).val(html);
    };

    htmlEditor.getInstance = function (textareaId) {
        var id = textareaId || htmlEditor.id;

        return CKEDITOR.instances[id];
    };

    /**
    * Initializes sidebar module.
    */
    htmlEditor.init = function () {
        bcms.logger.debug('Initializing bcms.htmlEditor module.');

        CKEDITOR.disableAutoInline = true;
    };

    /**
    * Register initialization
    */
    bcms.registerInit(htmlEditor.init);

    return htmlEditor;
});