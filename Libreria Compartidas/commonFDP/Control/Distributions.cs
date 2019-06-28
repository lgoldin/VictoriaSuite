using System;

namespace commonFDP
{
    public static class Distributions
    {
        public static DistributionResult WEIBULL05(double[] events){
            return new WEIBULL05(events).result;
        }

        public static DistributionResult Bionmial(double[] events){
            return new Binomial(events).result;
        }

        public static DistributionResult Normal(double[] events)
        {
            return new Normal(events).result;
        }
    }
}
