namespace BetterCms.Module.Root.Mvc.Grids.TableRenderers
{
    /// <summary>
    /// Helper class for rendering HTML tables for editing data with sorting
    /// </summary>
    /// <typeparam name="T">Table item type</typeparam>
    public class ScrollableEditableHtmlTableGridRenderer<T> : EditableHtmlTableGridRenderer<T> where T : class
    {
        public string InterncalTableCssClass { get; set; }

        protected override void RenderBodyStart()
        {
            RenderText("<tbody>");
            RenderText("<tr>");
            RenderText(string.Format("<td colspan=\"{0}\" class=\"{1}\" style=\"border:0;\">", GridModel.Columns.Count, InterncalTableCssClass));
            RenderText("<div class=\"bcms-scroll-tbody\">");
            RenderGridStart();
            base.RenderBodyStart();
        }

        protected override void RenderBodyEnd()
        {
            base.RenderBodyEnd();
            RenderGridEnd(false);
            RenderText("</div>");
            RenderText("</td>");
            RenderText("</tr>");
            RenderText("</tbody>");
        }
    }
}