using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [DataContract]
    [Serializable]
    public class GetBlogPostPropertiesResponse : ResponseBase<BlogPostPropertiesModel>
    {
        /// <summary>
        /// Gets or sets the blog post HTML contents.
        /// </summary>
        /// <value>
        /// The blog post HTML content.
        /// </value>
        [DataMember]
        public string HtmlContent { get; set; }

        /// <summary>
        /// Gets or sets the original text.
        /// </summary>
        /// <value>
        /// The original text.
        /// </value>
        [DataMember]
        public string OriginalText { get; set; }

        /// <summary>
        /// Gets or sets the content text mode.
        /// </summary>
        /// <value>
        /// The content text mode.
        /// </value>
        [DataMember]
        public ContentTextMode? ContentTextMode { get; set; }

        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        [DataMember]
        public LayoutModel Layout { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public IList<CategoryModel> Categories { get; set; }
        
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [DataMember]
        public LanguageModel Language { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [DataMember]
        public AuthorModel Author { get; set; }

        /// <summary>
        /// Gets or sets the list of tags.
        /// </summary>
        /// <value>
        /// The list of tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<TagModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the main image.
        /// </summary>
        /// <value>
        /// The main image.
        /// </value>
        [DataMember]
        public ImageModel MainImage { get; set; }

        /// <summary>
        /// Gets or sets the featured image.
        /// </summary>
        /// <value>
        /// The featured image.
        /// </value>
        [DataMember]
        public ImageModel FeaturedImage { get; set; }

        /// <summary>
        /// Gets or sets the secondary image.
        /// </summary>
        /// <value>
        /// The secondary image.
        /// </value>
        [DataMember]
        public ImageModel SecondaryImage { get; set; }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [DataMember]
        public MetadataModel MetaData { get; set; }

        /// <summary>
        /// Gets or sets the technical information (content, page content, region ids).
        /// </summary>
        /// <value>
        /// The technical information (content, page content, region ids).
        /// </value>
        [DataMember]
        public TechnicalInfoModel TechnicalInfo { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<AccessRuleModel> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets the list of child contents option values.
        /// </summary>
        /// <value>
        /// The list of child contents option values.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<ChildContentOptionValuesModel> ChildContentsOptionValues { get; set; }
    }
}