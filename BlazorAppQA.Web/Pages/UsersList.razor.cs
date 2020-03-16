using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.CommandHandlers.GetUsersListHandler;
using BlazorAppQA.Infrastructure.Common;
using BlazorAppQA.Web.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Web.Pages
{
    public class UsersListComponent : CustomComponentBase
    {
        public int TotalUsersCount { get; set; }
        public IEnumerable<dynamic> UsersList { get; set; }
        public string SearchQuery { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ExecuteAsync(async () =>
            {
                using var getUsersListCommandHandler = ServiceProvider.GetService<GetUsersListCommandHandler>();
                dynamic result = await getUsersListCommandHandler.HandleAsync(new GetUsersListCommand());

                TotalUsersCount = result.TotalItemCount;
                UsersList = (result.ItemsList as IEnumerable<object>).Select(x => x.ToExpando());
            });
        }

        public async Task OnSearchAsync()
        {
            await ExecuteAsync(async () =>
            {
                using var getUsersListCommandHandler = ServiceProvider.GetService<GetUsersListCommandHandler>();
                dynamic result = await getUsersListCommandHandler.HandleAsync(new GetUsersListCommand()
                {
                    Page = 1,
                    SearchQuery = SearchQuery
                });

                TotalUsersCount = result.TotalItemCount;
                UsersList = (result.ItemsList as IEnumerable<object>).Select(x => x.ToExpando());
            });
        }

        public async Task OnPageChangeAsync(int page)
        {
            await ExecuteAsync(async () =>
            {
                using var getUsersListCommandHandler = ServiceProvider.GetService<GetUsersListCommandHandler>();
                dynamic result = await getUsersListCommandHandler.HandleAsync(new GetUsersListCommand()
                {
                    Page = page,
                    SearchQuery = SearchQuery
                });

                TotalUsersCount = result.TotalItemCount;
                UsersList = (result.ItemsList as IEnumerable<object>).Select(x => x.ToExpando());
            });
        }
    }
}
