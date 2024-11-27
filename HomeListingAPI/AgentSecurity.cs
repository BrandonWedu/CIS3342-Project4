namespace HomeListingAPI
{
    public class AgentSecurity : ICloneable<AgentSecurity>
    {
        private int securityQuestionsID;
        private int agentID;
        private string question;
        private string answer;

        public int SecurityQuestionsID
        {
            get { return securityQuestionsID; }
            set { securityQuestionsID = value; }
        }

        public int AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        public AgentSecurity(int id, int agentID, string question, string answer)
        {
            SecurityQuestionsID = id;
            AgentID = agentID;
            Question = question;
            Answer = answer;
        }

        public AgentSecurity(int agentID, string question, string answer)
        {
            AgentID = agentID;
            Question = question;
            Answer = answer;
        }

        public AgentSecurity Clone()
        {
            return new AgentSecurity(SecurityQuestionsID, AgentID, Question, Answer);
        }
    }
}
