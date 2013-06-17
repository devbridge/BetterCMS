using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Api.Attributes;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class UpdateBlogPostRequest
    {
        [EmptyGuidValidation(ErrorMessage = "Blog post Id must be set.")]
        public Guid Id { get; set; }

        public virtual int Version { get; set; }

        [Required(ErrorMessage = "Blog post title must be set.")]
        [StringLength(MaxLength.Name, ErrorMessage = "Blog post title must be the string with a maximum length of {1}.")]
        public virtual string Title { get; set; }

        [StringLength(MaxLength.Text, ErrorMessage = "Blog post intro text must be the string with a maximum length of {1}.")]
        public virtual string IntroText { get; set; }

        public DateTime LiveFromDate { get; set; }

        public DateTime? LiveToDate { get; set; }

        public Guid? ImageId { get; set; }

        public Guid? AuthorId { get; set; }

        public Guid? CategoryId { get; set; }

        public IList<string> Tags { get; set; }
    }
}