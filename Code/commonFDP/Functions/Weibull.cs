using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP.Functions
{
   class Weibull : FDP, IFunctions
    {
        private double shape = 0;
        private double scale = 0;

        public string FDP => string.Format("f(x) = ({0}/{1})*(x/{1})^({0}-1)*e^(-(x/{1})^{0})", shape, scale);

        public string Inverse => string.Format("f(R) = {1}*(-ln(-R+1))^(1/{0})", shape, scale);

        public Weibull(double[] events,double shape, double scale)
        {
            try
            {
                this.shape = shape;
                this.scale = scale;
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
