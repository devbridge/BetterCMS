-- Fixed broken custom options
UPDATE [bcms_root].[PageContentOptions] 
SET [CustomOptionId] = 'FB118858-CD1F-4CC6-8C22-177652EEB2A7'
WHERE [Type] = 99 
	AND [CustomOptionId] IS NULL
	AND [IsDeleted] = 0
	AND [Key] IN (N'Albums folder', N'Album folder')