using System.Windows;
using Victoria.UI.SharedWPF;
using DiagramDesigner;
using System.ComponentModel;
using System.Windows.Input;
using Victoria.Shared.AnalisisPrevio;
using System.Windows.Controls;

namespace DiagramDesigner
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Closing += HideWindow;
            this.diagrama().dataGridVariables = this.dataGridVariables;
            this.diagrama().dimensiones = this.dimensiones;
        }

        public DialogResult Result { get; set; }

        public string StageName { get; set; }


        public DesignerCanvas diagrama() 
        {
            return this.MyDesigner;
        }

        public void HideWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) 
            {
                this.WindowState = WindowState.Normal;
                this.dataGridVariables.Height = 180;
            } 
            else 
            { 
                this.WindowState = WindowState.Maximized;
                this.dataGridVariables.Height = 300;
            }
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dataGridVariables_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
