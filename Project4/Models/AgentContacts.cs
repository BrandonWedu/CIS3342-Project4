namespace Project4.Models
{
    public class AgentContacts : ListOfObjects<AgentContact>
    {
        public AgentContacts() { }

        public AgentContacts(List<AgentContact> list) { List = list; }

        public AgentContacts Clone()
        {
            return new AgentContacts(List);
        }
    }
}
