using System;
using System.Collections.ObjectModel;
using Victoria.Shared.AnalisisPrevio;
using Victoria.Shared.EventArgs;

namespace Victoria.Shared
{
    public class Variable
    {
        private double actualValue;

        public delegate void VariableValueChangedEventHandler(object sender, VariableValueChangeEventArgs e);
        
        /// <summary>
        /// Event raised when variable value changed
        /// </summary>
        public event VariableValueChangedEventHandler ValueChanged;

        public string Name { get; set; }

        public double ActualValue
        {
            get 
            { 
                return actualValue; 
            }
            
            set
            {
                var oldValue = actualValue;
                actualValue = value;

                if (this.ValueChanged != null)
                {
                    this.ValueChanged.Invoke(this, new VariableValueChangeEventArgs(this, oldValue, actualValue));
                }
            }
        }

        public double InitialValue { get; set; }

        public VariableType Type { get; set; }
    }
}
