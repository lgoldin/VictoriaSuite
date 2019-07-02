using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commonFDP
{
    public class ResultadoAjuste
    {
        public string Funcion { get; }
        public string Inversa { get; }
        public double DesvioEstandar { get; }
        public double Entropia { get; }
        public double Media { get; }
        public double Varianza { get; }
        public FuncionDensidadProbabilidad FDP { get; }
        public ResultadoAjuste(string Funcion, string Inversa, double DesvioEstandar, double Media, double Varianza, FuncionDensidadProbabilidad fdp)
        {
            this.Funcion = Funcion;
            this.Inversa = Inversa;
            this.DesvioEstandar = DesvioEstandar;
            this.Media = Media;
            this.Varianza = Varianza;
            this.FDP = fdp;
        }
    }
}
