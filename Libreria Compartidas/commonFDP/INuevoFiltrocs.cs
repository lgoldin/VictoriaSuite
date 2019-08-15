using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commonFDP
{
    interface INuevoFiltro
    {
        List<Evento> FiltrarFechas(int idOrigen, List<Filtro> filtros);
        List<double> FiltrarIntervalos(List<double> intervalos, int selectedFiltro, int intervalo, int intervalo2);
    }
}
