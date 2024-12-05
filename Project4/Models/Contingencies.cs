namespace Project4.Models
{
    [Serializable]
    public class Contingencies : ListOfObjects<Contingency>
    {
        public Contingencies() { }

        public Contingencies(List<Contingency> list) { List = list; }

        public Contingencies Clone()
        {
            return new Contingencies(List);
        }
    }
}
