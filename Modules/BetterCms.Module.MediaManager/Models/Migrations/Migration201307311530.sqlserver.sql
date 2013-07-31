DECLARE @tree TABLE (
	Id int identity(1, 1) NOT NULL PRIMARY KEY,
	Title nvarchar(max),
	FolderId uniqueidentifier NULL,
	ids nvarchar(max)
);

WITH MediaFoldersTree AS (
	SELECT f.Id, m.FolderId as ParentId, 
		CAST('NULL' AS nvarchar(MAX)) AS [ParentIds]
	FROM bcms_media.MediaFolders f
		INNER JOIN bcms_media.Medias m
			ON m.Id = f.Id
	WHERE m.FolderId IS NULL

	UNION ALL

	SELECT f.Id, m.FolderId as ParentId, 
		CAST([ParentIds] + ' -> ' + CAST(m.FolderId as nvarchar(50)) AS nvarchar(MAX))
	FROM bcms_media.MediaFolders f
		INNER JOIN bcms_media.Medias m
			ON m.Id = f.Id
		INNER JOIN MediaFoldersTree t ON 
			t.Id = m.FolderId
	WHERE m.FolderId IS NOT NULL
)

INSERT INTO @tree (FolderId, ids)
SELECT Id, [ParentIds] + ' -> ' + CAST(Id as nvarchar(50)) FROM MediaFoldersTree

INSERT INTO @tree (FolderId, ids) VALUES (NULL, 'NULL')

DECLARE @count INT, @i INT, @childId UniqueIdentifier, @ids nvarchar(max), @stringId nvarchar(max)
SELECT @count = count(1) FROM @tree

SET @i = 1
WHILE @i <= @count
BEGIN
	SELECT @childId = FolderId, @ids = ids
	FROM @tree
	WHERE Id = @i

	WHILE LEN(@ids) > 0
	BEGIN
		IF PATINDEX('% -> %',@ids) > 0
		BEGIN
			SET @stringId = SUBSTRING(@ids, 0, PATINDEX('% -> %',@ids))
			SET @ids = SUBSTRING(@ids, LEN(@stringId + ' -> ') + 2, LEN(@ids))
		END
		ELSE
		BEGIN
			SET @stringId = @ids
			SET @ids = NULL
		END

		IF @stringId = 'NULL'
			INSERT INTO bcms_media.MediaFolderDependencies (
				Version, IsDeleted, CreatedOn, CreatedByuser, ModifiedOn, ModifiedByUser, ParentId, ChildId
			)
			SELECT 1, 0, getdate(), 'Admin', getdate(), 'Admin', NULL, @childId
		ELSE
			INSERT INTO bcms_media.MediaFolderDependencies (
				Version, IsDeleted, CreatedOn, CreatedByuser, ModifiedOn, ModifiedByUser, ParentId, ChildId
			)
			SELECT 1, 0, getdate(), 'Admin', getdate(), 'Admin', CAST(@stringId as uniqueidentifier), @childId
	END

	SET @i = @i + 1
END