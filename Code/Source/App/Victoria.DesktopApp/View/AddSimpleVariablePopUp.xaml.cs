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
    public partial class AddSimpleVariablePopUp : Window
    {
        public DialogResult Result { get; set; }

        public AddSimpleVariablePopUp()
        {
            InitializeComponent();
            this.nombreBox.Focus();
        }
        public AddSimpleVariablePopUp(string tittle)
        {
            InitializeComponent();
            this.tittleLabel.Content = tittle; 
            this.nombreBox.Focus();
        } 
        private void btnAccept_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Accept;
            this.Close();
            
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            this.Close();
        }

        private void SimpleVariablePopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.Result = UI.SharedWPF.DialogResult.Accept;
                this.Close();
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                this.Result = UI.SharedWPF.DialogResult.Cancel;
                this.Close();
                e.Handled = true;
            }
        }
    }
}
