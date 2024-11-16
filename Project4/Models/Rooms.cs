namespace Project4.Models
{
    public class Rooms : ListOfObjects<Room>
    {
        public Rooms() { }
        public Rooms(List<Room> list) { List = list; }
        public Rooms Clone()
        {
            return new Rooms(List);
        }
    }
}
