using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace commonFDP
{
    public abstract class FuncionDensidadProbabilidad
    {
        public UnivariateDiscreteDistribution DistribucionDiscreta { get; protected set; }

        public UnivariateContinuousDistribution DistribucionContinua { get; protected set; }

        public ResultadoAjuste Resultado { get; protected set; }

        public double Desvio { get; protected set; }

        public FuncionDensidadProbabilidad(double[] eventos)
        {
        }

        public double[] GenerarValores(int cantidad, double[] eventos)
        {
            try
            {
                if (DistribucionContinua != null)
                    return DistribucionContinua.Generate(cantidad);
                else
                    return DistribucionDiscreta.Generate(cantidad).Select(x => Convert.ToDouble(x)).ToArray();
            }
            catch (Exception)
            {
                Random r = new Random();
                int min = Convert.ToInt32(eventos.Min());
                int max = Convert.ToInt32(eventos.Max());
                double[] toReturn = new double[cantidad];
                for (int i = 0; i < cantidad; i++)
                    toReturn[i] = r.Next(min, max);
                return toReturn;
            }
        }

        public Dictionary<double, double> ObtenerDensidad(Dictionary<double, double> valoresReales)
        {
            Dictionary<double, double> densidades = new Dictionary<double, double>();
            if (DistribucionContinua != null)
                foreach (var item in valoresReales)
                    densidades.Add(item.Key, DistribucionContinua.ProbabilityDensityFunction(item.Key));
            else
            {
                foreach (var item in valoresReales)
                {
                    try
                    {
                        densidades.Add(item.Key, DistribucionDiscreta.ProbabilityMassFunction(Convert.ToInt32(item.Key)));
                    }
                    catch (Exception)
                    {
                        // densidades.Add(item.Key, DistribucionDiscreta.d(DistribucionDiscreta.Generate()));    
                    }
                }

            }
            return densidades.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
        }


        public Dictionary<double, double> ObtenerDensidadInversa(int cantidad)
        {
            Dictionary<double, double> densidades = new Dictionary<double, double>();
            var rand = new Random();
            double[] arr = new double[cantidad];
            for (int i = 0; i < cantidad; i++)
                arr[i] = rand.NextDouble();
            arr = arr.Distinct().ToArray();
            try
            {
                if (DistribucionContinua != null)
                    foreach (var item in arr)
                        densidades.Add(item, DistribucionContinua.QuantileDensityFunction(item));
                else
                    foreach (var item in arr)
                        densidades.Add(item, DistribucionDiscreta.QuantileDensityFunction(item));
                return densidades.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
            }
            catch (Exception)
            {
                FuncionDensidadProbabilidad aux = FactoryFuncionDensidad.Instancia(FuncionDensidad.LOG_LOGISTICA, arr);
                return aux.ObtenerDensidadInversa(cantidad);
            }
        }

        public Double getYforX(double x)
        {
            try
            {
                if (DistribucionContinua != null)
                   
                    return DistribucionContinua.QuantileDensityFunction(x);
                else
                    return DistribucionDiscreta.QuantileDensityFunction(x);
               
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public double CalcularDesvio(Dictionary<string, double> eventos)
        {
            try
            {
                if (DistribucionContinua != null)
                    Desvio = eventos.Sum(x => Math.Pow(DistribucionContinua.ProbabilityDensityFunction(Convert.ToDouble(x.Key)) - x.Value, 2));
                else
                    Desvio = eventos.Sum(x => Math.Pow(DistribucionDiscreta.ProbabilityMassFunction(Convert.ToInt32(x.Key)) - x.Value, 2));
                return Desvio;
            }
            catch (Exception)
            {
                return double.MaxValue;
            }
        }

    }
}
