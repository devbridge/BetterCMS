 BEGIN
    MERGE bcms_Pages.PageCategories AS target
    USING ( select Id, CategoryId from bcms_pages.Pages where CategoryId is not null) AS source (PageId, CategoryId)
    ON ( target.PageId = source.PageId and target.CategoryId = source.CategoryId)
	WHEN NOT MATCHED THEN
    INSERT ( PageId, CategoryId, Version, CreatedByUser, CreatedOn, ModifiedByUser, ModifiedOn)
    VALUES (source. PageId, source.CategoryId, 0, 'Admin', getDate(), 'Admin', getDate());
 END