﻿using System.Collections;

namespace Newsletter.Api.Shared.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        private readonly IEnumerable<string> _errorList;
        protected NotFoundException(string message, IEnumerable<string> errorList = default!)
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
