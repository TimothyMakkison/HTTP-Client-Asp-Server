namespace HTTP_Client_Asp_Server.Models
{
    public interface IAssignable<T>
    {
        public IAssignable<T> Set(T value);

        public bool Assigned { get; }
        public T Value { get; }
    }
}