namespace Crawlers.JobNaGringa.JobNaGringa;

public class JobNaGringaConfiguration
{
    public const string JobNaGringaNameConfiguration = "JobNaGringaConfiguration";
    public const string HttpClientName = "JobNaGringaHttpClientName";
    public string BaseUrl { get; set; }
    public string JobsUrl => $"{BaseUrl}/jobs";
}