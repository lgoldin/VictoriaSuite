using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;

namespace commonFDP
{
    class FuncionPoisson : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string lambda = "";
        public string StringFDP => string.Format("f(x) = (({0}^x)*(e^(-{0})))/x!", lambda);

        public string StringInversa => string.Format("F(x) = (e^(-{0}))*Σ(i=0;x) ({0}^i)/i!", lambda);

        public FuncionPoisson(double[] eventos) : base(eventos)
        {
            try
            {
                DistribucionDiscreta = new PoissonDistribution();
                DistribucionDiscreta.Fit(eventos);
                lambda = ((PoissonDistribution)DistribucionDiscreta).Lambda.ToString("0.0000");
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionDiscreta.StandardDeviation, DistribucionDiscreta.Mean, DistribucionDiscreta.Variance, this);
            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
