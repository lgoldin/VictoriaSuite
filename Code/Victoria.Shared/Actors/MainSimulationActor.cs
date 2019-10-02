using System;
using System.Configuration;
using Akka.Actor;
using Akka.Configuration.Hocon;
using Jint.Parser.Ast;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared.Actors
{
    public class MainSimulationActor : ReceiveActor
    {
        
        private IActorRef stageSimulationActor;
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));

        public MainSimulationActor()
        {
            try
            {
                //logger.Info("Inicio actor principal de simulación");

                Receive<ISimulation>(simulation => this.Execute(simulation));
                Receive<IStageSimulation>(simulationStage => this.UpdateSimulation(simulationStage));
                //logger.Info("Fin actor principal de simulación");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public IActorRef StageSimulationActor
        {

            get
            {
                try
                {
                    //logger.Info("Inicio Actor de simulación de escenario");
                    if (this.stageSimulationActor == null)
                    {
                        var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                        var system = ActorSystem.Create("MySystem", akkaConfiguration);

                        this.stageSimulationActor = system.ActorOf<StageSimulationActor>("stageSimulationActor");
                    }

                    //logger.Info("Fin Actor de simulación de escenario");
                    return this.stageSimulationActor;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                    throw ex;
                }
            }

            set
            {
                this.stageSimulationActor = value;
            }
        }
        
        private void Execute(ISimulation simulation)
        {
            try
            {
                //logger.Info("Inicio Ejecutar");
                var simulationStage = new StageSimulation(simulation);
                this.StageSimulationActor.Tell(simulationStage);
                //logger.Info("Fin Ejecutar");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        private void UpdateSimulation(IStageSimulation stageSimulation)
        {
            //logger.Info("Inicio Actualizar Simulacion");
            stageSimulation.GetSimulation().Update(stageSimulation);
            //logger.Info("Fin Actualizar Simulacion");
        }
    }
}
