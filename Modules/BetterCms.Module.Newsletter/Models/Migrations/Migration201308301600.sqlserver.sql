UPDATE bcms_root.ContentOptions SET Type = 5 WHERE Id = '33741162-AB86-4E62-A91C-6F7C2772D8F4' AND Type = 1

UPDATE pco
SET Type = 5
FROM bcms_root.PageContents pc
	INNER JOIN bcms_root.PageContentOptions pco
		ON pco.[Key] = 'Submit is disabled'
			AND pco.Type = 1
WHERE pc.ContentId = '87B734F8-2B28-49E8-B8FC-3B2982AAB796'