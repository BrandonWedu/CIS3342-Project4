namespace HomeListingAPI
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

        public int? ImageID
        {
            get { return imageID; }
            set { imageID = value; }
        }
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        public RoomType Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
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
