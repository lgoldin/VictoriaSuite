using Kairos.Entidades;
using Kairos.Services;
using Kairos.Services.Implementaciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kairos.Forms
{
    public partial class FrmPantallaPrincipal : Form
    {
        private bool timerActivo = false;

        public FrmPantallaPrincipal()
        {
            InitializeComponent();

            using (var db = new EventoContexto())
            {
                db.Database.EnsureCreated();
                cargarLista();
            }
        }

        private void cargarLista()
        {
            try
            {
                var resultado = (from o in new EventoContexto().Origenes select new { o.nombreOrigen, o.Id }).ToList();
                lbProyectosRecientes.ValueMember = "nombreOrigen";
                lbProyectosRecientes.Items.Clear();
                lbProyectosRecientes.Items.AddRange(resultado.ToArray());
            }
            catch
            {
                mostrarMensaje("Error al cargar la lista de proyectos", Color.FromArgb(255, 89, 89));
            }
        }

        private void btnProcesarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbProyectosRecientes.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione un proyecto de la lista", "Falta selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var seleccionado = lbProyectosRecientes.SelectedItem;
                    var a = new { nombreOrigen = "", Id = 0 };
                    a = Cast(a, seleccionado);
                    FrmProcesmientoDatos frm = new FrmProcesmientoDatos(a.nombreOrigen, a.Id);
                    Visible = false;
                    frm.ShowDialog();
                    Visible = true;
                }
            }
            catch
            {
                mostrarMensaje("Error al abrir el proyecto", Color.FromArgb(255, 89, 89));
            }
        }

        private static T Cast<T>(T typeHolder, Object x)
        {
            return (T)x;
        }

        private List<string> valoresNoPermitidos = new List<string>() { " ", ".", ":", "-", "" };
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private string extencion = "";

        private void btnImportarProyecto_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "All supported files (*.txt, *.xls, *xlsx)|*.txt;*.xls;*.xlsx|Text files (*.txt)|*.txt|Excel files (*.xls,*.xlsx)|*.xls; *.xlsx*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pnlImportacion.Visible = true;
                    int indice = openFileDialog1.FileName.LastIndexOf(".");
                    extencion = openFileDialog1.FileName.Substring(indice + 1);
                    switch (extencion)
                    {
                        case "txt":
                            lblTituloImportacion.Text = "Indique el delimitador";
                            pnlImportacionTxt.Visible = true;
                            pnlImportacionExcel.Visible = false;
                            pnlImportacionDB.Visible = false;
                            break;
                        case "xls":
                        case "xlsx":
                            lblTituloImportacion.Text = "Indique la ubicación de los datos";
                            pnlImportacionExcel.Visible = true;
                            pnlImportacionTxt.Visible = false;
                            pnlImportacionDB.Visible = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                mostrarMensaje("Error de importación", Color.FromArgb(255, 89, 89));
            }
        }

        private void txtNombreModificado_TextChanged(object sender, EventArgs e)
        {
            if (txtNombreProyecto.Text != "")
                btnAceptarModificar.Visible = true;
            else
                btnAceptarModificar.Visible = false;
        }

        private void btnCancelarModificacion_Click(object sender, EventArgs e)
        {
            modificacionInvisible();
        }

        private void btnAceptarModificacion_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreProyecto = txtNombreProyecto.Text;
                if (!ProyectoService.existeProyectoPorNombre(nombreProyecto))
                {
                    if (btnAceptarModificar.Text != "Crear")
                    {
                        var seleccionado = lbProyectosRecientes.SelectedItem;
                        var a = new { nombreOrigen = "", Id = 0 };
                        a = Cast(a, seleccionado);
                        ProyectoService.modificarProyecto(a.Id, nombreProyecto);

                        mostrarMensaje("Se modificó el proyecto " + nombreProyecto + " correctamente", Color.FromArgb(128, 255, 128));
                    }
                    else
                    {
                        ProyectoService.nuevoProyecto(nombreProyecto);

                        mostrarMensaje("Se creó el proyecto " + nombreProyecto + " correctamente", Color.FromArgb(128, 255, 128));
                    }

                    btnImportarProyecto.Enabled = false;
                    btnProcesarDatos.Enabled = false;
                    btnArduino.Enabled = false;
                    modificacionInvisible();
                    cargarLista();
                }
                else
                {
                    mostrarMensaje("Ya existe un proyecto con el mismo nombre", Color.FromArgb(255, 255, 0));
                }
            }
            catch
            {
                mostrarMensaje("Error al modificar el proyecto", Color.FromArgb(255, 89, 89));
            }
        }

        private void imgAgregar_Click(object sender, EventArgs e)
        {
            txtNombreProyecto.Clear();
            txtNombreProyecto.Focus();
            btnAceptarModificar.Text = "Crear";
            panelNombreProyecto.Visible = true;
        }

        private void imgBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                var seleccionado = lbProyectosRecientes.SelectedItem;
                if (seleccionado != null)
                {
                    var a = new { nombreOrigen = "", Id = 0 };
                    a = Cast(a, seleccionado);
                    ProyectoService.borrarProyecto(a.Id);

                    mostrarMensaje("Se eliminó el proyecto " + a.nombreOrigen + " correctamente", Color.FromArgb(128, 255, 128));

                    lbProyectosRecientes.ClearSelected();

                    pnlImportacion.Visible = false;
                    modificacionInvisible();
                    cargarLista();
                }
            }
            catch
            {
                mostrarMensaje("Error al eliminar el proyecto", Color.FromArgb(255, 89, 89));
            }
        }

        private void imgEditar_Click(object sender, EventArgs e)
        {
            try
            {
                var seleccionado = lbProyectosRecientes.SelectedItem;

                if (seleccionado != null)
                {
                    var a = new { nombreOrigen = "", Id = 0 };
                    a = Cast(a, seleccionado);
                    txtNombreProyecto.Text = a.nombreOrigen;

                    txtNombreProyecto.Focus();
                    btnAceptarModificar.Text = "Modificar";
                    btnAceptarModificar.Visible = true;
                    panelNombreProyecto.Visible = true;
                }
            }
            catch
            {
                mostrarMensaje("Error de edicion del proyecto", Color.FromArgb(255, 89, 89));
            }
        }

        private void modificacionInvisible()
        {
            panelNombreProyecto.Visible = false;
            btnAceptarModificar.Visible = false;
        }

        private void timerMensaje_Tick(object sender, EventArgs e)
        {
            panelNotificaciones.Visible = false;
            timerMensaje.Stop();
        }

        private void imgAgregar_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BackColor = Color.LavenderBlush;
            pb.BorderStyle = BorderStyle.FixedSingle;
        }

        private void imgAgregar_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BackColor = Color.FromArgb(255, 192, 192);
            pb.BorderStyle = BorderStyle.None;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnArduino_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = Cast(new { nombreOrigen = "", Id = 0 }, lbProyectosRecientes.SelectedItem);
                FrmCapturaDatosArduino frm = new FrmCapturaDatosArduino(new Origen { nombreOrigen = selectedItem.nombreOrigen, Id = selectedItem.Id });
                Visible = false;
                frm.ShowDialog();
                Visible = true;
            }
            catch
            {
                mostrarMensaje("Ërror del modulo de arduino", Color.FromArgb(255, 89, 89));
            }
        }

        private void rbOtro_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOtro.Checked == true)
            {
                btnAceptarImportacion.BackColor = Color.LightGray;
                btnAceptarImportacion.Enabled = false;
                txtOtro.BackColor = Color.White;
                txtOtro.Enabled = true;
            }
            else
            {
                btnAceptarImportacion.BackColor = Color.FromArgb(240, 100, 100);
                btnAceptarImportacion.Enabled = true;
                txtOtro.BackColor = Color.LightGray;
                txtOtro.Enabled = false;
            }
        }

        private string BuscarSeleccionado()
        {
            string resultado = "";
            if (rbComa.Checked == true)
                resultado = ",";
            else if (rbEnter.Checked == true)
                resultado = "enter";
            else if (rbPipe.Checked == true)
                resultado = "|";
            else if (rbTab.Checked == true)
                resultado = "\t";
            else if (rbOtro.Checked == true)
                resultado = txtOtro.Text;
            return resultado;
        }

        private bool EsValorPermitido(string valor)
        {
            if (this.valoresNoPermitidos.Contains(valor))
                return false;
            else
                return true;
        }

        private void btnAceptarImportacion_Click(object sender, EventArgs e)
        {
            IImportarService importador = null;
            try
            {

                switch (extencion)
                {
                    case "txt":
                        string delimitador = BuscarSeleccionado();
                        importador = new ImportarTxtFile(delimitador);
                        break;
                    case "xls":
                    case "xlsx":
                        int hoja = Convert.ToInt32(nudHoja.Value);
                        int columna = Convert.ToInt32(nudColumna.Value);
                        int filaIncial = Convert.ToInt32(nudFilaInicial.Value);
                        importador = new ImportarExcelFile(hoja, columna, filaIncial);
                        break;
                    default:
                        break;
                }
                pnlImportacionTxt.Visible = false;
                pnlImportacionExcel.Visible = false;
                if (importador != null)
                {
                    bool resultado = false;
                    string fileName = openFileDialog1.FileName;
                    var seleccionado = lbProyectosRecientes.SelectedItem;
                    var a = new { nombreOrigen = "", Id = 0 };
                    a = Cast(a, seleccionado);
                    resultado = importador.importarArchivoEnProyectoExistente(fileName, a.Id);
                    if (resultado)
                    {
                        mostrarMensaje("El archivo se importó correctamente", Color.FromArgb(128, 255, 128));
                        cargarLista();
                        lbProyectosRecientes.SelectedItem = seleccionado;
                    }
                    else
                    {
                        mostrarMensaje("No se puede abrir el archivo", Color.FromArgb(255, 89, 89));
                    }
                }
                pnlImportacion.Visible = false;
            }
            catch
            {
                mostrarMensaje("Error al importar", Color.FromArgb(255, 89, 89));
            }
        }

        private void btncancelarImportacion_Click(object sender, EventArgs e)
        {
            pnlImportacion.Visible = false;
        }

        private void txtOtro_TextChanged(object sender, EventArgs e)
        {
            bool mostrar = false;
            if (EsValorPermitido(txtOtro.Text))
                mostrar = true;
            EsconderOMostrarBotonAceptarImportacion(mostrar);
        }

        private void EsconderOMostrarBotonAceptarImportacion(bool mostrar)
        {
            if (mostrar)
            {
                btnAceptarImportacion.BackColor = Color.FromArgb(255, 128, 128);
                btnAceptarImportacion.Enabled = true;
            }
            else
            {
                btnAceptarImportacion.BackColor = Color.LightGray;
                btnAceptarImportacion.Enabled = false;
            }
        }

        private void btnProyectos_Click(object sender, EventArgs e)
        {
            pnlImportacion.Visible = false;
            pnlProyectos.Visible = true;
        }

        private void lbProyectosRecientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbProyectosRecientes.SelectedItem != null)
            {
                btnImportarProyecto.Enabled = true;
                btnProcesarDatos.Enabled = true;
                btnArduino.Enabled = true;
                btnImportarDesdeDB.Enabled = true;

                if (btnAceptarModificar.Text != "Crear")
                {
                    var selectedItem = Cast(new { nombreOrigen = "", Id = 0 }, lbProyectosRecientes.SelectedItem);
                    txtNombreProyecto.Text = selectedItem.nombreOrigen;
                }
            }
            else
            {
                btnImportarProyecto.Enabled = false;
                btnProcesarDatos.Enabled = false;
                btnArduino.Enabled = false;
                btnImportarDesdeDB.Enabled = false;
            }

        }
        private void mostrarMensaje(string mensaje, Color color)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.Visible = true;
            panelNotificaciones.Visible = true;
            lblMensaje.BackColor = color;
            if (this.timerActivo)
                timerMensaje.Stop();

            timerMensaje.Start();
            this.timerActivo = true;
        }

        private void btnImportarDesdeDB_Click(object sender, EventArgs e)
        {
            pnlImportacionTxt.Visible = false;
            pnlImportacionExcel.Visible = false;
            panelNombreProyecto.Visible = false;
            pnlImportacionDB.Visible = true;
            pnlImportacion.Visible = false;

        }

        private void btnAceptarImportacionDB_Click(object sender, EventArgs e)
        {
            try
            {
                string server = txtNombreServidor.Text;
                string database = txtNombreDB.Text;
                string nombreTabla = txtNombreTabla.Text;
                string nombreColumna = txtNombreColumna.Text;
                string user = txtUsuario.Text;
                string password = txtContrasenia.Text;

                string connString = $"Data Source = {server}; Initial Catalog = {database};";
                if (chUsuarioYContrasenia.Checked)
                {
                    connString = connString + $" User Id = {user}; Password = {password};";
                }

                IImportarDesdeDB importador = new ImportarDesdeDBImpl();

                List<DateTime> dates = importador.ObtenerColumnaDesdeDB(connString, database, nombreTabla, nombreColumna);
                if (dates != null)
                {
                    EventoService.agregarTodos(dates.Select(x =>
                    {
                        Evento aux = new Evento();
                        aux.activo = true;
                        aux.fecha = x;
                        var seleccionado = lbProyectosRecientes.SelectedItem;
                        var proyecto = new { nombreOrigen = "", Id = 0 };
                        proyecto = Cast(proyecto, seleccionado);
                        aux.idOrigen = proyecto.Id;
                        return aux;
                    }).ToList());
                    mostrarMensaje("Se importaron los datos correctamente", Color.FromArgb(128, 255, 128));
                    pnlImportacionDB.Visible = false;
                    LimpiarTxtImportacionDB();
                }
            }
            catch
            {
                mostrarMensaje("No se ha podido extraer la informacion solicitada. Por favor revise los datos", Color.FromArgb(255, 89, 89));
            }
        }

        private void chUsuarioYContrasenia_CheckedChanged(object sender, EventArgs e)
        {
            if (chUsuarioYContrasenia.Checked)
            {
                txtUsuario.Enabled = true;
                txtUsuario.BackColor = Color.FromName("Window");
                txtContrasenia.Enabled = true;
                txtContrasenia.BackColor = Color.FromName("Window");
                CheckearParametrosImportacionDB();
            }
            else if (!chUsuarioYContrasenia.Checked)
            {
                txtUsuario.Enabled = false;
                txtUsuario.BackColor = Color.FromName("LightGray");
                txtContrasenia.Enabled = false;
                txtContrasenia.BackColor = Color.FromName("LightGray");
            }
        }


        private void CheckearParametrosImportacionDB()
        {
            if (!string.IsNullOrEmpty(txtNombreServidor.Text) && !string.IsNullOrEmpty(txtNombreDB.Text)
                && !string.IsNullOrEmpty(txtNombreTabla.Text) && !string.IsNullOrEmpty(txtNombreColumna.Text)
                && ((chUsuarioYContrasenia.Checked && !string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtContrasenia.Text))
                            || !chUsuarioYContrasenia.Checked))
            {
                btnAceptarImportacionDB.Enabled = true;
                btnAceptarImportacionDB.BackColor = Color.FromArgb(240, 100, 100);
            }
            else
            {
                btnAceptarImportacionDB.Enabled = false;
                btnAceptarImportacionDB.BackColor = Color.FromName("LightGray");
            }
        }

        private void txtNombreServidor_TextChanged(object sender, EventArgs e)
        {
            CheckearParametrosImportacionDB();
        }

        private void txtNombreDB_TextChanged(object sender, EventArgs e)
        {
            CheckearParametrosImportacionDB();
        }

        private void txtNombreTabla_TextChanged(object sender, EventArgs e)
        {
            CheckearParametrosImportacionDB();
        }

        private void txtNombreColumna_TextChanged(object sender, EventArgs e)
        {
            CheckearParametrosImportacionDB();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            CheckearParametrosImportacionDB();
        }

        private void txtContrasenia_TextChanged(object sender, EventArgs e)
        {
            CheckearParametrosImportacionDB();
        }

        private void btnCancelarImportacionDB_Click(object sender, EventArgs e)
        {
            LimpiarTxtImportacionDB();
        }

        private void LimpiarTxtImportacionDB()
        {
            txtNombreServidor.Clear();
            txtNombreDB.Clear();
            txtNombreTabla.Clear();
            txtNombreColumna.Clear();
            txtUsuario.Clear();
            txtContrasenia.Clear();
            pnlImportacionDB.Visible = false;
            chUsuarioYContrasenia.Checked = false;
        }
    }
}
