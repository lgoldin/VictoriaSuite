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
       
            try
            {
                //logger.Info("Inicio Ejecutar Nodo");
                if (this.NextNode != null)
                {
                    //logger.Info("Fin Ejecutar Nodo");

                    return this.NextNode.Execute(variables, NotifyUIMethod);
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Ocurrio un error en la ejecución"));
                logger.Error(String.Format("Estado de Variables: {0}",VariablesToString(variables)));
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public String VariablesToString(IList<StageVariable> _variables)
        {
            String cadena = String.Empty;
            foreach (StageVariable v in _variables)
            {
                cadena += v.Name + ": " + v.ActualValue.ToString() + " | ";// System.Environment.NewLine;
            }
            return cadena.Remove(cadena.Length - 3);
        }
    }
}
