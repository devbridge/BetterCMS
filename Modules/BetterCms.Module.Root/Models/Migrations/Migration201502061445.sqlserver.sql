 BEGIN
	INSERT INTO bcms_root.WidgetCategories (WidgetId, CategoryId, Version, CreatedByUser, CreatedOn, ModifiedByUser, ModifiedOn)
	SELECT source.Id, source.CategoryId, 0, 'Admin', getDate(), 'Admin', getDate()
	FROM bcms_root.Widgets as source
	WHERE source.CategoryId is not null and NOT EXISTS (SELECT * FROM bcms_root.WidgetCategories WHERE CategoryId = source.CategoryId and WidgetId = source.Id)
 END