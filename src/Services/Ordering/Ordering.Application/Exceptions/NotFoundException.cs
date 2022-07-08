using System;


namespace Ordering.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {  // custom exception, the only custom thing is the message through base constructor
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found")
        {
        }
    }
}
