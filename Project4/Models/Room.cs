using Newtonsoft.Json;

namespace Project4.Models
{
    public enum RoomType
    {
        LivingRoom,
        Kitchen,
        DiningRoom,
        Bedroom,
        BathroomHalf,
        BathroomFull,
        HomeOffice,
        Laundry,
        Garage,
        Basement,
        Attic,
        Pantry,
        Mudroom,
        Library,
        Sunroom,
        Workshop,
        Storage,
        Outside
    }
    public class Room : ICloneable<Room>
    {
        private int? roomID;
        private RoomType type;
        private int height;
        private int width;

        public Room(RoomType type, int height, int width)
        {
            roomID = null;
            this.type = type;
            this.height = height;
            this.width = width;
        }
        public Room(int? roomID, RoomType type, int height, int width)
        {
            this.roomID = roomID;
            this.type = type;
            this.height = height;
            this.width = width;
        }

        public Room()
        {

        }
		[JsonProperty("roomID")]
		public int? RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }
		[JsonProperty("type")]
		public RoomType Type
        {
            get { return type; }
            set { type = value; }
        }
		[JsonProperty("height")]
		public int Height
        {
            get { return height; }
            set { height = value; }
        }
		[JsonProperty("width")]
		public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public Room Clone()
        {
            return new Room(RoomID, Type, Width, Height);  
        }
    }
}
