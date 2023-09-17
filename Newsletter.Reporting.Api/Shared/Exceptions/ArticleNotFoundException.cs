namespace Newsletter.Reporting.Shared.Exceptions
{
    public class ArticleNotFoundException : NotFoundException
    {
        public ArticleNotFoundException(Guid articleId, IEnumerable<string> errorList = default!)
            : base($"The article with Id: {articleId} does not exist in the database.", errorList)
        {

        }
    }
}
