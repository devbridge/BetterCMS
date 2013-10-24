UPDATE bcms_blog.BlogPosts
SET ActivationDate = bcms_pages.HtmlContents.ActivationDate,
	ExpirationDate = bcms_pages.HtmlContents.ExpirationDate
	
FROM bcms_blog.BlogPosts 
		LEFT JOIN bcms_root.PageContents ON (bcms_blog.BlogPosts.Id = bcms_root.PageContents.PageId and bcms_root.PageContents.IsDeleted <> 1)
		LEFT JOIN bcms_root.Contents ON (bcms_root.Contents.Id = bcms_root.PageContents.ContentId and bcms_root.Contents.IsDeleted <> 1)
		LEFT JOIN bcms_pages.HtmlContents ON (bcms_pages.HtmlContents.Id = bcms_root.Contents.Id)

WHERE (bcms_blog.BlogPosts.ActivationDate != bcms_pages.HtmlContents.ActivationDate	 OR
	  bcms_blog.BlogPosts.ActivationDate IS NULL AND bcms_pages.HtmlContents.ActivationDate IS NOT NULL OR
	  bcms_blog.BlogPosts.ActivationDate IS NOT NULL AND bcms_pages.HtmlContents.ActivationDate IS NULL OR
	  
      bcms_blog.BlogPosts.ExpirationDate != bcms_pages.HtmlContents.ExpirationDate OR
	  bcms_blog.BlogPosts.ExpirationDate IS NULL AND bcms_pages.HtmlContents.ExpirationDate IS NOT NULL OR
	  bcms_blog.BlogPosts.ExpirationDate IS NOT NULL AND bcms_pages.HtmlContents.ExpirationDate IS NULL)
	  
	  AND bcms_pages.HtmlContents.Id IS NOT NULL