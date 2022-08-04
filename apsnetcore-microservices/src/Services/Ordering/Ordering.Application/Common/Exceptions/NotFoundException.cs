using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message)
        {

        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {

        }
    }
}
