using System.Configuration;
using System.Linq;
using Akka.Actor;
using Akka.Configuration.Hocon;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared.Actors
{
    public class StageSimulationActor : ReceiveActor
    {
        public StageSimulationActor()
        {
            Receive<IStageSimulation>(stageSimulation => this.Execute(stageSimulation));
        }

        private void Execute(IStageSimulation stageSimulation)
        {
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
        }

        private IActorRef GetActor(IStageSimulation stageSimulation)
        {
            var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
            var system = ActorSystem.Create("MySystem", akkaConfiguration);

            return system.ActorOf(NodeActor.Props(stageSimulation), "nodeActor_" + stageSimulation.GetHashCode());
        }
    }
}
