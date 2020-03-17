namespace BlazorAppQA.Infrastructure.CommandHandlers.GetQuestionsListHandler
{
    public class GetQuestionsListCommand
    {
        public string SearchQuery { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}
