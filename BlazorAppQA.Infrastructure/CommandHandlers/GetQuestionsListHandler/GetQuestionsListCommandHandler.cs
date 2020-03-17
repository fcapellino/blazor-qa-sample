using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.ApplicationContext;
using BlazorAppQA.Infrastructure.BaseCommandHandler;
using BlazorAppQA.Infrastructure.Common;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Infrastructure.CommandHandlers.GetQuestionsListHandler
{
    public class GetQuestionsListCommandHandler : BaseCommandHandler<GetQuestionsListCommand>
    {
        private readonly IDataProtector _dataProtector;

        public GetQuestionsListCommandHandler(IServiceProvider provider)
            : base(provider)
        {
            _dataProtector = provider.GetService<IDataProtectionProvider>().CreateProtector(Assembly.GetExecutingAssembly().FullName);
        }

        public override async Task<dynamic> HandleAsync(GetQuestionsListCommand command)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var query = applicationDbContext.Questions
                .Include(q => q.User)
                .Include(q => q.Category)
                .OrderByDescending(q => q.Date)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(command.SearchQuery))
            {
                query = query.Where(q => (q.User.Email + q.User.UserName + q.Title + q.TagsArray + q.Description).ToLower().Contains(command.SearchQuery.ToLower().Trim()));
            }

            var queryResult = await query
                .ApplyPaging(command.Page, command.PageSize)
                .Select(q => new
                {
                    Id = _dataProtector.Protect(q.Id.ToString()),
                    q.Title,
                    q.Description,
                    q.Date,
                    Tags = q.TagsArray.Split(";", StringSplitOptions.None),
                    TotalAnswers = q.QuestionAnswers.Count,
                    CategoryName = q.Category.Name
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
