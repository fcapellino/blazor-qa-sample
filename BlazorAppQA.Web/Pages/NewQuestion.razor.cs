using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.CommandHandlers.InsertNewQuestionHandler;
using BlazorAppQA.Web.Common;
using BlazorInputFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Web.Pages
{
    [Authorize]
    public class NewQuestionComponent : CustomComponentBase
    {
        public InsertNewQuestionCommand InsertNewQuestionCommand { get; set; } = new InsertNewQuestionCommand();

        public async Task HandleFileSelectedAsync(IFileListEntry[] files)
        {
            await Task.FromResult(0).ContinueWith(t => InsertNewQuestionCommand.Files = files);
        }

        public async Task OnSubmitAsync()
        {
            await ExecuteAsync(async () =>
            {
                using var insertNewQuestionCommandHandler = ServiceProvider.GetService<InsertNewQuestionCommandHandler>();
                await insertNewQuestionCommandHandler.HandleAsync(InsertNewQuestionCommand);
                NavigationManager.NavigateTo(@"/");
            });
        }

        public async Task OnCancelAsync()
        {
            await ExecuteAsync(async () =>
            {
                await Task.FromResult(0).ContinueWith(t => NavigationManager.NavigateTo(@"/"));
            });
        }
    }
}
