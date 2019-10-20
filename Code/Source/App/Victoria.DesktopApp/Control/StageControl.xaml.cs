using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Victoria.DesktopApp.View;
using Victoria.ModelWPF;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.Control
{
    /// <summary>
    /// Interaction logic for StageControl.xaml
    /// </summary>
    public partial class StageControl : UserControl
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        public StageControl()
        {
            InitializeComponent();
        }

        private void BtnAddChart_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //logger.Info("Inicio Agregar gráfico");
                List<object> parameters = new List<object>();

                var addChartPopUp = new AddChartPopUp();
                foreach (var variable in ((StageViewModel)this.DataContext).Variables)
                {
                    variable.IsSelected = false;
                }
                addChartPopUp.DataContext = this.DataContext;
                addChartPopUp.ShowDialog();
                switch (addChartPopUp.Result)
                {
                    case UI.SharedWPF.DialogResult.Accept:
                        {
                            parameters.Add(addChartPopUp.ChartName);
                            parameters.Add(((StageViewModel)this.DataContext).Variables.Where(v => v.IsSelected).ToList());
                            ((StageViewModel)this.DataContext).AddChartCommand.Execute(parameters);
                        }
                        break;
                }
                //logger.Info("Fin Agregar gráfico");
            }
            catch
            {
                //logger.Error("Se produjo un error al Agregar un gráfico.");
                var viewException = new AlertPopUp("Se produjo un error al agregar un gráfico. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void btnExportStage_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //logger.Info("Inicio Exportar Escenario");
                using (var saveFileDialog = new System.Windows.Forms.SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|PDF files (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Exportar Simulacion";
                    saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        ((StageViewModel)this.DataContext).ExportStageCommand.Execute(saveFileDialog.FileName);
                }

                logger.Info("Se ha realizado la exportación del escenario.");
                //logger.Info("Fin Exportar Escenario");
            }
            catch (Exception ex)
            {
                //logger.Error("Se produjo un error al exportar el escenario");
                var viewException = new AlertPopUp("Se produjo un error al exportar el escenario. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void BtnDeleteChart_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //logger.Info("Inicio Eliminar Gráfico");
                ((StageViewModel)this.DataContext).DeleteChartCommand.Execute(null);
                //logger.Info("Fin Eliminar Grafico");
            }
            catch
            {
                //logger.Error("Se produjo un error al eliminar un gráfico");
                var viewException = new AlertPopUp("Se produjo un error al eliminar un gráfico. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void BtnStopExecutionStageCommand_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //logger.Info("Inicio ejecutar Escenario");
                ((StageViewModel)this.DataContext).StopExecutionStageCommand.Execute(null);
                //logger.Info("Fin Ejecutar Escenario");
            }
            catch
            {

                logger.Error("Se produjo un error al detener la ejecución del escenario");
                var viewException = new AlertPopUp( "Se produjo un error al ejecutar el escenario. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void BtnAddAnimation_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //logger.Info("Inicio Agregar Animación");
                if (!((StageViewModel)this.DataContext).DllConfigurations.Any())
                {
                    //logger.Error("No se encontraron animaciones disponbles. Revise la configuración de Victoria");
                    var viewException = new AlertPopUp("No se encontraron animaciones disponibles. Revise la configuración de Victoria.");
                    viewException.ShowDialog();
                    return;
                }

                var dllConfigurationsClone = new List<AnimationConfigurationBase>();
                foreach(var config in ((StageViewModel)this.DataContext).DllConfigurations)
                {
                    var newDllConfig = Activator.CreateInstance(config.GetType(), config.Variables) as AnimationConfigurationBase;
                    dllConfigurationsClone.Add(newDllConfig);
                }

               var addAnimationPopUp = new AddAnimationPopUp(((StageViewModel)this.DataContext).Variables, dllConfigurationsClone);

                addAnimationPopUp.ShowDialog();
                switch (addAnimationPopUp.Result)
                {
                    case UI.SharedWPF.DialogResult.Accept:
                        {
                            List<object> parameters = new List<object>();

                            parameters.Add(addAnimationPopUp.ResultConfig);
                            parameters.Add(((StageViewModel)this.DataContext).DllAnimations);

                            ((StageViewModel)this.DataContext).AddAnimationToCanvasCommand.Execute(parameters);
                        }
                        break;
                }
                //logger.Info("Fin Agregar Animación");

                logger.Info("Se ha incorporado una nueva animación.");
            }
            catch
            {
                //logger.Error("Se produjo un error al agregar una animación");
                var viewException = new AlertPopUp( "Se produjo un error al agregar una animación. Para ver detalles, despliegue el control correspondiente.");
                viewException.ShowDialog();
            }
        }

        private void BtnExecute_Animations(object sender, RoutedEventArgs e)
        {
            //logger.Info("Inicio Ejecutar Animaciones");
            var hasVectorVariable = (((StageViewModel)this.DataContext).Variables.Any(v => v.Name.Contains("(")));
            if ((((StageViewModel)this.DataContext).Variables.First(v => v.Name == "T").Values.Any() && 
                ((StageViewModel)this.DataContext).Animations.Any()) || (hasVectorVariable && ((StageViewModel)this.DataContext).Animations.Any()))
            {
                try
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        var animationsToExecute = this.CreateAnimationsClones(((StageViewModel)this.DataContext).Animations);
                        var animationWindow = new AnimationExecutionWindow(animationsToExecute);

                        animationWindow.Show();
                    }));

                }
                catch
                {
                    //logger.Error("Se produjo un error al intentar ejecutar las animaciones. Por favor revisa la configuracion de las mismas.");
                    var viewException = new AlertPopUp("Se produjo un error al intentar ejecutar las animaciones. Por favor revisa la configuración de las mismas.");
                    viewException.ShowDialog();
                }
            }else
            {
                //logger.Error("No se puede abrir la ventana de animaciones si no se ha ejecutado la simulación o no se han creado animaciones.");
                var viewException = new AlertPopUp("No se puede abrir la ventana de animaciones si no se ha ejecutado la simulación o no se han creado animaciones.");
                viewException.ShowDialog();
            }
            //logger.Info("Fin Ejecutar Animaciones");
        }

        private ObservableCollection<AnimationViewModel> CreateAnimationsClones(ObservableCollection<AnimationViewModel> animations)
        {
            var animationsToExecute = new ObservableCollection<AnimationViewModel>();
            foreach(var animation in animations)
            {
                var newAnimation = Activator.CreateInstance(animation.GetType()) as AnimationViewModel;
                newAnimation.InitializeAnimation(animation.AnimationConfig);
                newAnimation.BindSimulationVariableValues();
                newAnimation.X = animation.X;
                newAnimation.Y = animation.Y;
                newAnimation.AnimationOrientation = animation.AnimationOrientation;
                animationsToExecute.Add(newAnimation);
            }

            return animationsToExecute;
        }        

        private void BtnInformation_Animations(object sender, RoutedEventArgs e)
        {
            //logger.Info("Información Animaciones");
            var animationInformationPopUp = new AnimationInformationPopup();
            animationInformationPopUp.Show();
        }
    }
}
