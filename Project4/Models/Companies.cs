namespace Project4.Models
{
    public class Companies : ListOfObjects<Company>
    {
        public Companies() { }

        public Companies(List<Company> list) { List = list; }

        public Companies Clone()
        {
            return new Companies(List);
        }
    }
}
