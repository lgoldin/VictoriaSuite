using System;
using System.Collections.ObjectModel;
using Victoria.Shared.EventArgs;

namespace Victoria.Shared
{
    public class StageVariable
    {
        public string Name { get; set; }

        public double ActualValue { get; set; }

        public double InitialValue { get; set; }
    }
}
