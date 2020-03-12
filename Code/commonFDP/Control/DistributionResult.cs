using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP
{
    public class DistributionResult
    {
        public string Function { get; }
        public string Inverse { get; }
        public double StandarDesviation { get; }
        public double Entropy { get; }
        public double Median { get; }
        public double Varience { get; }
        public FDP FDP { get; }

        public DistributionResult(
            string function, 
            string inverse, 
            double standarDesviation, 
            double median, 
            double varience, 
            FDP fdp)
        {
            this.Function = function;
            this.Inverse = inverse;
            this.StandarDesviation = standarDesviation;
            this.Median = median;
            this.Varience = varience;
            this.FDP = fdp;
        }
    }
}
