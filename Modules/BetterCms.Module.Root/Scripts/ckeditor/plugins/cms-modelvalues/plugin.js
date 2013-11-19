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
            tags.push(["{{CmsPageMetaTitle}}", "Meta title", "Meta title"]);
            tags.push(["{{CmsPageMetaKeywords}}", "Meta keywords", "Meta keywords"]);
            tags.push(["{{CmsPageMetaDescription}}", "Meta description", "Meta description"]);
            tags.push(["{{CmsPageMainImageUrl}}", "Main image URL", "Main image URL"]);
            tags.push(["{{CmsPageSecondaryImageUrl}}", "Secondary image URL", "Secondary image URL"]);
            tags.push(["{{CmsPageFeaturedImageUrl}}", "Featured image URL", "Featured image URL"]);
            tags.push(["{{CmsPageCategory}}", "Category name", "Category name"]);
            tags.push(["{{CmsBlogAuthor}}", "Blog post author", "Blog post author"]);
            tags.push(["{{CmsBlogActivationDate:yyyy-MM-dd}}", "Blog post activation date", "Blog post activation date"]);
            tags.push(["{{CmsBlogExpirationDate:yyyy-MM-dd}}", "Blog post expiration date", "Blog post expiration date"]);

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
                           this.add(tags[tag][0], tags[tag][1], tags[tag][2]);
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