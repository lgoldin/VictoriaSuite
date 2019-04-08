using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Victoria.DesktopApp.DiagramDesigner.Commands
{
    public static class DebugCommands
    {
        public static RoutedUICommand StepOver = new RoutedUICommand("StepOver", "StepOver", typeof(DebugCommands));
        public static RoutedUICommand StepInto = new RoutedUICommand("StepInto", "StepInto", typeof(DebugCommands));
        public static RoutedUICommand Continue = new RoutedUICommand("Continue", "Continue", typeof(DebugCommands));
        public static RoutedUICommand ConditionedContinue  = new RoutedUICommand("ConditionedContinue", "ConditionedContinue", typeof(DebugCommands));
    }
}
