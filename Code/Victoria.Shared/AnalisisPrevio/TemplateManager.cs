using System;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Victoria.Shared.AnalisisPrevio
{
    public class TemplateManager
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public String obtenerContent(AnalisisPrevio analisisPrevio, string s)
        {
            //logger.Info("Inicio obtener contenido");
            foreach(var item in obtenerPlaceholders(analisisPrevio))
            {
                s = s.Replace(item.Key, item.Value);
            }
            //logger.Info("Fin obtener contenido");
            return s;

        }

        public string obtenerTemplate(AnalisisPrevio analisisPrevio)
        {
            //logger.Info("Inicio Obtener template");
            var template = (analisisPrevio.TipoDeEjercicio == AnalisisPrevio.Tipo.DeltaT ? @"diagramadT.xml" : @"diagramaEaE.xml");
            if(analisisPrevio.TipoDeEjercicio == AnalisisPrevio.Tipo.EaE) { 
                ObservableCollection<EventoAP> eventos = analisisPrevio.EventosEaE;
                if (analisisPrevio.TieneVectores())
                {
                    template = @"diagramaEaEVectores.xml";  
                } else 
                {
                    var eventoLlegada = analisisPrevio.ObtenerEventoAP("Llegada");
                    var eventoSalida = analisisPrevio.ObtenerEventoAP("Salida");
                    template = eventoLlegada != null && eventoLlegada.Arrepentimiento ? @"diagramaEaEArrepentimientoLlegada.xml" : template;
                    template = eventoSalida != null && eventoSalida.Arrepentimiento ? @"diagramaEaEArrepentimientoSalida.xml" : template;
                    template = eventoLlegada != null && eventoSalida != null && eventoSalida.Arrepentimiento && eventoLlegada.Arrepentimiento ? @"diagramaEaEArrepentimientoLlegadaSalida.xml" : template;
                } 
            } 
            else
            {
                template = !analisisPrevio.Tefs.Contains("TPLL".ToUpper())? @"diagramadTSinTPLL.xml" : template;
                template = !analisisPrevio.ComprometidosAnterior.Contains("Llegada".ToUpper()) ? @"diagramadTSinLlegada.xml" : template;
                template = !analisisPrevio.Tefs.Contains("TPLL".ToUpper()) && !analisisPrevio.ComprometidosAnterior.Contains("Llegada".ToUpper()) ? @"diagramadTSinLlegadaSinTPLL.xml" : template;
            }

            //logger.Info("Fin obtener Template");
            return template;
        }

        public Dictionary<string, string> obtenerPlaceholders(AnalisisPrevio analisisPrevio) 
        {
            //logger.Info("Inicio Obtener Marcadores de Posicion");
            var map = new Dictionary<string, string>();
            if(analisisPrevio.TipoDeEjercicio == AnalisisPrevio.Tipo.EaE) {
                EventoAP eventoLlegada = analisisPrevio.ObtenerEventoAP("Llegada");
                EventoAP eventoSalida = analisisPrevio.ObtenerEventoAP("Salida");
                map.Add("eventoLlegada", eventoLlegada == null || eventoLlegada.TEF == "" ? "???" : eventoLlegada.TEF);
                //map.Add("condicionLlegada", eventoLlegada == null || eventoLlegada.Condicion == null || eventoLlegada.Condicion == "" ? "???": eventoLlegada.Condicion);
                map.Add("encadenadorLlegada", eventoLlegada == null || eventoLlegada.Encadenador == null ? "???": eventoLlegada.Encadenador);
                map.Add("eventoSalida", eventoSalida == null || eventoSalida.TEF == "" ? "???": eventoSalida.TEF);
               //map.Add("condicionSalida", eventoSalida == null || eventoSalida.Condicion == null || eventoSalida.Condicion == "" ? "???" : eventoSalida.Condicion);  
                map.Add("encadenadorSalida", eventoSalida == null || eventoSalida.Encadenador == null ? "???" : eventoSalida.Encadenador);
            }
            map.Add("resultados", generarResultadosParaPlaceholder(analisisPrevio.VariablesResultado));
            //logger.Info("Fin Obtener Marcadores de Posicion");
            return map;
        }

        private string generarResultadosParaPlaceholder(ObservableCollection<VariableAP> variablesResultado)
        {
            //logger.Info("Inicio generar resultados para marcadores de posicion");
            var resultadoStr = "";
            foreach (var resultado in variablesResultado)
            {
                resultadoStr = resultadoStr + resultado + ": ";
            }

            //logger.Info("Fin generar resultados para marcadores de posicion");
            return resultadoStr;
        }
    }
}
