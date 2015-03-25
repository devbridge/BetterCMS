BEGIN TRY

-------------------
-- Creates category
-------------------
IF NOT EXISTS (
	SELECT 1 
	FROM bcms_root.Categories 
	WHERE (id = 'D837BEBF-67DE-4952-BC60-DB03043B1524' OR Name = 'Images Gallery') AND IsDeleted = 0
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
		'D837BEBF-67DE-4952-BC60-DB03043B1524',
		1,
		0,
		getdate(),
		'Better CMS',
		getdate(),
		'Better CMS',
		'Images Gallery')

-------------------------------------------------
-- Assigns widgets to created / existing category
-------------------------------------------------
DECLARE @categoryId uniqueidentifier
SELECT @categoryId = Id 
  FROM bcms_root.Categories 
 WHERE (id = 'D837BEBF-67DE-4952-BC60-DB03043B1524' OR Name = 'Images Gallery') AND IsDeleted = 0

UPDATE bcms_root.Widgets
SET CategoryId = @categoryId
WHERE Id IN ('8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC', 'F67BC85F-83A7-427E-85C5-A24D008B32E1')
	AND CategoryId IS NULL

END TRY
BEGIN CATCH
END CATCH