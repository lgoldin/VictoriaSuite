using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP.Functions
{
    class Poisson : FDP, IFunctions
    {
        private readonly string lambda = "";

        public string FDP => string.Format("f(x) = (({0}^x)*(e^(-{0})))/x!", lambda);
        public string Inverse => string.Format("F(x) = (e^(-{0}))*Σ(i=0;x) ({0}^i)/i!", lambda);

        public Poisson(double[] events)
        {
            try
            {
                this.discreteDistribution = new PoissonDistribution();
                this.discreteDistribution.Fit(events);
                this.lambda = ((PoissonDistribution)this.discreteDistribution).Lambda.ToString("0.0000");
                this.result = this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
