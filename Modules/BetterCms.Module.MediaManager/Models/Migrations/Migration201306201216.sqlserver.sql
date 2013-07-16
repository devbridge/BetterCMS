UPDATE bcms_media.Medias
SET PublishedOn = CreatedOn
FROM bcms_media.Medias
WHERE bcms_media.Medias.PublishedOn <= '1/1/1753 12:00:00 AM'