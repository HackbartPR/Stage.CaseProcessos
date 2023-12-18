namespace Stage.Domain.Config
{
    public class PagedBaseRequest
    {
        public PagedBaseRequest()
        {
            Page = 1;
            PageSize = 10;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public void ValidatePageRequest()
        {
            Page = Page < 1 ? 1 : Page;
            PageSize = PageSize < 1 ? 10 : PageSize;
        }
    }
}
