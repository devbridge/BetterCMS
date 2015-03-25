 BEGIN
	MERGE [bcms_root].[CategoryTreeCategorizableItems] AS target
    USING ( SELECT Id FROM [bcms_root].[CategoryTrees] WHERE [IsDeleted] = 0) AS source (Id)
    ON ( target.CategoryTreeId = source.Id and target.CategorizableItemId = 'B2F05159-74AF-4B67-AEB9-36B9CC9EED57')
	WHEN NOT MATCHED THEN
    INSERT ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
    VALUES (1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, 'B2F05159-74AF-4B67-AEB9-36B9CC9EED57');
 END