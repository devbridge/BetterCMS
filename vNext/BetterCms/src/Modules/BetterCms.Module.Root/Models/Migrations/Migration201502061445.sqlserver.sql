 BEGIN
    MERGE bcms_root.WidgetCategories AS target
    USING ( select Id, CategoryId from bcms_root.Widgets where CategoryId is not null) AS source (WidgetId, CategoryId)
    ON ( target.WidgetId = source.WidgetId and target.CategoryId = source.CategoryId)
	WHEN NOT MATCHED THEN
    INSERT ( WidgetId, CategoryId, Version, CreatedByUser, CreatedOn, ModifiedByUser, ModifiedOn)
    VALUES (source.WidgetId, source.CategoryId, 0, 'Admin', getDate(), 'Admin', getDate());
 END