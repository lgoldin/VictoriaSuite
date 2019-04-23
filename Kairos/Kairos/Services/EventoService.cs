using Kairos.Entidades;
using Kairos.FuncionesDensidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kairos.Services
{
    internal class EventoService
    {
        public static void agregarTodos(List<Evento> aAgregar)
        {
            using (var db = new EventoContexto())
            {
                db.Eventos.AddRange(aAgregar.Select(x =>
                {
                    x.fecha = obtenerFechaSinMilisegundos(x.fecha);
                    return x;
                }));
                db.SaveChanges();
            }
        }

        public static void agregarTodos(List<string> aAgregarEnString, int idOrigen)
        {
            using (var db = new EventoContexto())
            {
                foreach (string item in aAgregarEnString)
                {
                    if (item != "" && item != "1970-1-1 0:0:")
                        db.Eventos.Add(new Entidades.Evento { fecha = obtenerFechaSinMilisegundos(Convert.ToDateTime(item)), idOrigen = idOrigen, activo = true });
                }
                db.SaveChanges();
            }
        }

        public static void nuevo(DateTime fecha, int idOrigen)
        {
            using (var db = new EventoContexto())
            {
                db.Eventos.Add(new Evento { fecha = obtenerFechaSinMilisegundos(fecha), idOrigen = idOrigen, activo = true });
                db.SaveChanges();
            }
        }

        public static void nuevoPorIntervalo(int cantidad, Segmentacion segm, int idOrigen)
        {
            DateTime fechaAAgregar = DateTime.Now;
            using (var db = new EventoContexto())
            {
                DateTime maxFecha;
                if (db.Eventos.Where(x=>x.idOrigen==idOrigen).Count()>0)
                    maxFecha = db.Eventos.Where(x => x.idOrigen == idOrigen).Max(x => x.fecha);
                else
                    maxFecha = DateTime.Now;

                switch (segm)
                {
                    case Segmentacion.DIA:
                        fechaAAgregar = maxFecha.AddDays(cantidad);
                        break;
                    case Segmentacion.HORA:
                        fechaAAgregar = maxFecha.AddHours(cantidad);
                        break;
                    case Segmentacion.MINUTO:
                        fechaAAgregar = maxFecha.AddMinutes(cantidad);
                        break;
                    case Segmentacion.SEGUNDO:
                        fechaAAgregar = maxFecha.AddSeconds(cantidad);
                        break;
                    default:
                        break;
                }
                db.Eventos.Add(entity: new Evento { fecha = fechaAAgregar, idOrigen = idOrigen, activo = true });
                db.SaveChanges();
            }
        }

        public static void modificar(int idEvento, DateTime fecha)
        {
            using (var db = new EventoContexto())
            {
                Evento evento = db.Eventos.Find(idEvento);
                evento.fecha = obtenerFechaSinMilisegundos(fecha);
                db.Update(evento);
                db.SaveChanges();
            }
        }

        public static void borrar(int idEvento)
        {
            using (var db = new EventoContexto())
            {
                Evento evento = db.Eventos.Find(idEvento);
                db.Eventos.Remove(evento);
                db.SaveChanges();
            }
        }

        public static void borrar(List<Evento> aBorrar)
        {
            using (var db = new EventoContexto())
            {
                db.Eventos.RemoveRange(aBorrar);
                db.SaveChanges();
            }
        }

        public static List<Evento> cargarEventos(int idOrigen)
        {
            List<Evento> eventos = null;
            using (var db = new EventoContexto())
            {
                eventos = (from e in db.Eventos
                           where e.activo == true && e.idOrigen == idOrigen
                           orderby e.fecha ascending
                           select e).ToList();
            }
            return eventos;
        }

        private static DateTime obtenerFechaSinMilisegundos(DateTime aRemoverMilisegundos)
        {
            DateTime aux = aRemoverMilisegundos.AddTicks(-(aRemoverMilisegundos.Ticks % 10000000));
            return aux;
        }
    }
}
