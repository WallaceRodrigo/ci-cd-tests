namespace Crawlers.Core.Shared.Domain;

public class Technology : MutableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ExternalId { get; set; }
}