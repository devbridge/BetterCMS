-- Convert date type option values to ISO format.

UPDATE [bcms_root].[LayoutOptions]
   SET [DefaultValue] = SUBSTRING([DefaultValue], 7, 4) + '-' + SUBSTRING([DefaultValue], 1, 2) + '-' + SUBSTRING([DefaultValue], 4, 2)
 WHERE [Type] = 4 AND [DefaultValue] LIKE '__/__/____'

UPDATE [bcms_root].[ContentOptions]
   SET [DefaultValue] = SUBSTRING([DefaultValue], 7, 4) + '-' + SUBSTRING([DefaultValue], 1, 2) + '-' + SUBSTRING([DefaultValue], 4, 2)
 WHERE [Type] = 4 AND [DefaultValue] LIKE '__/__/____'

UPDATE [bcms_root].[PageOptions]
   SET [Value] = SUBSTRING([Value], 7, 4) + '-' + SUBSTRING([Value], 1, 2) + '-' + SUBSTRING([Value], 4, 2)
 WHERE [Type] = 4 AND [Value] LIKE '__/__/____'

UPDATE [bcms_root].[PageContentOptions]
   SET [Value] = SUBSTRING([Value], 7, 4) + '-' + SUBSTRING([Value], 1, 2) + '-' + SUBSTRING([Value], 4, 2)
 WHERE [Type] = 4 AND [Value] LIKE '__/__/____'

