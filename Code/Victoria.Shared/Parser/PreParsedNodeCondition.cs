using System.Collections.Generic;
using System;
namespace Victoria.Shared
{
    public class PreParsedNodeCondition : PreParsedNode
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public override void posprocesar(Dictionary<string, PreParsedNode> nodosPreProcesados, List<Node> nodos)
        {
            logger.Info("Inicio Posprocesar");
           ((NodeCondition)this.node).ChilNodeTrue = getRefNode(nodosPreProcesados, this.next[0]).node;
           ((NodeCondition)this.node).ChildNodeFalse = getRefNode(nodosPreProcesados, this.next[1]).node;
            nodos.Add(this.node);
            logger.Info("Fin Posprocesar");
        }

    }

}
