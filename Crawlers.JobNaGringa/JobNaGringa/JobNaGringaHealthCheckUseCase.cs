using Crawlers.Core.Shared.Domain;

namespace Crawlers.JobNaGringa.JobNaGringa;

public interface IJobNaGringaHealthCheckUseCase
{
    public Task<IList<Job>> HealthCheck();
}

public class JobNaGringaHealthCheckUseCase : IJobNaGringaHealthCheckUseCase
{
    private readonly IJobNaGringaCrawler _jobNaGringaCrawler;
    private readonly ILogger<JobNaGringaHealthCheckUseCase> _logger;
    
    public JobNaGringaHealthCheckUseCase(IJobNaGringaCrawler jobNaGringaCrawler, ILogger<JobNaGringaHealthCheckUseCase> logger)
    {
        _jobNaGringaCrawler = jobNaGringaCrawler;
        _logger = logger;
    }

    public async Task<IList<Job>> HealthCheck()
    {
        _logger.LogInformation("Running Job na Gringa HealthCheck");
        
        var jobs = new List<Job>();
        
        var job = await _jobNaGringaCrawler.Run(1);
        
        if (job.Count == 0)
        {
            _logger.LogError("HealthCheck Failed: no response for Job na Gringa crawler, returned empty jobs");
            return new List<Job>();
        }
        
        jobs.AddRange(job);

        return jobs;
    }
}