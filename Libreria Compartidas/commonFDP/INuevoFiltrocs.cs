﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commonFDP
{
    public interface INuevoFiltro
    {
        List<Evento> FiltrarFechas(int idOrigen, List<Filtro> filtros, List<Evento> eventos);
        List<double> FiltrarIntervalos(List<double> intervalos, int selectedFiltro, int intervalo, int intervalo2);
    }
}
