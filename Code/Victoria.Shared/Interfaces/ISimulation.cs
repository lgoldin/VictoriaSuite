using System.Collections.Generic;
using Victoria.Shared.EventArgs;

namespace Victoria.Shared.Interfaces
{
    public interface ISimulation
    {
        bool HasStatusChanged();

        void ChangeStatus(SimulationStatus status);

        void StopExecution(bool value);

        bool CanContinue();

        bool DebugginMode();

        void Update(IStageSimulation simulation);

        List<Diagram> GetDiagrams();

        List<Variable> GetVariables();

        double GetVariableValue(string name);
    }
}
