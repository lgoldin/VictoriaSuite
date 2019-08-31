using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Victoria.Shared.Debug;


namespace Victoria.Shared
{
    public class Node
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(System.AppDomain));
        public string Name { get; set; }

        public bool HasBreakPoint { get; set; }

        public Node NextNode { get; set; }

        public virtual bool canBeDebugged
        {
            get { return false; } 
        }

        public virtual Node Execute(IList<StageVariable> variables, Delegate NotifyUIMethod)
        {   
            logger.Info("Inicio Ejecutar Nodo");
            if (this.NextNode != null)
            {
                logger.Info("Fin Ejecutar Nodo");

                    return this.NextNode.Execute(variables, NotifyUIMethod);
            }
            logger.Info("Fin Ejecutar Nodo null");


            Debug.Debug.instance().execute(this, NotifyUIMethod,variables);
            return null;
        }
    }
}
