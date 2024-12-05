namespace Project4.Models
{
    public class Clients : ListOfObjects<Client>
    {
        public Clients() { }

        public Clients(List<Client> list) { List = list; }

        public Clients Clone()
        {
            return new Clients(List);
        }
    }
}
