/*jslint unparam: true, white: true, browser: true, devel: true */
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="bcms.codeEditor.js" company="Devbridge Group LLC">
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
bettercms.define('bcms.codeEditor', ['bcms.jquery', 'bcms', 'bcms.htmlEditor', 'bcms.modal', 'ace'], function ($, bcms, htmlEditor, modal) {
    'use strict';

    var codeEditor = {},
        selectors = {
            inputFieldForJS: '.bcms-code-field-javascript',
            inputFieldForCSS: '.bcms-code-field-css',
            alCodeEditors: '.bcms-code-field-javascript, .bcms-code-field-css'
        },
        links = {},
        globalization = {},
        events = {},
        ckEditorConfig = {
            extraPlugins: 'cms-imagemanager,cms-filemanager,cms-modelvalues,aceeditor,cms-option',

            toolbar: [
                ['CmsImageManager', 'CmsFileManager', 'Image'],
                ['CmsOption', 'CmsModelValues'],
                ['Maximize']
            ],

            codeEditorMode: true,
            toolbarCanCollapse: false
        };

    // Assign objects to module
    codeEditor.selectors = selectors;
    codeEditor.links = links;
    codeEditor.globalization = globalization;
    codeEditor.events = events;

    function initCkEditor(textarea, mode, options, heightOptions) {
        var configuration = $.extend({
                    aceEditorOptions: {
                        mode: mode
                    },
                    height: textarea.outerHeight()
                },
                ckEditorConfig, options),
            id = textarea.attr('id'),
            isInitialized = textarea.data('isInitialized');

        if (!isInitialized) {
            htmlEditor.initializeHtmlEditor(id, null, configuration, true, heightOptions);
            textarea.data('isInitialized', true);
        }
    }

    codeEditor.initialize = function (container, dialog, options, heightOptions) {
        if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
            bcms.logger.info('ACE editor is enabled only on IE versions > 8.');
            return;
        }

        if (!ace) {
            bcms.logger.error('Failed to load ACE editor.');
            return;
        }

        var jsObjects = [],
            cssObjects = [];
        container.find(selectors.inputFieldForJS).each(function () {
            jsObjects.push($(this));
        });
        container.find(selectors.inputFieldForCSS).each(function () {
            cssObjects.push($(this));
        });

        modal.maximizeChildHeight($.merge(jsObjects, cssObjects), dialog);

        $.each(jsObjects, function() {
            initCkEditor($(this), "ace/mode/javascript", options, heightOptions);
        });
        $.each(cssObjects, function () {
            initCkEditor($(this), "ace/mode/css", options, heightOptions);
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