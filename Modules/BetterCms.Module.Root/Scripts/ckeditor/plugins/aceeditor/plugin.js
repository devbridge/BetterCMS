/*
Based on source code from http://ckeditor.com/forums/CKEditor/ACE-Editor-Plugin-for-CKEditor-Source-View-Replacement-First-Draft
by http://www.angrysam.com/

Edited by the Devbridge Better CMS team.
*/
(function () {
    CKEDITOR.plugins.add('aceeditor', {
        requires: ['sourcearea'],

        // Initialize the plug-in.
        init: function (editor) {
            bettercms.requirejs(['bcms.jquery', 'bcms', 'ace'],
                function ($, bcms) {
                    'use strict';

                    if (!ace) {
                        bcms.logger.error('Failed to load ACE editor.');
                        return;
                    }
                    
                    if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
                        bcms.logger.info('ACE editor is enabled only on IE versions > 8.');
                        return;
                    }

                    bcms.logger.debug('Initializing ACE plug-in for CKEditor.');

                    // When the event "mode" is called in CKEditor it means someone clicked the "source" button.
                    editor.on('mode', function () {

                        bcms.logger.trace('editor mode: ' + editor.mode);

                        // If the user selected the "source" view.
                        if (editor.mode == 'source') {

                            // Define some variables for the textarea that CKEditor is using, the parent element which is the container for CKEditor.
                            var sourceAreaElement = $('textarea', '.' + editor.id);
                            var holderElement = sourceAreaElement.parent();

                            // Hide the textarea.
                            sourceAreaElement.hide();

                            // START ACE.
                            var editorID = editor.id;
                            sourceAreaElement.after('<div id="aceEditor_container_' + editorID + '" class="aceEditor_container" style=" background-color:white;"><div id="aceEditor_' + editorID + '" style="width:100%; height:100%;"></div></div>');

                            // Make the editor container fill the space.
                            $('#aceEditor_container_' + editorID).css(holderElement.position()).width(holderElement.width()).height(holderElement.height());

                            // Launch the editor and set the theme.
                            var aceEditor = ace.edit("aceEditor_" + editorID);
                            aceEditor.setTheme("ace/theme/chrome");
                            aceEditor.setShowPrintMargin(0);
                            aceEditor.setReadOnly(editor.readOnly);
                            aceEditor.getSession().setValue(editor.getData());
                            aceEditor.getSession().setUseWrapMode(false);
                            ace.config.loadModule('ace/ext/language_tools', function () {
                                aceEditor.setOptions({
                                    enableBasicAutocompletion: true,
                                    enableSnippets: true
                                });
                            });
                            aceEditor.$blockScrolling = Infinity;

                            if (editor.aceEditorOptions && editor.aceEditorOptions.mode) {
                                aceEditor.getSession().setMode(editor.aceEditorOptions.mode);
                            } else {
                                aceEditor.getSession().setMode("ace/mode/html");
                            }

                            // Set the z-index for ACEEditor really high.
                            $('#aceEditor_container_' + editorID).css('z-index', bcms.getHighestZindex() + 1);

                            // CUSTOM FUNCTIONS
                            //Changes source editor line wrap
                            var toggleLineWrapChange = function() {
                                aceEditor.getSession().setUseWrapMode(!aceEditor.getSession().getUseWrapMode());
                            };

                            // This function checks to see if we are returning to design view.  If so, purge all the ACE stuff.
                            var returnToDesignView = function(e) {

                                bcms.logger.trace('Before Command Exec: ' + e.data.name);

                                if (e.data.name == 'source') {

                                    // Set the value of the editor.
                                    bcms.logger.trace('going back to CKEditor Design view');

                                    // Set the data of the CKEditor to the value of ACE Editor.
                                    var currentValue = aceEditor.getSession().getValue();
                                    editor.setData(currentValue, function () {
                                        bcms.logger.trace('change saved');
                                    }, false);
                                    $('#' + editor.name).val(currentValue);

                                    // Destroy the editor.
                                    aceEditor.destroy();

                                    // Remove the container.
                                    $('#aceEditor_container_' + editorID).remove();

                                    // Remove the listeners.
                                    editor.removeListener('beforeCommandExec', returnToDesignView);
                                    editor.removeListener('resize', resizeACE);
                                    editor.removeListener('afterCommandExec', maximizeACE);

                                    editor.fire('dataReady');
                                }
                            };

                            // This function will update the z-index of the ACE Editor based on whether we are maximized or not.
                            var maximizeACE = function(e) {
                                bcms.logger.trace('After Command Exec: ' + e.data.name);

                                // If they are maximizing it.
                                if (e.data.name == 'maximize') {

                                    // If maximixed.
                                    if (e.data.command.state == 1) {
                                        $('#aceEditor_container_' + editorID).css('z-index', bcms.getHighestZindex() + 1);
                                    } else {
                                        $('#aceEditor_container_' + editorID).css('z-index', 'auto');
                                    }
                                }
                                aceEditor.blur();
                                aceEditor.focus();
                                // TODO: check why this is commented out by the plug-in creator.
                                // If we are still in source view, remove this listener.
                                //if (e.data.name == 'source'){
                                //    e.removeListener();
                                //}
                            };

                            // Resize
                            // This function will resize ACE editor to match the holderElement object's position.
                            var resizeACE = function() {
                                bcms.logger.trace('resizing ace');
                                // Make the editor container fill the space.
                                $('#aceEditor_container_' + editorID).css(holderElement.position()).width(holderElement.width()).height(holderElement.height());
                                aceEditor.resize(true);
                                aceEditor.blur();
                                aceEditor.focus();
                            };

                            // Update
                            // This function updates the value of CKeditor.
                            var updateCKEditor = function () {
                                bcms.logger.trace('change detected');

                                // Set the data of the CKEditor to the value of ACE Editor.
                                var currentValue = aceEditor.getSession().getValue();
                                editor.setData(currentValue, function () {
                                    bcms.logger.trace('change saved');
                                }, false);
                                $('#' + editor.name).val(currentValue);

                                return false;
                            };

                            // BIND EVENTS
                            // When the ace editor changes, update CKEditor's source code.
                            aceEditor.getSession().on('change', updateCKEditor);

                            // Commit source data back into 'source' mode.
                            editor.on('beforeCommandExec', returnToDesignView);

                            // Change Line Wrap Mode.
                            editor.on('onLineWrapChange', toggleLineWrapChange);

                            // When the editor fires the 'resize' event call the resize function.
                            editor.on('resize', resizeACE);

                            // Run this after a command executes in CKEditor.
                            editor.on('afterCommandExec', maximizeACE);

                            editor.aceEditor = aceEditor;
                        }
                    });

                    // If we are sending them back to the WYSIWYG editor.
                    editor.on('instanceReady', function (e) {
                        bcms.logger.trace('instance ready');
                        e.removeListener();
                        if (editor.mode == 'wysiwyg') {
                            var thisData = editor.getData().indexOf('<?php');
                            if (thisData !== -1) {
                                editor.execCommand('source');
                            };
                        }
                    });

                    // In case we want to do anything when CKEditor fires the 'dataReady' event.
                    editor.on('dataReady', function (e) {
                        bcms.logger.trace('data ready');
                    });
                });
        }
    });
})();