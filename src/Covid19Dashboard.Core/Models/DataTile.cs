namespace Covid19Dashboard.Core.Models
{
    public class DataTile
    {
        public string Data { get; set; }

        public string Description { get; set; }

        public string FullDescription { get { return Data + " " + Description; } }

        public string LastUpdate { get; set; }

        public string Tag { get; set; }

        public string Title { get; set; }
    }
}
