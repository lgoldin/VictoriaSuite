using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Optimization;
using Accord.Statistics.Distributions.Univariate;

namespace commonFDP
{
    class FuncionLogLogistica : FuncionDensidadProbabilidad, IFuncionRepresentable
    {
        private readonly string A = "";
        private readonly string B = "";

        public string StringFDP => string.Format("f(x) = ({0}/{1})*((x/{1})^({0}-1))*((1+(x/{1})^{0})^(-2))", A, B);

        public string StringInversa => string.Format("f(R) = {1}/((1/R-1)^(1/{0}))+{0}", A, B);

        public FuncionLogLogistica(double[] eventos) : base(eventos)
        {
            try
            {
                double[] eventosOrdenados = eventos.OrderBy(x => x).ToArray();
                double alfa = eventos.Count() % 2 == 0 ? (eventosOrdenados.ElementAt(eventos.Count() / 2) + eventosOrdenados.ElementAt((eventos.Count() / 2) + 1)) / 2 : eventos.OrderBy(x => x).ElementAt((eventos.Count() / 2) + 1);
                this.A = alfa.ToString("0.0000");
                double media = eventos.Average();
                int n = eventos.Count();
                double sigma = eventos.Sum(x => Math.Pow(x - media, 2)) / n;
                double k = Math.Sqrt(Math.Pow(sigma, 2) / (Math.Pow(sigma, 2) + Math.Pow(media, 2)));
                Func<double, double> function = x => Math.Sqrt(1 - (x / Math.Tan(x))) - k;
                BrentSearch search = new BrentSearch(function, (Math.PI / 2) * k, Math.Sqrt(3) * k);
                search.FindRoot();
                double beta = Math.PI / search.Solution;
                this.B = beta.ToString("0.0000");
                DistribucionContinua = new LogLogisticDistribution(alfa, beta);
                Resultado = new ResultadoAjuste(StringFDP, StringInversa, DistribucionContinua.StandardDeviation, DistribucionContinua.Mean, DistribucionContinua.Variance, this);
            }
            catch (Exception)
            {
                Resultado = null;
            }
        }
    }
}
