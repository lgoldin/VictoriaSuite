using System;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Victoria.Shared.AnalisisPrevio
{
    public class TemplateManager
    {
        public String obtenerContent(AnalisisPrevio analisisPrevio, string s)
        {
           
            foreach(var item in obtenerPlaceholders(analisisPrevio))
            {
                s = s.Replace(item.Key, item.Value);
            }

            return s;

        }

        public string obtenerTemplate(AnalisisPrevio analisisPrevio)
        {
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

            return template;
        }

        public Dictionary<string, string> obtenerPlaceholders(AnalisisPrevio analisisPrevio) 
        {
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
            return map;
        }

        private string generarResultadosParaPlaceholder(ObservableCollection<VariableAP> variablesResultado)
        {
            var resultadoStr = "";
            foreach (var resultado in variablesResultado)
            {
                resultadoStr = resultadoStr + resultado + ": ";
            }
            return resultadoStr;
        }
    }
}
