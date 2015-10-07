//todo this configuration should be used for CSS/JavaScript editors.
CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'cms-imagemanager,cms-filemanager,cms-dynamicregion,cms-modelvalues,aceeditor,cms-widget,cms-option';

    config.toolbar = [
        ['Undo', 'Redo'],
        ['CmsImageManager', 'CmsFileManager', 'Image'],
        ['CmsOption', 'CmsModelValues'],
        ['Maximize']
    ];

    config.height = 400;
    config.removePlugins = 'tabletools';
    config.disableNativeSpellChecker = false;
    config.allowedContent = true;
    config.extraAllowedContent = 'div[class]';
    config.autoParagraph = false;
};
