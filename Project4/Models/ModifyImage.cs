namespace Project4.Models
{
    public class ModifyImage
    {
        private byte[] image;
        public ModifyImage() { }

        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }
    }
}
