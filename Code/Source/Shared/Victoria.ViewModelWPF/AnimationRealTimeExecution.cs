using System.Collections.ObjectModel;
using Victoria.Shared;
using Victoria.Shared.Interfaces;

namespace Victoria.ViewModelWPF
{
    public class AnimationRealTimeExecution
    {
        public AnimationRealTimeExecution(ISimulation simulation, ObservableCollection<ModelWPF.Variable> variables, ObservableCollection<AnimationViewModel> animationsToExecute)
        {
            this.Variables = variables;
            this.AnimationToExecute = animationsToExecute;
            this.Simulation = simulation;
        }

        public ObservableCollection<AnimationViewModel> AnimationToExecute { get; set; }

        public ObservableCollection<ModelWPF.Variable> Variables { get; set; }

        public ISimulation Simulation { get; set; }
    }
}
