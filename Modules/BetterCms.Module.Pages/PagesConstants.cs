using System;

namespace BetterCms.Module.Pages
{
    public static class PagesConstants
    {
        public const string PageUrlRegularExpression = @"(^/$)|((?!.*//)(^(/{1}[\w\-]{0,260})+/{1}$))";

        public static class PageIds
        {
            public static Guid PageDefault = new Guid("B638896E-B8BB-472F-8DFF-A0B83FF1F36F");

            public static Guid Page404 = new Guid("B9654D7D-D8D7-4C55-950A-7DD6F7636B7A");

            public static Guid Page500 = new Guid("4E600192-A0C7-454A-9D64-E2540F211660");
        }

        public static class ContentIds
        {
            public static Guid PageDefaultHeader = new Guid("3A06E421-D77B-420F-B4E2-EB34479636B8");

            public static Guid PageDefaultBody = new Guid("50009AD8-AE85-4D19-805E-73C5F49290F1");

            public static Guid PageDefaultFooter = new Guid("EED5A7A2-49D3-420B-BA8B-528053FFF044");
            
            public static Guid Page404Header = new Guid("4B0B15CD-72BD-44B4-9044-75C7FA7BDA65");

            public static Guid Page404Body = new Guid("63F7FE60-DF85-42F0-9165-5A92A9AB4F34");

            public static Guid Page404Footer = new Guid("964D677B-86D8-402E-B944-83DED74C8429");

            public static Guid Page500Header = new Guid("4F97A243-9953-450E-AE6C-BC911A7240AF");

            public static Guid Page500Body = new Guid("4691F161-A002-45ED-8F75-4E0F797F6063");

            public static Guid Page500Footer = new Guid("7E5691AD-11A2-44C3-B830-161383386320");
        }
    }
}