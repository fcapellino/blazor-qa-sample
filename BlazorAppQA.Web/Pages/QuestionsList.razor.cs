using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.CommandHandlers.GetQuestionsListHandler;
using BlazorAppQA.Infrastructure.Common;
using BlazorAppQA.Web.Common;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Web.Pages
{
    public class QuestionsListComponent : CustomComponentBase
    {
        public int TotalQuestionsCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public IEnumerable<dynamic> QuestionsList { get; set; }
        public string SearchQuery { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ExecuteAsync(async () =>
            {
                using var getQuestionsListCommandHandler = ServiceProvider.GetService<GetQuestionsListCommandHandler>();
                dynamic result = await getQuestionsListCommandHandler.HandleAsync(new GetQuestionsListCommand());

                TotalQuestionsCount = result.TotalItemCount;
                QuestionsList = (result.ItemsList as IEnumerable<object>).Select(x => x.ToExpando());
            });
        }

        public async Task OnKeyUpEventAsync(KeyboardEventArgs e)
        {
            if (e.Key.Equals("Enter"))
            {
                await OnSearchAsync();
            }
        }

        public async Task OnSearchAsync()
        {
            await ExecuteAsync(async () =>
            {
                using var getQuestionsListCommandHandler = ServiceProvider.GetService<GetQuestionsListCommandHandler>();
                dynamic result = await getQuestionsListCommandHandler.HandleAsync(new GetQuestionsListCommand()
                {
                    Page = 1,
                    SearchQuery = SearchQuery
                });

                QuestionsList = (result.ItemsList as IEnumerable<object>).Select(x => x.ToExpando());
                TotalQuestionsCount = result.TotalItemCount;
                CurrentPage = 1;
            });
        }

        public async Task OnPageChangeAsync(int page)
        {
            await ExecuteAsync(async () =>
            {
                using var getQuestionsListCommandHandler = ServiceProvider.GetService<GetQuestionsListCommandHandler>();
                dynamic result = await getQuestionsListCommandHandler.HandleAsync(new GetQuestionsListCommand()
                {
                    Page = page,
                    SearchQuery = SearchQuery
                });

                QuestionsList = (result.ItemsList as IEnumerable<object>).Select(x => x.ToExpando());
                TotalQuestionsCount = result.TotalItemCount;
                CurrentPage = page;
            });
        }

        public async Task OnQuestionClickAsync(string id)
        {
            await ExecuteAsync(async () =>
            {
                await Task.FromResult(0).ContinueWith(t => NavigationManager.NavigateTo($"/questions/{id}"));
            });
        }

        public async Task OnUserClickAsync(string id)
        {
            await ExecuteAsync(async () =>
            {
                await Task.FromResult(0).ContinueWith(t => NavigationManager.NavigateTo($"/users/{id}"));
            });
        }
    }
}
