using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.ApplicationContext;
using BlazorAppQA.Infrastructure.BaseCommandHandler;
using BlazorAppQA.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Infrastructure.CommandHandlers.GetUsersListHandler
{
    public class GetUsersListCommandHandler : BaseCommandHandler<GetUsersListCommand>
    {
        public GetUsersListCommandHandler(IServiceProvider provider)
            : base(provider)
        {
        }

        public override async Task<dynamic> HandleAsync(GetUsersListCommand command)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var query = applicationDbContext.Users
                .OrderBy(u => u.UserName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(command.SearchQuery))
            {
                query = query.Where(u => (u.Email + u.UserName).ToLower().Contains(command.SearchQuery.ToLower().Trim()));
            }

            var queryResult = await query
                .ApplyPaging(command.Page, command.PageSize)
                .Select(u => new
                {
                    u.UserName,
                    u.Biography,
                    u.Base64AvatarImage,
                    TotalAnswers = u.UserAnswers.Count,
                    TotalQuestions = u.UserQuestions.Count
                })
                .ToListAsync();

            return new ListResult
            {
                TotalItemCount = await query.CountAsync(),
                ItemsList = queryResult
            };
        }

        public override void Dispose() { }
    }
}
