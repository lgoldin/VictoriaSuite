using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Victoria.Shared
{
    public class Diagram
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public ObservableCollection<Node> Nodes { get; set; }

        public string Name { get; set; }

        public virtual Node Execute(IList<StageVariable> variables)
        {
            try
            {
                logger.Info("Inicio Execute");
                return this.Nodes.First().Execute(variables);

            }
            catch (Exception ex)
            {
                logger.Error("Error Execute: " + ex.Message);
                throw ex;
            }
        }
    }
}
