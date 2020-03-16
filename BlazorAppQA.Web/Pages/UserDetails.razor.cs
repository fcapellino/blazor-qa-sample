using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.CommandHandlers.GetUserHandler;
using BlazorAppQA.Infrastructure.Common;
using BlazorAppQA.Web.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Web.Pages
{
    public class UserDetailsComponent : CustomComponentBase
    {
        [Parameter]
        public string Id { get; set; }
        public dynamic RegisteredUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ExecuteAsync(async () =>
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    using var getUserommandHandler = ServiceProvider.GetService<GetUserCommandHandler>();
                    object result = await getUserommandHandler.HandleAsync(new GetUserCommand()
                    {
                        ProtectedId = Id
                    });

                    RegisteredUser = result.ToExpando();
                }
            });
        }
    }
}
