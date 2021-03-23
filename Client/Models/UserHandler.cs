namespace HTTP_Client_Asp_Server.Models
{
    /// <summary>
    /// Responsible for determining if User has had its values assigned.
    /// </summary>
    public class UserHandler : IAssignable<User>
    {
        public bool Assigned { get; private set; } = false;

        public IAssignable<User> Set(User value)
        {
            Value = value;
            Assigned = true;
            return this;
        }

        public User Value { get; private set; }
    }
}