using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVisor.Core.Exceptions
{
    public class WrongFileFormatException : Exception
    {
        public WrongFileFormatException(string extension, string expectedExtension, string filePath) : base($"The file '{filePath}' is not a *.{expectedExtension} file. Instead it is a {extension}.") { }
    }
}
