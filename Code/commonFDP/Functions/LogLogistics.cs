using Accord.Math.Optimization;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commonFDP.Functions
{
    class LogLogistics : FDP, IFunctions
    {
        private readonly string A = "";
        private readonly string B = "";

        public string FDP => string.Format("f(x) = ({0}/{1})*((x/{1})^({0}-1))*((1+(x/{1})^{0})^(-2))", A, B);
        public string Inverse => string.Format("f(R) = {1}/((1/R-1)^(1/{0}))+{0}", A, B);

        public LogLogistics(double[] events)
        {
            try
            {
                double[] eventosOrdenados = events.OrderBy(x => x).ToArray();
                double alfa = events.Count() % 2 == 0 ? (eventosOrdenados.ElementAt(events.Count() / 2) + eventosOrdenados.ElementAt((events.Count() / 2) + 1)) / 2 : events.OrderBy(x => x).ElementAt((events.Count() / 2) + 1);
                this.A = alfa.ToString("0.0000");
                double media = events.Average();
                int n = events.Count();
                double sigma = events.Sum(x => Math.Pow(x - media, 2)) / n;
                double k = Math.Sqrt(Math.Pow(sigma, 2) / (Math.Pow(sigma, 2) + Math.Pow(media, 2)));
                Func<double, double> function = x => Math.Sqrt(1 - (x / Math.Tan(x))) - k;
                BrentSearch search = new BrentSearch(function, (Math.PI / 2) * k, Math.Sqrt(3) * k);
                search.FindRoot();
                double beta = Math.PI / search.Solution;
                this.B = beta.ToString("0.0000");
                this.continuousDistribution = new LogLogisticDistribution(alfa, beta);
                this.result = this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
