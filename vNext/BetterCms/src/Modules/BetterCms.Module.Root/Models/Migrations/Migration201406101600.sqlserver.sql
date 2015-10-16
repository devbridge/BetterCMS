IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = N'bcms_root' AND TABLE_NAME = N'PagesView')
	DROP VIEW [bcms_root].[PagesView]
GO

CREATE VIEW [bcms_root].[PagesView]
AS
      SELECT
			p.[Id],
			p.[IsDeleted],
			p.[Version],
			p.[CreatedOn],
			p.[CreatedByUser],
			p.[ModifiedOn],
			p.[ModifiedByUser],
			p.[DeletedOn],
			p.[DeletedByUser],

			IsInSitemap = CAST((case when (SELECT MAX(IsInSitemap)
						FROM (
							SELECT 0 as IsInSitemap

							UNION

							SELECT 1
                              FROM [bcms_pages].[SitemapNodes] AS n1
					    INNER JOIN [bcms_pages].[Sitemaps] AS s1 ON s1.Id = n1.SitemapId
							 WHERE (n1.PageId = p.Id OR n1.UrlHash = p.[PageUrlHash]) AND n1.IsDeleted = 0 AND s1.IsDeleted = 0

							UNION

							SELECT 1
                              FROM [bcms_pages].[SitemapNodeTranslations] AS t
					    INNER JOIN [bcms_pages].[SitemapNodes] AS n3 ON n3.Id = t.NodeId
					    INNER JOIN [bcms_pages].[Sitemaps] AS s3 ON s3.Id = n3.SitemapId
							 WHERE t.UrlHash = p.[PageUrlHash] AND t.IsDeleted = 0  AND n3.IsDeleted = 0 AND s3.IsDeleted = 0

							UNION

							SELECT 1
                              FROM [bcms_pages].[SitemapNodes] AS n1
					    INNER JOIN [bcms_pages].[Sitemaps] AS s1 ON s1.Id = n1.SitemapId
							 WHERE 
								   EXISTS (SELECT gp.Id
							                 FROM [bcms_root].[Pages] as gp
								            WHERE gp.LanguageGroupIdentifier = p.LanguageGroupIdentifier AND n1.PageId = gp.Id)
								   AND n1.IsDeleted = 0 AND s1.IsDeleted = 0
							) as x) = 0 then 0 else 1 end) AS BIT)

       FROM [bcms_root].[Pages] AS p
       WHERE p.IsDeleted = 0
GO