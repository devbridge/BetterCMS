/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.htmlEditor', ['bcms.jquery', 'bcms', 'ckeditor', 'bcms.markdown', 'bcms.content'], function ($, bcms, ckEditor, markdownEditor, bcmsContent) {
    'use strict';

    var htmlEditor = {},

        // Selectors used in the module to locate DOM elements:
        selectors = {
            imageButtonContainer: '.cke_button:has(.cke_button__image_icon)'
        },
        links = {},
        globalization = {
            smartTagPageTitle: null,
            smartTagPageUrl: null,
            smartTagPageId: null,
            smartTagPageCreatedOn: null,
            smartTagPageModifiedOn: null,
            smartTagPageOption: null,
            smartTagWidgetOption: null,
            smartTagPageMetaTitle: null,
            smartTagPageMetaKeywords: null,
            smartTagPageMetaDescription: null,
            smartTagPageMainImageUrl: null,
            smartTagPageSecondaryImageUrl: null,
            smartTagPageFeaturedImageUrl: null,
            smartTagPageCategory: null,
            smartTagBlogAuthor: null,
            smartTagBlogActivationDate: null,
            smartTagBlogExpirationDate: null
        },
        events = {
            insertImage: 'insertImage',
            insertFile: 'insertFile',
            insertDynamicRegion: 'insertDynamicRegion',
            insertWidget: 'insertWidget',
            editChildWidgetOptions: 'editChildWidgetOptions',
            editWidget: 'editWidget'
        },
        cmsEditorTypes = {
            widget: 'widget',
            page: 'page'
        };

    // Assign objects to module
    htmlEditor.selectors = selectors;
    htmlEditor.links = links;
    htmlEditor.globalization = globalization;
    htmlEditor.events = events;
    htmlEditor.cmsEditorTypes = cmsEditorTypes;

    /**
    * Html editor id
    */
    htmlEditor.id = null;

    function getSmartTags() {
        var tags = [];

        tags.push({ id: 'pageTitle', text: "{{CmsPageTitle}}", title: htmlEditor.globalization.smartTagPageTitle });
        tags.push({ id: 'pageUrl', text: "{{CmsPageUrl}}", title: htmlEditor.globalization.smartTagPageUrl });
        tags.push({ id: 'pageId', text: "{{CmsPageId}}", title: htmlEditor.globalization.smartTagPageId });
        tags.push({
            id: 'pageCreatedOn',
            text: "{{CmsPageCreatedOn:yyyy-MM-dd}}",
            title: htmlEditor.globalization.smartTagPageCreatedOn,
            openWith: "{{CmsPageCreatedOn:",
            closeWith: "}}",
            placeholder: "yyyy-MM-dd"
        });
        tags.push({
            id: 'pageModifiedOn',
            text: "{{CmsPageModifiedOn:yyyy-MM-dd}}",
            title: htmlEditor.globalization.smartTagPageModifiedOn,
            openWith: "{{CmsPageModifiedOn:",
            closeWith: "}}",
            placeholder: "yyyy-MM-dd"
        });
        tags.push({
            id: 'pageOption',
            text: "{{CmsPageOption:OptionKey}}",
            title: htmlEditor.globalization.smartTagPageOption,
            openWith: "{{CmsPageOption:",
            closeWith: "}}",
            placeholder: "OptionKey"
        });
        tags.push({
            id: 'widgetOption',
            text: "{{CmsWidgetOption:OptionKey}}",
            title: htmlEditor.globalization.smartTagWidgetOption,
            openWith: "{{CmsWidgetOption:",
            closeWith: "}}",
            placeholder: "OptionKey"
        });
        tags.push({ id: 'pageMetaTitle', text: "{{CmsPageMetaTitle}}", title: htmlEditor.globalization.smartTagPageMetaTitle });
        tags.push({ id: 'pageMetaKeywords', text: "{{CmsPageMetaKeywords}}", title: htmlEditor.globalization.smartTagPageMetaKeywords });
        tags.push({ id: 'pageMetaDescription', text: "{{CmsPageMetaDescription}}", title: htmlEditor.globalization.smartTagPageMetaDescription });
        tags.push({ id: 'pageMainImageUrl', text: "{{CmsPageMainImageUrl}}", title: htmlEditor.globalization.smartTagPageMainImageUrl });
        tags.push({ id: 'pageSecondaryImageUrl', text: "{{CmsPageSecondaryImageUrl}}", title: htmlEditor.globalization.smartTagPageSecondaryImageUrl });
        tags.push({ id: 'pageFeaturedImageUrl', text: "{{CmsPageFeaturedImageUrl}}", title: htmlEditor.globalization.smartTagPageFeaturedImageUrl });
        tags.push({ id: 'pageCategory', text: "{{CmsPageCategory}}", title: htmlEditor.globalization.smartTagPageCategory });
        tags.push({ id: 'pageAuthor', text: "{{CmsBlogAuthor}}", title: htmlEditor.globalization.smartTagBlogAuthor });
        tags.push({
            id: 'blogActivationDate',
            text: "{{CmsBlogActivationDate:yyyy-MM-dd}}",
            title: htmlEditor.globalization.smartTagBlogActivationDate,
            openWith: "{{CmsBlogActivationDate:",
            closeWith: "}}",
            placeholder: "yyyy-MM-dd"
        });
        tags.push({
            id: 'blogExpirationDate',
            text: "{{CmsBlogExpirationDate:yyyy-MM-dd}}",
            title: htmlEditor.globalization.smartTagBlogExpirationDate,
            openWith: "{{CmsBlogExpirationDate:",
            closeWith: "}}",
            placeholder: "yyyy-MM-dd"
        });

        return tags;
    }

    htmlEditor.initializeMarkdownEditor = function (id, editingContentId, options) {
        options = $.extend({
            smartTags: getSmartTags()
        }, options);

        markdownEditor.initializeInstance(htmlEditor, id, editingContentId, options);
    }

    htmlEditor.editChildWidgetOptions = function (editor, widgetId, assignmentId, contentId, optionListViewModel, onCloseClick) {
        bcms.trigger(htmlEditor.events.editChildWidgetOptions, {
            editor: editor,
            widgetId: widgetId,
            assignmentId: assignmentId,
            contentId: contentId,
            onCloseClick: onCloseClick,
            optionListViewModel: optionListViewModel
        });
    };

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
        CKEDITOR.instances[id].codeEditorMode = options.codeEditorMode;
        CKEDITOR.instances[id].aceEditorOptions = options.aceEditorOptions;
        CKEDITOR.instances[id].cmsEditorTypes = cmsEditorTypes;
        CKEDITOR.instances[id].cmsEditorType = options.cmsEditorType || cmsEditorTypes.page;

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

        CKEDITOR.instances[id].EditChildWidgetOptions = htmlEditor.editChildWidgetOptions;

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
                    editor.aceEditor.focus();
                } else {
                    editor.setData(editor.getData() + html);
                }
            } else {
                editor.insertHtml(html);
            }
        };

        CKEDITOR.instances[id].on('change', function () {
            $('#' + this.name).val(CKEDITOR.instances[id].getData());
        });

        CKEDITOR.instances[id].on('instanceReady', function () {
            // Hide native image button container
            $(selectors.imageButtonContainer).hide();

            var element = $('#' + id),
                isReadOnly = element.attr('readonly') === 'readonly' || element.attr('disabled') === 'disabled',
                instance = CKEDITOR.instances[id];
            instance.setReadOnly(isReadOnly);
            if (editMode === true) {
                instance.setMode('source');
                instance.addHtml(text);
                $('#' + this.name).val(text);
            }
        });

        CKEDITOR.instances[id].smartTags = getSmartTags();
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

    htmlEditor.isSourceMode = function (textareaId, contentTextMode) {
        if (contentTextMode === bcmsContent.contentTextModes.markdown
                || contentTextMode === bcmsContent.contentTextModes.simpleText) {
            return true;
        }

        var instance = htmlEditor.getInstance(textareaId);

        return instance != null && instance.mode === 'source';
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