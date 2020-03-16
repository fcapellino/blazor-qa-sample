namespace BlazorAppQA.Infrastructure.CommandHandlers.GetUsersListHandler
{
    public class GetUsersListCommand
    {
        public string SearchQuery { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}
