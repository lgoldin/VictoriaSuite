using System.Windows;

using Victoria.UI.SharedWPF;
using DiagramDesigner;
using System.ComponentModel;
using System.Windows.Input;
using Victoria.Shared.AnalisisPrevio;
using System.Windows.Controls;
using System.Collections.Generic;
using Victoria.DesktopApp.View;

namespace DiagramDesigner
{
    public partial class Window1 : Window
    {
        // Inicializa los componentes en la venta Diagrama, si creo uno nuevo debo ponerlo aca
        public Window1()
        {
            InitializeComponent();
            Closing += HideWindow;
            this.diagrama().dataGridVariables = this.dataGridVariables;
            this.diagrama().dimensiones = this.dimensiones;
            this.diagrama().dataGridVariablesSimulation = this.dataGridVariablesSimulation;            
            this.diagrama().groupBoxVariablesSimulation = this.groupBoxVariablesSimulation; //Solo inicializo este porque necesito el Setter            
            this.diagrama().debugButtonList = this.getListButtonDebug();
        }

        public DialogResult Result { get; set; }

        public string StageName { get; set; }


        public DesignerCanvas diagrama() 
        {
            return this.MyDesigner;
        } 
        /*
        public ToolBar toolBar()
        {
            return this.MyToolBar;
        }
        */
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
            } 
            else 
            { 
                this.WindowState = WindowState.Maximized;
            }

            this.resizeDatagrids(this.WindowState, this.dataGridVariables.Visibility);
        }

        private void resizeDatagrids(WindowState windowState , Visibility debugGridVisibility)
        {
            if (debugGridVisibility != Visibility.Visible)
            {
                this.dataGridVariables.Height = windowState == WindowState.Maximized ? 300 : 180;
            }
            else
            {
                this.dataGridVariables.Height = windowState == WindowState.Maximized ? 230 : 180;
                this.dataGridVariablesSimulation.Height = windowState == WindowState.Maximized ? 230 : 180;
            }
        }

        private void CloseRoutine()
        {
            //var closeDialog = new CloseDialog(((MainViewModel)this.DataContext).IsSimulationOpen);
            var closeDialog = new CloseDialog(true);
            closeDialog.ShowDialog();

            switch (closeDialog.Result)
            {
                case Victoria.UI.SharedWPF.DialogResult.CloseWithOutSave:
                    {
                        this.MyDesigner.setDebugButtonsVisibility(Visibility.Hidden);
                        this.Close();
                    }
                    break;
                case Victoria.UI.SharedWPF.DialogResult.Cancel:
                    {
                        return;
                    }
            }
            this.Close();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.CloseRoutine();
            //this.MyDesigner.setDebugButtonsVisibility(Visibility.Hidden);
            //this.Close();
        }

        private void dataGridVariables_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private List<Button> getListButtonDebug()
        {
            List<Button> debugButtonList = new List<Button>();
            ToolBar tb = (ToolBar)this.MyToolBar.Content;
            foreach (Grid g in tb.ItemContainerGenerator.Items)
            {
                if (g.Name.StartsWith("Debug"))
                {
                    foreach (Button b in g.Children)
                    {
                        debugButtonList.Add(b);
                    }
                }
            }
            return debugButtonList;
        }
    }
}
