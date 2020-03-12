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

        public delegate void DelegateNotifyUI();

        public void notifyUserInterace()
        {
            this.MainSimulationActor.Tell(this.stageSimulation);
        }

        public NodeActor(IStageSimulation stageSimulation)
        {
            try
            {
                //log.Info("Inicio Nodo Actor");
                this.stageSimulation = stageSimulation;

                Receive<Diagram>(diagram => this.Execute(diagram));
                Receive<Node>(node => this.Execute(node));
                //log.Info("Fin Nodo Actor");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }

        public static Props Props(IStageSimulation stageSimulation)
        {
            return Akka.Actor.Props.Create(() => new NodeActor(stageSimulation));
        }

        private IActorRef MainSimulationActor
        {
            get
            {
                //log.Info("Inicio Obtener Actor principal de Simulacion");
                if (this.mainSimulationActor == null)
                {
                    var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                    var system = ActorSystem.Create("MySystem", akkaConfiguration);

                    this.mainSimulationActor = system.ActorOf<MainSimulationActor>("mainSimulationActor");
                }
                //log.Info("Fin Obtener Actor Principal de Simulacion");
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
                DelegateNotifyUI notifyUserInteraceMethod = notifyUserInterace;
                log.Info("Inicio Ejecutar");
                Node node = diagram.Execute(this.stageSimulation.GetVariables(), notifyUserInteraceMethod); this.Self.Tell(node);
                //log.Info("Fin Ejectuar");
            }
            catch (Exception exception) 
            {
                log.Error("Error Ejecutar:" + exception.Message);
                this.//logger.Error(exception, exception.Message);
                stageSimulation.StopExecution(true);
                if(stageSimulation.DebugginMode())
                    stageSimulation.StopDebugExecution(true);
                this.MainSimulationActor.Tell(this.stageSimulation);
                this.Self.Tell(PoisonPill.Instance);
                this.MainSimulationActor.Tell(PoisonPill.Instance);
            }
        }

        private void Execute(Node node)
        {
            try
            {
                //log.Info("Inicio Ejecutar");            
                if (node != null && this.stageSimulation.CanContinue())
                {
                    DelegateNotifyUI notifyUserInteraceMethod = notifyUserInterace;

                    node = node.Execute(this.stageSimulation.GetVariables(), notifyUserInteraceMethod);

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
                //log.Info("Fin Ejecutar");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);
                throw ex;
            }
        }
    }
}
