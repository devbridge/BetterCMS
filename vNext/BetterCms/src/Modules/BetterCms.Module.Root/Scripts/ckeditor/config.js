/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'cms-imagemanager,cms-filemanager,cms-dynamicregion,cms-togglelinewrap,cms-modelvalues,aceeditor,cms-widget,cms-option';

    config.toolbar = [
        ['Undo', 'Redo'],
        ['Link', 'Unlink'],
        ['Bold', 'Italic', 'Underline', 'BulletedList', 'SpecialChar'],
        ['CmsImageManager', 'CmsFileManager', 'Image'],
        ['CmsDynamicRegion', 'CmsWidget', 'CmsOption', 'CmsModelValues'],
        ['Source', 'Maximize', 'CmsToggleLineWrap']
        //move to hidden items list ==>> ['Table', 'Strike', 'NumberedList', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'Styles', 'Format', 'Font', 'FontSize'],
    ];

    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
    config.allowedContent = true;
    config.extraAllowedContent = 'div[class]';
    config.autoParagraph = false;
    config.toolbarCanCollapse = true;
};

//todo check how to implement multiple toolbars
//some info here: http://ckeditor.com/latest/samples/old/datafiltering.html
//and here: http://docs.ckeditor.com/#!/api/CKEDITOR.config-cfg-customConfig
//CKEDITOR.replace('CodeEditor', {
//    customConfig: 'code-config.js'
//});
