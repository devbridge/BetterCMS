BEGIN
	INSERT INTO bcms_pages.PageCategories (PageId, CategoryId, Version, CreatedByUser, CreatedOn, ModifiedByUser, ModifiedOn)
	SELECT source.Id, source.CategoryId, 0, 'Admin', getDate(), 'Admin', getDate()
	FROM bcms_pages.Pages as source
	WHERE source.CategoryId is not null and NOT EXISTS (SELECT * FROM bcms_pages.PageCategories WHERE PageId = source.Id and CategoryId = source.CategoryId)
END