using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace Victoria.Shared.AnalisisPrevio
{
    public class AnalisisPrevio
    {

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        public enum Tipo
        {
            EaE,
            DeltaT
        };

        public enum TipoEvento
        {
            Independiente,
            ConDependencia
        };

        public Tipo TipoDeEjercicio { get; set; }

        public TipoEvento TipoDeEaE { get; set; }

        public ObservableCollection<string> Datos { get; set; }

        public ObservableCollection<string> VariablesDeControl { get; set; }

        public ObservableCollection<VariableAP> VariablesEstado { get; set;}

        public ObservableCollection<VariableAP> VariablesResultado { get; set; }

        public ObservableCollection<EventoAP> EventosEaE { get; set; }

        public ObservableCollection<string> Propios { get; set; }

        public ObservableCollection<string> ComprometidosFuturos { get; set; }

        public ObservableCollection<string> ComprometidosAnterior { get; set; }

        public ObservableCollection<string> Tefs { get; set; }

        public ObservableCollection<commonFDP.ResultadoAjuste> listFDP { get; set; }

        public AnalisisPrevio(Tipo tipo, TipoEvento tipoDeEaE)
        {
            //logger.Info("Inicio Analisis Previo");
            this.InicializarColecciones();
            this.TipoDeEjercicio = tipo;
            if (tipo == AnalisisPrevio.Tipo.EaE) this.TipoDeEaE = tipoDeEaE; 
            this.InicializarAnalisisPrevioPorDefecto();
            //logger.Info("Fin Analisis Previo");

        }

        private void InicializarColecciones() 
        {
            //logger.Info("Inicio Inicializar Colecciones");
            this.Datos = new ObservableCollection<string>();
            this.VariablesDeControl = new ObservableCollection<string>();
            this.VariablesEstado = new ObservableCollection<VariableAP>();
            this.VariablesResultado = new ObservableCollection<VariableAP>();
            this.listFDP = new ObservableCollection<commonFDP.ResultadoAjuste>();
            this.Propios = new ObservableCollection<string>();
            this.ComprometidosAnterior = new ObservableCollection<string>();
            this.ComprometidosFuturos = new ObservableCollection<string>();
            this.EventosEaE = new ObservableCollection<EventoAP>();
            this.Tefs = new ObservableCollection<string>();
            //logger.Info("Fin Inicializar Colecciones");
        }

        private void InicializarAnalisisPrevioPorDefecto() 
        {
            //logger.Info("Inicio Inicializar Analisis Previo por Defecto");
            if (Tipo.EaE.Equals(TipoDeEjercicio))
            {
                this.CargarMetodologiaEaEPorDefecto();
            }
            else
            {
                this.CargarMetodologiaDeltaTPorDefecto();
            }
            //logger.Info("Fin Inicializar Analisis Previo por Defecto");
        }

        private void CargarMetodologiaEaEPorDefecto()
        {
            //logger.Info("Inicio Cargar Metodo Logica EaE Por Defecto");
            if (this.TipoDeEaE == AnalisisPrevio.TipoEvento.Independiente)
            {
                this.inicializarEaETEI();
                this.Datos.Add("IA");
                this.Datos.Add("TA");
                this.VariablesEstado.Add(new VariableAP() { nombre = "NS", valor = 0, vector = false, i = 1, type = VariableType.State });
                this.VariablesResultado.Add(new VariableAP() { nombre = "PPS", valor = 0, vector = false, i = 1, type = VariableType.Result });
                this.VariablesResultado.Add(new VariableAP() { nombre = "PTO", valor = 0, vector = false, i = 1, type = VariableType.Result });
            }
            else 
            {
                this.inicializarEaETEventos();
                this.Datos.Add("VUA");
                this.Datos.Add("VUB");
                this.Datos.Add("CRA");
                this.Datos.Add("CRB");
                this.VariablesDeControl.Add("PM");
                this.VariablesDeControl.Add("PORC");
                this.VariablesResultado.Add(new VariableAP() { nombre = "CTM", valor = 0, vector = false, i = 1, type = VariableType.Result });
                this.VariablesEstado.Add(new VariableAP() { nombre = "CTM", valor = 0, vector = false, i = 1, type = VariableType.State });
            }
            //logger.Info("Fin  Cargar Metodo Logica EaE Por Defecto");
        }

        private void inicializarEaETEventos()
        {
            //logger.Info("Inicio Inicializar EaE T Eventos");
            EventoAP evento = new EventoAP();
            evento.Nombre = "Rotura A";
            evento.EventosNoCondicionados.Add("Rotura A");

            evento.EventosCondicionados.Add("Rotura B");
            evento.Condiciones.Add("TMP - T <= (PORC * PM) / 100)");
            
            evento.EventosCondicionados.Add("Mantenimiento");
            evento.Condiciones.Add("TMP - T <= (PORC * PM) / 100)");

            evento.Encadenador = "VUA";
            evento.Arrepentimiento = false;
            evento.Vector = false;
            evento.TEF = "TRA";

            EventoAP evento2 = new EventoAP();
            evento2.Nombre = "Rotura B";
            evento2.EventosNoCondicionados.Add("Rotura B");

            evento2.EventosCondicionados.Add("Rotura A");
            evento2.Condiciones.Add("TMP - T <= (PORC * PM) / 100)");

            evento2.EventosCondicionados.Add("Mantenimiento");
            evento2.Condiciones.Add("TMP - T <= (PORC * PM) / 100)");

            evento2.Encadenador = "VUB";
            evento2.Arrepentimiento = false;
            evento2.Vector = false;
            evento2.TEF = "TRB";

            EventoAP evento3 = new EventoAP();
            evento3.Nombre = "Mantenimiento";
            evento3.EventosNoCondicionados.Add("Rotura A");
            evento3.EventosNoCondicionados.Add("Rotura B");
            evento3.EventosNoCondicionados.Add("Mantenimiento");

            evento3.Encadenador = "PM";
            evento3.Arrepentimiento = false;
            evento3.Vector = false;
            evento3.TEF = "TMP";

            this.EventosEaE.Add(evento);
            this.EventosEaE.Add(evento2);
            this.EventosEaE.Add(evento3);


            //logger.Info("Fin Inicializar EaE T Eventos");
        }

        private void inicializarEaETEI()
        {
            //logger.Info("Incio Inicializar EaE TEI");
            EventoAP llegada = new EventoAP();
            llegada.Nombre = "Llegada";
            llegada.EventosNoCondicionados.Add("Llegada");
            llegada.EventosCondicionados.Add("Salida");
            llegada.Condiciones.Add("NS == 1");
            llegada.TEF = "TPLL";
            llegada.Encadenador = "IA";
            llegada.Vector = false;
            llegada.Arrepentimiento = false;
            this.EventosEaE.Add(llegada);

            EventoAP salida = new EventoAP();
            salida.Nombre = "Salida";
            salida.EventosCondicionados.Add("Salida");
            salida.Condiciones.Add("NS > 0");
            salida.TEF = "TPS";
            salida.Encadenador = "TA";
            salida.Vector = false;
            salida.Arrepentimiento = false;
            this.EventosEaE.Add(salida);

            //logger.Info("Fin Inicializar EaE TEI");
        }

        private void CargarMetodologiaDeltaTPorDefecto()
        {
            //logger.Info("Inicio Cargar Metodo logica Delta T Por Defecto");
            this.Datos.Add("VD");
            this.VariablesEstado.Add(new VariableAP() { nombre = "ST", valor = 0, vector = false, i = 1, type = VariableType.State });
            this.VariablesDeControl.Add("SR");
            this.VariablesDeControl.Add("TP");
            this.Propios.Add("Venta");
            this.ComprometidosFuturos.Add("Emisión");
            this.ComprometidosAnterior.Add("Llegada");
            this.Tefs.Add("TPLL");

            //logger.Info("Fin Cargar Metodo logica Delta T Por Defecto");
        }

        public EventoAP ObtenerEventoAP(string nombreEvento)
        {
            //logger.Info("Inicio obtener evento AP");
            return this.EventosEaE.Count == 0 ? null : this.EventosEaE.FirstOrDefault(item => item.Nombre.ToUpper() == nombreEvento.ToUpper());
        }

        public List<VariableAP> ObtenerVariablesAP() 
        {
            //logger.Info("Inicio Obtener Variables AP");
            var variables = new List<VariableAP>();
            
            variables.AddRange(this.VariablesEstado);
            variables.AddRange(this.VariablesResultado);
            variables.AddRange(this.ObtenerVariablesAPDeEventosEaE());
            variables.AddRange(this.ObtenerVariablesAPDeVariablesSimples());

            //logger.Info("Fin Obtener Variables AP");
            return variables;
        }

        private List<VariableAP> ObtenerVariablesAPDeEventosEaE() 
        {
            //logger.Info("Inicio Obtener Variables AP de Eventos Eae");
            var variablesEnEventos = new List<VariableAP>();

            foreach (EventoAP evento in this.EventosEaE)
            {
                variablesEnEventos.Add(evento.getAsVariableAP());
            }

            //logger.Info("Fin Obtener Variables AP de Eventos Eae");
            return variablesEnEventos;
        }

        private List<VariableAP> ObtenerVariablesAPDeVariablesSimples()
        {
            //logger.Info("Inicio Obtener Variables AP De Variables Simples");
            var variablesAP = new List<VariableAP>();

            foreach (string variableSimple in this.Datos)
            {
                variablesAP.Add((new VariableAP { nombre = variableSimple, valor = 0.0, vector = false, i = 0, type = VariableType.Data }));
            }

            foreach (string variableSimple in this.VariablesDeControl)
            {
                //TODO: cambiar cuando se tenga implementada la funcionalidad de grupos de vectores asociados a distintas variables de control
                variablesAP.Add((new VariableAP { nombre = variableSimple, valor = 0.0, vector = false, i = 0, type = VariableType.Control }));
            }

            //logger.Info("Fin Obtener Variables AP De Variables Simples");
            return variablesAP;
        }
 
        public Boolean TieneVectores() 
        {
            //logger.Info("Inicio Tiene Vectores");
            return this.VariablesEstado.Any(variable => variable.vector);
        }

        public void addFDPToList(commonFDP.ResultadoAjuste fdp)
        {
            listFDP.Add(fdp);
        }

    }
}
