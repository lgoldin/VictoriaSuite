using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Victoria.Shared.Prism;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using Akka.Actor;
using Akka.Configuration.Hocon;
using System.Configuration;
using Victoria.ViewModelWPF.Actors;

namespace Victoria.ViewModelWPF
{
    public class AnimationExecutionViewModel : AnimationExecutionViewModelBase
    {
        protected new DelegateCommand executeAnimationsCommand;
        protected new DelegateCommand stopAnimationsCommand;

        private ObservableCollection<AnimationViewModel> animations;
        private ObservableCollection<ModelWPF.Variable> variables;
        private bool executeButtonEnabled;
        private IActorRef actor;

        public int animationWait = 400;
        public ObservableCollection<AnimationViewModel> Animations
        {
            get { return animations; }
            set
            {
                animations = value;
                this.RaisePropertyChanged("Animations");
            }
        }

        public IActorRef Actor
        {
            get
            {
                if (this.actor == null)
                {
                    var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                    var system = ActorSystem.Create("MySystem", akkaConfiguration);
                    this.actor = system.ActorOf(AnimationActor.Props(this), "animationActor");
                }

                return this.actor;
            }

            set
            {
                this.actor = value;
            }
        }

        public ObservableCollection<ModelWPF.Variable> Variables
        {
            get { return variables; }
            set
            {
                variables = value;
                this.RaisePropertyChanged("Variables");
            }
        }

        public bool ExecuteButtonEnabled
        {
            get { return executeButtonEnabled; }
            set
            {
                executeButtonEnabled = value;
                this.RaisePropertyChanged("ExecuteButtonEnabled");
            }
        }

        public int AnimationWait
        {
            get { return animationWait; }
            set
            {
                animationWait = value;
                this.RaisePropertyChanged("AnimationWait");
            }
        }

        public bool AnimationsExecuting { get; set; }

        public new ICommand ExecuteAnimationsCommand
        {
            get
            {
                return this.executeAnimationsCommand;
            }
        }
        public new ICommand StopAnimationsCommand
        {
            get
            {
                return this.stopAnimationsCommand;
            }
        }

        public AnimationExecutionViewModel()
        {
            this.executeAnimationsCommand = new DelegateCommand(this.ExecuteAnimations);
            this.stopAnimationsCommand = new DelegateCommand(this.StopAnimations);
        }

        private void ExecuteAnimations()
        {
            this.AnimationsExecuting = true;
            this.Actor.Tell("Execute");
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            this.StopAnimations();
        }

        private void StopAnimations()
        {
            this.AnimationsExecuting = false;
        }
    }
}