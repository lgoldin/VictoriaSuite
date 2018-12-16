using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.DesktopApp.View
{
    class ValidationException : Exception
    {
        public ValidationException(string errorMessage): base(errorMessage)
        { 
        
        }
    }
}
