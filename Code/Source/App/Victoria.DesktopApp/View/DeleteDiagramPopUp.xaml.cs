using System.Windows;
using System.Windows.Input;
using Victoria.UI.SharedWPF;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for DeleteDiagramPopUp.xaml
    /// </summary>
    public partial class DeleteDiagramPopUp : Window
    {
        public DeleteDiagramPopUp()
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
        }

        public DialogResult Result { get; set; }

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
    }
}
