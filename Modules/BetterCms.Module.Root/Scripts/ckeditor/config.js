/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'cms-imagemanager,cms-filemanager';

    config.toolbar = [
		['Undo', 'Redo'],
		['Link', 'Unlink'],
		['CmsImageManager', 'CmsFileManager', 'Table', 'SpecialChar', 'HorizontalRule', 'Image'],
	    ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'],
        ['TextColor', 'BGColor'],
		['NumberedList', 'BulletedList', 'Outdent', 'Indent'],
		['Styles', 'Format', 'Font', 'FontSize'],
		['Source', 'Maximize', 'ShowBlocks']
    ];

    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
};
