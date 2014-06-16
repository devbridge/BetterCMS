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
                    'border: 1px solid black;' +
                    'height: 40px;' +
                    'width: 400px;' +
                    'float: left;' +
                    '}'
                );
            }
        },
        init: function(e) {
            var c = e.addCommand(b, a);
            e.ui.addButton('CmsWidget', {
                title: 'Insert Widget',
                label: 'Widget',
                icon: 'cmsdynamicregion',
                command: b
            });

            var command = new CKEDITOR.command(e, {
                exec: function (editor) {
                    if (!a.currentElement) {
                        return;
                    }
                    var id = CKEDITOR.plugins.cmswidget.getWidgetId(a.currentElement);
                    if (!id) {
                        return;
                    }

                    editor.EditChildWidgetOptions(editor, id);
                }
            });

            e.addCommand('cmsWidgetOptions', command);

            if (e.addMenuItems) {
                e.addMenuItems({
                    cmsWidgetOptions: {
                        label: 'Edit widget Options',
                        command: 'cmsWidgetOptions',
                        group: 'link',
                        order: 1
                    }
                });
            }

            if (e.contextMenu) {
                e.contextMenu.addListener(function(element, selection) {

                    var id = CKEDITOR.plugins.cmswidget.getWidgetId(element);
                    if (!id) {
                        return null;
                    }

                    a.currentElement = element;

                    return {
                        cmsWidgetOptions: CKEDITOR.TRISTATE_OFF
                    };
                });
            }
        },
        afterInit: function(e) {
            var dp = e.dataProcessor,
                hf = dp && dp.htmlFilter,
                df = dp && dp.dataFilter;
            if (df) {
                df.addRules({
                    elements: {
                        div: function(el) {
                            var regexp = /^{{DYNAMIC_REGION\:([a-zA-Z0-9]{8}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{4}\-[a-zA-Z0-9]{12})}}$/i;
                            if (el.children.length == 1 && CKEDITOR.htmlParser.text.prototype.isPrototypeOf(el.children[0]) && regexp.test(el.children[0].value)) {
                                var f = e.createFakeParserElement(el, 'bcms-draggable-region', 'cmsdynamicregion', false);
                                f.attributes.title = 'Dynamic Region';
                                //f.attributes.contenteditable = 'false';
                                f.attributes.isregion = 'true';
                                f.name = "div";
                                delete f.attributes["alt"];
                                delete f.attributes["align"];
                                delete f.attributes["src"];
                                f.add(new CKEDITOR.htmlParser.text('Content to add'));
                                f.isEmpty = false;
                                return f;
                            }
                            return null;
                        }
                    }
                }, {
                    applyToAll: true
                });
            }
            if (hf) {
                hf.addRules({
                    elements: {
                        div: function(el) {
                            if (el.attributes.isregion !== 'true') {
                                return null;
                            }
                            var attributes = el.attributes,
                                html = attributes && attributes['data-cke-realelement'],
                                fragment = html && new CKEDITOR.htmlParser.fragment.fromHtml(decodeURIComponent(html));
                            return fragment && fragment.children[0];
                        }
                    }
                }, {
                    applyToAll: true
                });
            }
        }
    });

    CKEDITOR.plugins.cmswidget = {
        getWidgetId: function (element) {
            if (!element
                        || element.isReadOnly()
                        || !element.$.tagName
                        || element.$.tagName.toUpperCase() != 'WIDGET') {
                return null;
            }

            var innerHtml = element.$.innerHTML;
            if (!innerHtml) {
                return null;
            }

            var regex = new RegExp('{{WIDGET\\:([a-zA-Z0-9]{8}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{12})}}');
            var result = regex.exec(innerHtml);

            if (result.length > 1) {
               return result[1];
            }

            return null;
        }
    };
})();