using System;
using System.Linq;
using Akka.Actor;
using System.Threading;

namespace Victoria.ViewModelWPF.Actors
{
    public class AnimationActor : ReceiveActor
    {
        private readonly AnimationExecutionViewModel animation;

        public AnimationActor(AnimationExecutionViewModel animation)
        {
            this.animation = animation;

            Receive<string>(message => this.Execute());
        }

        public static Props Props(AnimationExecutionViewModel animation)
        {
            return Akka.Actor.Props.Create(() => new AnimationActor(animation));
        }

        private void Execute()
        {
            var timeValuesList = this.animation.Variables.First(n => n.Name == "T").ValuesEnumerable.ToList();
            
            for (var timeValue = 0; timeValue < timeValuesList.Count; timeValue++)
            {
                if (this.animation.AnimationsExecuting)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (var singleAnimation in this.animation.Animations)
                        {
                            foreach (var variable in this.animation.Variables)
                            {
                                if (variable.Values.Count > timeValue)
                                {
                                    variable.ActualValue = variable.Values[timeValue];
                                }
                            }

                            singleAnimation.DoAnimation(timeValue);
                        }
                    }));

                    Thread.Sleep(this.animation.AnimationWait);
                }
                else
                {
                    break;
                }
            }

            this.animation.ExecuteButtonEnabled = true;
        }
    }
}
