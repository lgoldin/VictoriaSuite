using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using Victoria.DesktopApp.View;
using Victoria.DesktopApp;
using Victoria.ViewModelWPF;
using Victoria.ModelWPF;
using System.Reflection;
using Victoria.Shared.AnalisisPrevio;

namespace Victoria.DesktopApp
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields
        private bool isMaximized = true;

        //private ILogger logger = new Logger(typeof(MainWindow));

        #endregion

        #region Properties
        public bool IsMaximized
        {
            get { return isMaximized; }
            set
            {
                isMaximized = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMaximized"));
            }
        }

        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string simulation, bool isPlainXml)
        {
            InitializeComponent();
            ((MainViewModel)this.DataContext).MostrarMensajeCerrarSimulacion = this.MostrarCloseSimulationDialog;
            if (isPlainXml)
            {
                ((MainViewModel)this.DataContext).OpenXmlPlainSimulationCommand.Execute(simulation);
            }
            else
            {
                ((MainViewModel)this.DataContext).OpenSimulationCommand.Execute(simulation);
            }

            this.Loaded += MainWindow_Loaded; 
        }

        #endregion

        #region Events

        private void mainBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {

            }

        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            CloseRoutine();
        }

        public UI.SharedWPF.DialogResult MostrarCloseSimulationDialog()
        {
            var closeSimulationDialog = new CloseSimulationDialog();
            closeSimulationDialog.ShowDialog();
            return closeSimulationDialog.Result;

        }

        private void BtnMaximizeRestore_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsMaximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }

            IsMaximized = !IsMaximized;

        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnOpenSimulation_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((MainViewModel)this.DataContext).IsSimulationOpen)
                {
                    var closeSimulationDialog = new CloseSimulationDialog();
                    closeSimulationDialog.ShowDialog();

                    switch (closeSimulationDialog.Result)
                    {

                        case UI.SharedWPF.DialogResult.SaveAndClose:
                            {
                                ((MainViewModel)this.DataContext).SaveSimulationCommand.Execute(null);
                            }
                            break;
                        case UI.SharedWPF.DialogResult.Cancel:
                            {
                                return;
                            }
                    }

                    ((MainViewModel) this.DataContext).DeleteAllStages();
                }

                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Vic files (*.vic)|*.vic";
                    openFileDialog.Title = "Abrir Simulacion";
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //validar que el xml sea valido sino error.
                        ((MainViewModel)this.DataContext).OpenSimulationCommand.Execute(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al abrir la simulación. Para obtener más detalles despligue el control.");
                viewException.ShowDialog();
            }

        }

        private void btnAddStage_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var addStagePopUp = new AddStageWindow();
                addStagePopUp.ShowDialog();
                switch (addStagePopUp.Result)
                {
                    case UI.SharedWPF.DialogResult.Accept:
                        {
                            ((MainViewModel)this.DataContext).AddStageCommand.Execute(addStagePopUp.StageName);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al agregar un escenario. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void btnSaveSimulation_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                ((MainViewModel)this.DataContext).SaveSimulationCommand.Execute(null);
                //this.PopupGuardar.IsOpen = false;
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al guardar la simulación. Para ver datalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }
        private void btnSaveAsSimulation_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Vic files (*.vic)|*.vic";
                    saveFileDialog.Title = "Guardar Simulacion";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        ((MainViewModel)this.DataContext).SaveSimulationCommand.Execute(saveFileDialog.FileName);
                }
                //this.PopupGuardar.IsOpen = false;
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al guardar la simulación. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }
        
        private void btnExportarSimulacion_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Exportar Simulacion";
                    saveFileDialog.InitialDirectory = Environment.CurrentDirectory;

                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        ((MainViewModel)this.DataContext).ExportSimulationCommand.Execute(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp( "Se produjo un error al exportar la simulación. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void BtnExecuteSimulationCommand_OnClick(object sender, RoutedEventArgs e)
        {
            this.executeSimulation(false);
        }

        public void stopDebug()
        {
            try
            {
                ((MainViewModel)this.DataContext).StopDebugCommand.Execute(null);
            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al parar la simulacion");
                viewException.ShowDialog();
            }
        }

        public void executeSimulation(bool debugginMode)
        {
            try
            {
                if (debugginMode)
                {
                    ((MainViewModel)this.DataContext).DebugSimulationCommand.Execute(null);
                }
                else
                {
                    ((MainViewModel)this.DataContext).ExecuteSimulationCommand.Execute(null);
                }

            }
            catch (Exception ex)
            {
                var viewException = new AlertPopUp("Se produjo un error al ejecutar la simulación. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void BtnHelp_OnClick(object sender, RoutedEventArgs e)
        {
            var parentFolder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var sourcePath = System.IO.Path.Combine(parentFolder, @"Manual de usuario\Manual de usuario Victoria.pdf");

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.FileName = "Manual de usuario Victoria.pdf";
            saveFileDialog.Filter = "Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.Copy(sourcePath, saveFileDialog.FileName, true);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnGuardar_OnChecked(object sender, RoutedEventArgs e)
        {
            //PopupGuardar.IsOpen = true;
            //PopupGuardar.Closed += (senderClosed, eClosed) =>
            {
                //btnGuardar.IsChecked = false;
            };
        }

        private void BtnGuardar_OnUnchecked(object sender, RoutedEventArgs e)
        {
            //PopupGuardar.IsOpen = false;
        }

        private void btnBack_OnClick(object sender, RoutedEventArgs e)
        {
            CloseRoutine();
        }

        private void CloseRoutine()
        {
            var closeDialog = new CloseDialog(((MainViewModel)this.DataContext).IsSimulationOpen);
            closeDialog.ShowDialog();

            switch (closeDialog.Result)
            {
                case UI.SharedWPF.DialogResult.CloseWithOutSave:
                    {
                        this.Close();
                    }
                    break;
                case UI.SharedWPF.DialogResult.SaveAndClose:
                    {
                        //CHEQUEAR SI ES SAVE, O SAVEAS !!
                        ((MainViewModel)this.DataContext).SaveSimulationCommand.Execute(null);

                        this.Close();
                    }
                    break;
                case UI.SharedWPF.DialogResult.Cancel:
                    {
                        return;
                    }
            }
            this.Close();
        }

        private void BtnAnalisisSensibilidad_OnClick(object sender, RoutedEventArgs e)
        {

            AnalisisSensibilidadPopUp sensibilidadWindow = new AnalisisSensibilidadPopUp(((MainViewModel)this.DataContext).SimulationFile);
            sensibilidadWindow.ShowDialog();
           
            btnAnalisisSensibilidad.Focusable = false;
        }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            btnAnalisisSensibilidad.IsEnabled = ((MainViewModel)this.DataContext).Stages.Count(x => x.Simulation.GetVariables().Count(y => y.Type == VariableType.Control) > 0) > 0;                
        }

        public ObservableCollection<Victoria.ModelWPF.Variable> getSimulationVariables() {

            ObservableCollection<Victoria.ModelWPF.Variable> variablesList = new ObservableCollection<Victoria.ModelWPF.Variable>();
            foreach (StageViewModel stageViewModel in ((MainViewModel)this.DataContext).Stages){
                variablesList = stageViewModel.getVariables;
            }
            
            return variablesList;
        }

      

    }
}
