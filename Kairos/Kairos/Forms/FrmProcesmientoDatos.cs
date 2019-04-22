using Kairos.Entidades;
using Kairos.Filtros;
using Kairos.FuncionesDensidad;
using Kairos.Modelo;
using Kairos.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kairos.Forms
{
    public partial class FrmProcesmientoDatos : Form
    {
        private TipoAccionProcesamiento tipoAccion;
        private Origen proyecto = new Origen();
        private List<Evento> eventos = null;
        private List<Evento> eventosSeleccionados = new List<Evento>();
        private bool timerActivo = false;
        private List<Filtro> filtros = null;
        private readonly INuevoFiltro filtrador = new FiltroImpl();
        private List<double> intervalos = new List<double>();
        private List<Double> intervalosParciales = new List<double>();

        public FrmProcesmientoDatos(string nombreProyecto, int idProyecto)
        {
            InitializeComponent();
            this.proyecto.nombreOrigen = nombreProyecto;
            this.proyecto.Id = idProyecto;
            lblTituloProyecto.Text = nombreProyecto;
            filtros = new List<Filtro>();
            setupFiltrosCheckboxList();
            cargarFiltros();
            cambiarFiltrosVistaFecha(0);
            cargarEventos();
            actualizarEstadisticas();
        }

        private void setupFiltrosCheckboxList()
        {
            ((ListBox)this.chlFiltros).DisplayMember = "Name";
            ((ListBox)this.chlFiltros).ValueMember = "IsChecked";
            for (int i = 0; i < this.filtros.Count; i++)
            {
                if (!chlFiltros.Items.Contains(filtros[i]))
                {
                    chlFiltros.Items.Add(filtros[i]);
                    Filtro obj = (Filtro)chlFiltros.Items[i];
                    chlFiltros.SetItemChecked(i, obj.IsChecked);
                }
            }
        }

        private void cargarEventos()
        {
            try
            {
                eventos = EventoService.cargarEventos(this.proyecto.Id);
                dgwEventos.DataSource = eventos;
                dgwEventos.Columns[1].Width = 235;
                dgwEventos.Columns[1].DefaultCellStyle.Format = "dd'/'MM'/'yyyy HH:mm:ss";
                dgwEventos.Columns[0].Visible = false;
                dgwEventos.Columns[2].Visible = false;
                dgwEventos.Columns[3].Visible = false;
                dgwEventos.Columns[4].Visible = false;

                if (eventos != null && tipoAccion == TipoAccionProcesamiento.FILTRAR)
                    filtrar();
            }
            catch
            {
                mostrarMensaje("Error al cargar los eventos", Color.FromArgb(255, 89, 89));
            }
        }

        private void btnAgregarRegistro_Click(object sender, EventArgs e)
        {
            tipoAccion = TipoAccionProcesamiento.AGREGAR_REGISTRO;
            modificarLayout(tipoAccion);
            botonSeleccionado(btnAgregarRegistro);
        }

        private void btnModificarRegistro_Click(object sender, EventArgs e)
        {
            if (eventos.Count() > 0)
            {
                tipoAccion = TipoAccionProcesamiento.MODIFICAR_REGISTRO;
                modificarLayout(tipoAccion);
                botonSeleccionado(btnModificarRegistro);
                dtp1.Value = eventosSeleccionados[0].fecha;
                dtp2.Value = eventosSeleccionados[0].fecha;
            }
            else
            {
                mostrarMensaje("No hay ningún evento", Color.FromArgb(255, 255, 0));
            }
        }

        private void btnBorrarSeleccionados_Click(object sender, EventArgs e)
        {
            try
            {
                if (eventos.Count() > 0)
                {
                    try
                    {
                        tipoAccion = TipoAccionProcesamiento.BORRAR_SELECCIONADOS;
                        modificarLayout(tipoAccion);
                        botonSeleccionado(btnBorrarSeleccionados);
                        int cant = eventosSeleccionados.Count;

                        EventoService.borrar(eventosSeleccionados);
                        cargarEventos();

                        mostrarMensaje("Registros eliminados correctamente", Color.FromArgb(128, 255, 128));
                        actualizarEstadisticas();
                    }
                    catch (Exception)
                    {
                        mostrarMensaje("Error al eliminar registros", Color.FromArgb(255, 89, 89));
                    }

                }
                else
                {
                    mostrarMensaje("No hay ningún evento", Color.FromArgb(255, 255, 0));
                }
            }
            catch
            {
                mostrarMensaje("Error al eliminar los registros", Color.FromArgb(255, 89, 89));
            }

        }

        private void btnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            tipoAccion = TipoAccionProcesamiento.SELECCIONAR_TODOS;
            modificarLayout(tipoAccion);
            dgwEventos.SelectAll();
            botonSeleccionado(btnSeleccionarTodos);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (eventos.Count() > 0)
            {
                tipoAccion = TipoAccionProcesamiento.FILTRAR;
                modificarLayout(tipoAccion);
                botonSeleccionado(btnFiltrar);
            }
            else
            {
                mostrarMensaje("No hay ningún evento", Color.FromArgb(255, 255, 0));
            }
        }

        private void modificarLayout(TipoAccionProcesamiento tipoAccion)
        {
            hacerVisible(tipoAccion);
            switch (tipoAccion)
            {
                case TipoAccionProcesamiento.AGREGAR_REGISTRO:
                    lblTituloAccion.Text = "Agregar Registro";
                    lblAccion1.Text = "Fecha";
                    dtp1.Format = DateTimePickerFormat.Short;
                    lblAccion2.Text = "Hora";
                    dtp2.Format = DateTimePickerFormat.Custom;
                    dtp2.CustomFormat = "HH:mm:ss";
                    rbAgregarPorFechaYHora.Checked = true;
                    rbAgregarPorFechaYHora.Visible = true;
                    rbAgregarPorIntervalo.Checked = false;
                    rbAgregarPorIntervalo.Visible = true;
                    nudAgregarPorIntervalo.Visible = false;
                    cbAgregarPorIntervalo.Visible = false;
                    cambiarFiltrosVistaFecha(0);
                    break;
                case TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    lblTituloAccion.Text = "Modificar Registro";
                    lblAccion1.Text = "Fecha";
                    dtp1.Format = DateTimePickerFormat.Short;
                    lblAccion2.Text = "Hora";
                    dtp2.Format = DateTimePickerFormat.Custom;
                    dtp2.CustomFormat = "HH:mm:ss";
                    rbAgregarPorFechaYHora.Visible = false;
                    rbAgregarPorIntervalo.Visible = false;
                    nudAgregarPorIntervalo.Visible = false;
                    cbAgregarPorIntervalo.Visible = false;
                    cambiarFiltrosVistaFecha(0);
                    break;
                case TipoAccionProcesamiento.FILTRAR:
                    lblTituloAccion.Text = "Filtrar";

                    dtp1.Format = DateTimePickerFormat.Short;
                    if (rbFecha.Checked)
                    {
                        lblAccion1.Text = "Fecha";
                        txtIntervalo.Visible = false;
                        txtIntervalo2.Visible = false;
                        cambiarFiltrosVistaFecha(0);
                    }
                    else
                    {
                        lblAccion1.Text = "Intervalo";
                        dtp1.Visible = false;
                        txtIntervalo.Visible = true;
                    }
                    lblAccion2.Text = "Hora";
                    dtp2.Format = DateTimePickerFormat.Custom;
                    dtp2.CustomFormat = "HH:mm:ss";
                    rbAgregarPorFechaYHora.Visible = false;
                    rbAgregarPorIntervalo.Visible = false;
                    nudAgregarPorIntervalo.Visible = false;
                    cbAgregarPorIntervalo.Visible = false;


                    break;
                default:
                    break;
            }
        }

        private void hacerVisible(TipoAccionProcesamiento tipoAccion)
        {
            switch (tipoAccion)
            {
                case TipoAccionProcesamiento.AGREGAR_REGISTRO:
                case TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    pnlModificable.Visible = true;
                    foreach (Control control in pnlModificable.Controls)
                    {
                        control.Visible = true;
                    }
                    lblTipoFiltro.Visible = false;
                    cmbTipoFiltro.Visible = false;
                    btnLimpiar.Visible = false;
                    txtIntervalo.Visible = false;
                    txtIntervalo2.Visible = false;
                    break;
                case TipoAccionProcesamiento.BORRAR_SELECCIONADOS:
                case TipoAccionProcesamiento.SELECCIONAR_TODOS:
                    pnlModificable.Visible = false;
                    break;
                case TipoAccionProcesamiento.FILTRAR:
                    pnlModificable.Visible = true;
                    foreach (Control control in pnlModificable.Controls)
                    {
                        control.Visible = true;
                    }
                    if (rbFecha.Checked)
                    {
                        lblTipoFiltro.Visible = true;
                        cmbTipoFiltro.Visible = true;
                        btnLimpiar.Visible = false;

                        cargarFiltros();
                    }
                    else
                    {
                        cmbTipoFiltro.DataSource = null;
                        cmbTipoFiltro.Items.Clear();
                        List<ComboItem> tipos = new List<ComboItem> { new ComboItem(0, "Intervalo menor a"), new ComboItem(1, "Intervalo mayor a"), new ComboItem(2, "Intervalo entre") };
                        cmbTipoFiltro.DisplayMember = "Display";
                        cmbTipoFiltro.ValueMember = "Value";
                        cmbTipoFiltro.DataSource = tipos;

                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Text = "Intervalo";
                        dtp1.Visible = false;
                        lblAccion2.Visible = false;
                        dtp2.Visible = false;
                        txtIntervalo.Visible = true;
                    }
                    break;
                default:
                    break;
            }
        }

        private void botonSeleccionado(Control boton)
        {
            btnBorrarSeleccionados.BackColor = Color.FromArgb(187, 0, 4);
            foreach (Control control in pnlAcciones.Controls)
            {
                if (control != btnBorrarSeleccionados)
                {
                    control.BackColor = Color.Black;
                    control.ForeColor = Color.White;
                }


            }

            if (boton != btnBorrarSeleccionados)
            {
                boton.BackColor = Color.FromArgb(38, 38, 38);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            Segmentacion segmentacion=Segmentacion.SEGUNDO;
            switch (tipoAccion)
            {
                case TipoAccionProcesamiento.AGREGAR_REGISTRO:
                    if (rbAgregarPorFechaYHora.Checked)
                    {
                        fecha = new DateTime(dtp1.Value.Year, dtp1.Value.Month, dtp1.Value.Day, dtp2.Value.Hour, dtp2.Value.Minute, dtp2.Value.Second);
                        EventoService.nuevo(fecha, this.proyecto.Id);
                    }
                    else if (rbAgregarPorIntervalo.Checked)
                    {

                        switch (cbAgregarPorIntervalo.SelectedItem)
                        {
                            case "Segundos":
                                segmentacion = Segmentacion.SEGUNDO;
                                break;
                            case "Minutos":
                                segmentacion = Segmentacion.MINUTO;
                                break;
                            case "Horas":
                                segmentacion = Segmentacion.HORA;
                                break;
                            case "Dias":
                                segmentacion = Segmentacion.DIA;
                                break;
                            default:
                                break;
                        }
                        EventoService.nuevoPorIntervalo(Convert.ToInt32(nudAgregarPorIntervalo.Value),segmentacion,this.proyecto.Id);
                    }
                    mostrarMensaje("Registro agregado correctamente", Color.FromArgb(128, 255, 128));
                    cargarEventos();
                    actualizarEstadisticas();

                    break;
                case TipoAccionProcesamiento.MODIFICAR_REGISTRO:
                    fecha = new DateTime(dtp1.Value.Year, dtp1.Value.Month, dtp1.Value.Day, dtp2.Value.Hour, dtp2.Value.Minute, dtp2.Value.Second);
                    if (eventosSeleccionados.Count == 1)
                    {
                        EventoService.modificar(eventosSeleccionados[0].Id, fecha);
                        cargarEventos();
                        mostrarMensaje("Registro modificado correctamente", Color.FromArgb(128, 255, 128));
                        actualizarEstadisticas();
                    }
                    else
                        mostrarMensaje("Seleccione solo un registro", Color.FromArgb(255, 89, 89));
                    break;
                case TipoAccionProcesamiento.FILTRAR:
                    agregarFiltro();
                    filtrar();
                    mostrarMensaje("Filtro aplicado correctamente", Color.FromArgb(128, 255, 128));
                    actualizarEstadisticas();
                    break;
                default:
                    break;
            }
        }

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

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgwEventos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwEventos.SelectedRows.Count > 0)
            {
                eventosSeleccionados = new List<Evento>();
                if (eventos != null)
                {
                    foreach (DataGridViewRow item in dgwEventos.SelectedRows)
                    {
                        eventosSeleccionados.Add(eventos[item.Index]);
                    }
                    if (tipoAccion == TipoAccionProcesamiento.MODIFICAR_REGISTRO && eventosSeleccionados.Count == 1)
                    {
                        dtp1.Value = eventosSeleccionados[0].fecha;
                        dtp2.Value = eventosSeleccionados[0].fecha;
                    }
                }

            }
        }

        //filtros

        private class ComboItem
        {
            public int Value { get; set; }
            public string Display { get; set; }
            public ComboItem(int value, string display) { this.Value = value; this.Display = display; }
        }

        private void cargarFiltros()
        {
            List<ComboItem> tipos = new List<ComboItem> { new ComboItem(0, "Fecha menor a"), new ComboItem(1, "Fecha mayor a"), new ComboItem(2, "Fecha entre"), new ComboItem(3, "Hora menor a"), new ComboItem(4, "Hora mayor a"), new ComboItem(5, "Hora entre") };
            cmbTipoFiltro.DisplayMember = "Display";
            cmbTipoFiltro.ValueMember = "Value";
            cmbTipoFiltro.DataSource = tipos;
        }

        private void cmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoFiltro.SelectedValue != null)
            {
                if (rbFecha.Checked)
                    cambiarFiltrosVistaFecha(Convert.ToInt32(cmbTipoFiltro.SelectedValue));
                else if (rbIntervalos.Checked)
                    cambiarFiltrosVistaIntervalos(Convert.ToInt32(cmbTipoFiltro.SelectedValue));
            }
        }

        private void cambiarFiltrosVistaFecha(int valorSeleccionado)
        {
            if (tipoAccion == TipoAccionProcesamiento.FILTRAR)
            {
                if (valorSeleccionado > 2)
                {
                    dtp1.Format = DateTimePickerFormat.Custom;
                    dtp1.CustomFormat = "HH:mm:ss";
                    dtp1.ShowUpDown = true;
                    dtp2.Format = DateTimePickerFormat.Custom;
                    dtp2.CustomFormat = "HH:mm:ss";
                    dtp2.ShowUpDown = true;
                }
                else
                {
                    dtp1.Format = DateTimePickerFormat.Short;
                    dtp1.ShowUpDown = false;
                    dtp2.Format = DateTimePickerFormat.Short;
                    dtp2.ShowUpDown = false;
                }
                switch (valorSeleccionado)
                {
                    case 0:
                        btnAceptar.Visible = true;
                        lblTituloAccion.Visible = true;
                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Visible = true;
                        lblAccion1.Text = "Fecha";
                        dtp1.Visible = true;
                        lblAccion2.Visible = false;
                        dtp2.Visible = false;
                        btnLimpiar.Visible = true;
                        break;
                    case 1:
                        btnAceptar.Visible = true;
                        lblTituloAccion.Visible = true;
                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Text = "Fecha";
                        dtp1.Visible = true;
                        lblAccion2.Visible = false;
                        dtp2.Visible = false;
                        btnLimpiar.Visible = true;
                        break;
                    case 2:
                        btnAceptar.Visible = true;
                        lblTituloAccion.Visible = true;
                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Text = "Fecha desde";
                        dtp1.Visible = true;
                        lblAccion2.Visible = true;
                        lblAccion2.Text = "Fecha hasta";
                        dtp2.Visible = true;
                        btnLimpiar.Visible = true;
                        break;
                    case 3:
                        btnAceptar.Visible = true;
                        lblTituloAccion.Visible = true;
                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Text = "Hora";
                        dtp1.Visible = true;
                        lblAccion2.Visible = false;
                        dtp2.Visible = false;
                        btnLimpiar.Visible = true;
                        break;
                    case 4:
                        btnAceptar.Visible = true;
                        lblTituloAccion.Visible = true;
                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Text = "Hora";
                        dtp1.Visible = true;
                        lblAccion2.Visible = false;
                        dtp2.Visible = false;
                        btnLimpiar.Visible = true;
                        break;
                    case 5:
                        btnAceptar.Visible = true;
                        lblTituloAccion.Visible = true;
                        lblTituloAccion.Text = "Filtrar";
                        lblAccion1.Text = "Hora desde";
                        dtp1.Visible = true;
                        lblAccion2.Visible = true;
                        lblAccion2.Text = "Hora hasta";
                        dtp2.Visible = true;
                        btnLimpiar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void cambiarFiltrosVistaIntervalos(int valorSeleccionado)
        {
            btnAceptar.Visible = true;
            lblTituloAccion.Visible = true;
            lblTituloAccion.Text = "Filtrar";

            switch (valorSeleccionado)
            {
                case 0: //intervalo menor a
                case 1: //intervalo mayor a
                    lblAccion1.Visible = true;
                    lblAccion1.Text = "Intervalo";
                    txtIntervalo.Visible = true;
                    txtIntervalo2.Visible = false;
                    btnLimpiar.Visible = true;
                    lblAccion2.Visible = false;
                    break;
                case 2://intervalo entre
                    lblAccion1.Visible = true;
                    lblAccion1.Text = "Intervalo desde";
                    txtIntervalo.Visible = true;
                    lblAccion2.Visible = true;
                    lblAccion2.Text = "Intervalo hasta";
                    txtIntervalo2.Visible = true;
                    btnLimpiar.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void agregarFiltro()
        {
            int selectedValue = Convert.ToInt32(cmbTipoFiltro.SelectedValue);
            Filtro auxFiltro = null;
            DateTime fecha = DateTime.Now;
            DateTime fecha2 = DateTime.Now;
            fecha = dtp1.Value;
            fecha2 = dtp2.Value;
            double intervalo = -1;
            double intervalo2 = -1;
            if (rbFecha.Checked)
            {
                switch (selectedValue)
                {
                    case 0:
                        auxFiltro = new Filtro(TipoFiltro.FECHA_MENOR, fecha);
                        break;
                    case 1:
                        auxFiltro = new Filtro(TipoFiltro.FECHA_MAYOR, fecha);
                        break;
                    case 2:
                        auxFiltro = new Filtro(TipoFiltro.FECHA_ENTRE, fecha, fecha2);
                        break;
                    case 3:
                        auxFiltro = new Filtro(TipoFiltro.HORA_MENOR, fecha);
                        break;
                    case 4:
                        auxFiltro = new Filtro(TipoFiltro.HORA_MAYOR, fecha);
                        break;
                    case 5:
                        auxFiltro = new Filtro(TipoFiltro.HORA_ENTRE, fecha, fecha2);
                        break;
                    default:
                        auxFiltro = null;
                        break;
                }
            }
            else if (rbIntervalos.Checked)
            {
                intervalo = Convert.ToDouble(txtIntervalo.Text);
                intervalo2 = Convert.ToDouble(txtIntervalo2.Text);

                switch (selectedValue)
                {
                    case 0:
                        auxFiltro = new Filtro(TipoFiltro.INTERVALO_MENOR, intervalo);
                        break;
                    case 1:
                        auxFiltro = new Filtro(TipoFiltro.INTERVALO_MAYOR, intervalo);
                        break;
                    case 2:
                        auxFiltro = new Filtro(TipoFiltro.INTERVALO_ENTRE, intervalo, intervalo2);
                        break;
                    default:
                        break;
                }
            }
            filtros.Add(auxFiltro);
            setupFiltrosCheckboxList();
        }

        private void filtrar()
        {
            if (rbFecha.Checked)
            {
                List<Evento> filtrado = filtrador.FiltrarFechas(this.proyecto.Id, filtros);
                if (filtrado != null)
                {
                    eventos = filtrado;
                    dgwEventos.DataSource = filtrado;
                    dgwEventos.Columns[1].Width = 235;
                    dgwEventos.Columns[0].Visible = false;
                    dgwEventos.Columns[1].DefaultCellStyle.Format = "dd'/'MM'/'yyyy HH:mm:ss";
                }
            }
            else if (rbIntervalos.Checked)
            {
                List<double> filtrado = filtrador.FiltrarIntervalos(this.intervalosParciales, cmbTipoFiltro.SelectedIndex, Convert.ToInt32(txtIntervalo.Text), Convert.ToInt32(txtIntervalo2.Text));
                intervalosParciales = filtrado; //para filtros acumulativos

                //leno dataGridView con los intervalos
                DataTable tabla = new DataTable();
                tabla.Columns.Add("Intervalos");
                foreach (var item in filtrado)
                {
                    DataRow row = tabla.NewRow();
                    row["Intervalos"] = item;
                    tabla.Rows.Add(row);
                }
                dgwEventos.DataSource = tabla;
                dgwEventos.Columns[0].Visible = true;
                dgwEventos.Columns[0].Width = 235;

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbFecha.Checked)
                {
                    cmbTipoFiltro.SelectedIndex = 0;
                    filtros.Clear();
                    chlFiltros.Items.Clear();
                    filtrar();
                }
                else if (rbIntervalos.Checked)
                {
                    quitarFiltrosIntervalos();

                    cmbTipoFiltro.SelectedIndex = 0;

                    //leno dataGridView con los intervalos
                    DataTable tabla = new DataTable();
                    tabla.Columns.Add("Intervalos");
                    foreach (var item in intervalos)
                    {
                        DataRow row = tabla.NewRow();
                        row["Intervalos"] = item;
                        tabla.Rows.Add(row);
                    }
                    dgwEventos.DataSource = tabla;
                    dgwEventos.Columns[0].Visible = true;
                    dgwEventos.Columns[0].Width = 235;

                    intervalosParciales = intervalos;
                }
                actualizarEstadisticas();
            }
            catch
            {
                mostrarMensaje("Error al limpiar los filtros", Color.FromArgb(255, 89, 89));
            }
        }

        public void quitarFiltrosIntervalos()
        {
            List<Filtro> filtrosAux = new List<Filtro>();
            foreach (Filtro item in chlFiltros.Items)
            {
                if (item.Name.Contains("Intervalo"))
                    this.filtros.Remove(item);

            }
            chlFiltros.Items.Clear();
            setupFiltrosCheckboxList();

        }

        private bool _updatingCheckList = false;

        private void chlFiltros_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingCheckList) return;
            _updatingCheckList = true;
            if (e.CurrentValue != e.NewValue)
            {
                CheckState state = e.NewValue;
                if (state == CheckState.Checked)
                    this.filtros[e.Index].IsChecked = true;
                else
                    this.filtros[e.Index].IsChecked = false;
                filtrar();
            }
            _updatingCheckList = false;
        }

        private void btnCalcularFDP_Click(object sender, EventArgs e)
        {
            if (eventos.Count() >= 15)
            {
                try
                {
                    MetodologiaAjuste metodologia = MetodologiaAjuste.EVENTO_A_EVENTO;
                    Segmentacion segmentacion = Segmentacion.SEGUNDO;
                    int flagIntervalos = 0;

                    if (rbFecha.Checked)
                    {
                        metodologia = rbEventoAEvento.Checked ? MetodologiaAjuste.EVENTO_A_EVENTO : MetodologiaAjuste.DT_CONSTANTE;
                        segmentacion = rbDia.Checked ? Segmentacion.DIA : (rbHora.Checked ? Segmentacion.HORA : (rbMinuto.Checked ? Segmentacion.MINUTO : Segmentacion.SEGUNDO));
                        FrmAjusteFunciones frm = new FrmAjusteFunciones(metodologia, segmentacion, eventos, flagIntervalos, this.proyecto);
                        this.Visible = false;
                        frm.ShowDialog();
                        this.Visible = true;
                    }
                    else if (rbIntervalos.Checked)
                    {
                        metodologia = MetodologiaAjuste.EVENTO_A_EVENTO;
                        flagIntervalos = 1;
                        FrmAjusteFunciones frm = new FrmAjusteFunciones(metodologia, segmentacion, intervalosParciales, flagIntervalos, this.proyecto);
                        this.Visible = false;
                        frm.ShowDialog();
                        this.Visible = true;
                    }
                }
                catch
                {
                    mostrarMensaje("Error al calcular funciones", Color.FromArgb(255, 89, 89));
                }
            }
            else
            {
                mostrarMensaje("Debe haber al menos 15 eventos en el proyecto", Color.FromArgb(255, 255, 0));
            }
        }

        private void rbDtConstante_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDtConstante.Checked)
                pnlSegmentacion.Visible = true;
            else
                pnlSegmentacion.Visible = false;
        }

        private void rbIntervalos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbIntervalos.Checked)
                {
                    tipoAccion = TipoAccionProcesamiento.FILTRAR;
                    modificarLayout(tipoAccion);
                    botonSeleccionado(btnFiltrar);
                    rbDtConstante.Visible = false;
                    if (rbDtConstante.Checked)
                    {
                        rbEventoAEvento.Checked = true;
                        pnlSegmentacion.Visible = false;
                    }

                    if (eventos != null && eventos.Count > 0)
                    {
                        //calculo intervalos
                        var eventosOrdenados = eventos.OrderBy(x => x.fecha);
                        List<double> lista = new List<double>();
                        lista = eventosOrdenados.Select(x => x.fecha.TimeOfDay)
                        .Zip(eventosOrdenados.Select(x => x.fecha)
                        .Skip(1), (x, y) => y - x)
                        .Select(x => Math.Abs(x.TimeOfDay.TotalSeconds))
                        .ToList();

                        this.intervalos = lista; //para limpir los filtros y volver al original
                        this.intervalosParciales = lista; //para los filtros consecutivos

                        //leno dataGridView con los intervalos
                        DataTable tabla = new DataTable();
                        tabla.Columns.Add("Intervalos");
                        foreach (var item in lista)
                        {
                            DataRow row = tabla.NewRow();
                            row["Intervalos"] = item;
                            tabla.Rows.Add(row);
                        }
                        dgwEventos.DataSource = tabla;
                        dgwEventos.Columns[0].Visible = true;
                        dgwEventos.Columns[0].Width = 235;

                        //deshabilito funciones
                        btnAgregarRegistro.Enabled = false;
                        btnModificarRegistro.Enabled = false;
                        btnBorrarSeleccionados.Enabled = false;
                        btnBorrarSeleccionados.BackColor = Color.Black;
                        btnSeleccionarTodos.Enabled = false;
                        chlFiltros.Enabled = false;

                        modificarLayout(TipoAccionProcesamiento.FILTRAR);

                    }
                    actualizarEstadisticas();

                }
            }
            catch
            {
                mostrarMensaje("Error al calcular los intervalos", Color.FromArgb(255, 89, 89));
            }
        }
        private void rbFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFecha.Checked)
            {
                quitarFiltrosIntervalos();
                cargarEventos();
                btnAgregarRegistro.Enabled = true;
                btnModificarRegistro.Enabled = true;
                btnBorrarSeleccionados.Enabled = true;
                btnBorrarSeleccionados.BackColor = Color.FromArgb(134, 0, 3);
                btnSeleccionarTodos.Enabled = true;
                chlFiltros.Enabled = true;
                modificarLayout(tipoAccion);
                botonSeleccionado(btnFiltrar);
                rbDtConstante.Visible = true;
                actualizarEstadisticas();
            }
        }

        private void actualizarEstadisticas()
        {
            try
            {
                if (rbFecha.Checked)
                {
                    lblCantidad.Text = eventos.Count().ToString();
                }
                else if (rbIntervalos.Checked)
                {
                    lblCantidad.Text = intervalosParciales.Count().ToString();
                }
                lblMedia.Text = calcularMedia().ToString("0.0000");
            }
            catch
            {
                mostrarMensaje("Error al calcular estadisticas", Color.FromArgb(255, 89, 89));
            }

        }
        private double calcularMedia()
        {
            double resultado = 0;
            if (rbFecha.Checked)
            {
                List<double> intervalos = FdPUtils.CalcularIntervalos(eventos);
                if (eventos.Count() > 0)
                    resultado = intervalos.Sum() / intervalos.Count();
                else
                    resultado = 0;
            }
            else if (rbIntervalos.Checked)
            {
                resultado = intervalosParciales.Sum() / intervalosParciales.Count();
            }
            return resultado;

        }

        private void rbAgregarPorFechaYHora_CheckedChanged(object sender, EventArgs e)
        {
            nudAgregarPorIntervalo.Visible = false;
            cbAgregarPorIntervalo.Visible = false;
            dtp1.Visible = true;
            dtp2.Visible = true;
            lblAccion1.Visible = true;
            lblAccion2.Visible = true;
        }

        private void rbAgregarPorIntervalo_CheckedChanged(object sender, EventArgs e)
        {
            nudAgregarPorIntervalo.Visible = true;
            cbAgregarPorIntervalo.Visible = true;
            dtp1.Visible = false;
            dtp2.Visible = false;
            lblAccion1.Visible = false;
            lblAccion2.Visible = false;
            cbAgregarPorIntervalo.SelectedItem = "Segundos";

        }
    }
}
