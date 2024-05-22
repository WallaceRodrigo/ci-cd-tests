namespace Crawlers.Core.Shared.Domain;

public class Role : MutableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ExternalId { get; set; }
}