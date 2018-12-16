using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared
{
    public class VariableArray : Variable
    {
        public List<Variable> Variables{ get; set; }

        public string Dimension { get; set; }
    }
}
