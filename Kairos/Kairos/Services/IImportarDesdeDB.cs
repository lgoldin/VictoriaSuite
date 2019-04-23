using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Services
{
    interface IImportarDesdeDB
    {
        List<DateTime> ObtenerColumnaDesdeDB(string connectionString, string database, string nombreDeTabla, string nombreColumna);
    }
}
