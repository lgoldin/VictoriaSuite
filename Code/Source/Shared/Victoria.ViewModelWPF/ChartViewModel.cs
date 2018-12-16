using System.Collections.Generic;
using System.IO;
using OxyPlot.Wpf;
using Victoria.Shared;
using OxyPlot;
using OxyPlot.Axes;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using LineSeries = OxyPlot.Series.LineSeries;
using Akka.Configuration.Hocon;
using Akka.Actor;
using System.Configuration;
using Victoria.ViewModelWPF.Actors;

namespace Victoria.ViewModelWPF
{
    public class ChartViewModel : ChartViewModelBase
    {
        private IActorRef actor;

        public PlotModel PlotModel { get; private set; }

        public bool HasNewData { get; set; }

        public LinearAxis HorizontalAxis { get; set; }

        public LinearAxis VerticalAxis { get; set; }
        
        public IActorRef Actor
        {
            get
            {
                if (this.actor == null)
                {
                    var akkaConfiguration = ((AkkaConfigurationSection)ConfigurationManager.GetSection("akka")).AkkaConfig;
                    var system = ActorSystem.Create("MySystem", akkaConfiguration);
                    this.actor = system.ActorOf(ChartActor.Props(this), "chartActor");
                }

                return this.actor;
            }

            set
            {
                this.actor = value;
            }
        }

        public ChartViewModel(string name, Variable independentVariable, List<Variable> dependentVariables) : base(name, independentVariable, dependentVariables)
        {
            this.PlotModel = new PlotModel();
            this.PlotModel.Title = name;
            this.VerticalAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 100,
                Title = "Unidades"
            };
            this.PlotModel.Axes.Add(this.VerticalAxis);
            this.HorizontalAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 100,
                Title = "Tiempo"
            };

            this.PlotModel.Axes.Add(this.HorizontalAxis);
            this.PlotModel.LegendTitle = "Referencias";
            this.PlotModel.LegendOrientation = LegendOrientation.Vertical;
            this.PlotModel.LegendPlacement = LegendPlacement.Outside;
            this.PlotModel.LegendPosition = LegendPosition.RightMiddle;
            this.PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            this.PlotModel.LegendBorder = OxyColors.Black;

            foreach (var dependentVariable in dependentVariables)
            {
                this.PlotModel.Series.Add(new LineSeries { LineStyle = LineStyle.Solid, Title = dependentVariable.Name });
            }
            this.IsVisible = true;
            this.RaisePropertyChanged("PlotModel");
        }

        public void ExportChart(Stream stream, int width, int height)
        {
            PngExporter.Export(this.PlotModel, stream, width, height, OxyColor.FromRgb(255, 255, 255));
        }

        public void Update()
        {
            this.Actor.Tell("Update");
        }

        public override void Reset()
        {
            this.Actor.Tell("Reset");
        }

        protected override void IndependentVariableValueChanged(object sender, Shared.EventArgs.VariableValueChangeEventArgs e)
        {
            this.Actor.Tell("AddPoint");
        }
    }
}
