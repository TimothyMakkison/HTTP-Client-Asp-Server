namespace HTTP_Client_Asp_Server.Models
{
    public class CryptoKey
    {
        public string Key { get; set; }
        public bool HasKey => Key != null && Key != string.Empty;
    }
}