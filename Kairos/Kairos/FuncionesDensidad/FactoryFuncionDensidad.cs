using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kairos.Modelo;
using Kairos.FuncionesDensidad.Implementacion;

namespace Kairos.FuncionesDensidad
{
    public static class FactoryFuncionDensidad
    {
        public static FuncionDensidadProbabilidad Instancia(FuncionDensidad funcion, double[] eventos)
        {
            switch (funcion)
            {
                case FuncionDensidad.WEIBULL05:
                    return new FuncionWeibull0_5(eventos);
                case FuncionDensidad.BINOMIAL:
                    return new FuncionBinomial(eventos);
                case FuncionDensidad.EXPONENCIAL:
                    return new FuncionExponencial(eventos);
                case FuncionDensidad.LOGISTICA:
                    return new FuncionLogistica(eventos);
                case FuncionDensidad.LOG_NORMAL:
                    return new FuncionLogNormal(eventos);
                case FuncionDensidad.LOG_LOGISTICA:
                    return new FuncionLogLogistica(eventos);
                case FuncionDensidad.NORMAL:
                    return new FuncionNormal(eventos);
                case FuncionDensidad.WEIBULL15:
                    return new FuncionWeibull1_5(eventos);
                case FuncionDensidad.WEIBULL3:
                    return new FuncionWeibull3(eventos);
                case FuncionDensidad.POISSON:
                    return new FuncionPoisson(eventos);
                case FuncionDensidad.UNIFORME:
                    return new FuncionUniforme(eventos);
                case FuncionDensidad.WEIBULL5:
                    return new FuncionWeibull5(eventos);
                default:
                    return null;
            }
        }
    }
}
