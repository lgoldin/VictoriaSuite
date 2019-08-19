using System;
using System.Collections.Generic;
using System.Linq;

namespace Victoria.Shared
{

    public class PreParsedNode
    {
        public string name;
        public Node node;
        public IList<string> next;


        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public virtual void posprocesar(Dictionary<string, PreParsedNode> nodosPreProcesados, List<Node> nodos)
        {
            logger.Info("Inicio Posprocesar");
            this.node.NextNode = this.next != null && this.next.Any() ? getRefNode(nodosPreProcesados, this.next[0]).node : null;
            nodos.Add(this.node);
            logger.Info("Fin Posprocesar");
        }

        public PreParsedNode getRefNode(Dictionary<string, PreParsedNode> nodos, string reference)
        {
            logger.Info("Inicio Obtener referencia Nodo");
            if (!nodos.ContainsKey(reference))
            {
                throw new InvalidNodeReferenceException(this.name, this.next);
            }
            logger.Info("Fin obtener referencia nodo");
            return nodos[reference];
        }
    }

}