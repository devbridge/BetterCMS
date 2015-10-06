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