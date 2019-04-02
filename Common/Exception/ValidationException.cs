using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exception
{
    public class ValidationException : System.Exception
    {
        public ValidationException(string msg)
            : base(msg)
        {
        }
    }
}
