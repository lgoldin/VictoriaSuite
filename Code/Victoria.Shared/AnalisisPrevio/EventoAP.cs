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
                return _nombre;
            }

            set
            {
                _nombre = value;
            }
        }

        private Boolean _vector = false;

        public Boolean Vector
        {
            get
            {
                return _vector;
            }

            set
            {
                _vector = value;
            }
        }

        public ObservableCollection<string> EventosNoCondicionados
        {
            get
            {
                return _eventosNoCondicionados;
            }

            set
            {
                _eventosNoCondicionados = value;
            }
        }

        public ObservableCollection<string> EventosCondicionados
        {
            get
            {
                return _eventosCondicionados;
            }

            set
            {
                _eventosCondicionados = value;
            }
        }

        public ObservableCollection<string> Condiciones
        {
            get
            {
                return _condiciones;
            }

            set
            {
                _condiciones = value;
            }
        }

        public string Encadenador
        {
            get
            {
                return _encadenador;
            }

            set
            {
                _encadenador = value;
            }
        }
        public string TEF
        {
            get
            {
                return _tef;
            }

            set
            {
                _tef = value;
            }
        }

        public string Dimension
        {
            get
            {
                return _dimension;
            }

            set
            {
                _dimension = value;
            }
        }

        public bool Arrepentimiento
        {
            get
            {
                return _arrepentimiento;
            }

            set
            {
                _arrepentimiento = value;
            }
        }

        public string ArrepentimientoStr
        {
            get
            {
                return _arrepentimiento ? "Sí" : "No";
            }

            set
            {
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
