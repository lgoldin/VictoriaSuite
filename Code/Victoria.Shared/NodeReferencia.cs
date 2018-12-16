using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared
{
    public class NodeReferencia : Node
    {
        public string Code { get; set; }

        public override Node Execute(IList<StageVariable> variables)
        {
            return this.NextNode;
        }
    }
}
