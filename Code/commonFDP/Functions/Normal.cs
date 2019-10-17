using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP
{
    class Normal : FDP, IFunctions
    {
        private readonly string media = "";
        private readonly string sigma = "";

        public string FDP => string.Format("f(x) = (e^((-1/2)*((x-{0})/{1})^2))/({1}*(6,2838)^(1/2))", media, sigma);
        public string Inverse => string.Format("f(R) = {0}+{1}*(2^(1/2))*erf^(-1)(2*R-1)", media, sigma);

        public Normal(double[] eventos)
        {
            try
            {
                this.continuousDistribution = new NormalDistribution();
                this.continuousDistribution.Fit(eventos);
                media = ((NormalDistribution)this.continuousDistribution).Mean.ToString("0.0000");
                sigma = ((NormalDistribution)this.continuousDistribution).StandardDeviation.ToString("0.0000");
                this.result = this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this); 
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
