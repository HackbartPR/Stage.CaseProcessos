namespace Stage.Domain.Entities
{
    public class Notification
    {
        public Notification(string error, string message)
        {
            Error = error;
            Message = message;
        }

        public string Error { get; }

        public string Message { get; }
    }
}
