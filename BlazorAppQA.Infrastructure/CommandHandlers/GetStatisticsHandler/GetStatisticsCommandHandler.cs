﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlazorAppQA.Infrastructure.ApplicationContext;
using BlazorAppQA.Infrastructure.BaseCommandHandler;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Infrastructure.CommandHandlers.GetStatisticsHandler
{
    public class GetStatisticsCommandHandler : BaseCommandHandler<GetStatisticsCommand>
    {
        private readonly IDataProtector _dataProtector;

        public GetStatisticsCommandHandler(IServiceProvider provider)
            : base(provider)
        {
            _dataProtector = provider.GetService<IDataProtectionProvider>().CreateProtector(Assembly.GetExecutingAssembly().FullName);
        }

        public override async Task<dynamic> HandleAsync(GetStatisticsCommand command)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var totalQuestions = await applicationDbContext.Questions.CountAsync();
            var totalAnswers = await applicationDbContext.Answers.CountAsync();

            var topUsersList = await applicationDbContext.Users
               .OrderByDescending(u => u.UserAnswers.Count)
               .Take(5)
               .Select(u => new
               {
                   Id = _dataProtector.Protect(u.Id.ToString()),
                   u.UserName,
                   AnswersProvided = u.UserAnswers.Count
               })
               .OrderBy(u => u.UserName)
               .ToListAsync();

            return new
            {
                TotalQuestions = totalQuestions,
                TotalAnswers = totalAnswers,
                TopUsersList = topUsersList
            };
        }

        public override void Dispose() { }
    }
}
