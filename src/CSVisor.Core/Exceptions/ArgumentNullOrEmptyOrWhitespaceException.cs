using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVisor.Core.Exceptions
{
    public class ArgumentNullOrEmptyOrWhitespaceException : ArgumentException
    {
        public ArgumentNullOrEmptyOrWhitespaceException(string paramName) : base("Argument is null or empty or contains only whitespaces.", paramName) { }
    }
}
