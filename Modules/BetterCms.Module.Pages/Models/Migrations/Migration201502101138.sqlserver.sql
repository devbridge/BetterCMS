 BEGIN
	MERGE [bcms_root].[CategoryTreeCategorizableItems] AS target
    USING ( SELECT Id FROM [bcms_root].[CategoryTrees] WHERE [IsDeleted] = 0) AS source (Id)
    ON ( target.CategoryTreeId = source.Id and target.CategorizableItemId = 'DC861498-FCD1-4F19-9C75-AE71916EF7BF')
	WHEN NOT MATCHED THEN
    INSERT ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
    VALUES (1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, 'DC861498-FCD1-4F19-9C75-AE71916EF7BF');
 END
