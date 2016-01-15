// --------------------------------------------------------------------------------------------------------------------
// <copyright file="plugin.js" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
                        var widgetId = CKEDITOR.plugins.cmswidget.getWidgetId(a.currentElement),
                            assignId = CKEDITOR.plugins.cmswidget.getAssignId(a.currentElement),
                            optionListViewModel = null;

                        if (editor.childWidgetOptions) {
                            optionListViewModel = editor.childWidgetOptions[assignId];
                        }

                        editor.EditChildWidgetOptions(editor, widgetId, assignId, editor.contentId, optionListViewModel,
                            function (viewModel) {
                                if (!viewModel.isValid(true)) {
                                    return false;
                                }

                                if (!editor.childWidgetOptions) {
                                    editor.childWidgetOptions = {};
                                }

                                editor.childWidgetOptions[assignId] = viewModel;

                                return true;
                            },
                            editor.cmsEditorType);
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

                        editor.EditWidget(editor, id);
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
                icon: 'cmswidget',
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
                        widgetId = CKEDITOR.plugins.cmswidget.getWidgetId(element);

                    if (widgetId) {
                        menu.cmsEditWidget = CKEDITOR.TRISTATE_OFF;
                    }

                    menu.cmsWidgetOptions = CKEDITOR.TRISTATE_OFF;

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

            return element.data('id');
        },

        getAssignId: function (element) {
            if (!CKEDITOR.plugins.cmswidget.isWidget(element)) {
                return null;
            }

            var innerHtml = element.$.innerHTML;
            if (!innerHtml) {
                return null;
            }

            return element.data('assign-id');
        }
    };
})();