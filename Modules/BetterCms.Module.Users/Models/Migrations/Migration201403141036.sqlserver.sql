IF NOT EXISTS (SELECT 1 FROM [BetterCms].[bcms_users].[Roles] WHERE IsDeleted = 0 AND Name = 'BcmsManageUsers')
BEGIN
    INSERT INTO [BetterCms].[bcms_users].[Roles] ([Version], [IsDeleted], [CreatedOn], [CreatedByUser], [ModifiedOn], [ModifiedByUser], [Name], [Description], [IsSystematic])
    VALUES (1, 0, getdate(), 'Better CMS', getdate(), 'Better CMS', 'BcmsManageUsers', 'Can manage Better CMS users.', 1)

    DECLARE @roleId uniqueidentifier

	SELECT @roleId = Id
	FROM [BetterCms].[bcms_users].[Roles] 
	WHERE IsDeleted = 0 AND Name = 'BcmsManageUsers'

	INSERT INTO [BetterCms].[bcms_users].[UserRoles] ([Version], [IsDeleted], [CreatedOn], [CreatedByUser], [ModifiedOn], [ModifiedByUser], [RoleId], [UserId])
	SELECT 1, 0, getdate(), 'Better CMS', getdate(), 'Better CMS', @roleId, u.Id
	FROM (SELECT DISTINCT u.Id
	        FROM [BetterCms].[bcms_users].[Users] as u
	        INNER JOIN [BetterCms].[bcms_users].[UserRoles] as ur ON ur.UserId = u.Id
	        INNER JOIN [BetterCms].[bcms_users].[Roles] as r ON r.Id = ur.RoleId
	        WHERE u.IsDeleted = 0 AND ur.IsDeleted = 0 AND r.Name = 'BcmsAdministration') as u
END	  