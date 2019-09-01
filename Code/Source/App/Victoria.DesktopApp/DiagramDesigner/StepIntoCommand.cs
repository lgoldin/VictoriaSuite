using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.DesktopApp.DiagramDesigner
{
    public class StepIntoCommand : System.Windows.Input.RoutedCommand
    {
        public  StepIntoCommand()
        {

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        
        public event EventHandler CanExecuteChanged;
    }
}
