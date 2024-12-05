using Newtonsoft.Json;

namespace Project4.Models
{
    public class Images : ListOfObjects<Image>, ICloneable<Images>
    {
        public Images() { }
        public Images(List<Image> list) { List = list; }
        public Images Clone()
        {
            return new Images(List);
        }
	}
}
