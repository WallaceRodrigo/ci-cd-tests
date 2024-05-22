using Crawlers.Core.Shared;
using Crawlers.Core.Shared.Domain;
using HtmlAgilityPack;

namespace Crawlers.JobNaGringa.JobNaGringa;

public interface IJobNaGringaParser
{
    public Task<Job?> Parse(string url);
}

public class JobNaGringaParser : IJobNaGringaParser
{
    private readonly IIdGenService _idGenService;
    private readonly ILogger<JobNaGringaParser> _logger;
    private readonly IBaseHttpClient _baseHttpClient;
    
    public JobNaGringaParser(IIdGenService idGenService, 
        ILogger<JobNaGringaParser> logger, 
        IBaseHttpClient baseHttpClient)
    {
        _idGenService = idGenService;
        _logger = logger;
        _baseHttpClient = baseHttpClient;
    }

    public async Task<Job?> Parse(string url)
    {
        var html = await _baseHttpClient.GetAsync(url);

        if (string.IsNullOrEmpty(html))
        {
            _logger.LogError("empty response for url: {url}", url);
            return null;
        }
        
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var headerDiv = doc.DocumentNode.SelectSingleNode("//div[@class='job-single_header']");

        if (headerDiv == null)
        {
            _logger.LogError("Error parsing job na gringa header div for url: {url}", url);
            return null;
        }
        
        var titleNode = headerDiv.SelectSingleNode("//h1[@class='job-single_heading']");
        var title = titleNode?.InnerHtml;
        
        if (string.IsNullOrEmpty(title))
        {
            _logger.LogInformation("empty title on job na gringa for url: {url}", url);
            return null;
        }
        
        var descriptionDiv = doc.DocumentNode.SelectSingleNode("//div[@class='job-position_body-content w-richtext']");
        var descriptionText = descriptionDiv?.InnerHtml;

        if (string.IsNullOrEmpty(descriptionText))
        {
            _logger.LogInformation("empty description on job na gringa for url: {url}", url);
            return null;
        }
        
        return new Job
        {
            ExternalId = await _idGenService.GetIdAsync(),
            Title = title,
            Description = descriptionText,
            ExternalLink = url,
            Source = Source.JobNaGringa,
            Seniority = null, // TODO: fill Seniority
            Technologies = null, //TODO: use chatgpt to get the technologies
        };
    }
}