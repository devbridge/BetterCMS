namespace BetterCms.Module.Root
{
    public static class RootModuleConstants
    {
        /// <summary>
        /// Module permissions.
        /// </summary>
        public static class UserRoles
        {
            /// <summary>
            /// Permission for user to edit site settings.
            /// </summary>
            public const string EditSiteSettings = "EditSiteSettings";
        }

        public const string EditableGridTemplate = "~/Areas/bcms-root/Views/Shared/EditableGrid/Grid.cshtml";
        public const string EditableGridCellTemplate = "~/Areas/bcms-root/Views/Shared/EditableGrid/Partial/Cell.cshtml";
        public const string EditableGridHeaderTemplate = "~/Areas/bcms-root/Views/Shared/EditableGrid/Partial/Header.cshtml";
        public const string EditableGridTopBlockTemplate = "~/Areas/bcms-root/Views/Shared/EditableGrid/Partial/TopBlock.cshtml";
    }
}