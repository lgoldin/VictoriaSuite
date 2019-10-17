using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP.Functions
{
    class Uniform : FDP, IFunctions
    {
        private readonly string A = "";
        private readonly string B = "";

        public string FDP => string.Format("f(x) = 1/({0}-{1})", B, A);
        public string Inverse => string.Format("f(R) = R*({0}-{1})+{1}", B, A);

        public Uniform(double[] events)
        {
            try
            {
                this.continuousDistribution = new UniformContinuousDistribution();
                this.continuousDistribution.Fit(events);
                this.A = ((UniformContinuousDistribution)continuousDistribution).Minimum.ToString("0.0000");
                this.B = ((UniformContinuousDistribution)continuousDistribution).Maximum.ToString("0.0000");
                this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
