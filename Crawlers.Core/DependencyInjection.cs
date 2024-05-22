using Crawlers.Core.Shared;
using Microsoft.EntityFrameworkCore;

namespace Crawlers.Core;

public static class DependencyInjection
{
    public static void InjectCoreServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<JobDatabaseContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddHttpClient();
        services.AddTransient<IIdGenService, IdGenService>();
        services.AddTransient<IBaseHttpClient, BaseHttpClient>();
        services.AddTransient<IJobsRepository, JobsRepository>();
        services.Configure<IdGenConfiguration>(
            configuration.GetSection(IdGenConfiguration.IdGen));
    }
}