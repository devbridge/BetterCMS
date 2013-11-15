(function () {
    CKEDITOR.plugins.add('cms-modelvalues',
    {
        requires: ['richcombo'],
        init: function (editor) {
            var config = editor.config,
               lang = editor.lang.format;

            var tags = [];
            //this.add('value', 'drop_text', 'drop_label');
            tags.push(["{{CMSPAGETITLE}}", "Title", "Title"]);
            tags.push(["{{CMSPAGEURL}}", "Url", "Url"]);
            tags.push(["{{CMSPAGECREATIONDATE:yyyy-MM-dd}}", "Creation Date", "Creation Date"]);
            tags.push(["{{CMSPAGEOPTION:OptionKey}}", "Option", "Option"]);

            // Create style objects for all defined styles.
            editor.ui.addRichCombo('CmsModelValues',
               {
                   label: "CMS data",
                   title: "CMS data",
                   voiceLabel: "CMS data",
                   className: 'cke_format',
                   multiSelect: false,

                   panel:
                   {
                       // TODO: css: [config.contentsCss, CKEDITOR.getUrl(editor.skinPath + 'editor.css')],
                       voiceLabel: lang.panelVoiceLabel
                   },

                   init: function () {
                       var tag;
                      
                       for (tag in tags) {
                           this.add(tags[tag][0], tags[tag][1], tags[tag][2]);
                       }
                   },

                   onClick: function (value) {
                       editor.focus();
                       editor.fire('saveSnapshot');
                       editor.insertHtml(value);
                       editor.fire('saveSnapshot');
                   }
               });
        }
    });
})();