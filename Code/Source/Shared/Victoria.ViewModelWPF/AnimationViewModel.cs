using System.Windows.Shapes;
using Victoria.ModelWPF;
using Victoria.Shared.Prism;
using System.Collections.ObjectModel;

namespace Victoria.ViewModelWPF
{
    public abstract class AnimationViewModel : NotificationObject
    {
        private string animationName;
        private bool canExecute;
        private string variableToAnimateValue;
        public string animationOrientation = "Vertical";
        private ObservableCollection<Shape> animationElementsList;
        public AnimationConfigurationBase AnimationConfig { get; set; }

        public abstract string ConfigurationType { get; set; }

        public string VariableToAnimateName { get; set; }

        public ObservableCollection<Shape> AnimationElementsList
        {
            get { return animationElementsList; }
            set
            {
                animationElementsList = value;
                this.RaisePropertyChanged("AnimationElementsList");
            }
        }

        public string AnimationName
        {
            get { return animationName; }
            set
            {
                animationName = value;
                this.RaisePropertyChanged("AnimationName");
            }
        }

        public string AnimationOrientation
        {
            get { return animationOrientation; }
            set
            {
                animationOrientation = value;
                this.RaisePropertyChanged("AnimationOrientation");
            }
        }

        private double x;
        private double y;

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                this.RaisePropertyChanged("Y");
            }
        }

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                this.RaisePropertyChanged("X");
            }
        }

        public string VariableToAnimateValue
        {
            get { return variableToAnimateValue; }
            set
            {
                variableToAnimateValue = value;
                this.RaisePropertyChanged("VariableToAnimateValue");
            }
        }

        public virtual void InitializeAnimation(AnimationConfigurationBase animationConfig)
        {
            this.AnimationConfig = animationConfig;
            this.AnimationName = this.AnimationConfig.AnimationName;
        }

        public abstract void BindSimulationVariableValues();

        public abstract void DoAnimation(int index);
    }
}
