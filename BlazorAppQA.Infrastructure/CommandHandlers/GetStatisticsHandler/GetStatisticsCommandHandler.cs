using System;
using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.ApplicationContext;
using BlazorAppQA.Infrastructure.BaseCommandHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Infrastructure.CommandHandlers.GetStatisticsHandler
{
    public class GetStatisticsCommandHandler : BaseCommandHandler<GetStatisticsCommand>
    {
        public GetStatisticsCommandHandler(IServiceProvider provider)
            : base(provider)
        {
        }

        public override async Task<dynamic> HandleAsync(GetStatisticsCommand command)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var totalQuestions = await applicationDbContext.Questions.CountAsync();
            var totalAnswers = await applicationDbContext.Answers.CountAsync();

            return new
            {
                TotalQuestions = totalQuestions,
                TotalAnswers = totalAnswers
            };
        }

        public override void Dispose() { }
    }
}
