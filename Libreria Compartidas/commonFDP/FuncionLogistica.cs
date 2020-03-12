using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Analysis;
using Accord.Statistics.Distributions.Univariate;

namespace commonFDP
{
    class FuncionLogistica : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string MU = "";
        private readonly string S = "";

        public string StringFDP => string.Format("f(x) = (e^(({0}-x)/{1}))/({1}*(1+e^(({0}-x)/{1}))^2)", MU, S);

        public string StringInversa => string.Format("f(R) = {0}-{1}*ln(1/R-1)", MU, S);

        public FuncionLogistica(double[] eventos) : base(eventos)
        {
            try
            {
                double media = eventos.Average();
                int n = eventos.Count();
                double sigma = eventos.Sum(x => Math.Pow(x - media, 2)) / n;
                DistribucionContinua = new LogisticDistribution(media, sigma);
                this.MU = ((LogisticDistribution)DistribucionContinua).Location.ToString("0.0000");
                this.S = ((LogisticDistribution)DistribucionContinua).Scale.ToString("0.0000");
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionContinua.StandardDeviation, DistribucionContinua.Mean, DistribucionContinua.Variance, this);

            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
