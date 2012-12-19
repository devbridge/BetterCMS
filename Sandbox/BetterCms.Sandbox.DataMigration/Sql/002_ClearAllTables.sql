BEGIN TRAN

DELETE FROM bcms_pages.Redirects
DELETE FROM bcms_pages.PageControls
DELETE FROM bcms_pages.PageHtmlControls
DELETE FROM bcms_pages.Controls
DELETE FROM bcms_pages.HtmlControls
DELETE FROM bcms_root.SiteSettings
DELETE FROM bcms_pages.PageTags
DELETE FROM bcms_pages.Tags
DELETE FROM bcms_pages.PageCategories
DELETE FROM bcms_pages.Categories
DELETE FROM bcms_pages.LayoutRegions
DELETE FROM bcms_pages.Contents
DELETE FROM bcms_pages.Approvals
DELETE FROM bcms_pages.Drafts
DELETE FROM bcms_pages.Pages
DELETE FROM bcms_root.Pages
DELETE FROM bcms_pages.Authors
DELETE FROM bcms_pages.Regions
DELETE FROM bcms_pages.Sections
DELETE FROM bcms_pages.Layouts
DELETE FROM bcms_root.Users

/* DEBUG */
SELECT 'bcms_pages.Approvals', count(*) FROM bcms_pages.Approvals
UNION ALL
SELECT 'bcms_pages.PageCategories', count(*) FROM bcms_pages.PageCategories
UNION ALL
SELECT 'bcms_pages.Authors', count(*) FROM bcms_pages.Authors
UNION ALL
SELECT 'bcms_pages.Redirects', count(*) FROM bcms_pages.Redirects
UNION ALL
SELECT 'bcms_pages.Categories', count(*) FROM bcms_pages.Categories
UNION ALL
SELECT 'bcms_pages.Contents', count(*) FROM bcms_pages.Contents
UNION ALL
SELECT 'bcms_pages.Controls', count(*) FROM bcms_pages.Controls
UNION ALL
SELECT 'bcms_pages.Drafts', count(*) FROM bcms_pages.Drafts
UNION ALL
SELECT 'bcms_pages.HtmlControls', count(*) FROM bcms_pages.HtmlControls
UNION ALL
SELECT 'bcms_pages.Layouts', count(*) FROM bcms_pages.Layouts
UNION ALL
SELECT 'bcms_pages.LayoutRegions', count(*) FROM bcms_pages.LayoutRegions
UNION ALL
SELECT 'bcms_pages.PageControls', count(*) FROM bcms_pages.PageControls
UNION ALL
SELECT 'bcms_pages.Pages', count(*) FROM bcms_pages.Pages
UNION ALL
SELECT 'bcms_pages.PageHtmlControls', count(*) FROM bcms_pages.PageHtmlControls
UNION ALL
SELECT 'bcms_pages.PageTags', count(*) FROM bcms_pages.PageTags
UNION ALL
SELECT 'bcms_pages.Regions', count(*) FROM bcms_pages.Regions
UNION ALL
SELECT 'bcms_pages.Sections', count(*) FROM bcms_pages.Sections
UNION ALL
SELECT 'bcms_pages.Tags', count(*) FROM bcms_pages.Tags
UNION ALL
SELECT 'bcms_root.Users', count(*) FROM bcms_root.Users
UNION ALL
SELECT 'bcms_root.Pages', count(*) FROM bcms_root.Pages
UNION ALL
SELECT 'bcms_root.SiteSettings', count(*) FROM bcms_root.SiteSettings

/*
DROP TABLE bcms_pages.Redirects
DROP TABLE bcms_pages.PageControls
DROP TABLE bcms_pages.PageHtmlControls
DROP TABLE bcms_pages.Controls
DROP TABLE bcms_pages.HtmlControls
DROP TABLE bcms_root.SiteSettings
DROP TABLE bcms_pages.PageTags
DROP TABLE bcms_pages.Tags
DROP TABLE bcms_pages.PageCategories
DROP TABLE bcms_pages.Categories
DROP TABLE bcms_pages.LayoutRegions
DROP TABLE bcms_pages.Contents
DROP TABLE bcms_pages.Approvals
DROP TABLE bcms_pages.Drafts
DROP TABLE bcms_pages.Pages
DROP TABLE bcms_root.Pages
DROP TABLE bcms_pages.Authors
DROP TABLE bcms_pages.Regions
DROP TABLE bcms_pages.Sections
DROP TABLE bcms_pages.Layouts
DROP TABLE bcms_root.Users

DROP TABLE bcms_root.VersionInfo
DROP TABLE bcms_pages.VersionInfo
DROP TABLE bcms_media.VersionInfo

alter table bcms_pages.Contents drop constraint FK_Cms_Contents_Cms_Approvals
alter table bcms_pages.Approvals drop constraint FK_Cms_Approvals_Cms_Contents

alter table bcms_pages.Contents drop constraint FK_Cms_Contents_Cms_Drafts
alter table bcms_pages.Drafts drop constraint FK_Cms_Drafts_Cms_Contents

select * from information_schema.tables where table_type = 'BASE TABLE' ORDER BY TABLE_NAME
select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_TYPE = 'FOREIGN KEY'
*/

ROLLBACK