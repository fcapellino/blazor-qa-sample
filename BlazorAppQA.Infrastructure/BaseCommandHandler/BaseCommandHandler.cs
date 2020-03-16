using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAppQA.Infrastructure.BaseCommandHandler
{
    public abstract class BaseCommandHandler<TCommand> : IDisposable
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        protected BaseCommandHandler(IServiceProvider provider)
        {
            _serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
        }

        public abstract Task<dynamic> HandleAsync(TCommand command);
        public abstract void Dispose();
    }
}
