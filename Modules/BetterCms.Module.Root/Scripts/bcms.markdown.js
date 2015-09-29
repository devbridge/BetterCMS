bettercms.define("bcms.markdown", ['bcms.jquery', 'bcms', 'bcms.jquery.markitup'],
    function($, bcms, markitup) {
        "use strict";

        var markdownEditor = {};

        /*
        * Initializes markdown editor instance
        */
        markdownEditor.initializeInstance = function (id, editingContentId, options) {
            options = $.extend({
                previewParserPath: '',
                onShiftEnter: { keepDefault: false, openWith: '\n\n' },
                markupSet: [
                    { name: 'Heading 1', key: '1', openWith: '# ', placeHolder: 'Chapter' },
                    { name: 'Heading 2', key: '2', openWith: '## ', placeHolder: 'Section' },
                    { name: 'Heading 3', key: '3', openWith: '### ', placeHolder: 'Subsection' },
                    { name: 'Heading 4', key: '4', openWith: '#### ', placeHolder: 'SubSubsection' },
                    { name: 'Heading 5', key: '5', openWith: '##### ', placeHolder: 'SubSubsection' },
                    { name: 'Heading 6', key: '6', openWith: '###### ', placeHolder: 'SubSubsection' },
                    { separator: '---------------' },
                    { name: 'Bold', key: 'B', openWith: '**', closeWith: '**' },
                    { name: 'Italic', key: 'I', openWith: "_", closeWith: "_" },
                    { separator: '---------------' },
                    { name: 'Bulleted List', openWith: '- ' },
                    { name: 'Numeric List', openWith: '+' },
                    { separator: '---------------' },
                    { name: 'Picture', key: 'P', replaceWith: '[[[![Alternative text]!] [![Url:!:http://]!] center]]' },
                    { name: 'Link', key: 'L', openWith: '[[', closeWith: ' [![Url:!:http://]!]]]', placeHolder: 'Your text to link here...' },
                    { separator: '---------------' },
                    { name: 'Quotes', openWith: '-------\n', closeWith: '\n-------\n' },
                    { name: 'Code Block / Code', openWith: '``', closeWith: '``' }
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