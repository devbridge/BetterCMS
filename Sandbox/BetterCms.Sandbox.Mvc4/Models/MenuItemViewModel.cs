namespace BetterCms.Sandbox.Mvc4.Models
{
    public class MenuItemViewModel
    {
        public string Caption { get; set; }

        public string Url { get; set; }

        public bool IsPublished { get; set; }

        public override string ToString()
        {
            return string.Format("Caption: {0}, Url: {1}", Caption, Url);
        }
    }
}