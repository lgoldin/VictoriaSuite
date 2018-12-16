using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared
{
    public class StageVariableArray : StageVariable
    {
        public List<StageVariable> Variables{ get; set; }
    }
}
