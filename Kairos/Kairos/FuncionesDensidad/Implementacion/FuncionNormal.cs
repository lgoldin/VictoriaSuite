using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;
using Kairos.Entidades;
using Kairos.Modelo;

namespace Kairos.FuncionesDensidad.Implementacion
{
    class FuncionNormal : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string media = "";
        private readonly string sigma = "";
        public string StringFDP => string.Format("f(x) = (e^((-1/2)*((x-{0})/{1})^2))/({1}*(6,2838)^(1/2))", media,sigma);
        public string StringInversa => string.Format("f(R) = {0}+{1}*(2^(1/2))*erf^(-1)(2*R-1)", media, sigma);

        public FuncionNormal(double[] eventos) : base(eventos)
        {
            try
            {
                DistribucionContinua = new NormalDistribution();
                DistribucionContinua.Fit(eventos);
                media = ((NormalDistribution)DistribucionContinua).Mean.ToString("0.0000");
                sigma = ((NormalDistribution)DistribucionContinua).StandardDeviation.ToString("0.0000");
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionContinua.StandardDeviation, DistribucionContinua.Mean, DistribucionContinua.Variance, this);
            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
