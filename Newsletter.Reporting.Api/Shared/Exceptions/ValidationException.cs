using System.Collections;

namespace Newsletter.Reporting.Shared.Exceptions
{
    public abstract class ValidationException : Exception
    {
        private readonly IEnumerable<string> _errorList;
        protected ValidationException(string message, IEnumerable<string> errorList = default!) 
            : base(message)
        {
            _errorList = errorList;
        }

        public override IDictionary Data
        {
            get
            {
                return new Dictionary<string, IEnumerable<string>>
                {
                    { "errors", _errorList }
                };

            }
        }
    }
}
