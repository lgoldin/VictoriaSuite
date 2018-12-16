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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victoria.UI.SharedWPF;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for SelectCondicionPopUp.xaml
    /// </summary>
    public partial class SelectCondicionPopUp : Window
    {
        public DialogResult Result { get; set; }

        public SelectCondicionPopUp(IEnumerable<string> eventList)
        {
            InitializeComponent();
            this.cmbCondition.ItemsSource = eventList;
            this.cmbCondition.SelectedIndex = 0;
            this.cmbCondition.Focus();
        }

        private void StageNamePopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                this.validateAndReturn();
            }

            if (e.Key == Key.Escape)
            {
                this.Result = UI.SharedWPF.DialogResult.Cancel;
                this.Close();
                e.Handled = true;
            }
        }

        private void btnAccept_OnClick(object sender, RoutedEventArgs e)
        {
            this.validateAndReturn();
        }

        private void validateAndReturn()
        {
            if (!String.IsNullOrEmpty(this.cmbCondition.SelectionBoxItem.ToString()))
            {
                this.Result = UI.SharedWPF.DialogResult.Accept;
                this.Close();
            }
            else
            {
                new InformationPopUp("Debe elegir un Evento.").Show();
            }
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            this.Close();
        }
    }
}
