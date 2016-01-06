 BEGIN
	INSERT INTO [bcms_root].[CategoryTreeCategorizableItems] ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
	SELECT 1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, '11F2C2CF-BF7C-467C-B424-E8C368F88183'
	FROM [bcms_root].[CategoryTrees] as source
	WHERE source.IsDeleted = 0 and NOT EXISTS (SELECT * FROM [bcms_root].[CategoryTreeCategorizableItems] WHERE CategoryTreeId = source.Id and CategorizableItemId = '11F2C2CF-BF7C-467C-B424-E8C368F88183')
 END

BEGIN
	INSERT INTO [bcms_root].[CategoryTreeCategorizableItems] ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
	SELECT 1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, '90EE1A64-A469-4F7A-A056-AE7BDC6C2D06'
	FROM [bcms_root].[CategoryTrees] as source
	WHERE source.IsDeleted = 0 and NOT EXISTS (SELECT * FROM [bcms_root].[CategoryTreeCategorizableItems] WHERE CategoryTreeId = source.Id and CategorizableItemId = '90EE1A64-A469-4F7A-A056-AE7BDC6C2D06')
END
