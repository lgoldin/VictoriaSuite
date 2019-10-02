using System.Configuration;
using System.Linq;
using Akka.Actor;
using Akka.Configuration.Hocon;
using Victoria.Shared.Interfaces;
using System;
namespace Victoria.Shared.Actors
{
    public class StageSimulationActor : ReceiveActor
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public StageSimulationActor()
        {
            try
            {
                //logger.Info("Inicio Actor de Simulacion de Escenario");
                Receive<IStageSimulation>(stageSimulation => this.Execute(stageSimulation));
                //logger.Info("Fin Actor de Simulacion de Escenario");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private void Execute(IStageSimulation stageSimulation)
        {
            try
            {
                //logger.Info("Inicio Ejecutar");
                foreach (var variable in stageSimulation.GetVariables().Where(v => v.Name != "T"))
                {
                    if (variable is StageVariableArray)
                    {
                        foreach (var variableItem in ((StageVariableArray)variable).Variables)
                        {
                            variableItem.ActualValue = variableItem.InitialValue;
                        }
                    }
                    else
                    {
                        variable.ActualValue = variable.InitialValue;
                    }
                }

                var timeVariable = stageSimulation.GetVariables().First(v => v.Name == "T");
                timeVariable.ActualValue = timeVariable.InitialValue;

                this.GetActor(stageSimulation).Tell(stageSimulation.GetMainDiagram());
                //logger.Info("Fin Ejecutar");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private IActorRef GetActor(IStageSimulation stageSimulation)
        {
            try
            {
                //logger.Info("Inicio Obtener Actor");
                var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                var system = ActorSystem.Create("MySystem", akkaConfiguration);
                //logger.Info("Fin Obtener Actor");
                return system.ActorOf(NodeActor.Props(stageSimulation), "nodeActor_" + stageSimulation.GetHashCode());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
