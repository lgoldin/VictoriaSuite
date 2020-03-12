using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP.Functions
{
    class Exponential : FDP, IFunctions
    {
        private readonly string L    = "";

        public string FDP => string.Format("f(x) = {0}*e^(-{0}*x)", L);
        public string Inverse => string.Format("f(R) = ln(-R+1)/(-{0})", L);

        public Exponential(double[] eventos)
        {
            try
            {
                this.continuousDistribution = new ExponentialDistribution();
                this.continuousDistribution.Fit(eventos);
                this.L = ((ExponentialDistribution)continuousDistribution).Rate.ToString("0.0000");
                this.result = this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
