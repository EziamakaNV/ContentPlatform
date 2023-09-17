namespace Newsletter.Reporting.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? PublishedOnUtc { get; set; }
    }
}
