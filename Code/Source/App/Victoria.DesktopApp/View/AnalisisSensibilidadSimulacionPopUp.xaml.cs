using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Victoria.Shared.EventArgs;
using Victoria.UI.SharedWPF;
using Victoria.ViewModelWPF;
using iTextSharp;
using iTextSharp.text.pdf;
using System.Data;
using System.IO;
using iTextSharp.text;
using Victoria.Shared;
using Victoria.Shared.AnalisisPrevio;
using System.Collections.ObjectModel;
using Victoria.DesktopApp.Helpers;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for CloseDialog.xaml
    /// </summary>
    public partial class AnalisisSensibilidadSimulacionPopUp : Window
    {
        private IList<StageViewModelBase> stages;

        private AnalisisSensibilidadPopUp analisisSensibilidadPopUp;
        private List<string> vicPaths;

        public DialogResult Result { get; set; }
        public bool IsSimulationOpen { get; set; }
        private string simulationPath { get; set; }
        private int StagesFinished { get; set; }
        private bool simulationStoped { get; set; }
        private DateTime simulationStartedTime { get; set; }
        private TimeSpan simulationTotalTime { get; set; }
        private double barraProgresoValue { get; set; }
        private bool conTiempoFinal { get; set; }
        
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private UpdateProgressBarDelegate updatePbDelegate { get; set; }
        
        public AnalisisSensibilidadSimulacionPopUp(string simulationPath, List<string> vicPaths, AnalisisSensibilidadPopUp analisisSensibilidadPopUp)
        {
            InitializeComponent();
            this.simulationPath = simulationPath;
            this.simulationStoped = false;
            this.vicPaths = vicPaths;
            this.analisisSensibilidadPopUp = analisisSensibilidadPopUp;
            this.StagesFinished = 0;
            double totalValue = 0;
            this.stages = new ObservableCollection<StageViewModelBase>();
            this.simulationStartedTime = DateTime.Now;
            this.conTiempoFinal = false;

            foreach (string vicPath in vicPaths) {
                string simulationFile = File.ReadAllText(vicPath);
                var simulation = XMLParser.GetSimulation(simulationFile);
                if (simulation.Stages.Any())
                {
                    StageViewModel stage = new StageViewModel(simulation) { Name = simulation.Stages.First().Name };
                    this.stages.Add(stage);
                    Variable tiempoFinal = stage.Simulation.GetVariables().First(v => v.Name == "TF");
                    if (tiempoFinal != null && tiempoFinal.InitialValue > 0)
                    {
                        this.conTiempoFinal = true;
                        totalValue = totalValue + stage.Simulation.GetVariables().First(v => v.Name == "TF").InitialValue;
                        stage.Variables.First(v => v.Name == "T").PropertyChanged += VariableTChanged;
                    }
                    stage.Simulation.SimulationStatusChanged += OnSimulationStatusChanged;
                    //stage.ExecuteStageCommand.Execute(null);
                }
            }
            if (this.conTiempoFinal)
            {
                barraProgreso.Minimum = 0;
                barraProgreso.Maximum = totalValue;
                barraProgreso.Value = 0;
                this.barraProgresoValue = 0;
                this.updatePbDelegate = new UpdateProgressBarDelegate(barraProgreso.SetValue);
            }
            else
            {
                barraProgreso.IsIndeterminate = true;
            }
            // Despues que quedó configurada la barra y eso, mando a ejecutar
            foreach (StageViewModel stage in this.stages)
            {
                stage.ExecuteStageCommand.Execute(null);
            }
            
        }

        private void VariableTChanged(object sender, PropertyChangedEventArgs e)
        {
            double value = 0;
            foreach (StageViewModel stage in this.stages)
            {
                value = value + stage.Variables.First(v => v.Name == "T").ActualValue;
            }
            if (value > 0 && value > this.barraProgresoValue)
            {
                this.barraProgresoValue = value;
                Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { ProgressBar.ValueProperty, value });
            }
        }

        private void OnSimulationStatusChanged(object sender, SimulationStatusChangedEventArgs simulationStatusChangedEventArgs)
        {
            switch (simulationStatusChangedEventArgs.Status)
            {
                case SimulationStatus.Stoped:
                    this.StagesFinished += 1;
                    if (this.StagesFinished == this.stages.Count)
                    {
                        this.simulationStoped = true;
                        DateTime now = DateTime.Now;
                        this.simulationTotalTime = now.Subtract(this.simulationStartedTime);

                        //Genera lista con las diferentes exportaciones.
                        List<IResults> lstExportarResultados = new List<IResults> {
                                                                    new ExcelResults(this.simulationPath, "resultados.xlsx", this.stages, this.simulationTotalTime),
                                                                    new PDFResults(this.simulationPath, "resultados.pdf", this.stages, this.simulationTotalTime)
                                                                };

                        //Se exportan los distintos formatos incluidos en la lista
                        lstExportarResultados.ForEach(r => r.Print());                        
                        
                        // ABRO LA VENTANA DEL ARCHIVO QUE CONTIENE LOS RESULTADOS
                        System.Diagnostics.Process.Start("explorer.exe", "/select," + simulationPath);
                        // CIERRO LOS POPUPS ABIERTOS
                        analisisSensibilidadPopUp.Dispatcher.Invoke(
                          System.Windows.Threading.DispatcherPriority.Normal,
                          new Action(
                            delegate()
                            {
                                analisisSensibilidadPopUp.Close();
                            }
                        ));
                        this.Dispatcher.Invoke(
                          System.Windows.Threading.DispatcherPriority.Normal,
                          new Action(
                            delegate()
                            {
                                this.Close();
                            }
                        ));
                    }
                    break;
                case SimulationStatus.Started:
                    // asd
                    break;
            }
        }        

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            foreach (StageViewModel stage in this.stages)
            {
                this.simulationStoped = true;
                stage.StopExecutionStageCommand.Execute(null);
            }
            this.Close();
        }
    }

}
