IF (CAST(SUBSTRING(CAST(SERVERPROPERTY('productversion') as nvarchar(100)), 0, CHARINDEX('.', CAST(SERVERPROPERTY('productversion') as nvarchar(100)))) as INT) <= 9)
 BEGIN
    -- 'SQL <= 2005'
    -- Make lowercase and remove all trailing slashes and spaces.
    UPDATE bcms_pages.SitemapNodes
    SET UrlHash = substring(lower(master.dbo.fn_varbintohexstr(HashBytes('MD5', lower(replace(replace(rtrim(replace(replace(ltrim(rtrim(Url)), N' ', N'${space}$' ), N'/', N' ' )), N' ', N'/' ), N'${space}$', N' ' ))))),3,32)
    FROM bcms_pages.SitemapNodes
    -- Restore root path.
    UPDATE bcms_pages.SitemapNodes
    SET UrlHash = substring(lower(master.dbo.fn_varbintohexstr(HashBytes('MD5', N'/'))),3,32)
    FROM bcms_pages.SitemapNodes
    WHERE Url = N'/'
 END
ELSE
 BEGIN
    -- Make lowercase and remove all trailing slashes and spaces.
    UPDATE bcms_pages.SitemapNodes
    SET UrlHash = substring(lower(convert(varchar(34), HashBytes('MD5', lower(replace(replace(rtrim(replace(replace(ltrim(rtrim(Url)), N' ', N'${space}$' ), N'/', N' ' )), N' ', N'/' ), N'${space}$', N' ' ))),1)),3,32)
    FROM bcms_pages.SitemapNodes
    -- Restore root path.
    UPDATE bcms_pages.SitemapNodes
    SET UrlHash = substring(lower(convert(varchar(34), HashBytes('MD5', N'/'),1)),3,32)
    FROM bcms_pages.SitemapNodes
    WHERE Url = N'/'
 END