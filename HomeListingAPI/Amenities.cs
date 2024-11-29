namespace HomeListingAPI
{
	[Serializable]
	public class Amenities : ListOfObjects<Amenity>
    {
        public Amenities() { }
        public Amenities(List<Amenity> list) { List = list; }
        public Amenities Clone()
        {
            return new Amenities(List);
        }
    }
}
