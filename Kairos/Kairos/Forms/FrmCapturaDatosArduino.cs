using Kairos.Arduino;
using Kairos.Entidades;
using Kairos.Services.Implementaciones;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace Kairos.Forms
{
    public partial class FrmCapturaDatosArduino : Form
    {
        private string tipoArchivo = "";
        private bool timerActivo = false;
        private SerialPort serialPort = new SerialPort();
        private EscrituraTxt escribirDistancias;
        private string estado = null;               //Estado del Form que puede ser "iniciado" o "finalizado"

        private Origen proyecto;
        public FrmCapturaDatosArduino(Origen proyecto)
        {
            InitializeComponent();
            this.proyecto = proyecto;
            // Creamos el evento para leer datos desde puerto serial
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            //show list of valid com ports
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxPuerto.Items.Add(s);
            }
            // Inicializamos comboBox de lectura de puerto y parametro de velocidad de transmicion por puerto serial
            if (comboBoxPuerto.Items.Count > 0)
            {
                comboBoxPuerto.SelectedIndex = 0;
            }
            // Asignamos las propiedades ---- PARA LEER DESDE UN ARCHIVO DE CONFIGURACION probando branch1 -----
            serialPort.BaudRate = 9600;
            /* var portNames = SerialPort.GetPortNames();

             foreach (var port in portNames)
             {
                 serialPort.PortName = port;
                 try
                 {
                     serialPort.Open();
                     break;
                 }
                 catch
                 {
                     continue;
                 }
             }*/
            // mostrarMensaje("Seleccione un puerto", Color.FromArgb(255, 89, 89));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string inData;
            //Si el puerto esta abierto
            if (serialPort.IsOpen)
            {
                //Si hay algun archivo de escritura abierto
                if (escribirDistancias.estaAbierto())
                {
                    // Obtenemos el puerto serie que lanza el evento
                    // Leemos el dato recibido del puerto serie
                    try
                    {
                        SerialPort currentSerialPort = (SerialPort)sender;
                        inData = currentSerialPort.ReadLine();
                        escribirDistancias.escribir(inData + "\n");
                    }
                    catch
                    {

                    }
                    // Y lo escribimos en un label inferior del Form
                    //labelLectura.Text = inData;
                }

            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (escribirDistancias != null)
            {
                escribirDistancias.cerrarArchivo();
                try
                {
                    // Cerramos el puerto serie
                    serialPort.Close();
                }
                catch
                {
                    mostrarMensaje("Error al cerrar el puerto serie", Color.FromArgb(255, 89, 89));
                }
            }

        }

        private void buttonComenzar_Click(object sender, EventArgs e)
        {
            try
            {
                //Si no se inicio ya una grabacion
                if (estado != "iniciado")
                {
                    //Seteo el puerto para la lectura del Puerto Serial
                    if (comboBoxPuerto.Enabled == true)
                    {
                        serialPort.PortName = comboBoxPuerto.Text;
                    }
                    // Valida que se haya seleccionado algun archivo
                    if (textBoxExplorar.Text == "")
                    {
                        // Set the error if the age is too young.
                        errorProviderExplorar.SetError(textBoxExplorar, "Seleccione archivo donde se grabaran los datos");
                        return;
                    }
                    // Instancio clase para controlar escritura de archivo
                    escribirDistancias = new EscrituraTxt(textBoxExplorar.Text);
                    // Creamos el evento para manejo del puerto serial
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                    // Controlamos que el puerto indicado esté operativo y lo abrimos para la conexion
                    // Sino mandamos mensaje de error ARDUINO y cerramos el Form
                    if (serialPort.IsOpen == false)
                    {
                        serialPort.Open();
                        DateTime localDate = DateTime.Now;
                        serialPort.Write(localDate.ToString("dd/MM/yyyy hh:mm:ss"));
                    }
                    // Cambiamos el label inferior de Estado
                    labelEstado.Text = "Capturando Datos...";
                    //Deshabilito componentes del Form una vez iniciada la grabacion
                    comboBoxPuerto.Enabled = false;
                    textBoxExplorar.Enabled = false;
                    buttonExplorar.Enabled = false;
                    //Marco como iniciado el estado del form
                    estado = "iniciado";
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al detectar el Arduino", Color.FromArgb(255, 89, 89));
            }

        }


        private void buttonFinalizar_Click(object sender, EventArgs e)
        {
            if (estado == "iniciado")
            {
                // Cierro Archivo
                escribirDistancias.cerrarArchivo();
                if (serialPort.IsOpen)
                    serialPort.Close();
                // Limpiamos label de Estado y textBox
                labelEstado.Text = "Presione comenzar para capturar datos";
                labelLectura.Text = "";
                //Vuelvo a habilitar componentes del Form
                textBoxExplorar.Enabled = true;
                buttonExplorar.Enabled = true;
                //Marco como estado del form = finalizada la grabacion
                estado = "finalizado";
                //importar
                ImportarTxtFile importador = new ImportarTxtFile("enter");
                bool resultado = importador.importarArchivoEnProyectoExistente(textBoxExplorar.Text, this.proyecto.Id);
                if (resultado)
                    mostrarMensaje("El archivo se importo correctamente", Color.FromArgb(128, 255, 128));
                else
                    mostrarMensaje("Error al importar el archivo", Color.FromArgb(255, 89, 89));
            }

        }



        private void buttonExplorar_Click(object sender, EventArgs e)
        {
            if (rbExistingFile.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "txt Files|*.txt";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBoxExplorar.Text = ofd.FileName;
                }
            }
        }

        //habilitar o deshabilitar explorar
        private void rbNewFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNewFile.Checked)
            {
                textBoxExplorar.Enabled = true;
                this.tipoArchivo = "N";
                buttonExplorar.Enabled = false;
                buttonExplorar.BackColor = Color.FromArgb(100, 100, 100);
            }
        }

        private void rbExistingFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExistingFile.Checked)
            {
                textBoxExplorar.Enabled = true;
                this.tipoArchivo = "E";
                buttonExplorar.Enabled = true;
                buttonExplorar.BackColor = Color.FromArgb(255, 128, 128);
            }
        }

        //mensajeria
        private void mostrarMensaje(string mensaje, Color color)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.Visible = true;
            pnlMensaje.Visible = true;
            pnlMensaje.BackColor = color;
            if (this.timerActivo)
                timerMensaje.Stop();

            timerMensaje.Start();
            this.timerActivo = true;
        }

        private void timerMensaje_Tick(object sender, EventArgs e)
        {
            pnlMensaje.Visible = false;
            timerMensaje.Stop();
            this.timerActivo = false;
        }
    }
}