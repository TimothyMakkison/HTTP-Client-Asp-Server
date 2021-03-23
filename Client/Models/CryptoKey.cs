namespace HTTP_Client_Asp_Server.Models
{
    public class CryptoKey : IAssignable<string>
    {
        public bool Assigned { get; private set; }

        public string Value { get; private set; }

        public IAssignable<string> Set(string value)
        {
            Value = value;
            Assigned = true;
            return this;
        }
    }
}