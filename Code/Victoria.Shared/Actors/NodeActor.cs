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

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AppDomain));
        private readonly ILoggingAdapter logger = Context.GetLogger();
        
        private readonly IStageSimulation stageSimulation;

        private IActorRef mainSimulationActor;

        public NodeActor(IStageSimulation stageSimulation)
        {
            log.Info("Inicio Nodo Actor");
            this.stageSimulation = stageSimulation;

            Receive<Diagram>(diagram => this.Execute(diagram));
            Receive<Node>(node => this.Execute(node));
            log.Info("Fin Nodo Actor");
        }

        public static Props Props(IStageSimulation stageSimulation)
        {
            return Akka.Actor.Props.Create(() => new NodeActor(stageSimulation));
        }

        private IActorRef MainSimulationActor
        {
            get
            {
                log.Info("Inicio Obtener Actor principal de Simulacion");
                if (this.mainSimulationActor == null)
                {
                    var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                    var system = ActorSystem.Create("MySystem", akkaConfiguration);

                    this.mainSimulationActor = system.ActorOf<MainSimulationActor>("mainSimulationActor");
                }
                log.Info("Fin Obtener Actor Principal de Simulacion");
                return this.mainSimulationActor;
            }

            set
            {
                this.mainSimulationActor = value;

            }

        }

        private void Execute(Diagram diagram)
        {
            log.Info("Inicio Ejecutar");
            try
            {
                Node node = diagram.Execute(this.stageSimulation.GetVariables());
                this.Self.Tell(node);
            }
            catch (Exception exception) 
            {
                log.Error("Error Ejecutar:" + exception.Message);
                this.logger.Error(exception, exception.Message);
                stageSimulation.StopExecution(true);
                this.MainSimulationActor.Tell(this.stageSimulation);
            }
            log.Info("Fin Ejectuar");

        }

        private void Execute(Node node)
        {
            log.Info("Inicio Ejecutar");
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
            log.Info("Fin Ejecutar");
        }
    }
}
