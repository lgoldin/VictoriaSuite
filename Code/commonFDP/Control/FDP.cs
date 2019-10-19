using Accord.Statistics.Distributions.Univariate;
using commonFDP;
using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP
{
    public abstract class FDP
    {
        public UnivariateDiscreteDistribution discreteDistribution{ get; protected set; }

        public UnivariateContinuousDistribution continuousDistribution { get; protected set; }

        public DistributionResult result = null;
    }
}
