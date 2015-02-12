 BEGIN
	MERGE [bcms_root].[CategoryTreeCategorizableItems] AS target
    USING ( SELECT Id FROM [bcms_root].[CategoryTrees] WHERE [IsDeleted] = 0) AS source (Id)
    ON ( target.CategoryTreeId = source.Id and target.CategorizableItemId = '75E6C021-1D1F-459E-A416-D18477BF2020')
	WHEN NOT MATCHED THEN
    INSERT ([Version], [CreatedByUser], [CreatedOn], [ModifiedByUser], [ModifiedOn], [CategoryTreeId], [CategorizableItemId])
    VALUES (1, 'Better CMS', getDate(), 'Better CMS', getDate(), source.Id, '75E6C021-1D1F-459E-A416-D18477BF2020');
 END
