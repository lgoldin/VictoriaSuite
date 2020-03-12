using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared.EventArgs
{
    public class SimulationStatusChangedEventArgs : System.EventArgs
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        public SimulationStatusChangedEventArgs(SimulationStatus status)
        {
            //logger.Info("Inicio Estado de simulación cambiando Args de evento");
            this.Status = status;

            //logger.Info("Fin Estado de simulación cambiando Args de evento");
        }

        /// <summary>
        /// Simulation Status
        /// </summary>
        public SimulationStatus Status { get; set; }


    }

    public enum SimulationStatus
    {
        Started,
        Stoped
    }
}
