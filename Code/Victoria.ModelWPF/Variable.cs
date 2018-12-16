﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Victoria.Shared;
using Victoria.Shared.AnalisisPrevio;
using Victoria.Shared.EventArgs;
using Victoria.Shared.Prism;

namespace Victoria.ModelWPF
{
    public class Variable : NotificationObject
    {
        #region Fields

        private string _name;
        private ObservableCollection<double> _values;
        private double initialValue;
        private double actualValue;
        private bool isSelected;

        #endregion

        #region Properties

        public Shared.Variable VariableComponent { get; set; }

        public bool TimeHasChange { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public double InitialValue
        {
            get
            {
                return initialValue;
            }
            set
            {
                initialValue = value;
                this.VariableComponent.InitialValue = value;
                this.RaisePropertyChanged("InitialValue");
            }
        }

        public double ActualValue
        {
            get
            {
                return actualValue;
            }
            set
            {
                actualValue = value;
                this.RaisePropertyChanged("ActualValue");
            }
        }

        public ObservableCollection<double> Values
        {
            get { return _values; }
            set
            {
                if (this._values != null)
                    this._values.CollectionChanged -= Values_CollectionChanged;

                _values = value;
                this._values.CollectionChanged += Values_CollectionChanged;
                this.RaisePropertyChanged("Values");
            }
        }

        public IEnumerable<Double> ValuesEnumerable
        {
            get { return _values.ToList(); }
        }

        public bool IsSelected
        {
            get { return isSelected || IsTimeVariable; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsTimeVariable
        {
            get { return this.Name.Equals("T"); }
        }
        
        public VariableType Type { get; set; }

        #endregion

        public Variable()
        {
        }

        public Variable(Shared.Variable variable)
        {
            this.VariableComponent = variable;
            variable.ValueChanged += variable_ValueChanged;
            this.Name = variable.Name;
            this.Values = new ObservableCollection<double>();
            this.initialValue = variable.InitialValue;
            this.Type = variable.Type;
        }

        private void variable_ValueChanged(object sender, VariableValueChangeEventArgs e)
        {
            this.Values.Add(e.NewValue);
            if (this.IsTimeVariable)
            {
                this.TimeHasChange = true;
            }
        }

        private void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.ActualValue = Convert.ToDouble(e.NewItems[0]);
        }
    }
}
