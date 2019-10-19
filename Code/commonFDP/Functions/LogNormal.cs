using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP.Functions
{
    class LogNormal : FDP, IFunctions
    {
        private readonly string media = "";
        private readonly string sigma = "";

        public string FDP => string.Format("f(x) = (e^((-1/2)*((ln x-{0})/{1})^2))/(x*{1}*(6,2838)^(1/2))", media, sigma);
        public string Inverse => string.Format("f(R) = e^({0}+{1}*((2^(1/2))*erf^(-1)(2R-1)))", media, sigma);

        public LogNormal(double[] eventos)
        {
            try
            {
                this.continuousDistribution = new LognormalDistribution();
                this.continuousDistribution.Fit(eventos);
                media = ((LognormalDistribution)this.continuousDistribution).Mean.ToString("0.0000");
                sigma = ((LognormalDistribution)this.continuousDistribution).StandardDeviation.ToString("0.0000");
                this.result = this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
