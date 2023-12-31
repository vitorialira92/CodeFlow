using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlowBackend.Exceptions
{
    public class EnterProjectException : Exception
    {
        public string Message { get; private set; }
        public EnterProjectException(string message) { 
            this.Message = message;
        }
    }
}
