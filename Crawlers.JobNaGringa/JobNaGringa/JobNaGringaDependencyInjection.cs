using Crawlers.Core;

namespace Crawlers.JobNaGringa.JobNaGringa;

public static class JobNaGringaDependencyInjection
{
    public static void InjectJobNaGringaServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.InjectCoreServices(configuration);
        
        services.Configure<JobNaGringaConfiguration>(
            configuration.GetSection(JobNaGringaConfiguration.JobNaGringaNameConfiguration));
        
        services.AddTransient<IJobNaGringaUseCase, JobNaGringaUseCase>();
        services.AddTransient<IJobNaGringaCrawler, JobNaGringaCrawler>();
        services.AddTransient<IJobNaGringaParser, JobNaGringaParser>();
        services.AddTransient<IJobNaGringaHealthCheckUseCase, JobNaGringaHealthCheckUseCase>();
    }
}