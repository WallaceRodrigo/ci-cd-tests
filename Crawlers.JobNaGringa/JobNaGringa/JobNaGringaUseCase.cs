using Crawlers.Core.Shared;
using Crawlers.Core.Shared.Domain;

namespace Crawlers.JobNaGringa.JobNaGringa;

public interface IJobNaGringaUseCase
{
    public Task<IList<Job>> RunCrawler();
}

public class JobNaGringaUseCase : IJobNaGringaUseCase
{
    private readonly ILogger<JobNaGringaUseCase> _logger;
    private readonly IJobNaGringaCrawler _jobNaGringaCrawler;
    private readonly IJobsRepository _jobsRepository;

    public JobNaGringaUseCase(IJobNaGringaCrawler jobNaGringaCrawler, 
        IJobsRepository jobsRepository,
        ILogger<JobNaGringaUseCase> logger)
    {
        _jobNaGringaCrawler = jobNaGringaCrawler;
        _jobsRepository = jobsRepository;
        _logger = logger;
    }
    
    public async Task<IList<Job>> RunCrawler()
    {
        try
        {
            _logger.LogInformation("Starting JobNaGringaCrawler");
            
            var jobs = await _jobNaGringaCrawler.Run();
            var newJobs = await _jobsRepository.GetJobsNotAlreadyInsertedForSource(jobs, Source.JobNaGringa);
            
            _logger.LogInformation("got: {count} new jobs for job na gringa crawler", newJobs.Count);
            
            if (newJobs.Any())
            {
                await _jobsRepository.AddJobs(newJobs);
            }

            return newJobs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error JobNaGringaCrawler");
        }

        return new List<Job>();
    }
}