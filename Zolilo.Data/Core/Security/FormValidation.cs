using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zolilo.Security
{

    /// <summary>
    /// throw new SecurityValidationException when detecting invalid user input that makes it past client-side validation 
    /// </summary>
    public class SecurityValidationException : FormatException
    {
        public SecurityValidationException(string message) : base(message)
        {
        }
    }
}