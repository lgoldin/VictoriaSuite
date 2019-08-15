using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commonFDP
{
    class Filtro
    {
        public bool IsChecked { get; set; }
        public string Name { get; }

        public TipoFiltro Tipo { get; set; }

        public DateTime Fecha1 { get; set; }

        public DateTime Fecha2 { get; set; }

        public double intervalo { get; set; }

        public double intervalo2 { get; set; }

        public Filtro(TipoFiltro tipo, DateTime fecha)
        {
            this.Tipo = tipo;
            this.Fecha1 = fecha;
            this.IsChecked = true;
            switch (tipo)
            {
                case TipoFiltro.HORA_MENOR:
                    this.Name = "Hora < a " + fecha.ToShortTimeString();
                    break;
                case TipoFiltro.HORA_MAYOR:
                    this.Name = "Hora > a " + fecha.ToShortTimeString();
                    break;
                case TipoFiltro.FECHA_MENOR:
                    this.Name = "Fecha < a " + fecha.ToShortDateString();
                    break;
                case TipoFiltro.FECHA_MAYOR:
                    this.Name = "Fecha > a " + fecha.ToShortDateString();
                    break;
                default:
                    break;
            }
        }

        public Filtro(TipoFiltro tipo, DateTime fecha1, DateTime fecha2)
        {
            this.Tipo = tipo;
            this.Fecha1 = fecha1;
            this.Fecha2 = fecha2;
            this.IsChecked = true;
            switch (tipo)
            {
                case TipoFiltro.HORA_ENTRE:
                    this.Name = "Hora > a " + fecha1.ToShortTimeString() + " y < a" + fecha2.ToShortTimeString();
                    break;
                case TipoFiltro.FECHA_ENTRE:
                    this.Name = "Fecha > a " + fecha1.ToShortDateString() + " y < a" + fecha2.ToShortDateString();
                    break;
                default:
                    throw new ArgumentException("Los tipos de filtro para 2 fechas son entre");
            }
        }

        public Filtro(TipoFiltro tipo, double intervalo)
        {
            this.Tipo = tipo;
            this.intervalo = intervalo;
            this.IsChecked = true;
            switch (tipo)
            {
                case TipoFiltro.INTERVALO_MENOR:
                    this.Name = "Intervalo < a " + intervalo;
                    break;
                case TipoFiltro.INTERVALO_MAYOR:
                    this.Name = "Intervalo > a " + intervalo;
                    break;
                default:
                    break;
            }
        }

        public Filtro(TipoFiltro tipo, double intervalo1, double intervalo2)
        {
            this.Tipo = tipo;
            this.intervalo = intervalo1;
            this.intervalo2 = intervalo2;
            this.IsChecked = true;
            switch (tipo)
            {
                case TipoFiltro.INTERVALO_ENTRE:
                    this.Name = "Intervalo > a " + intervalo + " y < a " + intervalo2;
                    break;
                default:
                    break;
            }
        }
    }
}
