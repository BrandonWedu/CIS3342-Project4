using Newtonsoft.Json;

namespace Project4.Models
{
    public class Image : ICloneable<Image>
    {
        //Fields
        private int? imageID;
        private string url;
        private RoomType type;
        private string description;
        private bool mainImage;

        public Image(string url, RoomType type, string description, bool mainImage)
        {
            this.imageID = null;
            this.url = url;
            this.type = type;
            this.description = description;
            this.mainImage = mainImage;
        }
        public Image(int? imageID, string url, RoomType type, string description, bool mainImage)
        {
            this.imageID = imageID;
            this.url = url;
            this.type = type;
            this.description = description;
            this.mainImage = mainImage;
        }

        public Image()
        {

        }
		[JsonProperty("imageID")]
		public int? ImageID
        {
            get { return imageID; }
            set { imageID = value; }
        }
		[JsonProperty("url")]
		public string Url
        {
            get { return url; }
            set { url = value; }
        }
		[JsonProperty("type")]
		public RoomType Type
        {
            get { return type; }
            set { type = value; }
        }
		[JsonProperty("description")]
		public string Description
        {
            get { return description; }
            set { description = value; }
        }
		[JsonProperty("mainImage")]
		public bool MainImage
        {
            get { return mainImage; }
            set { mainImage = value; }
        }
        public Image Clone()
        {
            return new Image(ImageID, Url, Type, Description, MainImage);
        }
    }
}
