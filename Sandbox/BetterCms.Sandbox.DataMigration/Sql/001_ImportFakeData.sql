/*
* LAYOUTS
*/
INSERT INTO bcms_root.layouts (
	Name
	, LayoutPath
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
)
SELECT * FROM (
	SELECT 
		'One column' AS Name
		, '~/Views/Shared/Layouts/01.cshtml' AS MasterPagePath
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Two columns' AS Name
		, '~/Views/Shared/Layouts/02.cshtml' AS MasterPagePath
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Two columns and header' AS Name
		, '~/Views/Shared/Layouts/03.cshtml' AS MasterPagePath
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Two columns, header and footer' AS Name
		, '~/Views/Shared/Layouts/04.cshtml' AS MasterPagePath
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, getdate() AS DeletedOn, 'Admin' as DeletedByUser
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_root.layouts l WHERE l.name = x.name AND l.IsDeleted = x.IsDeleted)

/*
* REGIONS
*/
INSERT INTO bcms_root.Regions (
	Name, RegionIdentifier
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
)
SELECT * FROM (
	SELECT 
		'CMSHeader' AS Name, 'CMSHeader' As RegionIdentifier
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'CMSFooter' AS Name, 'CMSFooter' As RegionIdentifier
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'CMSLeftColumn' AS Name, 'CMSLeftColumn' As RegionIdentifier
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'CMSRightColumn' AS Name, 'CMSRightColumn' As RegionIdentifier
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_root.Regions r WHERE r.RegionIdentifier = x.RegionIdentifier AND r.IsDeleted = 0)

/*
* LAYOUT REGIONS
*/
INSERT INTO bcms_root.LayoutRegions (
	LayoutId, RegionId, [Version], CreatedByUser,ModifiedByUser
)
SELECT * FROM (
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'One Column') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSLeftColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
		
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two Columns') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSLeftColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two Columns') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSRightColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns and header') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSHeader') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns and header') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSLeftColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns and header') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSRightColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns, header and footer') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSHeader') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns, header and footer') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSLeftColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns, header and footer') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSRightColumn') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
	UNION
	
	SELECT (SELECT top 1 Id FROM bcms_root.layouts where IsDeleted=0 AND Name = 'Two columns, header and footer') AS LayoutId
		, (SELECT top 1 Id FROM bcms_root.regions where IsDeleted=0 AND Name = 'CMSFooter') As RegionId
		, 1 as [Version]
		, '' as CreatedByUser
		, '' as ModifiedByUser
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_root.LayoutRegions r WHERE r.RegionId = x.RegionId AND r.LayoutId  = r.LayoutId)

/*
* AUTHORS
*/
INSERT INTO bcms_pages.authors (
	FirstName, LastName, DisplayName
	, Title, Email, Twitter
	, ProfileImageUrl, ProfileThumbnailUrl, ShortDescription, LongDescription
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
)
SELECT * FROM (
	SELECT 
		'Aurimas' as FirstName, 'Adomavicius' as LastName, 'Aurimas Adomavicius' as DisplayName
		, 'Mr.' as Title, 'aurimas@devbridge.com' as Email, '---' as Twitter
		, 'http://www.devbridge.com/Content/employee/fullimages/aurimas.png' as ProfileImageUrl
		, 'http://www.devbridge.com/Content/employee/fullimages/aurimas.png' as ProfileThumbnailUrl
		, '' as ShortDescription, '' as LongDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Audrunas' as FirstName, 'Matonis' as LastName, 'Audrunas Matonis' as DisplayName
		, 'Mr.' as Title, 'audrunas@devbridge.com' as Email, '---' as Twitter
		, 'http://www.devbridge.com/Content/employee/fullimages/audrunas.png' as ProfileImageUrl
		, 'http://www.devbridge.com/Content/employee/fullimages/audrunas.png' as ProfileThumbnailUrl
		, '' as ShortDescription, '' as LongDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Martin' as FirstName, 'Stasaitis' as LastName, 'Martin Stasaitis' as DisplayName
		, 'Mr.' as Title, 'martin@devbridge.com' as Email, '---' as Twitter
		, 'http://www.devbridge.com/Content/employee/fullimages/martin.png' as ProfileImageUrl
		, 'http://www.devbridge.com/Content/employee/fullimages/martin.png' as ProfileThumbnailUrl
		, '' as ShortDescription, '' as LongDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
	
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_pages.Authors a WHERE a.DisplayName = x.DisplayName AND a.IsDeleted = 0)

/*
* CATEGORIES
*/
INSERT INTO bcms_root.Categories (
	Name
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
)
SELECT * FROM (
	SELECT 
		'Promotional Sliders' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Authors' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Other Misc Stuff' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
	
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_root.Categories c WHERE c.Name = x.Name AND c.IsDeleted = 0)

/*
* REDIRECTS
*/
INSERT INTO bcms_pages.Redirects (
	PageUrl, RedirectUrl
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
)
SELECT * FROM (
	SELECT 
		'/page-1-old/' AS PageUrl, '/page-1-new/' As RedirectUrl
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'/page-2-old/' AS PageUrl, '/page-2-new/' As RedirectUrl
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'/page-3-old/' AS PageUrl, '/page-3-new/' As RedirectUrl
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_pages.Redirects r WHERE r.PageUrl = x.PageUrl AND r.RedirectUrl = x.RedirectUrl AND r.IsDeleted = 0)

/*
* TAGS
*/
INSERT INTO bcms_root.Tags (
	Name
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
)
SELECT * FROM (
	SELECT 
		'Page Tag 1' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Misc Tag 2' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Various Tag' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Mark' As Name
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
	
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_root.Tags c WHERE c.Name = x.Name AND c.IsDeleted = 0)

/*
* ROOT.PAGES
*/
INSERT INTO bcms_root.Pages (
	PageUrl, Title
	, IsPublished
	, MetaTitle, MetaKeywords, MetaDescription
	, Version, IsDeleted
	, CreatedOn, CreatedByUser
	, ModifiedOn, ModifiedByUser
	, DeletedOn, DeletedByUser
	, LayoutId
)
SELECT * FROM (
	SELECT 
		'/page-01/' as PageUrl, 'Page with one column layout' as Title
		, 1 as IsPublished
		, 'One column page meta title' as MetaTitle
		, 'One column page meta keywords' as MetaKeywords
		, 'One column page meta description' as MetaDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		, (select top 1 Id from bcms_root.Layouts) as LayoutId
	
	UNION
	
	SELECT 
		'/page-02/' as PageUrl, 'Page with two columns layout' as Title
		, 0 as IsPublished
		, 'Two columns page meta title' as MetaTitle
		, 'Two columns page meta keywords' as MetaKeywords
		, 'Two columns page meta description' as MetaDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		, (select top 1 Id from bcms_root.Layouts) as LayoutId
		
	UNION
	
	SELECT 
		'/page-03/' as PageUrl, 'Page with two columns with header layout' as Title
		, 1 as IsPublished
		, 'Two columns with header page meta title' as MetaTitle
		, 'Two columns with header page meta keywords' as MetaKeywords
		, 'Two columns with header page meta description' as MetaDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		, (select top 1 Id from bcms_root.Layouts) as LayoutId
		
	UNION
	
	SELECT 
		'/page-04/' as PageUrl, 'Page with two columns with header and footer layout' as Title
		, 0 as IsPublished
		, 'Two columns with header and footer page meta title' as MetaTitle
		, 'Two columns with header and footer page meta keywords' as MetaKeywords
		, 'Two columns with header and footer page meta description' as MetaDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		, (select top 1 Id from bcms_root.Layouts) as LayoutId

		UNION	
			SELECT 
		'/404/' as PageUrl, 'Page Not Found' as Title
		, 0 as IsPublished
		, 'Page Not Found meta title' as MetaTitle
		, 'Page Not Found meta keywords' as MetaKeywords
		, 'Page Not Found meta description' as MetaDescription
		, 1 AS Version, 0 As IsDeleted
		, getdate() AS CreatedOn, 'Admin' as CreatedByUser
		, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser
		, NULL AS DeletedOn, NULL as DeletedByUser
		, (select top 1 Id from bcms_root.Layouts) as LayoutId
		
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_root.Pages p WHERE p.Title = x.Title AND p.IsDeleted = 0)

/*
* PAGES.PAGES [bcms_pages.Pages]
*/
INSERT INTO bcms_pages.Pages (
	Id
)
SELECT * FROM (
	SELECT 
		(SELECT top 1 Id FROM bcms_root.Pages WHERE Title = N'Page with one column layout' AND IsDeleted = 0) AS Id		
		
	UNION
	
	SELECT 
		(SELECT top 1 Id FROM bcms_root.Pages WHERE Title = N'Page with two columns layout' AND IsDeleted = 0) AS Id
		
		
	UNION
	
	SELECT 
		(SELECT top 1 Id FROM bcms_root.Pages WHERE Title = N'Page with two columns with header layout' AND IsDeleted = 0) AS Id
		
		
	UNION
	
	SELECT 
		(SELECT top 1 Id FROM bcms_root.Pages WHERE Title = N'Page with two columns with header and footer layout' AND IsDeleted = 0) AS Id
		UNION
	
	SELECT 
		(SELECT top 1 Id FROM bcms_root.Pages WHERE Title = N'Page Not Found' AND IsDeleted = 0) AS Id	
	
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_pages.Pages p WHERE p.Id = x.Id)

/*
* MEDIAS - FOLDER [bcms_media.Medias]
*/
INSERT INTO bcms_media.Medias (
	Title, [Type]
	, Version, IsDeleted, CreatedOn, CreatedByUser, ModifiedOn, ModifiedByUser, DeletedOn, DeletedByUser)
SELECT * FROM (
	SELECT 
		'Folder 1' As Title, 1 as [Type]
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION
	
	SELECT 
		'Folder 2' As Title, 1 as [Type]
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION SELECT 
		'Folder 3' As Title, 1 as [Type]
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION SELECT 
		'One more folder' As Title, 1 as [Type]
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_media.Medias m WHERE m.Title = x.Title AND m.Type = x.Type AND m.IsDeleted = 0)

/*
* MEDIAS - FOLDERS [bcms_media.MediaFolders]
*/
INSERT INTO bcms_media.MediaFolders (Id)
SELECT * FROM (
	SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Folder 1') AS Id
	UNION
	SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Folder 2') AS Id
	UNION
	SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Folder 3') AS Id
	UNION
	SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'One more folder') AS Id
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_media.MediaFolders m WHERE m.Id = x.Id)

/*
* MEDIAS - FOLDER [bcms_media.Images]
*/
INSERT INTO bcms_media.Medias (
	Title, [Type], FolderId
	, Version, IsDeleted, CreatedOn, CreatedByUser, ModifiedOn, ModifiedByUser, DeletedOn, DeletedByUser)
SELECT * FROM (
	SELECT 
		'Image1.jpg' As Title, 1 as [Type]
		, (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title='Folder 1') AS FolderId
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
		
	UNION SELECT 
		'Image2.jpg' As Title, 1 as [Type]
		, (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title='Folder 1') AS FolderId
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
				
	UNION SELECT 
		'Image3.jpg' As Title, 1 as [Type]
		, (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title='Folder 1') AS FolderId
		, 1 AS Version, 0 As IsDeleted, getdate() AS CreatedOn, 'Admin' as CreatedByUser, getdate() AS ModifiedOn, 'Admin' as ModifiedByUser, NULL AS DeletedOn, NULL as DeletedByUser
	
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_media.Medias m WHERE m.Title = x.Title AND m.Type = x.Type AND m.IsDeleted = 0)

/*
* MEDIAS - FILES [bcms_media.MediaFiles]
*/
INSERT INTO bcms_media.MediaFiles (
	Id, 
	FileName, FileUri, PublicUrl, Size, IsTemporary, IsStored
)
SELECT * FROM (
	SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Image1.jpg') AS Id,
		FileName = 'Image1.jpg', 
		FileUri = 'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG', 
		PublicUrl = 'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG', Size = 300, IsTemporary = 0, IsStored = 1
	
	UNION SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Image2.jpg') AS Id,
		FileName = 'Image2.jpg', 
		FileUri = 'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG', 
		PublicUrl = 'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG', Size = 300, IsTemporary = 0, IsStored = 1
	
	UNION SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Image3.jpg') AS Id,
		FileName = 'Image3.jpg', 
		FileUri = 'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG', 
		PublicUrl = 'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG', Size = 300, IsTemporary = 0, IsStored = 1
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_media.MediaFiles m WHERE m.Id = x.Id)

/*
* MEDIAS - IMAGES [bcms_media.MediaImages]
*/
INSERT INTO bcms_media.MediaImages (
	Id, 
	Width, Height, 
	OriginalWidth, OriginalHeight, OriginalSize, OriginalUri, 
	ThumbnailWidth, ThumbnailHeight, ThumbnailSize, ThumbnailUri)
SELECT * FROM (
	SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Image1.jpg') AS Id,
		300 AS Width, 300 AS Height, 
		300 AS OriginalWidth, 300 AS OriginalHeight, 300 AS OriginalSize, 
		'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG' AS OriginalUri, 
		50 AS ThumbnailWidth, 50 AS ThumbnailHeight, 50 AS ThumbnailSize, 
        'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG' AS ThumbnailUri
	
	UNION SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Image2.jpg') AS Id,
		300 AS Width, 300 AS Height, 
		300 AS OriginalWidth, 300 AS OriginalHeight, 300 AS OriginalSize, 
		'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG' AS OriginalUri, 
		50 AS ThumbnailWidth, 50 AS ThumbnailHeight, 50 AS ThumbnailSize, 
        'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG' AS ThumbnailUri
	
	UNION SELECT (SELECT top 1 Id FROM bcms_media.Medias WHERE IsDeleted=0 AND Type=1 AND Title = 'Image3.jpg') AS Id,
		300 AS Width, 300 AS Height, 
		300 AS OriginalWidth, 300 AS OriginalHeight, 300 AS OriginalSize, 
		'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG' AS OriginalUri, 
		50 AS ThumbnailWidth, 50 AS ThumbnailHeight, 50 AS ThumbnailSize, 
        'http://upload.wikimedia.org/wikipedia/commons/c/cd/Panda_Cub_from_Wolong,_Sichuan,_China.JPG' AS ThumbnailUri
) x
WHERE NOT EXISTS (SELECT 1 FROM bcms_media.MediaImages m WHERE m.Id = x.Id)