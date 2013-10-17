-- Make lowercase and remove all trailing slashes and spaces.
UPDATE bcms_root.Pages
SET PageUrlHash = substring(lower(convert(varchar(34), HashBytes('MD5', lower(replace(replace(rtrim(replace(replace(ltrim(rtrim(PageUrl)), N' ', N'${space}$' ), N'/', N' ' )), N' ', N'/' ), N'${space}$', N' ' ))),1)),3,32)
FROM bcms_root.Pages
-- Restore root path.
UPDATE bcms_root.Pages
SET PageUrlHash = substring(lower(convert(varchar(34), HashBytes('MD5', N'/'),1)),3,32)
FROM bcms_root.Pages
WHERE PageUrl = N'/'