using System;
using System.Collections.ObjectModel;
using System.Linq;
using Akka.Actor;
using Victoria.ModelWPF;
using Victoria.Shared.Interfaces;

namespace Victoria.ViewModelWPF.Actors
{
    public class AnimationRealTimeActor : ReceiveActor
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(System.AppDomain));
        public AnimationRealTimeActor()
        {
            Receive<AnimationRealTimeExecution>(animationRealTime => this.PrepareExecution(animationRealTime));
            Receive<string>(message =>
            {
                switch (message)
                {
                    default:
                    case "Execute":
                        this.Execute();
                        break;
                    case "Animate":
                        this.Animate();
                        break;
                }
            });
        }

        public ObservableCollection<Variable> Variables { get; set; }

        public ISimulation Simulation { get; set; }

        public ObservableCollection<AnimationViewModel> Animations { get; set; }

        public int Index { get; set; }

        public bool CanContinue { get; set; }

        private void PrepareExecution(AnimationRealTimeExecution animationRealTime)
        {
            this.Index = 0;
            this.CanContinue = true;
            this.Animations = animationRealTime.AnimationToExecute;
            this.Variables = animationRealTime.Variables;
            this.Simulation = animationRealTime.Simulation;
            
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, TimeSpan.FromMilliseconds(40), Self, "Execute", Self);
        }

        private void Execute()
        {
            try
            {
                Self.Tell("Animate");
                if (!this.Simulation.CanContinue())
                {
                    Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(20), Self, PoisonPill.Instance, Self);
                }
            }
            catch
            {
                Self.Tell(PoisonPill.Instance);
            }
        }

        private void Animate()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        foreach (var singleAnimation in this.Animations.Where(x => x.AnimationConfig.CanExecute))
                        {
                            foreach (var animationVariable in singleAnimation.AnimationConfig.Variables)
                            {
                                animationVariable.Values.Add(this.Simulation.GetVariableValue(animationVariable.Name));
                            }

                            singleAnimation.BindSimulationVariableValues();
                            singleAnimation.DoAnimation(this.Index);
                        }
                    }
                    catch
                    {
                        //TODO:Something
                    }
                    finally
                    {
                        this.Index++;
                    }
                }
                ));
            }
            catch(Exception ex)
            {
                logger.Error(ex.Source + " - " + ex.Message + ": " + ex.StackTrace);                                
            }

        }
    }
}
