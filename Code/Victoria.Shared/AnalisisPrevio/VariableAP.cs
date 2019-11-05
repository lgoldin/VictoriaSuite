using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared.AnalisisPrevio
{
    public class VariableAP
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public string nombre { get; set; }

        public double valor { get; set; }

        public bool vector { get; set; }

        public VariableType type { get; set; }

        public string dimension { get; set; }

        public bool notDimensionable
        {
            get
            {
                return this.vector == false;
            }
        }

        public double i { get; set; }

        public string GetNameForDesigner() 
        {
            try
            {
                //logger.Info("Obtener Nombre para Diseñador");
                return this.vector ? this.nombre.Split('(')[0] + "(I)" : this.nombre;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
