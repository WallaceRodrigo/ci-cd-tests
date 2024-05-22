using System.Diagnostics;
using Crawlers.Core.Shared;
using Crawlers.Core.Shared.Domain;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace Crawlers.JobNaGringa.JobNaGringa;

public interface IJobNaGringaCrawler
{
    public Task<IList<Job>> Run(int? limit = null);
}

public class JobNaGringaCrawler : IJobNaGringaCrawler
{
    private readonly IJobNaGringaParser _jobNaGringaParser;
    private readonly ILogger<JobNaGringaCrawler> _logger;
    private readonly IBaseHttpClient _baseHttpClient;
    private readonly IOptions<JobNaGringaConfiguration> _options;

    public JobNaGringaCrawler(IJobNaGringaParser jobNaGringaParser, 
        ILogger<JobNaGringaCrawler> logger, 
        IBaseHttpClient baseHttpClient, 
        IOptions<JobNaGringaConfiguration> options)
    {
        _jobNaGringaParser = jobNaGringaParser;
        _logger = logger;
        _baseHttpClient = baseHttpClient;
        _options = options;
    }
    
    public async Task<IList<Job>> Run(int? limit = null)
    {
        _logger.LogInformation("Running Job na Gringa crawler");
        
        var sw = new Stopwatch();
        
        sw.Start();
        
        var html = await _baseHttpClient.GetAsync(_options.Value.JobsUrl);

        if (string.IsNullOrEmpty(html))
        {
            _logger.LogError("empty html for url: {url}", _options.Value.JobsUrl);
            return new List<Job>();
        }
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var urls = doc
            .DocumentNode
            .SelectNodes("//a[contains(@class, 'job-card_link w-inline-block')]/@href")?
            .Select(node => node.GetAttributeValue("href", ""))
            .ToList();

        if (urls == null || urls.Count == 0)
        {
            _logger.LogError("error getting links - job na gringa");
            return new List<Job>();
        }
        
        if (limit.HasValue)
        {
            urls = urls.Take((int)limit).ToList();
        }
        
        _logger.LogInformation("got {jobs} jobs", urls.Count);

        var jobs = new List<Job>();
        foreach (var url in urls) // TODO: parallelize instead of foreach
        {
            var postUrl = $"{_options.Value.BaseUrl}{url}";
            try
            {
                var job = await _jobNaGringaParser.Parse(postUrl);
                if (job != null)
                {
                    jobs.Add(job);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error getting url: {url}", url);
            }
        }
        
        sw.Stop();
        _logger.LogInformation("finished running job na gringa crawler in {elapsed} s", sw.ElapsedMilliseconds / 100);

        return jobs;
    }
}