namespace Newsletter.Reporting.Shared.Exceptions
{
    public class CreateArticleValidationException : ValidationException
    {
        public CreateArticleValidationException(string message, IEnumerable<string> errorList)
            : base(message, errorList)
        {

        }
    }
}
