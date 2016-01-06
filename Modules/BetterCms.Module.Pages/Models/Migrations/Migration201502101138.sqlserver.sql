  BEGIN
	INSERT INTO [bcms_root].[CategoryTreeCategorizableItems] ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
	SELECT 1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, 'DC861498-FCD1-4F19-9C75-AE71916EF7BF'
	FROM [bcms_root].[CategoryTrees] as source
	WHERE source.IsDeleted = 0 and NOT EXISTS (SELECT * FROM [bcms_root].[CategoryTreeCategorizableItems] WHERE CategoryTreeId = source.Id and CategorizableItemId = 'DC861498-FCD1-4F19-9C75-AE71916EF7BF')
 END
