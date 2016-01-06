 BEGIN
	INSERT INTO [bcms_root].[CategoryTreeCategorizableItems] ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
	SELECT 1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, '75E6C021-1D1F-459E-A416-D18477BF2020'
	FROM [bcms_root].[CategoryTrees] as source
	WHERE source.IsDeleted = 0 and NOT EXISTS (SELECT * FROM [bcms_root].[CategoryTreeCategorizableItems] WHERE CategoryTreeId = source.Id and CategorizableItemId = '75E6C021-1D1F-459E-A416-D18477BF2020')
 END
