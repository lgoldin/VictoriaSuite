using System.Collections.Generic;
using Victoria.Shared.EventArgs;

namespace Victoria.Shared.Interfaces
{
    public interface IStageSimulation
    {
        void StopExecution(bool value);

        void StopDebugExecution(bool value);

        bool DebugginMode();

        bool CanContinue();

        List<StageVariable> GetVariables();

        Diagram GetMainDiagram();

        bool GetExecutionStatus();

        bool MustNotifyUI();

        ISimulation GetSimulation();
    }
}
