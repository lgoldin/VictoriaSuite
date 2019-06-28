using commonFDP.Functions;
using System;

namespace commonFDP
{
    public static class Distributions
    {


        public static DistributionResult Binomial(double[] events){
            return new Binomial(events).result;
        }

        public static DistributionResult Normal(double[] events){
            return new Normal(events).result;
        }

        public static DistributionResult Exponential(double[] events){
            return new Exponential(events).result;
        }

        public static DistributionResult Logistics(double[] events) {
            return new Logistics(events).result;
        }

        public static DistributionResult LogLogistics(double[] events){
            return new LogLogistics(events).result;
        }

        public static DistributionResult LogNormal(double[] events){
            return new LogNormal(events).result;
        }

        public static DistributionResult Poisson(double[] events){
            return new Poisson(events).result;
        }

        public static DistributionResult Uniform(double[] events){
            return new Uniform(events).result;
        }

        /*
         * Weibull0_5  : shape = 0.5 , scale = 1
         * Weibull1_5  : shape = 1.5 , scale = 1
         * Weibull3  : shape = 3 , scale = 1
         * Weibull5  : shape = 5 , scale = 1
         */
        public static DistributionResult Weibull(double[] events,double shape, double scale){
            return new Weibull(events,shape,scale).result;
        }

    }
}
