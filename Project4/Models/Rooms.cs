namespace Project4.Models
{
    public class Rooms : ListOfObjects<Room>, ICloneable<Rooms>
    {
        public Rooms() { }
        public Rooms(List<Room> list) { List = list; }
        public Rooms Clone()
        {
            return new Rooms(List);
        }
        public int GetBedrooms()
        {
            int count = 0;
            foreach (Room room in List)
            {
                if (room.Type == RoomType.Bedroom)
                {
                    count++;
                }
            }
            return count;
        }
        public int GetFullBaths()
        {
            int count = 0;
            foreach (Room room in List)
            {
                if (room.Type == RoomType.BathroomFull)
                {
                    count++;
                }
            }
            return count;
        }
        public int GetHalfBaths()
        {
            int count = 0;
            foreach (Room room in List)
            {
                if (room.Type == RoomType.BathroomHalf)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
