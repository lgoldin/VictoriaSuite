using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.DesktopApp.DiagramDesigner
{
    class DiagramValidationException : Exception
    {
        public DiagramValidationException(string errorMessage): base(errorMessage)
        {
            
        }

    }
}
