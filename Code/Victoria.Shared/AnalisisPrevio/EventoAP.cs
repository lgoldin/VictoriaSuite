using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Victoria.Shared.AnalisisPrevio
{
    public class EventoAP
    {
        #region Fields

        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AppDomain));
        private string _nombre;

        private ObservableCollection<string> _eventosNoCondicionados = new ObservableCollection<string>();

        private ObservableCollection<string> _eventosCondicionados = new ObservableCollection<string>();

        private ObservableCollection<string> _condiciones = new ObservableCollection<string>();

        private string _encadenador;

        private string _tef;

        private string _dimension;

        private bool _arrepentimiento;

        private string _arrepentimientoStr;

        #endregion Fields

        #region Properties

        public string Nombre
        {
            get
            {
                logger.Info("Obtener Nombre");
                return _nombre;
            }

            set
            {
                logger.Info("Setear Nombre");
                _nombre = value;
            }
        }

        private Boolean _vector = false;

        public Boolean Vector
        {
            get
            {
                logger.Info("Obtener Vector");
                return _vector;
            }

            set
            {
                logger.Info("Setear Vector");
                _vector = value;
            }
        }

        public ObservableCollection<string> EventosNoCondicionados
        {
            get
            {
                logger.Info("Obtener Eventos No Condicionados");
                return _eventosNoCondicionados;
            }

            set
            {
                logger.Info("Setear Eventos No Condicionados");
                _eventosNoCondicionados = value;
            }
        }

        public ObservableCollection<string> EventosCondicionados
        {
            get
            {
                logger.Info("Obtener Eventos Condicionados");
                return _eventosCondicionados;
            }

            set
            {
                logger.Info("Setear Eventos Condicionados");
                _eventosCondicionados = value;
            }
        }

        public ObservableCollection<string> Condiciones
        {
            get
            {
                logger.Info("Obtener Condiciones");
                return _condiciones;
            }

            set
            {
                logger.Info("Setear Condiciones");
                _condiciones = value;
            }
        }

        public string Encadenador
        {
            get
            {
                logger.Info("Obtener Encadenador");    
                return _encadenador;
            }

            set
            {
                logger.Info("Setear Encadenador");
                _encadenador = value;
            }
        }
        public string TEF
        {
            get
            {
                logger.Info("Obtener TEF");
                return _tef;
            }

            set
            {
                logger.Info("Setear TEF");
                _tef = value;
            }
        }

        public string Dimension
        {
            get
            {   logger.Info("Obtener Dimension");
                return _dimension;
            }

            set
            {
                logger.Info("Setear Dimension");
                _dimension = value;
            }
        }

        public bool Arrepentimiento
        {
            get
            {
                logger.Info("Obtener Arrepentimiento");
                return _arrepentimiento;
            }

            set
            {
                logger.Info("Setear Arrepentimiento");
                _arrepentimiento = value;
            }
        }

        public string ArrepentimientoStr
        {
            get
            {
                logger.Info("Obtener Arrepentimiento Str");
                return _arrepentimiento ? "Sí" : "No";
            }

            set
            {
                logger.Info("Setear Obtener Arrepentimiento");
                _arrepentimientoStr = value;
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Nombre;
        }

        public bool validarEncadenador(List<string> itemsSource)
        {
            return itemsSource.Any(item => item == Encadenador);
        }

        public VariableAP getAsVariableAP()
        {
            return (new VariableAP() { nombre = TEF, valor = !String.Equals(TEF, "TPS") ? 0.0 : 99999999, vector = Vector, i = (Vector) ? 1 : 0, dimension = Dimension, type = VariableType.Other });
        }


        #endregion Methods
    }
}
