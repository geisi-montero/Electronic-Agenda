using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Agenda
{

    public partial class AgendaElectronica : Form
    {
        Conexion conexion = new Conexion();
        int idSeleccionado = 0;
        public AgendaElectronica()
        {
            InitializeComponent();
        }

        private void CargarDatos()
        {
            using (OracleConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = "SELECT * FROM AGENDA ORDER BY ID";

                OracleDataAdapter da = new OracleDataAdapter(sql, cn);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvAgenda.DataSource = dt;
            }
        }
        private void Limpiar()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDireccion.Clear();
            txtMovil.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtBuscar.Clear();

            cmbGenero.SelectedIndex = -1;
            cmbEstadoCivil.SelectedIndex = -1;

            dtpFecha.Value = DateTime.Now;

            idSeleccionado = 0;

            txtNombre.Focus();
        }

        private void MostrarAgenda()
        {
            using (OracleConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "SELECT * FROM AGENDA ORDER BY ID";

                OracleDataAdapter da = new OracleDataAdapter(query, cn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvAgenda.DataSource = dt;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatos();
            MostrarAgenda();
            dtpFecha.Format = DateTimePickerFormat.Custom;
            dtpFecha.CustomFormat = "dd/MM/yyyy";

            dgvAgenda.EnableHeadersVisualStyles = false;

            dgvAgenda.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvAgenda.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvAgenda.DefaultCellStyle.BackColor = Color.White;
            dgvAgenda.DefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);

            dgvAgenda.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            dgvAgenda.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvAgenda.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);

            dgvAgenda.GridColor = Color.FromArgb(229, 231, 235);

            dgvAgenda.EnableHeadersVisualStyles = false;

            dgvAgenda.Font = new Font("Segoe UI", 10);
            dgvAgenda.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvAgenda.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvAgenda.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (OracleConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"INSERT INTO AGENDA
                       (NOMBRE, APELLIDO, FECHA_NACIMIENTO,
                        DIRECCION, GENERO, ESTADO_CIVIL,
                        MOVIL, TELEFONO, CORREO)
                       VALUES
                       (:nombre, :apellido, :fecha,
                        :direccion, :genero, :estado,
                        :movil, :telefono, :correo)";

                OracleCommand cmd = new OracleCommand(sql, cn);

                cmd.Parameters.Add(":nombre", txtNombre.Text);
                cmd.Parameters.Add(":apellido", txtApellido.Text);
                cmd.Parameters.Add(":fecha", dtpFecha.Value.Date);
                cmd.Parameters.Add(":direccion", txtDireccion.Text);
                cmd.Parameters.Add(":genero", cmbGenero.Text);
                cmd.Parameters.Add(":estado", cmbEstadoCivil.Text);
                cmd.Parameters.Add(":movil", txtMovil.Text);
                cmd.Parameters.Add(":telefono", txtTelefono.Text);
                cmd.Parameters.Add(":correo", txtCorreo.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Registro guardado correctamente.");

                CargarDatos();

                Limpiar();
            }
    }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            using (OracleConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "DELETE FROM AGENDA WHERE ID = :id";

                OracleCommand cmd = new OracleCommand(query, cn);
                cmd.Parameters.Add(":id", txtId.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Registro eliminado");

                MostrarAgenda();
                Limpiar();
            }
            Limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (OracleConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = "SELECT * FROM AGENDA WHERE UPPER(NOMBRE) LIKE UPPER(:nombre)";

                OracleCommand cmd = new OracleCommand(query, cn);
                cmd.Parameters.Add(":nombre", "%" + txtBuscar.Text + "%");

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

        
                dgvAgenda.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    DataRow fila = dt.Rows[0];

                    txtId.Text = fila["ID"].ToString();
                    txtNombre.Text = fila["NOMBRE"].ToString();
                    txtApellido.Text = fila["APELLIDO"].ToString();
                    dtpFecha.Value = Convert.ToDateTime(fila["FECHA_NACIMIENTO"]);
                    txtDireccion.Text = fila["DIRECCION"].ToString();
                    cmbGenero.Text = fila["GENERO"].ToString();
                    cmbEstadoCivil.Text = fila["ESTADO_CIVIL"].ToString();
                    txtMovil.Text = fila["MOVIL"].ToString();
                    txtTelefono.Text = fila["TELEFONO"].ToString();
                    txtCorreo.Text = fila["CORREO"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontraron registros.");

                    Limpiar();
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            using (OracleConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string query = @"UPDATE AGENDA SET
        NOMBRE = :nombre,
        APELLIDO = :apellido,
        FECHA_NACIMIENTO = :fecha,
        DIRECCION = :direccion,
        GENERO = :genero,
        ESTADO_CIVIL = :estado,
        MOVIL = :movil,
        TELEFONO = :telefono,
        CORREO = :correo
        WHERE ID = :id";

                OracleCommand cmd = new OracleCommand(query, cn);

                cmd.Parameters.Add(":nombre", txtNombre.Text);
                cmd.Parameters.Add(":apellido", txtApellido.Text);
                cmd.Parameters.Add(":fecha", dtpFecha.Value);
                cmd.Parameters.Add(":direccion", txtDireccion.Text);
                cmd.Parameters.Add(":genero", cmbGenero.Text);
                cmd.Parameters.Add(":estado", cmbEstadoCivil.Text);
                cmd.Parameters.Add(":movil", txtMovil.Text);
                cmd.Parameters.Add(":telefono", txtTelefono.Text);
                cmd.Parameters.Add(":correo", txtCorreo.Text);
                cmd.Parameters.Add(":id", txtId.Text); 

                cmd.ExecuteNonQuery();

                MessageBox.Show("Registro actualizado");

                MostrarAgenda();
                Limpiar();
            }
    }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvAgenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAgenda.Rows[e.RowIndex];

                txtId.Text = row.Cells["ID"].Value.ToString();
                txtNombre.Text = row.Cells["NOMBRE"].Value.ToString();
                txtApellido.Text = row.Cells["APELLIDO"].Value.ToString();
                dtpFecha.Value = Convert.ToDateTime(row.Cells["FECHA_NACIMIENTO"].Value);
                txtDireccion.Text = row.Cells["DIRECCION"].Value.ToString();
                cmbGenero.Text = row.Cells["GENERO"].Value.ToString();
                cmbEstadoCivil.Text = row.Cells["ESTADO_CIVIL"].Value.ToString();
                txtMovil.Text = row.Cells["MOVIL"].Value.ToString();
                txtTelefono.Text = row.Cells["TELEFONO"].Value.ToString();
                txtCorreo.Text = row.Cells["CORREO"].Value.ToString();
            }
        }
    }

}

