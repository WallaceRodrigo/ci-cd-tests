namespace Crawlers.Core.Shared.Domain;

public class Job : MutableEntity
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ExternalLink { get; set; }
    public Source Source { get; set; }
    public Seniority? Seniority { get; set; }
    public DateTime? PostedAt { get; set; }
    //TODO: how can we display how many years of experience this job requires for each technology ? i.e. Java - 5yoe
    public ICollection<Technology> Technologies { get; set; }
}


public enum Source
{
    LatamRecruit,
    JobNaGringa,
    LinkedinJobs,
    WeWorkRemotly
}

public enum Seniority
{
    Internship,
    Junior,
    Midlevel,
    Senior,
    Manager,
    TechLead
}