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
(function () {
    CKEDITOR.plugins.add('cms-modelvalues',
    {
        requires: ['richcombo'],
        init: function (editor) {
            var config = editor.config,
               lang = editor.lang.format,
               tags = editor.smartTags;

            // Create style objects for all defined styles.
            editor.ui.addRichCombo('CmsModelValues',
               {
                   label: "Smart tags",
                   title: "Smart tags",
                   voiceLabel: "Smart tags",
                   className: 'cke_format',
                   multiSelect: false,
                   modes: { wysiwyg: 1, source: 1 },

                   panel:
                   {
                       css: [config.contentsCss, CKEDITOR.basePath + 'skins/' + config.skin + '/editor.css'],
                       voiceLabel: lang.panelVoiceLabel
                   },

                   init: function () {
                       var tag;
                      
                       for (tag in tags) {
                           if ((editor.cmsEditorType === editor.cmsEditorTypes.page && tags[tag].id === 'widgetOption')) {
                               continue;
                           }
                           this.add(tags[tag].text, tags[tag].title, tags[tag].title);
                       }
                   },

                   onClick: function (value) {
                       editor.focus();
                       editor.fire('saveSnapshot');
                       editor.addHtml(value);
                       editor.fire('saveSnapshot');
                   }
               });
        }
    });
})();