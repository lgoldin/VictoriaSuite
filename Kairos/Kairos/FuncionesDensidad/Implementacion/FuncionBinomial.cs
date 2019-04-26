using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kairos.Entidades;
using Kairos.Modelo;
using Accord.Statistics.Distributions.Univariate;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;

namespace Kairos.FuncionesDensidad.Implementacion
{
    class FuncionBinomial : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string n = "";
        private readonly string p = "";
        public string StringFDP => string.Format("f(x) = ({0}|x)({1}^x)*(1-{1})^({0}-x)", n,p);

        public string StringInversa => string.Format("F(x) = Σ(i=0;{0}) ({1}^i)*(1-{1})^({0}-i)",n,p);

        public FuncionBinomial(double[] eventos) : base(eventos)
        {
            try
            {
                DistribucionDiscreta = new BinomialDistribution();
                DistribucionDiscreta.Fit(eventos);
                n = ((BinomialDistribution)DistribucionDiscreta).NumberOfTrials.ToString("0.0000");
                p = ((BinomialDistribution)DistribucionDiscreta).ProbabilityOfSuccess.ToString("0.0000");
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionDiscreta.StandardDeviation, DistribucionDiscreta.Mean, DistribucionDiscreta.Variance, this);
            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
