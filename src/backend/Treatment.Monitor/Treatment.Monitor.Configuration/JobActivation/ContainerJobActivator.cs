using System;
using Hangfire;
using Hangfire.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Treatment.Monitor.Configuration.JobActivation
{
    public class ContainerJobActivator : JobActivator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContainerJobActivator([NotNull] IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context) =>
            new ContainerJobActivatorScope(_serviceScopeFactory.CreateScope());

        [Obsolete("Obsolete in base class")]
        public override JobActivatorScope BeginScope() =>
            new ContainerJobActivatorScope(_serviceScopeFactory.CreateScope());
    }
}