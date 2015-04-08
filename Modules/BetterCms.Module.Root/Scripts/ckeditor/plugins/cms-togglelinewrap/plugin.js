(function () {
    var a = {
        modes: { wysiwyg: 0, source: 1 },
        exec: function (editor) {
            editor.fire('onLineWrapChange');
            this.state == CKEDITOR.TRISTATE_ON ? this.setState(CKEDITOR.TRISTATE_OFF) : this.setState(CKEDITOR.TRISTATE_ON);
        }
    },
    b = 'cms-togglelinewrap';
    CKEDITOR.plugins.add(b, {
        requires: ['aceeditor'],
        init: function (c) {
                    var d = c.addCommand(b, a);
                    c.ui.addButton('CmsToggleLineWrap', {
                        label: 'Toggle Line Wrap in Source Mode',
                        icon: 'cmstogglelinebreak',
                        command: b
                    });
        }
    });
})();