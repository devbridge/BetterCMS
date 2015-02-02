 BEGIN
    MERGE bcms_Pages.PageCategories AS target
    USING ( select Id, CategoryId from bcms_pages.Pages where CategoryId is not null) AS source (PageId, CategoryId)
    ON ( target.PageId = source.PageId and target.CategoryId = source.CategoryId)
	WHEN NOT MATCHED THEN
    INSERT ( PageId, CategoryId)
    VALUES (source. PageId, source.CategoryId);
 END