using Microsoft.Data.SqlClient;
using System.Data;

namespace Project4.Models
{
    public class SecurityQuestionList
    {
        internal static List<string> GetAllQuestions()
        {

            List<string> questions = new List<string>();
            questions.Add("What is your mother's maiden name?");
            questions.Add("What is the name of the town where you were born?");
            questions.Add("What is the name of your first pet?");
            questions.Add("What was your high school mascot?");
            questions.Add("What is the name of the street you grew up on?");
            questions.Add("What is the name of your first school?");
            questions.Add("What was the name of your childhood best friend?");
            questions.Add("What is your father's middle name?");
            questions.Add("What was the name of your favorite teacher in school?");
            questions.Add("What is the first name of your oldest sibling?");
            return questions;
        }
    }
}
