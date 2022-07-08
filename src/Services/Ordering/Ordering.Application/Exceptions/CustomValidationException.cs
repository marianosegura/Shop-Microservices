using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class CustomValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get;  }


        public CustomValidationException(IEnumerable<ValidationFailure> failures)
            : this()  // call the below constructor to set message
        {  // fluent validators (like UpdateOrderCommandValidator) throw ValidationFailure objects
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(group => group.Key, group => group.ToArray());
        }


        public CustomValidationException()  // no errors provided constructor
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
}
