using System;
using System.Collections.Generic;
using System.Text;

namespace commonFDP
{
    public class ComboItem
    {
            public int Value { get; set; }
            public string Display { get; set; }
            public ComboItem(int value, string display) { this.Value = value; this.Display = display; }
    }
}
