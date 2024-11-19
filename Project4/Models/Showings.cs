namespace Project4.Models
{
    public class Showings : ListOfObjects<Showing>, ICloneable<Showings>
    {
        public Showings() { }
        public Showings(List<Showing> list) { List = list; }
        public Showings Clone()
        {
            return new Showings(List);
        }
    }
}
