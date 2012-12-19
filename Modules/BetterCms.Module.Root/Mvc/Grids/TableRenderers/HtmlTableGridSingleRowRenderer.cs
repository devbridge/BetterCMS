namespace BetterCms.Module.Root.Mvc.Grids.TableRenderers
{
    public class HtmlTableGridSingleRowRenderer<T> : EditableHtmlTableGridRenderer<T> where T : class
    {
        protected override bool RenderHeader()
        {
            return true;
        }
    }
}