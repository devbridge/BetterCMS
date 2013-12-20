/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'cms-imagemanager,cms-filemanager,cms-dynamicregion,cms-modelvalues,aceeditor';

    config.toolbar = [
		['Undo', 'Redo'],
		['Link', 'Unlink'],
		['CmsImageManager', 'CmsFileManager', 'Table', 'SpecialChar', 'HorizontalRule', 'Image'],
	    ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'],
        ['TextColor', 'BGColor'],
		['NumberedList', 'BulletedList', 'Outdent', 'Indent'],
		['Styles', 'Format', 'Font', 'FontSize'],
        ['CmsDynamicRegion', 'CmsModelValues'],
		['Source', 'Maximize', 'ShowBlocks']
    ];

    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
    config.allowedContent = true;
    config.extraAllowedContent = 'div[class]';
    config.autoParagraph = false;
};
