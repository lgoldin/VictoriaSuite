using System;
using System.Collections.Generic;
using System.Text;
using Accord.Statistics;
using Accord.Statistics.Distributions.Univariate;

namespace commonFDP
{
    class WEIBULL05 : FDP, IFunctions
    {
        private readonly double shape = 0.5;
        private readonly double scale = 1;

        public string FDP => string.Format("f(x) = ({0}/{1})*(x/{1})^({0}-1)*e^(-(x/{1})^{0})", shape, scale);

        public string Inverse => string.Format("f(R) = {1}*(-ln(-R+1))^(1/{0})", shape, scale);

        public WEIBULL05(double[] events)
        {
            try
            {
                this.continuousDistribution = new WeibullDistribution(shape, scale);
                this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
