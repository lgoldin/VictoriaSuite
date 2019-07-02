using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace commonFDP.Functions
{
    class Logistics : FDP, IFunctions
    {
        private readonly string MU = "";
        private readonly string S = "";

        public string FDP => string.Format("f(x) = (e^(({0}-x)/{1}))/({1}*(1+e^(({0}-x)/{1}))^2)", MU, S);
        public string Inverse => string.Format("f(R) = {0}-{1}*ln(1/R-1)", MU, S);

        public Logistics(double[] events)
        {
            try
            {
                double media = events.Average();
                int n = events.Count();
                double sigma = events.Sum(x => Math.Pow(x - media, 2)) / n;
                this.continuousDistribution = new LogisticDistribution(media, sigma);
                this.MU = ((LogisticDistribution)this.continuousDistribution).Location.ToString("0.0000");
                this.S = ((LogisticDistribution)this.continuousDistribution).Scale.ToString("0.0000");
                this.result = this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
