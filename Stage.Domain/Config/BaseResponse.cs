namespace Stage.Domain.Config
{
    public class BaseResponse<T>
    {
        public BaseResponse(T result, Dictionary<string, string> notifications, bool success)
        {
            Result = result;
            Notifications = notifications;
            Success = success;
        }

        public T Result { get; }

        public Dictionary<string, string> Notifications { get; }

        public bool Success { get; }
    }
}
