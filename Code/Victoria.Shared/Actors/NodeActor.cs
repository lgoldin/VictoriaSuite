using System;
using System.Configuration;
using Akka.Actor;
using Akka.Configuration.Hocon;
using Akka.Event;
using Victoria.Shared.Interfaces;

namespace Victoria.Shared.Actors
{
    public class NodeActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();
        
        private readonly IStageSimulation stageSimulation;

        private IActorRef mainSimulationActor;

        public NodeActor(IStageSimulation stageSimulation)
        {
            this.stageSimulation = stageSimulation;

            Receive<Diagram>(diagram => this.Execute(diagram));
            Receive<Node>(node => this.Execute(node));
        }

        public static Props Props(IStageSimulation stageSimulation)
        {
            return Akka.Actor.Props.Create(() => new NodeActor(stageSimulation));
        }

        private IActorRef MainSimulationActor
        {
            get
            {
                if (this.mainSimulationActor == null)
                {
                    var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                    var system = ActorSystem.Create("MySystem", akkaConfiguration);

                    this.mainSimulationActor = system.ActorOf<MainSimulationActor>("mainSimulationActor");
                }

                return this.mainSimulationActor;
            }

            set
            {
                this.mainSimulationActor = value;
            }
        }

        private void Execute(Diagram diagram)
        {

            try
            {
                // Chequeo si el nodo tiene breakpoint en modoDebug
                //while (this.stageSimulation.DebugginMode() && XMLParser.getExecutingNode() != null)
                //{
                //
                //}

                // Seteo en el XMLParser que estoy en modo debug
                if (this.stageSimulation.DebugginMode())
                    XMLParser.setDebuggingNode(true);

                Node node = diagram.Execute(this.stageSimulation.GetVariables());
                this.Self.Tell(node);

            }
            catch (Exception exception) 
            {
                this.logger.Error(exception, exception.Message);
                stageSimulation.StopExecution(true);
                this.MainSimulationActor.Tell(this.stageSimulation);
            }
        }

        private void Execute(Node node)
        {
            if (node != null && this.stageSimulation.CanContinue())
            {
                 node = node.Execute(this.stageSimulation.GetVariables());
                    
                if (node != null)
                {
                    this.Self.Tell(node);
                }
                else
                {
                    stageSimulation.StopExecution(true);
                }                    
            }
            else
            {
                stageSimulation.StopExecution(true);
            }

            if (this.stageSimulation.MustNotifyUI())
            {
                this.MainSimulationActor.Tell(this.stageSimulation);
            }
        }
    }
}
