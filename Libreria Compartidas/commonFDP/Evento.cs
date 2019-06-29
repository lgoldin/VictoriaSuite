using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Data;
using commonFDP;


namespace commonFDP

{
    public class Evento : IEquatable<Evento>
    {
        public int Id { get; set; }
        public DateTime fecha { get; set; }
        public bool activo { get; set; }

        public int idOrigen { get; set; }
        public Origen origen { get; set; }
        public double vIntervalo { get; set; }


        public bool Equals(Evento other)
        {
            return this.Id == other.Id;
        }
    }


    public class Origen
    {
        public int Id { get; set; }
        public DateTime fechaCreacion { get; set; }

        public string nombreOrigen { get; set; }

        public bool activo { get; set; }

        //public List<Evento> eventos { get; set; }
    }

}
