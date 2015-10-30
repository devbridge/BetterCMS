UPDATE [bcms_root].[WidgetCategories] SET [IsDeleted] = 1, [DeletedOn] = (getutcdate()), [DeletedByUser] = 'Migration201510231700' 
WHERE Id IN (
	SELECT WC.Id AS Id FROM [bcms_root].[WidgetCategories] WC INNER JOIN [bcms_root].[Categories] C on WC.CategoryId = C.Id 
	WHERE C.IsDeleted = 1 AND WC.IsDeleted = 0
)