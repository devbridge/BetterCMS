 BEGIN
	MERGE [bcms_root].[CategoryTreeCategorizableItems] AS target
    USING ( SELECT Id FROM [bcms_root].[CategoryTrees] WHERE [IsDeleted] = 0) AS source (Id)
    ON ( target.CategoryTreeId = source.Id and target.CategorizableItemId = '11F2C2CF-BF7C-467C-B424-E8C368F88183')
	WHEN NOT MATCHED THEN
    INSERT ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
    VALUES (1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, '11F2C2CF-BF7C-467C-B424-E8C368F88183');
 END
 BEGIN
	MERGE [bcms_root].[CategoryTreeCategorizableItems] AS target
    USING ( SELECT Id FROM [bcms_root].[CategoryTrees] WHERE [IsDeleted] = 0) AS source (Id)
    ON ( target.CategoryTreeId = source.Id and target.CategorizableItemId = '90EE1A64-A469-4F7A-A056-AE7BDC6C2D06')
	WHEN NOT MATCHED THEN
    INSERT ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
    VALUES (1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, '90EE1A64-A469-4F7A-A056-AE7BDC6C2D06');
 END
