using Kairos.Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Kairos.Services.Implementaciones
{
    internal class ImportarTxtFile : IImportarService
    {
        private string delimitador;

        public ImportarTxtFile(string delimitador)
        {
            this.delimitador = delimitador;
        }

        public bool importarArchivoEnNuevoProyecto(string pathArchivo, string nombreProyecto)
        {
            try
            {
                List<string> eventos = new List<string>();

                if (delimitador == "enter")
                    eventos = leerDelimitadorEnter(pathArchivo);
                else
                    eventos = leerDelimitadorCaracter(pathArchivo, this.delimitador);

                Origen origen = ProyectoService.nuevoProyecto(nombreProyecto);
                EventoService.agregarTodos(eventos, origen.Id);
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }

        }
        //lL
        public List<string> leerDelimitadorEnter(string pathArchivo)
        {
            StreamReader objReader = new StreamReader(pathArchivo);
            string sLine = "";
            List<string> eventosLeidos = new List<string>();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    eventosLeidos.Add(sLine);
            }
            objReader.Close();
            return eventosLeidos;
        }


        public List<string> leerDelimitadorCaracter(string pathArchivo, string caracter)
        {
            StreamReader objReader = new StreamReader(pathArchivo);
            List<string> eventosLeidos = new List<string>();
            string todoElArchivo = objReader.ReadToEnd();
            objReader.Close();
            foreach (var item in todoElArchivo.Split(Convert.ToChar(caracter)))
            {
                DateTime aux = DateTime.Parse(item);
                eventosLeidos.Add(aux.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            return eventosLeidos;
        }

        public bool importarArchivoEnProyectoExistente(string pathArchivo, int idProyecto)
        {
            bool resultado = false;
            try
            {
                List<string> eventos = new List<string>();
                if (delimitador == "enter")
                    eventos = leerDelimitadorEnter(pathArchivo);
                else
                    eventos = leerDelimitadorCaracter(pathArchivo, this.delimitador);

                Origen auxOrigen = ProyectoService.obtenerProyectoPorId(idProyecto);
                if (auxOrigen != null)
                {
                    EventoService.agregarTodos(eventos, auxOrigen.Id);
                    resultado = true;
                }
                else
                    resultado = false;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                resultado = false;
            }
            return resultado;
        }
    }
}

