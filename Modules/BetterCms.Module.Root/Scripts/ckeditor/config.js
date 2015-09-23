/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'cms-imagemanager,cms-filemanager,cms-dynamicregion,cms-togglelinewrap,cms-modelvalues,aceeditor,cms-widget';

    config.toolbar = [
		['Undo', 'Redo'],
		['Link', 'Unlink'],
        ['Bold', 'Italic', 'Underline', 'BulletedList', 'SpecialChar'],
		['CmsImageManager', 'CmsFileManager', 'Image'],
		//move to more item list ==>> ['Table', 'Strike', 'NumberedList', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'Styles', 'Format', 'Font', 'FontSize'],
        ['CmsDynamicRegion', 'CmsWidget', 'CmsModelValues'],
		['Source', 'Maximize', 'CmsToggleLineWrap']
    ];

    config.height = 500;
    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
    config.allowedContent = true;
    config.extraAllowedContent = 'div[class]';
    config.autoParagraph = false;
};
