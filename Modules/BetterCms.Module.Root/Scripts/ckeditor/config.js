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
        ['Source', 'Maximize', 'CmsToggleLineWrap'],
        ['Table', 'Strike', 'NumberedList', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'Styles', 'Format', 'Font', 'FontSize']
    ];

    config.skin = 'bettercms';
    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
    config.allowedContent = true;
    config.extraAllowedContent = 'div[class]';
    config.autoParagraph = false;
    config.toolbarCanCollapse = true;
};