/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.codeEditor', ['bcms.jquery', 'bcms', 'ace'], function ($, bcms) {
    'use strict';

    var codeEditor = {},
        selectors = {
            inputFieldForJS: '.bcms-code-field-javascript',
            inputFieldForCSS: '.bcms-code-field-css',
        },
        links = {},
        globalization = {},
        events = {};

    // Assign objects to module
    codeEditor.selectors = selectors;
    codeEditor.links = links;
    codeEditor.globalization = globalization;
    codeEditor.events = events;

    codeEditor.initialize = function (container) {
        if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
            bcms.logger.info('ACE editor is enabled only on IE versions > 8.');
            return;
        }

        if (!ace) {
            bcms.logger.error('Failed to load ACE editor.');
            return;
        }

        var initAceEditor = function (inputField, mode) {
            inputField = $(inputField);
            var id = inputField.attr('id'),
                isReadOnly = inputField.attr('readonly') === 'readonly',
                editorId = "aceEditor_" + id,
                containerId = "aceEditor_container_" + id,
                aceContainer = $('<div id="' + containerId + '" class="bcms-editor-field-area-container" style="padding:0;"><div id="' + editorId + '" style="width:100%; height:100%;"></div></div>');
            
            inputField.hide();
            inputField.after(aceContainer);
            
            var aceEditor = ace.edit(editorId);
            aceEditor.setReadOnly(isReadOnly);
            aceEditor.getSession().setMode(mode);
            aceEditor.setTheme("ace/theme/chrome");
            aceEditor.setShowPrintMargin(0);
            aceEditor.getSession().setValue(inputField.val());
            aceEditor.getSession().setUseWrapMode(false);
            ace.config.loadModule('ace/ext/language_tools', function () {
                aceEditor.setOptions({
                    enableBasicAutocompletion: true,
                    enableSnippets: true,
                    maxLines: 20,
                    minLines: 5
                });
            });
            aceEditor.getSession().on('change', function() {
                inputField.val(aceEditor.getSession().getValue());
            });
            aceContainer.data('aceEditor', aceEditor);
        };
        
        container.find(selectors.inputFieldForJS).each(function () {
            initAceEditor(this, "ace/mode/javascript");
        });
        
        container.find(selectors.inputFieldForCSS).each(function () {
            initAceEditor(this, "ace/mode/css");
        });
    };

    /**
    * Initializes module.
    */
    codeEditor.init = function () {
        bcms.logger.debug('Initializing bcms.codeEditor module.');
    };

    /**
    * Register initialization
    */
    bcms.registerInit(codeEditor.init);

    return codeEditor;
});