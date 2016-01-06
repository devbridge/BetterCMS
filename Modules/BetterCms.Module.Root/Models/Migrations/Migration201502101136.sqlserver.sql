 BEGIN
	INSERT INTO [bcms_root].[CategoryTreeCategorizableItems] ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
	SELECT 1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, 'B2F05159-74AF-4B67-AEB9-36B9CC9EED57'
	FROM [bcms_root].[CategoryTrees] as source
	WHERE source.IsDeleted = 0 and NOT EXISTS (SELECT * FROM [bcms_root].[CategoryTreeCategorizableItems] WHERE CategoryTreeId = source.Id and CategorizableItemId = 'B2F05159-74AF-4B67-AEB9-36B9CC9EED57')
 END