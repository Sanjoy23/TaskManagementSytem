

using Microsoft.Extensions.DependencyInjection;

namespace Infrastruture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicaton(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }
    }
}
