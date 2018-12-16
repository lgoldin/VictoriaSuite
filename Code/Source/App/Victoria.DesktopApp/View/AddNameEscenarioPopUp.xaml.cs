using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Victoria.UI.SharedWPF;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for AddSimpleVariablePopUp.xaml
    /// </summary>
    public partial class AddNameEscenarioPopUp : Window
    {
        public DialogResult Result { get; set; }

        public string nombreEscenario { get; set; }

        private List<string> gbList { get; set; }

        public string escenarioUno{ get; set; }

        public string escenarioDos { get; set; }

        public AddNameEscenarioPopUp()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            nombreBox.Focusable = true;
            FocusManager.SetFocusedElement(this, nombreBox);
        }

        public AddNameEscenarioPopUp(List<string> listaGb, string escenarioUno, string escenarioDos)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            nombreBox.Focusable = true;
            FocusManager.SetFocusedElement(this, nombreBox);
            btnAccept.IsEnabled = false;

            gbList = listaGb;
            this.escenarioUno = escenarioUno;
            this.escenarioDos = escenarioDos;
        }

        private void btnAccept_OnClick(object sender, RoutedEventArgs e)
        {
            if (gbList.Count(x => x == nombreBox.Text) > 0 || escenarioUno == nombreBox.Text || escenarioDos == nombreBox.Text)
            {
                LblError.Visibility = System.Windows.Visibility.Visible;
                btnAccept.IsEnabled = false;
                return;
            }

            this.Result = UI.SharedWPF.DialogResult.Accept;
            nombreEscenario = this.nombreBox.Text;
            
            this.Close();
            
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            this.Close();
        }

        private void txtBox_nombreBoxChaged(object sender, EventArgs e)
        {
            setVisibilityBtnAceptar();
            LblError.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void setVisibilityBtnAceptar()
        {
            btnAccept.IsEnabled = !string.IsNullOrWhiteSpace(nombreBox.Text);
        }

        private void nombreBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnAccept_OnClick(new Object(), new RoutedEventArgs());
            }
        }
    }
}
