using FluentValidation;

namespace BlazorAppQA.Infrastructure.CommandHandlers.VoteForAnswerHandler
{
    public class VoteForAnswerCommand
    {
        public string ProtectedAnswerId { get; set; }
        public bool Upvote { get; set; }
    }

    public class VoteForAnswerCommandValidator
        : AbstractValidator<VoteForAnswerCommand>
    {
        public VoteForAnswerCommandValidator()
        {
            RuleFor(x => x.ProtectedAnswerId)
                .NotEmpty();

            RuleFor(x => x.Upvote)
                .NotEmpty();
        }
    }
}
