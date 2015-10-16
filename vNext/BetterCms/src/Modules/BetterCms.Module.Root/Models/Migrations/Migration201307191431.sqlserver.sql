-- Make lowercase and remove all trailing slashes and spaces.
UPDATE bcms_root.Pages
SET PageUrlLowerTrimmed = lower(replace(replace(rtrim(replace(replace(ltrim(rtrim(PageUrl)), ' ', '${space}$' ), '/', ' ' )), ' ', '/' ), '${space}$', ' ' ))
FROM bcms_root.Pages
-- Restore root path.
UPDATE bcms_root.Pages
SET PageUrlLowerTrimmed = '/'
FROM bcms_root.Pages
WHERE PageUrlLowerTrimmed = ''