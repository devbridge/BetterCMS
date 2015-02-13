BEGIN TRY

-------------------
-- Creates category
-------------------
IF NOT EXISTS (
	SELECT 1 
	FROM bcms_root.Categories 
	WHERE (id = '3C24B0A2-F4BD-4FBC-BCA3-C077F907C384' OR Name = 'Search') AND IsDeleted = 0
)
	INSERT INTO bcms_root.Categories (
		Id, 
		[Version], 
		IsDeleted, 
		CreatedOn,
		CreatedByUser,
		ModifiedOn,
		ModifiedByUser,
		Name)
	VALUES (
		'3C24B0A2-F4BD-4FBC-BCA3-C077F907C384',
		1,
		0,
		getdate(),
		'Better CMS',
		getdate(),
		'Better CMS',
		'Search')

-------------------------------------------------
-- Assigns widgets to created / existing category
-------------------------------------------------
DECLARE @categoryId uniqueidentifier
SELECT @categoryId = Id 
  FROM bcms_root.Categories 
 WHERE (id = '3C24B0A2-F4BD-4FBC-BCA3-C077F907C384' OR Name = 'Search') AND IsDeleted = 0

UPDATE bcms_root.Widgets
SET CategoryId = @categoryId
WHERE Id IN ('D31DB767-B352-4E5B-A0DA-6696A53B87F6', '663A1D0C-FADA-4ACC-A34F-7437523AE65B')
	AND CategoryId IS NULL

END TRY
BEGIN CATCH
END CATCH