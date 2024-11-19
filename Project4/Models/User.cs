namespace Project4.Models
{
    public class User : ICloneable<User>
    {
        public User() { }

        public User Clone()
        {
            return new User();
        }
    }
}
