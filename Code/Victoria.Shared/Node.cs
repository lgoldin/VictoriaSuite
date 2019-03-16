using Akka.Actor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Victoria.Shared
{
    public class Node
    {
        public string Name { get; set; }

        public bool HasBreakPoint { get; set; }

        public Node NextNode { get; set; }
        
        public virtual Node Execute(IList<StageVariable> variables)
        {
            if (this.NextNode != null)
            {
                return this.NextNode.Execute(variables);
            }

            return null;
        }
    }
}
