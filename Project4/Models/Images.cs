namespace Project4.Models
{
    public class Images : ListOfObjects<Image>
    {
        public Images() { }
        public Images(List<Image> images) 
        { 
            List = images; 
        }
    }
}
