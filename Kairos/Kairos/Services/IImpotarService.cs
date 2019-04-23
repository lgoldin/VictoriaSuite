using Kairos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Services
{
    interface IImportarService
    {
        bool importarArchivoEnNuevoProyecto(string pathArchivo, string nombreProyecto);
        bool importarArchivoEnProyectoExistente(string pathArchivo, int idProyecto);
    }
}
