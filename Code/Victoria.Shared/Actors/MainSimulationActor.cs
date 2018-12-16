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

        public MainSimulationActor()
        {
            Receive<ISimulation>(simulation => this.Execute(simulation));
            Receive<IStageSimulation>(simulationStage => this.UpdateSimulation(simulationStage));
        }

        public IActorRef StageSimulationActor
        {
            get
            {
                if (this.stageSimulationActor == null)
                {
                    var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                    var system = ActorSystem.Create("MySystem", akkaConfiguration);

                    this.stageSimulationActor = system.ActorOf<StageSimulationActor>("stageSimulationActor");
                }

                return this.stageSimulationActor;
            }

            set
            {
                this.stageSimulationActor = value;
            }
        }
        
        private void Execute(ISimulation simulation)
        {
            var simulationStage = new StageSimulation(simulation);
            this.StageSimulationActor.Tell(simulationStage);
        }

        private void UpdateSimulation(IStageSimulation stageSimulation)
        {
            stageSimulation.GetSimulation().Update(stageSimulation);
        }
    }
}
