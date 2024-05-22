using Crawlers.Core.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Crawlers.Core.Shared;

public interface IJobsRepository
{
    public Task<IList<Job>> GetJobs();
    public Task AddJob(Job job);
    public Task AddJobs(IList<Job> jobs);
    public Task<IList<Job>> GetJobs(int limit);
    public Task<IList<Job>> GetJobsNotAlreadyInserted(IList<Job> jobs);
    public Task<IList<Job>> GetJobsNotAlreadyInsertedForSource(IList<Job> jobs, Source source);
}

public class JobsRepository : IJobsRepository
{
    private readonly JobDatabaseContext _databaseContext;

    public JobsRepository(JobDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IList<Job>> GetJobs()
    {
        return await _databaseContext
            .Jobs
            .Include(j => j.Technologies)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IList<Job>> GetJobs(int limit)
    {
        return await _databaseContext
            .Jobs
            .Include(j => j.Technologies)
            .OrderByDescending(j => j.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task<Job?> GetJobByExternalId(string externalId)
    {
        return await _databaseContext
            .Jobs
            .Include(j => j.Technologies)
            .Where(j => j.ExternalId == externalId)
            .OrderByDescending(j => j.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task AddJobs(IList<Job> jobs)
    {
        await _databaseContext
            .Jobs
            .AddRangeAsync(jobs);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task AddJob(Job job)
    {
        await _databaseContext.Jobs.AddAsync(job);
        await _databaseContext.SaveChangesAsync();
    }
    
    public async Task<IList<Job>> GetJobsNotAlreadyInserted(IList<Job> jobs)
    {
        var equalityComparer = new JobEqualityComparer();
        
        var alreadyInsertedJobs = await _databaseContext
            .Jobs
            .ToListAsync();
        
        var newJobs = jobs
            .Where(newJob => !alreadyInsertedJobs.Any(dbJob => equalityComparer.Equals(newJob, dbJob)))
            .ToList();

        return newJobs;
    }   
    
    public async Task<IList<Job>> GetJobsNotAlreadyInsertedForSource(IList<Job> jobs, Source source)
    {
        var equalityComparer = new JobEqualityComparer();
        
        var alreadyInsertedJobs = await _databaseContext
            .Jobs
            .Where(job => job.Source == source)
            .ToListAsync();
        
        var newJobs = jobs
            .Where(newJob => !alreadyInsertedJobs.Any(dbJob => equalityComparer.Equals(newJob, dbJob)))
            .ToList();

        return newJobs;
    }
}