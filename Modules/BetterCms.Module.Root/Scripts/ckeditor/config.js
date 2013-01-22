/**
 * @license Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here.
    // For the complete reference:
    // http://docs.ckeditor.com/#!/api/CKEDITOR.config

    config.extraPlugins = 'cms-imagemanager,cms-filemanager';

    // The toolbar groups arrangement, optimized for two toolbar rows.
    config.toolbar = [
        { name: 'clipboard', items: ['PasteText', 'PasteFromWord', 'Undo', 'Redo'] },
        { name: 'links', items: ['Link', 'Unlink'] },
        { name: 'insert', items: ['CmsImageManager', 'CmsFileManager', 'Table', 'SpecialChar'] },
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'RemoveFormat'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList'] },
        { name: 'styles', items: ['Styles', 'Format'] },
        { name: 'source', items: ['Source'] },
        { name: 'maximize', items: ['Maximize'] }
    ];

    // Remove some buttons, provided by the standard plugins, which we don't
    // need to have in the Standard(s) toolbar.
    config.removeButtons = 'Underline,Subscript,Superscript';
};
