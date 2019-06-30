using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;

namespace commonFDP
{
    class FuncionUniforme : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string A = "";
        private readonly string B = "";

        public string StringFDP => string.Format("f(x) = 1/({0}-{1})", B, A);

        public string StringInversa => string.Format("f(R) = R*({0}-{1})+{1}", B, A);

        public FuncionUniforme(double[] eventos) : base(eventos)
        {
            try
            {
                DistribucionContinua = new UniformContinuousDistribution();
                DistribucionContinua.Fit(eventos);
                A = ((UniformContinuousDistribution)DistribucionContinua).Minimum.ToString("0.0000");
                B = ((UniformContinuousDistribution)DistribucionContinua).Maximum.ToString("0.0000");
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionContinua.StandardDeviation, DistribucionContinua.Mean, DistribucionContinua.Variance, this);
            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
