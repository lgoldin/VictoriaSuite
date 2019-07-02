using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP
{
    class Binomial : FDP, IFunctions
    {
        private readonly string n = "";
        private readonly string p = "";

        public string FDP => string.Format("f(x) = ({0}/{1})*(x/{1})^({0}-1)*e^(-(x/{1})^{0})", n, p);

        public string Inverse => string.Format("f(R) = {1}*(-ln(-R+1))^(1/{0})", n, p);

        public Binomial(double[] events)
        {
            try
            {
                this.discreteDistribution = new BinomialDistribution();
                this.discreteDistribution.Fit(events);
                n = ((BinomialDistribution)this.discreteDistribution).NumberOfTrials.ToString("0.0000");
                p = ((BinomialDistribution)this.discreteDistribution).ProbabilityOfSuccess.ToString("0.0000");
                this.result = new DistributionResult(this.FDP, this.Inverse, this.continuousDistribution.StandardDeviation, this.continuousDistribution.Mean, this.continuousDistribution.Variance, this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepcion en clase " + this.GetType().ToString() + e.Message);
            }
        }
    }
}
