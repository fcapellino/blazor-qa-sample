using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.CommandHandlers.GetStatisticsHandler;
using BlazorAppQA.Infrastructure.Common;
using BlazorAppQA.Web.Common;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Web.Pages
{
    public class RightPanelComponent : CustomComponentBase
    {
        public dynamic Statistics { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await ExecuteAsync(async () =>
            {
                using var getStatisticsCommandHandler = ServiceProvider.GetService<GetStatisticsCommandHandler>();
                object result = await getStatisticsCommandHandler.HandleAsync(new GetStatisticsCommand());
                Statistics = result.ToExpando();
            });
        }
    }
}
