using Stage.Domain.Notifications;

namespace Stage.Domain.Config
{
    public class PagedBaseResponse<T> where T : class
    {
        public PagedBaseResponse() { }

        public T Result { get; set; } = null!;

        public Dictionary<string, string> Notifications { get; set; } = new();

        public bool Success { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }
    }
}
