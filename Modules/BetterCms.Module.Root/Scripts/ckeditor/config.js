/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'cms-imagemanager,cms-filemanager,cms-dynamicregion,cms-togglelinewrap,cms-modelvalues,aceeditor,cms-widget,cms-option';

    config.toolbar = [
        ['Undo', 'Redo'],
        ['Link', 'Unlink'],
        ['Bold', 'Italic', 'Underline'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['BulletedList', 'NumberedList'],
        ['CmsModelValues'],
        ['CmsImageManager', 'CmsFileManager', 'Image'],
        ['CmsDynamicRegion', 'CmsWidget', 'CmsOption'],
        ['Source', 'Maximize', 'CmsToggleLineWrap'],
        '/',
        ['Styles'],
        ['Format'],
        ['Font'],
        ['FontSize'],
        ['Copy', 'Cut', 'Paste', 'PasteFromWord'],
        ['Strike', 'SpecialChar', 'Table']
    ];

    config.skin = 'bettercms';
    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
    config.allowedContent = true;
    config.extraAllowedContent = 'div[class]';
    config.autoParagraph = false;
    config.toolbarCanCollapse = true;
    config.forcePasteAsPlainText = true;
};