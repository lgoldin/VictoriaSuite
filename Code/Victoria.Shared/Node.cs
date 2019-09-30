using Akka.Actor;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Victoria.Shared
{
    public class Node
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(System.AppDomain));
        public string Name { get; set; }

        public Node NextNode { get; set; }
        
        public virtual Node Execute(IList<StageVariable> variables)
        {
            //logger.Info("Inicio Ejecutar Nodo");
            if (this.NextNode != null)
            {
                //logger.Info("Fin Ejecutar Nodo");

                return this.NextNode.Execute(variables);
            }
            //logger.Info("Fin Ejecutar Nodo null");


            return null;
        }
    }
}
