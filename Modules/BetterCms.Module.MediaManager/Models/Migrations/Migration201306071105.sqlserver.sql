UPDATE bcms_media.Medias
SET ContentType = 2
FROM bcms_media.Medias INNER JOIN bcms_media.MediaFolders ON (bcms_media.Medias.Id = bcms_media.MediaFolders.Id)
WHERE bcms_media.Medias.ContentType != 2 /* If not a folder. */