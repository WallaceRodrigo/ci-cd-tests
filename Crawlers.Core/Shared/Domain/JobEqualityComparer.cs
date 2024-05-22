namespace Crawlers.Core.Shared.Domain;

public class JobEqualityComparer : IEqualityComparer<Job>
{
    public bool Equals(Job? x, Job? y)
    {
        return x.Title == y.Title && x.ExternalLink == y.ExternalLink;
    }

    public int GetHashCode(Job job)
    {
        return HashCode.Combine(job.Title, job.ExternalLink);
    }
}