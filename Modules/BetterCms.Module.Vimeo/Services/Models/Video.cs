namespace BetterCms.Module.Vimeo.Services.Models
{
    internal class Video
    {
        // Basic
        public string Embed_Privacy { get; set; }
        public string Id { get; set; }
        public string Is_HD { get; set; }
        public string Is_Watchlater { get; set; }
        public string License { get; set; }
        public string Modified_Date { get; set; }
        // public string owner { get; set; }
        public string Privacy { get; set; }
        public string Title { get; set; }
        public string Upload_Date { get; set; }

        // Full
        public int Allow_Adds { get; set; }
        public int Is_Transcoding { get; set; }
        public string Description { get; set; }
        public string Number_of_Likes { get; set; }
        public string Number_of_Plays { get; set; }
        public string Number_of_Comments { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Duration { get; set; }
        public Owner Owner { get; set; }
//        public Tags tags { get; set; }
//        public Cast Cast { get; set; }
//        public Urls Urls { get; set; }
        public ThumbnailList Thumbnails { get; set; }

        public bool IsPublic()
        {
            return Embed_Privacy == "anywhere";
        }
    }
}