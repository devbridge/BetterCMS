(function () {
    CKEDITOR.plugins.add('cms-modelvalues',
    {
        requires: ['richcombo'],
        init: function (editor) {
            var config = editor.config,
               lang = editor.lang.format;

            var tags = [];
            tags.push(["{{CmsPageTitle}}", "Page title", "Page title"]);
            tags.push(["{{CmsPageUrl}}", "Page url", "Page url"]);
            tags.push(["{{CmsPageId}}", "Page id", "Page id"]);
            tags.push(["{{CmsPageCreatedOn:yyyy-MM-dd}}", "Page creation date", "Page creation date"]);
            tags.push(["{{CmsPageModifiedOn:yyyy-MM-dd}}", "Page last modification date", "Page last modification date"]);
            tags.push(["{{CmsPageOption:OptionKey}}", "Page option", "Page option"]);
            tags.push(["{{CmsPageMetaTitle}}", "Page meta title", "Page meta title"]);
            tags.push(["{{CmsPageMetaKeywords}}", "Page meta keywords", "Page meta keywords"]);
            tags.push(["{{CmsPageMetaDescription}}", "Page meta description", "Page meta description"]);
            // TODO: tags.push(["{{CmsPageMainImageUrl}}", "Page main image URL", "Page main image URL"]);
            // TODO: tags.push(["{{CmsPageSecondaryImageUrl}}", "Page secondary image URL", "Page secondary image URL"]);
            // TODO: tags.push(["{{CmsPageFeaturedImageUrl}}", "Page featured image URL", "Page featured image URL"]);
            // TODO: tags.push(["{{CmsPageCategory}}", "Page category name", "Page category name"]);

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
                       css: [config.contentsCss, CKEDITOR.basePath + 'skins/' + config.skin + '/editor.css'],
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