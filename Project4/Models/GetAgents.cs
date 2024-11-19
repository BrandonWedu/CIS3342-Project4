namespace Project4.Models
{
    public class GetAgents
    {
        private AgentList allAgents;


        public GetAgents()
        {
            //Get CompanyInfo Database info
            //Get AgentAccount Database info
            //Get AgentContact Database info
            //Get AgentPeronsalInformation info
            //Get AgentSecurityQuestions info
            // Write all this information to a Agent object and add it to a list object
        }

        public AgentList GetAllAgents()
        {
            return allAgents;
        }

        public Agent GetAgentByAgentID(int id)
        {
            List<Agent> selectedAgents = new List<Agent>();
            //foreach (Agent currentAgent in allAgents)
            //{
            //    if (currentAgent.AgentID == id)
            //    {
            //        selectedAgents.Add(currentAgent);
            //    }
            //}
            return selectedAgents[0];
        }
    }
}
