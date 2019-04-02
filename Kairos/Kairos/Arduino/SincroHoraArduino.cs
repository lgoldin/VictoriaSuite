using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kairos.Arduino
{
    class SincroHoraArduino
    {

        public SincroHoraArduino()
        {
            DateTime localDate = DateTime.Now;
            MessageBox.Show(localDate.ToString());
        }


    }
}
