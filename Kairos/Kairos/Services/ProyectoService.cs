using Kairos.Entidades;
using System;
using System.Linq;

namespace Kairos.Services
{
    internal class ProyectoService
    {
        public static bool existeProyectoPorNombre(string nombreProyecto)
        {
            using (var db = new EventoContexto())
            {
                return db.Origenes.FirstOrDefault(x => x.nombreOrigen == nombreProyecto) != null;
            }
        }

        public static Origen obtenerProyectoPorId(int idProyecto)
        {
            Origen aDevolver = null;
            using (var db = new EventoContexto())
            {
                aDevolver = db.Origenes.Find(idProyecto);
            }
            return aDevolver;
        }

        public static Origen nuevoProyecto(string nombre)
        {
            Origen aGuardar = new Entidades.Origen { nombreOrigen = nombre, fechaCreacion = DateTime.Now, activo = true };
            using (var db = new EventoContexto())
            {
                db.Origenes.Add(aGuardar);
                db.SaveChanges();
            }
            return aGuardar;
        }

        public static void modificarProyecto(int id, string nombre)
        {
            using (var db = new EventoContexto())
            {
                Origen proyecto = db.Origenes.Find(id);
                proyecto.nombreOrigen = nombre;
                //falta db.Update(proyecto);
                db.SaveChanges();
            }
        }

        public static void borrarProyecto(int id)
        {
            using (var db = new EventoContexto())
            {
                Origen proyecto = db.Origenes.Find(id);
                db.Origenes.Remove(proyecto);
                db.SaveChanges();
            }
        }

    }
}
