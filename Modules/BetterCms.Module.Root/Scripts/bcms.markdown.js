bettercms.define("bcms.markdown", ['bcms.jquery', 'bcms', 'bcms.jquery.markitup'],
    function ($, bcms) {
        "use strict";

        var markdownEditor = {};

        function addBlockToTextarea(html, settings) {
            if (document.selection) {
                var newSelection = document.selection.createRange();
                newSelection.text = html;
            } else {
                settings.textarea.value = settings.textarea.value.substring(0, settings.caretPosition) + html + settings.textarea.value.substring(settings.caretPosition + settings.selection.length, settings.textarea.value.length);
            }

            settings.textarea.setSelectionRange(settings.selectionStart, settings.selectionStart + settings.selectionLength);
            settings.textarea.scrollTop = settings.scrollPosition;
            settings.textarea.focus();
        }

        /*
        * Initializes markdown editor instance
        */
        markdownEditor.initializeInstance = function (htmlEditor, id, editingContentId, options) {
            options = $.extend({
                previewParserPath: '',
                onShiftEnter: { keepDefault: false, openWith: '\n\n' },
                markupSet: [
                    { name: 'Heading 1', key: '1', openWith: '# ', placeHolder: 'Chapter', className: 'markItUpButtonH1' },
                    { name: 'Heading 2', key: '2', openWith: '## ', placeHolder: 'Section', className: 'markItUpButtonH2' },
                    { name: 'Heading 3', key: '3', openWith: '### ', placeHolder: 'Subsection', className: 'markItUpButtonH3' },
                    { name: 'Heading 4', key: '4', openWith: '#### ', placeHolder: 'SubSubsection', className: 'markItUpButtonH4' },
                    { name: 'Heading 5', key: '5', openWith: '##### ', placeHolder: 'SubSubsection', className: 'markItUpButtonH5' },
                    { name: 'Heading 6', key: '6', openWith: '###### ', placeHolder: 'SubSubsection', className: 'markItUpButtonH6' },
                    { separator: '---------------' },
                    { name: 'Bold', key: 'B', openWith: '**', closeWith: '**', className: 'markItUpButtonBold' },
                    { name: 'Italic', key: 'I', openWith: "_", closeWith: "_", className: 'markItUpButtonItalic' },
                    { separator: '---------------' },
                    { name: 'Bulleted List', openWith: '- ', className: 'markItUpButtonListBullet' },
                    { name: 'Numeric List', openWith: '+', className: 'markItUpButtonListNumeric' },
                    { separator: '---------------' },
                    {
                        name: 'Picture',
                        key: 'P',
                        className: 'markItUpButtonPicture',
                        beforeInsert: function (obj) {
                            var settings = {
                                selection: obj.selection,
                                caretPosition: obj.caretPosition,
                                scrollPosition: obj.scrollPosition,
                                textarea: obj.textarea
                            };

                            bcms.trigger(htmlEditor.events.insertImage, {
                                addHtml: function (html, imgObject) {
                                    var markdown = $.format("![{0}]({1})", imgObject.alt, imgObject.src);

                                    settings.selectionStart = settings.caretPosition + 2;
                                    settings.selectionLength = imgObject.alt.length;

                                    addBlockToTextarea(markdown, settings);
                                }
                            });
                        }
                    },
                    {
                        name: 'Link',
                        key: 'L',
                        className: 'markItUpButtonLink',
                        beforeInsert: function (obj) {
                            var settings = {
                                selection: obj.selection,
                                caretPosition: obj.caretPosition,
                                scrollPosition: obj.scrollPosition,
                                textarea: obj.textarea
                            };

                            bcms.trigger(htmlEditor.events.insertFile, {
                                addHtml: function (html, fileObject) {
                                    var markdown = $.format("[{0}]({1})", fileObject.html, fileObject.href);

                                    settings.selectionStart = settings.caretPosition + 1;
                                    settings.selectionLength = fileObject.html.length;

                                    addBlockToTextarea(markdown, settings);
                                }
                            });
                        }
                    },
                    { separator: '---------------' },
                    {
                        name: 'Widget',
                        className: 'markItUpButtonWidget',
                        beforeInsert: function (obj) {
                            var settings = {
                                selection: obj.selection,
                                caretPosition: obj.caretPosition,
                                scrollPosition: obj.scrollPosition,
                                textarea: obj.textarea
                            };

                            bcms.trigger(htmlEditor.events.insertWidget, {
                                editor: {
                                    addHtml: function (html) {
                                        settings.selectionStart = settings.caretPosition + html.length;
                                        settings.selectionLength = 0;

                                        addBlockToTextarea(html, settings);
                                    },
                                    mode: 'source'
                                },
                                editorId: id
                            });
                        }
                    },
                    { separator: '---------------' },
                    { name: 'Quotes', openWith: '-------\n', closeWith: '\n-------\n', className: 'markItUpButtonQuotes' },
                    { name: 'Code Block / Code', openWith: '``', closeWith: '``', className: 'markItUpButtonCode' }
                ]
            }, options);

            $('#' + id).markItUp(options);
        };

        /**
        * Initializes markdown editor module.
        */
        markdownEditor.init = function () {
            bcms.logger.debug('Initializing bcms.markdown module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(markdownEditor.init);

        return markdownEditor;
    });