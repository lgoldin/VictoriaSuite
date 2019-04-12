using ClosedXML.Excel;
using Kairos.Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kairos.Services.Implementaciones
{
    internal class ImportarExcelFile : IImportarService
    {
        private int hoja = -1;
        private int columna = -1;
        private int filaInicial = -1;

        public ImportarExcelFile(int hoja, int columna, int filaInicial)
        {
            this.hoja = hoja;
            this.columna = columna;
            this.filaInicial = filaInicial;
        }

        public bool importarArchivoEnNuevoProyecto(string pathArchivo, string nombreProyecto)
        {
            bool resultado = false;
            try
            {
                Origen nuevoOrigen = ProyectoService.nuevoProyecto(nombreProyecto);
                List<Evento> eventos = new List<Evento>();
                using (var archivo = new XLWorkbook(pathArchivo))
                {
                    var hoja = archivo.Worksheet(this.hoja);
                    int numeroFila = this.filaInicial;
                    int columna = this.columna;
                    while (!hoja.Cell(numeroFila, columna).IsEmpty())
                    {
                        DateTime auxFecha = hoja.Cell(numeroFila, columna).GetDateTime();
                        eventos.Add(new Evento() { fecha = auxFecha, origen = nuevoOrigen, activo = true });
                        numeroFila++;
                    }
                }
                EventoService.agregarTodos(eventos);

                resultado = true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                resultado = false;
            }
            return resultado;
        }

        public bool importarArchivoEnProyectoExistente(string pathArchivo, int idProyecto)
        {
            bool resultado = false;
            try
            {
                Origen auxOrigen = ProyectoService.obtenerProyectoPorId(idProyecto);

                if (auxOrigen != null)
                {
                    List<Evento> eventos = new List<Evento>();
                    using (var archivo = new XLWorkbook(pathArchivo))
                    {
                        var hoja = archivo.Worksheet(this.hoja);
                        int numeroFila = this.filaInicial;
                        int columna = this.columna;
                        while (!hoja.Cell(numeroFila, columna).IsEmpty())
                        {
                            DateTime auxFecha = hoja.Cell(numeroFila, columna).GetDateTime();
                            eventos.Add(new Evento() { fecha = auxFecha, idOrigen = auxOrigen.Id, activo = true });
                            numeroFila++;
                        }
                    }

                    EventoService.agregarTodos(eventos);
                    resultado = true; 
                }
                else
                    resultado = false;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                resultado = false;
            }
            return resultado;
        }
    }
}
