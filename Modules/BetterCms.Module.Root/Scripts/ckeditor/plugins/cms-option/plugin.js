(function () {
    var a = {
        modes: { wysiwyg: 1, source: 1 },
        exec: function (editor) {
            var tags = editor.smartTags,
                        i, tag;

            for (i = 0; i < tags.length; i++) {
                if ((editor.cmsEditorType === editor.cmsEditorTypes.widget && tags[i].id === 'widgetOption')
                    || (editor.cmsEditorType === editor.cmsEditorTypes.page && tags[i].id === 'pageOption')) {
                    tag = tags[i];
                    break;
                }
            }

            if (tag != null) {
                editor.addHtml(tag.text);
            }
        }
    },
    b = 'cms-option';
    CKEDITOR.plugins.add(b, {
        init: function (c) {
            var d = c.addCommand(b, a);
            c.ui.addButton('CmsOption', {
                label: 'Option',
                title: 'Insert Option',
                icon: 'cmsoption',
                command: b
            });
        }
    });
})();