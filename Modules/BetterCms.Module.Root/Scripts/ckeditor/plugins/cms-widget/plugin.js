(function() {
    var a = {
            modes: { wysiwyg: 1, source: 1 },
            exec: function(editor) {
                editor.InsertWidget(editor);
            },
            currentElement: null
        },
        b = 'cms-widget';
    CKEDITOR.plugins.add(b, {
        onLoad: function() {
            if (CKEDITOR.addCss) {
                CKEDITOR.addCss(
                    'widget {' +
                    '  position: relative;' +
                    '  height: 35px;' +
                    '  line-height: 35px;' +
                    '  padding: 10px;' +
                    '  margin: 0 -2px 0 0;' +
                    '  color: #0099ee;' +
                    "  background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAFRlDm/AAAAHUlEQVQI12P4//8/AxRDqQcPHjAAMUIQyoFQEAAA0/UuXeg1e3EAAAAASUVORK5CYII=') repeat 0 0;" +
                    '  border: 1px dashed #0099ee;' +
                    '  font-size: 12px;' +
                    '  font-weight: 700;' +
                    '}'
                );
            }
        },
        init: function(e) {
            var c = e.addCommand(b, a),
                editWidgetOptionsCommand = new CKEDITOR.command(e, {
                    exec: function(editor) {
                        if (!a.currentElement) {
                            return;
                        }
                        var id = CKEDITOR.plugins.cmswidget.getWidgetId(a.currentElement);
                        if (!id) {
                            return;
                        }

                        editor.EditChildWidgetOptions(editor, id);
                    }
                }),

                editWidgetCommand = new CKEDITOR.command(e, {
                    exec: function(editor) {
                        if (!a.currentElement) {
                            return;
                        }
                        var id = CKEDITOR.plugins.cmswidget.getWidgetId(a.currentElement);
                        if (!id) {
                            return;
                        }

                        editor.EditChildWidget(editor, id);
                    }
                }),

                removeWidgetCommand = new CKEDITOR.command(e, {
                    exec: function(editor) {
                        if (!a.currentElement) {
                            return;
                        }

                        a.currentElement.remove();
                        a.currentElement = null;
                    }
                });

            e.ui.addButton('CmsWidget', {
                title: 'Insert Widget',
                label: 'Widget',
                icon: 'cmsdynamicregion',
                command: b
            });

            e.addCommand('cmsWidgetOptions', editWidgetOptionsCommand);
            e.addCommand('cmsEditWidget', editWidgetCommand);
            e.addCommand('cmsRemoveWidget', removeWidgetCommand);

            e.on('doubleclick', function (evt) {
                var element = evt.data.element,
                    id = CKEDITOR.plugins.cmswidget.getWidgetId(element);

                if (id) {
                    a.currentElement = element;
                    e.execCommand('cmsWidgetOptions');
                }
            }, null, null, 0);

            if (e.addMenuItems) {
                e.addMenuItems({
                    cmsWidgetOptions: {
                        label: 'Edit widget options',
                        command: 'cmsWidgetOptions',
                        group: 'link',
                        order: 1
                    },

                    cmsEditWidget: {
                        label: 'Edit widget',
                        command: 'cmsEditWidget',
                        group: 'link',
                        order: 2
                    },

                    cmsRemoveWidget: {
                        label: 'Remove widget',
                        command: 'cmsRemoveWidget',
                        group: 'link',
                        order: 3
                    }
                });
            }

            if (e.contextMenu) {
                e.contextMenu.addListener(function(element) {

                    if (!CKEDITOR.plugins.cmswidget.isWidget(element)) {
                        return null;
                    }
                    a.currentElement = element;

                    var menu = {
                            cmsRemoveWidget: CKEDITOR.TRISTATE_OFF
                        },
                        id = CKEDITOR.plugins.cmswidget.getWidgetId(element);

                    if (id) {
                        menu.cmsWidgetOptions = CKEDITOR.TRISTATE_OFF;
                        menu.cmsEditWidget = CKEDITOR.TRISTATE_OFF;
                    }

                    return menu;
                });
            }
        }
    });

    CKEDITOR.plugins.cmswidget = {
        isWidget: function(element) {
            return element
                && !element.isReadOnly()
                && element.$.tagName
                && element.$.tagName.toUpperCase() == 'WIDGET';
        },

        getWidgetId: function (element) {
            if (!CKEDITOR.plugins.cmswidget.isWidget(element)) {
                return null;
            }

            var innerHtml = element.$.innerHTML;
            if (!innerHtml) {
                return null;
            }

            var regex = new RegExp('{{WIDGET\\:([a-zA-Z0-9]{8}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{12})}}');
            var result = regex.exec(innerHtml);

            if (result && result.length > 1) {
               return result[1];
            }

            return null;
        }
    };
})();