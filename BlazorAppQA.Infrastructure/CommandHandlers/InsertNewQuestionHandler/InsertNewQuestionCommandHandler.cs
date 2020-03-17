﻿using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using BlazorAppQA.Infrastructure.ApplicationContext;
using BlazorAppQA.Infrastructure.BaseCommandHandler;
using BlazorAppQA.Infrastructure.Common;
using BlazorAppQA.Infrastructure.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Infrastructure.CommandHandlers.InsertNewQuestionHandler
{
    public class InsertNewQuestionCommandHandler : BaseCommandHandler<InsertNewQuestionCommand>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly HttpContext _httpContext;

        public InsertNewQuestionCommandHandler(IServiceProvider provider)
            : base(provider)
        {
            _hostingEnvironment = provider.GetService<IWebHostEnvironment>();
            _httpContext = provider.GetService<IHttpContextAccessor>().HttpContext;
        }

        public override async Task<dynamic> HandleAsync(InsertNewQuestionCommand command)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                using var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                var tagsArray = command.Tags.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim().ToLowerInvariant());
                var newQuestion = new Question()
                {
                    UserId = int.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Date = DateTime.Now,
                    Title = command.Title.Trim(),
                    Description = command.Description.Trim(),
                    TagsArray = string.Join(";", tagsArray)
                };

                applicationDbContext.Questions.Add(newQuestion);
                if (command.Files != null)
                {
                    await command.Files.AsEnumerable().ForEachAsync(async file =>
                    {
                        var fileName = Guid.NewGuid();
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, $"qimages\\{fileName.ToString()}.jpg");
                        using var fileSteam = new FileStream(filePath, FileMode.Create);
                        await file.Data.CopyToAsync(fileSteam);

                        newQuestion.QuestionImages.Add(new QuestionImage()
                        {
                            FileName = fileName
                        });
                    });
                }

                await applicationDbContext.SaveChangesAsync();
                transactionScope.Complete();
            }

            return default;
        }

        public override void Dispose() { }
    }
}