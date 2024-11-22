namespace HomeListingAPI
{
    public class Utilities : ListOfObjects<Utility>
    {
        public Utilities() { }
        public Utilities(List<Utility> list) { List = list; }
        public Utilities Clone()
        {
            return new Utilities(List);
        }
    }
}
