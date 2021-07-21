using System;
using Hangfire;
using Hangfire.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Treatment.Monitor.Notifier.JobActivation
{
    public class ContainerJobActivatorScope : JobActivatorScope
    {
        private readonly IServiceScope _serviceScope;

        public ContainerJobActivatorScope([NotNull] IServiceScope serviceScope)
        {
            _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
        }

        public override object Resolve(Type type) =>
            ActivatorUtilities.GetServiceOrCreateInstance(_serviceScope.ServiceProvider, type);
    }
}