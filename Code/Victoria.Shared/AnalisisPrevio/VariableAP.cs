using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victoria.Shared.AnalisisPrevio
{
    public class VariableAP
    {
        public string nombre { get; set; }

        public double valor { get; set; }

        public bool vector { get; set; }

        public VariableType type { get; set; }

        public string dimension { get; set; }

        public bool notDimensionable
        {
            get
            {
                return this.vector == false;
            }
        }

        public double i { get; set; }

        public string GetNameForDesigner() 
        { 
            return this.vector ? this.nombre.Split('(')[0] + "(I)" : this.nombre;
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
