using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Victoria.Shared
{
    public class Diagram
    {
        public ObservableCollection<Node> Nodes { get; set; }

        public string Name { get; set; }

        public virtual Node Execute(IList<StageVariable> variables)
        {
            try
            {
                return this.Nodes.First().Execute(variables);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
