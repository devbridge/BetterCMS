//todo plugin is empty please implement functionality.
(function () {
    CKEDITOR.plugins.add('cms-option', {
        init: function (e) {
            e.ui.addButton('CmsOption', {
                title: 'Insert Option',
                label: 'Option',
                icon: 'cmsoption'
            });
        }
    });
})();
