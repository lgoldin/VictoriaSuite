using System.Collections.Generic;

namespace Victoria.Shared
{
    public class NodeResult : Node
    {
        public IEnumerable<string> Variables { get; set; }

        public override Node Execute(IList<StageVariable> variables)
        {
            logger.Info("Ejecutar");
            return null;
        }
    }
}
