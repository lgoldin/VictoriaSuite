using System.Linq;
using Akka.Actor;
using OxyPlot;
using OxyPlot.Series;

namespace Victoria.ViewModelWPF.Actors
{
    public class ChartActor : ReceiveActor
    {
        private readonly ChartViewModel chart;

        public ChartActor(ChartViewModel chart)
        {
            this.chart = chart;

            Receive<string>(message =>
            {
                switch (message)
                {
                    default:
                    case "AddPoint":
                        this.AddPoint();
                        break;
                    case "Update":
                        this.Update();
                        break;
                    case "Reset":
                        this.Reset();
                        break;
                }
            });
        }

        public static Props Props(ChartViewModel chart)
        {
            return Akka.Actor.Props.Create(() => new ChartActor(chart));
        }

        private void AddPoint()
        {
            lock (this.chart.PlotModel.SyncRoot)
            {
                if (!this.chart.PlotModel.Series.Any(s => ((DataPointSeries)s).Points.Any()))
                {
                    this.chart.PlotModel.Axes[1].Minimum = this.chart.IndependentVariable.ActualValue;
                }

                if (this.chart.PlotModel.Axes[1].Minimum > this.chart.IndependentVariable.ActualValue)
                {
                    this.chart.PlotModel.Axes[1].Minimum = this.chart.IndependentVariable.ActualValue;
                }
                if (this.chart.PlotModel.Axes[1].Maximum < this.chart.IndependentVariable.ActualValue)
                {
                    this.chart.PlotModel.Axes[1].Maximum = this.chart.IndependentVariable.ActualValue;
                }

                for (int i = 0; i < this.chart.DependentVariables.Count; i++)
                {
                    var s = (LineSeries)this.chart.PlotModel.Series[i];
                    s.Points.Add(new DataPoint(this.chart.IndependentVariable.ActualValue, this.chart.DependentVariables[i].ActualValue));

                    if (this.chart.PlotModel.Axes[0].Minimum > this.chart.DependentVariables[i].ActualValue - 1)
                    {
                        this.chart.PlotModel.Axes[0].Minimum = this.chart.DependentVariables[i].ActualValue - 1;
                    }
                    if (this.chart.PlotModel.Axes[0].Maximum < this.chart.DependentVariables[i].ActualValue + 1)
                    {
                        this.chart.PlotModel.Axes[0].Maximum = this.chart.DependentVariables[i].ActualValue + 1;
                    }
                }

                this.chart.HasNewData = true;
            }
        }

        private void Update()
        {
            if (this.chart.HasNewData)
            {
                lock (this.chart.PlotModel.SyncRoot)
                {
                    this.chart.PlotModel.InvalidatePlot(true);
                    this.chart.HasNewData = false;
                }
            }
        }

        private void Reset()
        {
            foreach (DataPointSeries serie in this.chart.PlotModel.Series)
            {
                serie.Points.Clear();
            }

            this.chart.HorizontalAxis.Maximum = this.chart.HorizontalAxis.Minimum;

            this.chart.PlotModel.ResetAllAxes();
            this.chart.PlotModel.InvalidatePlot(true);
        }
    }
}
