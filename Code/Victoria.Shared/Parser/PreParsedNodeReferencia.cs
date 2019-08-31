using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared.Parser
{
    public class PreParsedNodeReferencia : PreParsedNode
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public override void posprocesar(Dictionary<string, PreParsedNode> nodosPreProcesados, List<Node> nodos)
        {
            logger.Info("Inicio Posprocesar");
            if (this.next == null || !this.next.Any())
                this.node.NextNode = (getRefNode(nodosPreProcesados, "r" + this.name)).node;
            else
                this.node.NextNode = getRefNode(nodosPreProcesados, this.next[0]).node;

            nodos.Add(this.node);
            logger.Info("Fin Posprocesar");
        }
    }
}
