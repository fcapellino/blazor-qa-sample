using System.Linq;
using System.Text.RegularExpressions;
using BlazorInputFile;
using FluentValidation;

namespace BlazorAppQA.Infrastructure.CommandHandlers.InsertNewQuestionHandler
{
    public class InsertNewQuestionCommand
    {
        public string Title { get; set; }
        public string Tags { get; set; }
        public string ProtectedCategoryId { get; set; }
        public string Description { get; set; }
        public IFileListEntry[] Files { get; set; }
    }

    public class InsertNewQuestionCommandValidator
        : AbstractValidator<InsertNewQuestionCommand>
    {
        public InsertNewQuestionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Tags)
                .NotEmpty()
                .Must(x => Regex.IsMatch(x, @"[^;]+"));

            RuleFor(x => x.ProtectedCategoryId)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1600);

            RuleFor(x => x.Files)
                .Must(x =>
                {
                    return x == null || (x.Length < 5 && x.All(file => file != null));
                });
        }
    }
}
