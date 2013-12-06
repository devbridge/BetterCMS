INSERT INTO [bcms_root].[Layouts]
           ([Id]
           ,[Version]
           ,[IsDeleted]
           ,[CreatedOn]
           ,[CreatedByUser]
           ,[ModifiedOn]
           ,[ModifiedByUser]
           ,[Name]
           ,[LayoutPath])
     VALUES
           ('24A9FDBC-7A9B-4C02-AFF5-A0B09F2CA9E7'
           ,1
           ,0
           ,getdate()
           ,'Better CMS'
           ,getdate()
           ,'Better CMS'
           ,'Blank Template'
           ,'~/Areas/bcms-installation/Views/Shared/BlankLayout.cshtml')

IF NOT EXISTS (
	SELECT 1 
	FROM [bcms_root].[Regions]
	WHERE [RegionIdentifier] = 'CMSMainContent' AND IsDeleted = 0
)
INSERT INTO [bcms_root].[Regions]
           ([Id]
           ,[Version]
           ,[IsDeleted]
           ,[CreatedOn]
           ,[CreatedByUser]
           ,[ModifiedOn]
           ,[ModifiedByUser]
           ,[RegionIdentifier])
     VALUES
           ('CA3216C0-3E27-4785-BB55-245963177CC6'
           ,1
           ,0
           ,getdate()
           ,'Better CMS'
           ,getdate()
           ,'Better CMS'
           ,'CMSMainContent')

DECLARE @regionId uniqueidentifier
SELECT @regionId = Id 
  FROM [bcms_root].[Regions] 
 WHERE [RegionIdentifier] = 'CMSMainContent' AND IsDeleted = 0

INSERT INTO [bcms_root].[LayoutRegions]
           ([Id]
           ,[Version]
           ,[IsDeleted]
           ,[CreatedOn]
           ,[CreatedByUser]
           ,[ModifiedOn]
           ,[ModifiedByUser]
           ,[LayoutId]
           ,[RegionId])
     VALUES
           ('26CE115F-2BE0-4B11-8495-AF3E0A9FBDAF'
           ,1
           ,0
           ,getdate()
           ,'Better CMS'
           ,getdate()
           ,'Better CMS'
           ,'24A9FDBC-7A9B-4C02-AFF5-A0B09F2CA9E7'
           ,@regionId)
