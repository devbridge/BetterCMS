using System;

namespace BetterCms.Module.Templates
{
    public class TemplatesModuleConstants
    {
        public static class TemplateIds
        {
            public static Guid Wide = new Guid("8862ACC2-59C2-4C79-B01F-80D15282EBA3");

            public static Guid TwoColumns = new Guid("347003A1-E068-4C3D-BC53-78785818700E");

            public static Guid ThreeColumns = new Guid("60F03242-F889-4B2C-ACCC-FA5D0350555B");
        }

        public static class RegionIds
        {
            public static Guid MainContent = new Guid("4161FA8D-299E-460B-8CA1-D27BA455B7B4");

            public static Guid Header = new Guid("7BCC6C70-711F-49F4-8A1D-674219D6AC79");

            public static Guid Footer = new Guid("195A5513-CE64-4454-8878-D61502560CD7");
            
            public static Guid LeftSide = new Guid("C0D1D26F-C487-45B3-9AD1-9C0AF4F393AD");

            public static Guid RightSide = new Guid("6908212F-7BC8-4EFF-B6B2-4DAF5A324CA9");
        }
    }
}