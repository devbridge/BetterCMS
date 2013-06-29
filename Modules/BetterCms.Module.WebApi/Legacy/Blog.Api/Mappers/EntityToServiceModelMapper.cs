using System;
using System.Linq.Expressions;

using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Api.Mappers
{
    public static class EntityToServiceModelMapper
    {
        public static readonly Expression<Func<BlogPost, BlogPostModel>> ToBlogPostModel = blog => new BlogPostModel
        {
            Id = blog.Id,
            Version = blog.Version,
            PageUrl = blog.PageUrl,
            Title = blog.Title,
            IntroText = blog.Description,
            CreatedOn = blog.CreatedOn,
            ModifiedOn = blog.ModifiedOn,
            CreatedByUser = blog.CreatedByUser,
            ModifiedByUser = blog.ModifiedByUser,
            Status = blog.Status,
            ActivationDate = blog.ActivationDate,
            ExpirationDate = blog.ExpirationDate,
            CategoryId = blog.Category.Id,
            CategoryName = blog.Category.Name,
            AuthorId = blog.Author.Id,
            AuthorName = blog.Author.Name,
            MainImageId = blog.Image.Id,
            MainImagePublicUrl = blog.Image.PublicUrl,
            FeaturedImageId = blog.FeaturedImage.Id,
            FeaturedImagePublicUrl = blog.FeaturedImage.PublicUrl,
            SecondaryImageId = blog.SecondaryImage.Id,
            SecondaryImagePublicUrl = blog.SecondaryImage.PublicUrl
        };

        public static readonly Expression<Func<Author, AuthorModel>> ToAuthor = author => new AuthorModel
        {
            Id = author.Id,
            Version = author.Version,
            Name = author.Name,
            CreatedOn = author.CreatedOn,
            ModifiedOn = author.ModifiedOn,
            CreatedByUser = author.CreatedByUser,
            ModifiedByUser = author.ModifiedByUser,
            ImageId = author.Image.Id,
            ImagePublicUrl = author.Image.PublicUrl
        };
    }
}