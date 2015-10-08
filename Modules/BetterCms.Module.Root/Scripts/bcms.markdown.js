bettercms.define("bcms.markdown", ['bcms.jquery', 'bcms', 'bcms.jquery.markitup'],
    function ($, bcms) {
        "use strict";

        var markdownEditor = {},
            selectors = {
                widgetOptionsButton: '.markItUpButtonWidgetOption'
            },
            currentEditor,
            currentId,
            currentCursorPosition,
            currentValue,
            widgetPositions = [];

        /**
         * Adds text block to the specified textarea
         */
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

        /**
         * Parses textarea and looks for widget positions
         */
        function recalculateWidgetPositions(html) {
            var pattern = /<widget [^>]*>[^>.]*?<\/widget>/g,
                match;
            widgetPositions = [];

            while ((match = pattern.exec(html)) != null) {
                widgetPositions.push({
                    start: match.index,
                    end: match[0].length + match.index,
                    value: match[0]
                });
            }
        }

        /**
         * Gets current cursor position in the specified textarea
         */
        function getTextareaCursorPosition(textarea) {
            var caretPosition;

            if (document.selection) {
                if (browser.msie) { // ie
                    var range = document.selection.createRange(), rangeCopy = range.duplicate();
                    rangeCopy.moveToElementText(textarea);
                    caretPosition = -1;
                    while (rangeCopy.inRange(range)) {
                        rangeCopy.moveStart('character');
                        caretPosition++;
                    }
                } else { // opera
                    caretPosition = textarea.selectionStart;
                }
            } else { // gecko & webkit
                caretPosition = textarea.selectionStart;
            }

            return caretPosition;
        }

        /**
         * Called when cursor position changes: checks if widget option button should be enabled
         */
        function onCursorPositionChanged() {
            var textarea = $('#' + currentId),
                value = textarea.val(),
                position = getTextareaCursorPosition(textarea.get(0)),
                isWidgetSelected = false,
                i;

            if (value !== currentValue) {
                recalculateWidgetPositions(value);
            }

            if (position !== currentCursorPosition || value !== currentValue) {
                for (i = 0; i < widgetPositions.length; i++) {
                    if (position >= widgetPositions[i].start && position <= widgetPositions[i].end) {
                        isWidgetSelected = true;
                        break;
                    }
                }
            }

            /* TODO: uncomment when widget options icon will be implemented
            if (isWidgetSelected) {
                $(selectors.widgetOptionsButton).show();
            } else {
                $(selectors.widgetOptionsButton).hide();
            }*/

            currentValue = value;
            currentCursorPosition = position;
        }

        /**
         * Inserts an image to the textarea
         */
        function insertImage(obj) {
            var settings = {
                selection: obj.selection,
                caretPosition: obj.caretPosition,
                scrollPosition: obj.scrollPosition,
                textarea: obj.textarea
            };

            bcms.trigger(currentEditor.events.insertImage, {
                addHtml: function (html, imgObject) {
                    var markdown = $.format("![{0}]({1})", imgObject.alt, imgObject.src);

                    settings.selectionStart = settings.caretPosition + 2;
                    settings.selectionLength = imgObject.alt.length;

                    addBlockToTextarea(markdown, settings);
                }
            });
        }

        /**
         * Inserts a file to the textarea
         */
        function insertFile(obj) {
            var settings = {
                selection: obj.selection,
                caretPosition: obj.caretPosition,
                scrollPosition: obj.scrollPosition,
                textarea: obj.textarea
            };

            bcms.trigger(currentEditor.events.insertFile, {
                addHtml: function (html, fileObject) {
                    var markdown = $.format("[{0}]({1})", fileObject.html, fileObject.href);

                    settings.selectionStart = settings.caretPosition + 1;
                    settings.selectionLength = fileObject.html.length;

                    addBlockToTextarea(markdown, settings);
                }
            });
        }

        /**
         * Inserts a widget to the textarea
         */
        function insertWidget(obj) {
            var settings = {
                selection: obj.selection,
                caretPosition: obj.caretPosition,
                scrollPosition: obj.scrollPosition,
                textarea: obj.textarea
            };

            bcms.trigger(bcms.events.insertWidget, {
                onInsert: function(widget) {
                    settings.selectionStart = settings.caretPosition + widget.html.length;
                    settings.selectionLength = 0;

                    addBlockToTextarea(widget.html, settings);
                }
            });
        }

        /**
         * Inserts a widget option to the textarea
         */
        function insertOption(obj, smartTags) {
            var settings = {
                selection: obj.selection,
                caretPosition: obj.caretPosition,
                scrollPosition: obj.scrollPosition,
                textarea: obj.textarea
            },
            i,
            html;

            for (i = 0; i < smartTags.length; i ++) {
                if (smartTags[i].id === 'pageOption') {

                    html = smartTags[i].openWith + smartTags[i].placeholder + smartTags[i].closeWith;
                    settings.selectionStart = smartTags[i].openWith.length;
                    settings.selectionLength = smartTags[i].placeholder.length;

                    addBlockToTextarea(html, settings);

                    break;
                }
            }
        }

        /**
         * Inserts a widget to the textarea
         */
        function editWidgetOptions(obj) {
            /* TODO: implement editing of child widget options */
        }

        /**
         * Initializes markdown editor instance
         */
        markdownEditor.initializeInstance = function (editor, id, editingContentId, options) {

            currentEditor = editor;
            currentId = id;

            var smartTagsList = [],
                i,
                item,
                textarea;

            if (options.smartTags.length > 0) {
                for (i = 0; i < options.smartTags.length; i ++) {
                    item = options.smartTags[i];

                    smartTagsList.push({
                        name: item.title,
                        replaceWith: item.openWith != null || item.closeWith != null ? '' : item.text,
                        placeHolder: item.placeholder,
                        openWith: item.openWith,
                        closeWith: item.closeWith
                    });
                }
            }

            options = $.extend({
                previewParserPath: '',
                onShiftEnter: { keepDefault: false, openWith: '\n\n' },
                hideIcons: false,
                markupSet: []
            }, options);

            if (!options.hideIcons) {
                options.markupSet = [
                    { name: 'Heading 1', key: '1', openWith: '# ', placeHolder: 'Your title here...', className: 'markItUpButtonH1' },
                    { name: 'Heading 2', key: '2', openWith: '## ', placeHolder: 'Your title here...', className: 'markItUpButtonH2' },
                    { name: 'Heading 3', key: '3', openWith: '### ', placeHolder: 'Your title here...', className: 'markItUpButtonH3' },
                    { name: 'Heading 4', key: '4', openWith: '#### ', placeHolder: 'Your title here...', className: 'markItUpButtonH4' },
                    { name: 'Heading 5', key: '5', openWith: '##### ', placeHolder: 'Your title here...', className: 'markItUpButtonH5' },
                    { name: 'Heading 6', key: '6', openWith: '###### ', placeHolder: 'Your title here...', className: 'markItUpButtonH6' },
                    { name: 'Bold', key: 'B', openWith: '**', closeWith: '**', className: 'markItUpButtonBold' },
                    { name: 'Italic', key: 'I', openWith: "_", closeWith: "_", className: 'markItUpButtonItalic' },
                    { name: 'Bulleted List', openWith: '* ', className: 'markItUpButtonListBullet' },
                    { name: 'Numeric List', openWith: '1. ', className: 'markItUpButtonListNumeric' },
                    { name: 'Picture', key: 'P', className: 'markItUpButtonPicture', afterInsert: insertImage },
                    { name: 'Link', key: 'L', className: 'markItUpButtonLink', afterInsert: insertFile },
                    { name: 'Widget', className: 'markItUpButtonWidget', afterInsert: insertWidget },
                    { name: 'Widget Option', className: 'markItUpButtonOption', afterInsert: function(obj) {
                        insertOption(obj, options.smartTags);
                    } },
                    { name: 'Quotes', openWith: '> ', className: 'markItUpButtonQuotes' },
                    { name: 'Code Block / Code', openWith: '`', closeWith: '`', className: 'markItUpButtonCode' },
                    { name: 'Smart tags', dropMenu: smartTagsList }
                    //  TODO: uncomment when widget options icon will be implemented
                    // { name: 'Widget options', className: 'markItUpButtonWidget markItUpButtonWidgetOption', afterInsert: editWidgetOptions }
                ];
            }

            textarea = $('#' + id);
            textarea.markItUp(options);

            setTimeout(function () {
                //  TODO: uncomment when widget options icon will be implemented
                //  $(selectors.widgetOptionsButton).hide();
                textarea.on("keyup click focus", onCursorPositionChanged);
                onCursorPositionChanged();
            }, 100);
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