using DiagramDesigner;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : Window
    {
        private Window1 diagramWindow;

        public string SimulationXML { get; set; } //acá pone una url relativa a la pc donde esta el .vic
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));

        public Window1 DiagramWindow 
        {
            get
            {
                if (this.diagramWindow == null)
                {
                    this.diagramWindow = new Window1();
                }

                return this.diagramWindow;
            }

            set
            {
                this.diagramWindow = value;
            }
        }

        public WelcomeView()
        {
            InitializeComponent();
        }

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

        private void btnOpenSimulation_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //logger.Info("Abrir Simulacion");
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Vic files (*.vic)|*.vic";
                    openFileDialog.Title = "Abrir Simulacion";
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SimulationXML = openFileDialog.FileName;
                        var mainWindow = new MainWindow(SimulationXML, false);
                        mainWindow.Show();
                        logger.Info(String.Format("Se abrio la simulacion alojada en el archivo ", openFileDialog.FileName));
                    }
                }
                
            }
            catch (Exception ex)
            {
                var viewExpection = new AlertPopUp("Se produjo un error al abrir la silumación. Para ver detalles, despliegue el control correspondiente.");
                viewExpection.ShowDialog();
                logger.Error("Se produjo un error al abrir la silumación: "+ex.Message);
            }
        }

        private void btnAnalisisPrevio_OnClick(object sender, RoutedEventArgs e)
        {
            //logger.Info("Inicio Analisis Previo");
            var addExercisePopUp = new AddAnalisisPrevioPopUp(DiagramWindow);
                addExercisePopUp.ShowDialog();
            //logger.Info("Fin Analisis Previo");

        }

        private void BtnMinimize_OnClick(object sender, RoutedEventArgs e)
        {
            //logger.Info("Inicio Boton Minimizar");
            this.WindowState = WindowState.Minimized;
            //logger.Info("Fin Boton Minimizar");

        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            //logger.Info("Inicio Boton Cerrar");
            this.Close();
            //logger.Info("Fin Boton Cerrar");
            logger.Info("FIN VICTORIA SUITE");

        }

        private void BtnNew_Exercise_OnClick(object sender, RoutedEventArgs e)
        {
            //logger.Info("Inicio Nuevo Ejercicio");
            DiagramWindow.diagrama().Children.Clear();
            DiagramWindow.diagrama().AbrirDiagrama();
            if (DiagramWindow.diagrama().Children.Count > 0)
            {
                DiagramWindow.Height = 650;
                DiagramWindow.ShowDialog();
            }
            //logger.Info("Fin Nuevo Ejercicio");
        }
    }
}