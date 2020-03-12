using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;
using Accord.Math;

namespace commonFDP
{
    class FuncionLogNormal : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string media = "";
        private readonly string sigma = "";

        public string StringFDP => string.Format("f(x) = (e^((-1/2)*((ln x-{0})/{1})^2))/(x*{1}*(6,2838)^(1/2))", media, sigma);

        public string StringInversa => string.Format("f(R) = e^({0}+{1}*((2^(1/2))*erf^(-1)(2R-1)))", media, sigma);

        public FuncionLogNormal(double[] eventos) : base(eventos)
        {
            try
            {
                DistribucionContinua = new LognormalDistribution();
                DistribucionContinua.Fit(eventos);
                media = ((LognormalDistribution)DistribucionContinua).Mean.ToString("0.0000");
                sigma = ((LognormalDistribution)DistribucionContinua).StandardDeviation.ToString("0.0000");
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionContinua.StandardDeviation, DistribucionContinua.Mean, DistribucionContinua.Variance, this);
            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
