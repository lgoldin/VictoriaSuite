using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;
using Kairos.Entidades;
using Kairos.Modelo;

namespace Kairos.FuncionesDensidad
{
    public static class FdPUtils
    {
        public static Dictionary<string, double> AgruparIntervalos(List<Evento> eventos)
        {
            if (eventos != null && eventos.Count > 0)
            {
                double cantEventos = (double)eventos.Count;
                var eventosOrdenados = eventos.OrderBy(x => x.fecha);
                return eventosOrdenados.Select(x => x.fecha.TimeOfDay)
                    .Zip(eventosOrdenados.Select(x => x.fecha)
                    .Skip(1), (x, y) => y - x)
                    .Select(x => Math.Abs(x.TimeOfDay.TotalSeconds))
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key.ToString(), x => x.Count() / (cantEventos > 1 ? cantEventos - 1 : cantEventos));
            }
            else
                return null;
        }

        public static List<double> CalcularIntervalos(List<Evento> eventos)
        {
            if (eventos != null && eventos.Count > 0)
            {
                double cantEventos = (double)eventos.Count;
                var eventosOrdenados = eventos.OrderBy(x => x.fecha);
                return eventosOrdenados.Select(x => x.fecha.TimeOfDay)
                    .Zip(eventosOrdenados.Select(x => x.fecha)
                    .Skip(1), (x, y) => y - x)
                    .Select(x => Math.Abs(x.TimeOfDay.TotalSeconds)).ToList();
                /*return eventosOrdenados.Select(x => x.fecha)
                    .Zip(eventosOrdenados.Select(x => x.fecha)
                    .Skip(1), (x, y) => y.Date == x.Date ? y - x : TimeSpan.MaxValue)
                    .Where(x => x != TimeSpan.MaxValue)
                    .Select(x => Math.Abs(x.TotalMinutes))
                    .ToList();*/
            }
            else
                return null;
        }

        public static List<double> AgruparSegmentacion(Segmentacion segmentacion, List<Evento> eventos)
        {
            if (eventos != null && eventos.Count > 0)
            {
                double cantEventos = (double)eventos.Count;
                switch (segmentacion)
                {
                    case Segmentacion.DIA:
                        return eventos.Select(x => new { Mes = x.fecha.Month, Anio = x.fecha.Year, Dia = x.fecha.Day })
                            .GroupBy(x => new { x.Mes, x.Anio, x.Dia }).Select(x=> Convert.ToDouble(x.Count())).ToList();
                    case Segmentacion.HORA:
                        return eventos.Select(x => new { Mes = x.fecha.Month, Anio = x.fecha.Year, Dia = x.fecha.Day, Hora = x.fecha.Hour })
                            .GroupBy(x => new { x.Anio, x.Mes, x.Dia, x.Hora }).Select(x => Convert.ToDouble(x.Count())).ToList();
                    case Segmentacion.MINUTO:
                        return eventos.Select(x => new { Mes = x.fecha.Month, Anio = x.fecha.Year, Dia = x.fecha.Day, Hora = x.fecha.Hour, Minuto = x.fecha.Minute })
                            .GroupBy(x => new { x.Anio, x.Mes, x.Dia, x.Hora, x.Minuto }).Select(x => Convert.ToDouble(x.Count())).ToList();
                    case Segmentacion.SEGUNDO:
                        return eventos.Select(x => new { Mes = x.fecha.Month, Anio = x.fecha.Year, Dia = x.fecha.Day, Hora = x.fecha.Hour, Minuto = x.fecha.Minute, Segundo = x.fecha.Second })
                            .GroupBy(x => new { x.Anio, x.Mes, x.Dia, x.Hora, x.Minuto, x.Segundo }).Select(x => Convert.ToDouble(x.Count())).ToList();
                    default:
                        return null;
                }
            }
            else
                return null;
        }

        public static Dictionary<string,double> AgruparSegmentacionProbabilidad(List<double>listaSegmentada)
        {
            if (listaSegmentada != null && listaSegmentada.Count > 0)
            {
                double cantEventos = (double)listaSegmentada.Count;

                return listaSegmentada.GroupBy(x => x).ToDictionary(x => x.Key.ToString(), x => x.Count() / cantEventos);
            }
            else
                return null;
        }

    }
}
