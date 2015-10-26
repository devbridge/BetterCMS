using System;
using System.Collections.Generic;

namespace BetterCms.Module.Installation.Models.Blog
{
    public class BlogItem
    {
        public string Title { get; set; }

        public string IntroText { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Url { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Categories { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Title: {0}, IntroText: {1}, PublishedOn: {2}, Url: {3}, Author: {4}, ImageUrl: {5}", Title, IntroText, PublishedOn, Url, Author, ImageUrl);
        }
    }
}