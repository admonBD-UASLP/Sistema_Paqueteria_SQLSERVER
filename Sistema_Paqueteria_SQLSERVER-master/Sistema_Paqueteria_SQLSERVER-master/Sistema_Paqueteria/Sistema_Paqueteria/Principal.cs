using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sistema_Paqueteria
{
    public partial class Principal : Form
    {
        SqlCommand cmd;
        SqlConnection c;
        SqlDataReader dr;
        SqlDataAdapter da;
        DataTable dt;

        public Principal()
        {
            InitializeComponent();
            c = new SqlConnection("Data Source=.;Initial Catalog=Paqueteria;Integrated Security=True");
            timeSucursal1.Format = DateTimePickerFormat.Time;
            timeSucursal1.ShowUpDown = true;
            timeSucursal2.Format = DateTimePickerFormat.Time;
            timeSucursal2.ShowUpDown = true;
            updateCombos();
        }

        private void updateCombos()
        {
            actualizarComboEdos();
            actualizarComboSuc();
            actualizaComboRepartidor();
            actualizaComboUnidad();
        }

        private void sucursalesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();

        }

        private void Principal_Load(object sender, EventArgs e)
        {
            this.loadDataGrid("Select * from Administracion.Vehiculos", vehiculosDataGridView);
            this.loadDataGrid("Select * From Administracion.TipoConductor", tipoConductorDataGridView);
            this.loadDataGrid("Select * From Administracion.Conductores", conductoresDataGridView);
            this.loadDataGrid("Select * From Administracion.Clientes", clientesDataGridView);
            this.loadDataGrid("Select * From Administracion.Ciudades", ciudadesDataGridView);
            this.loadDataGrid("Select * from Administracion.Estados", estadosDataGridView);
            this.loadDataGrid("Select * From Administracion.Sucursales", sucursalesDataGridView);

        }

        private void clic_insertarEstado(object sender, EventArgs e)
        {
            if (textBoxEdo.Text != "")
            {
                try
                {
                    c.Open();
                    cmd = new SqlCommand("INSERT INTO Administracion.Estados (Nombre) VALUES('" + textBoxEdo.Text + "')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.loadDataGrid("Select * from Administracion.Estados", estadosDataGridView);
                    MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    textBoxEdo.Text = "";
                    updateCombos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Campo vacio,Introduzca un Estado");
        }

        private void click_Eliminar_Edo(object sender, EventArgs e)
        {
            if (this.estadosDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.estadosDataGridView.Rows[this.estadosDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.Estados WHERE IdEstado=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * from Administracion.Estados", estadosDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        textBoxEdo.Text = "";
                        updateCombos();
                    }
                    else
                        MessageBox.Show("Seleccione un Estado de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona un Estado de la lista");
        }

        private void estadosDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxEdo.Text = this.estadosDataGridView.CurrentRow.Cells[1].Value.ToString();
        }

        private void estadosDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxEdo.Text = this.estadosDataGridView.CurrentRow.Cells[1].Value.ToString();
        }

        private void buttUpdEdo_Click(object sender, EventArgs e)
        {
            if (this.estadosDataGridView.CurrentRow != null)
            {
                String id = this.estadosDataGridView.Rows[this.estadosDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (textBoxEdo.Text != null)
                    {
                        try
                        {
                            c.Open();
                            cmd = new SqlCommand("UPDATE Administracion.Estados SET Nombre='" + textBoxEdo.Text + "'" + " WHERE IdEstado=" + id, c);
                            cmd.ExecuteNonQuery();
                            c.Close();
                            this.loadDataGrid("Select * from Administracion.Estados", estadosDataGridView);
                            MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            updateCombos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            c.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Campo Estado Vacio");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un estado");
            }
            else
                MessageBox.Show("Seleccione un Estado de la lista");
        }

        private void actualizarComboEdos()
        {
            //comboSucEdo.Items.Clear();
            DataTable dt = new DataTable();
            c.Open();
            string query = "SELECT IdEstado, Nombre FROM Administracion.Estados ORDER BY Nombre ASC";

            SqlCommand cmd = new SqlCommand(query, c);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            c.Close();
            comboSucEdo.DisplayMember = "Nombre";
            comboSucEdo.ValueMember = "IdEstado";
            comboSucEdo.DataSource = dt;
        }

        private void actualizarComboSuc()
        {
            DataTable dt = new DataTable();
            c.Open();
            string query = "SELECT IdCiudad, Nombre FROM Administracion.Ciudades ORDER BY Nombre ASC";

            SqlCommand cmd = new SqlCommand(query, c);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            c.Close();
            comboCd_Suc.DisplayMember = "Nombre";
            comboCd_Suc.ValueMember = "IdCiudad";
            comboCd_Suc.DataSource = dt;
        }

        private void actualizaComboRepartidor()
        {
            DataTable dt = new DataTable();
            c.Open();
            string query = "SELECT IdTipoConductor, Tipo FROM Administracion.TipoConductor ORDER BY Tipo ASC";

            SqlCommand cmd = new SqlCommand(query, c);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            c.Close();
            comboTipoCond.DisplayMember = "Tipo";
            comboTipoCond.ValueMember = "IdTipoConductor";
            comboTipoCond.DataSource = dt;
        }

        private void actualizaComboUnidad()
        {
            DataTable dt = new DataTable();
            c.Open();
            string query = "SELECT IdUnidad, Matricula FROM Administracion.Vehiculos ORDER BY Matricula ASC";

            SqlCommand cmd = new SqlCommand(query, c);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            c.Close();
            comboUniCond.DisplayMember = "Matricula";
            comboUniCond.ValueMember = "IdUnidad";
            comboUniCond.DataSource = dt;
        }
        private void buttInsCd_Click(object sender, EventArgs e)
        {
            if (textBoxCd.Text != "")
            {
                if (comboSucEdo.SelectedItem != null)
                {
                    try
                    {
                        String valor = comboSucEdo.SelectedValue.ToString();
                        c.Open();
                        cmd = new SqlCommand("INSERT INTO Administracion.Ciudades (Nombre, IdEstado) VALUES('" + textBoxCd.Text + "', '" + Convert.ToInt32(valor) + "')", c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.Ciudades", ciudadesDataGridView);
                        MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        textBoxCd.Text = "";
                        updateCombos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex.Message);
                        c.Close();
                    }
                }
                else
                    MessageBox.Show("Seleccione un Estado del combo");
            }
            else
                MessageBox.Show("Campo vacio,Introduzca un Estado");
        }

        private void buttDelCd_Click(object sender, EventArgs e)
        {
            if (this.ciudadesDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.ciudadesDataGridView.Rows[this.ciudadesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.Ciudades WHERE IdCiudad=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.Ciudades", ciudadesDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK);
                        textBoxCd.Text = "";
                        updateCombos();
                    }
                    else
                        MessageBox.Show("Seleccione una Ciudad de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona un Estado de la lista");
        }

        private void buttUpdCd_Click(object sender, EventArgs e)
        {
            if (this.ciudadesDataGridView.CurrentRow != null)
            {
                String id = this.ciudadesDataGridView.Rows[this.ciudadesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (textBoxCd.Text != null)
                    {
                        if (comboSucEdo.SelectedItem != null)
                        {
                            try
                            {
                                String valor = comboSucEdo.SelectedValue.ToString();
                                c.Open();
                                cmd = new SqlCommand("UPDATE Administracion.Ciudades SET Nombre='" + textBoxCd.Text + "'" + ", " + "IdEstado='" + Convert.ToInt32(valor) + "'" + " WHERE IdCiudad=" + id, c);
                                cmd.ExecuteNonQuery();
                                c.Close();
                                this.loadDataGrid("Select * From Administracion.Ciudades", ciudadesDataGridView);
                                MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                updateCombos();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                c.Close();
                            }
                        }
                        else
                            MessageBox.Show("Error: Combobox de Estado esta Vacio");
                    }
                    else
                    {
                        MessageBox.Show("Error: Campo Ciudad Vacio");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un estado");
            }
            else
                MessageBox.Show("Seleccione un Estado de la lista");
        }

        private void ciudadesDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxCd.Text = this.ciudadesDataGridView.CurrentRow.Cells[2].Value.ToString();
            comboSucEdo.Text = this.ciudadesDataGridView.CurrentRow.Cells[1].Value.ToString();
        }

        private void buttInsertSuc_Click(object sender, EventArgs e)
        {
            if (textBoxSuc.Text != "" && textBoxTel_Suc.Text != "" && textBoxDir_Suc.Text != "" && timeSucursal1.Text != "" && timeSucursal2.Text != "" && comboCd_Suc.SelectedValue != null)
            {
                try
                {
                    String valor = comboCd_Suc.SelectedValue.ToString();
                    c.Open();
                    string h1, h2;
                    h1 = timeSucursal1.Text.Replace(" a. m.", "").Replace(" p. m.", "");
                    h2 = timeSucursal2.Text.Replace(" a. m.", "").Replace(" p. m.", "");                    
                    cmd = new SqlCommand("INSERT INTO Administracion.Sucursales (IdCiudad, nombre, direccion, telefono, horaApertura, horaCierre) VALUES('" + Convert.ToInt32(valor) + "'" + "," + "'" + textBoxSuc.Text + "'" + "," + "'" + textBoxDir_Suc.Text + "'" + "," + "'" + textBoxTel_Suc.Text + "'" + "," + "'" + h1 + "'" + "," + "'" + h2 + "')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.loadDataGrid("Select * From Administracion.Sucursales", sucursalesDataGridView);
                    MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    textBoxSuc.Text = "";
                    actualizarComboEdos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Error: LLene todos los campos");
        }

        private void sucursalesDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxSuc.Text = this.sucursalesDataGridView.CurrentRow.Cells[2].Value.ToString();
            comboCd_Suc.SelectedValue = this.sucursalesDataGridView.CurrentRow.Cells[1].Value.ToString();
            textBoxDir_Suc.Text = this.sucursalesDataGridView.CurrentRow.Cells[3].Value.ToString();
            textBoxTel_Suc.Text = this.sucursalesDataGridView.CurrentRow.Cells[4].Value.ToString();
            timeSucursal1.Text = this.sucursalesDataGridView.CurrentRow.Cells[5].Value.ToString();
            timeSucursal2.Text = this.sucursalesDataGridView.CurrentRow.Cells[6].Value.ToString();
        }

        private void buttDelSuc_Click(object sender, EventArgs e)
        {
            if (this.sucursalesDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.sucursalesDataGridView.Rows[this.sucursalesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.Sucursales WHERE IdSucursal=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.Sucursales", sucursalesDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK);
                        textBoxCd.Text = "";
                        updateCombos();
                    }
                    else
                        MessageBox.Show("Seleccione una Sucursal de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona una Sucursal de la lista");
        }

        private void buttUpdSuc_Click(object sender, EventArgs e)
        {
            if (this.sucursalesDataGridView.CurrentRow != null)
            {
                String id = this.sucursalesDataGridView.Rows[this.sucursalesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (textBoxSuc.Text != "" && textBoxTel_Suc.Text != "" && textBoxDir_Suc.Text != "" && timeSucursal1.Text != "" && timeSucursal2.Text != "")
                    {
                        if (comboCd_Suc.SelectedItem != null)
                        {
                            try
                            {
                                String valor = comboCd_Suc.SelectedValue.ToString();
                                c.Open();
                                cmd = new SqlCommand("UPDATE Administracion.Sucursales SET IdCiudad='" + Convert.ToInt32(valor) + "'" + ", " + "nombre='" + textBoxSuc.Text + "'" + ", " + "direccion='" + textBoxDir_Suc.Text + "'" + ", " + "telefono='" + textBoxTel_Suc.Text + "'" + ", " + "horaApertura='" + timeSucursal1.Text + "'" + ", " + "horaCierre='" + timeSucursal2.Text + "'" + " WHERE IdSucursal=" + id, c);
                                cmd.ExecuteNonQuery();
                                c.Close();
                                this.loadDataGrid("Select * From Administracion.Sucursales", sucursalesDataGridView);
                                MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                updateCombos();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                c.Close();
                            }
                        }
                        else
                            MessageBox.Show("Error: Combobox de Ciudad esta Vacio");
                    }
                    else
                    {
                        MessageBox.Show("Error: Todos los campos son requeridos, introduzca valores");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un estado");
            }
            else
                MessageBox.Show("Seleccione un Estado de la lista");
        }

        private void buttonInsClie_Click(object sender, EventArgs e)
        {
            if (tB_Nombre_Clie.Text != "" && tB_Ap_Clie.Text != "" && tB_Am_Clie.Text != "" && tB_dir_Clie.Text != "" && tB_Cp_Clie.Text != "" && tB_tel_Clie.Text != "" && tB_cel_Clie.Text != "")
            {
                try
                {
                    c.Open();
                    cmd = new SqlCommand("INSERT INTO Administracion.Clientes (Nombre, ApellidoPaterno, ApellidoMaterno, Direccion, CP, telefono, celular) VALUES('" + tB_Nombre_Clie.Text + "'" + "," + "'" + tB_Ap_Clie.Text + "'" + "," + "'" + tB_Am_Clie.Text + "'" + "," + "'" + tB_dir_Clie.Text + "'" + "," + "'" + Convert.ToInt32(tB_Cp_Clie.Text) + "'" + "," + "'" + tB_tel_Clie.Text + "'" + ", " + "'" + tB_cel_Clie.Text + "')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.loadDataGrid("Select * From Administracion.Clientes", clientesDataGridView);
                    MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    textBoxSuc.Text = "";
                    actualizarComboEdos();
                    tB_Nombre_Clie.Text = "";
                    tB_Ap_Clie.Text = "";
                    tB_Am_Clie.Text = "";
                    tB_dir_Clie.Text = "";
                    tB_Cp_Clie.Text = "";
                    tB_tel_Clie.Text = "";
                    tB_cel_Clie.Text = "";
                    tB_Nombre_Clie.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Error: LLene todos los campos");
        }

        private void clientesDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            tB_Nombre_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[1].Value.ToString();
            tB_Ap_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[2].Value.ToString();
            tB_Am_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[3].Value.ToString();
            tB_dir_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[4].Value.ToString();
            tB_Cp_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[5].Value.ToString();
            tB_tel_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[6].Value.ToString();
            tB_cel_Clie.Text = this.clientesDataGridView.CurrentRow.Cells[7].Value.ToString();
        }

        private void buttonDelClie_Click(object sender, EventArgs e)
        {
            if (this.clientesDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.clientesDataGridView.Rows[this.clientesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.Clientes WHERE IdCliente=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.Clientes", clientesDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK);
                        textBoxCd.Text = "";
                        updateCombos();
                        tB_Nombre_Clie.Text = "";
                        tB_Ap_Clie.Text = "";
                        tB_Am_Clie.Text = "";
                        tB_dir_Clie.Text = "";
                        tB_Cp_Clie.Text = "";
                        tB_tel_Clie.Text = "";
                        tB_cel_Clie.Text = "";
                        tB_Nombre_Clie.Focus();
                    }
                    else
                        MessageBox.Show("Seleccione un Cliente de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona un Cliente de la lista");
        }

        private void buttonUpdClie_Click(object sender, EventArgs e)
        {
            if (this.clientesDataGridView.CurrentRow != null)
            {
                String id = this.clientesDataGridView.Rows[this.clientesDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (tB_Nombre_Clie.Text != "" && tB_Ap_Clie.Text != "" && tB_Am_Clie.Text != "" && tB_dir_Clie.Text != "" && tB_Cp_Clie.Text != "" && tB_tel_Clie.Text != "" && tB_cel_Clie.Text != "")
                    {
                        try
                        {
                            c.Open();
                            cmd = new SqlCommand("UPDATE Administracion.Clientes SET Nombre='" + tB_Nombre_Clie.Text + "'" + ", " + "ApellidoPaterno='" + tB_Ap_Clie.Text + "'" + ", " + "ApellidoMaterno='" + tB_Am_Clie.Text + "'" + ", " + "Direccion='" + tB_dir_Clie.Text + "'" + ", " + "CP='" + tB_Cp_Clie.Text + "'" + ", " + "telefono='" + tB_tel_Clie.Text + "'" + ", " + "celular='" + tB_cel_Clie.Text + "'" + " WHERE IdCliente=" + id, c);
                            cmd.ExecuteNonQuery();
                            c.Close();
                            this.loadDataGrid("Select * From Administracion.Clientes", clientesDataGridView);
                            MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            updateCombos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            c.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Todos los campos son requeridos, introduzca valores");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un estado");
            }
            else
                MessageBox.Show("Seleccione un Estado de la lista");
        }

        private void buttonInsCond_Click(object sender, EventArgs e)
        {
            if (tB_Nom_Cond.Text != "" && tB_Ap_Cond.Text != "" && tB_Am_Cond.Text != "" && tB_Dir_Cond.Text != "" && tBTelCond.Text != "" && tBCelCond.Text != "" && tBLicCond.Text != "")
            {
                if (comboTipoCond.SelectedItem != null && comboUniCond.SelectedItem != null)
                {
                    try
                    {
                        String valor = comboTipoCond.SelectedValue.ToString();
                        String valor2 = comboUniCond.SelectedValue.ToString();
                        c.Open();
                        cmd = new SqlCommand("INSERT INTO Administracion.Conductores (Nombre, ApellidoPaterno, ApellidoMaterno, Direccion, Telefono, Celular, Licencia, IdUnidad, IdTipoConductor) VALUES('" + tB_Nom_Cond.Text + "'" + "," + "'" + tB_Ap_Cond.Text + "'" + "," + "'" + tB_Am_Cond.Text + "'" + "," + "'" + tB_Dir_Cond.Text + "'" + "," + "'" + tBTelCond.Text + "'" + "," + "'" + tBCelCond.Text + "'" + ", " + "'" + tBLicCond.Text + "'" + ", " + "'" + Convert.ToInt32(valor2) + "'" + ", " + "'" + Convert.ToInt32(valor) + "')", c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.Conductores", conductoresDataGridView);
                        MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        actualizarComboEdos();
                        tB_Nom_Cond.Text = "";
                        tB_Ap_Cond.Text = "";
                        tB_Am_Cond.Text = "";
                        tB_Dir_Cond.Text = "";
                        tBTelCond.Text = "";
                        tBCelCond.Text = "";
                        tBLicCond.Text = "";
                        tB_Nom_Cond.Focus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        c.Close();
                    }
                }
                else
                    MessageBox.Show("Error: Seleccione valores de los comboBox");
            }
            else
                MessageBox.Show("Error: LLene todos los campos");
        }

        private void buttonInsVeh_Click(object sender, EventArgs e)
        {
            if (tBMarcaVeh.Text != "" && tBMatVeh.Text != "" && tBModVeh.Text != "")
            {
                try
                {
                    c.Open();
                    cmd = new SqlCommand("INSERT INTO Administracion.Vehiculos (Matricula, Modelo, Marca) VALUES('" + tBMatVeh.Text + "'" + "," + "'" + tBModVeh.Text + "'" + "," + "'" + tBMarcaVeh.Text + "')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.loadDataGrid("Select * from Administracion.Vehiculos", vehiculosDataGridView);
                    MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    textBoxSuc.Text = "";
                    actualizarComboEdos();
                    tBMarcaVeh.Text = "";
                    tBMatVeh.Text = "";
                    tBModVeh.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Error: LLene todos los campos");
        }

        private void vehiculosDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            tBMatVeh.Text = this.vehiculosDataGridView.CurrentRow.Cells[1].Value.ToString();
            tBModVeh.Text = this.vehiculosDataGridView.CurrentRow.Cells[2].Value.ToString();
            tBMarcaVeh.Text = this.vehiculosDataGridView.CurrentRow.Cells[3].Value.ToString();
        }

        private void buttonElimVeh_Click(object sender, EventArgs e)
        {
            if (this.vehiculosDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.vehiculosDataGridView.Rows[this.vehiculosDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.Vehiculos WHERE IdUnidad=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * from Administracion.Vehiculos", vehiculosDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK);
                        updateCombos();
                        tBMarcaVeh.Text = "";
                        tBMatVeh.Text = "";
                        tBModVeh.Text = "";
                        tBMatVeh.Focus();
                    }
                    else
                        MessageBox.Show("Seleccione una Unidad de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona una Unidad de la lista");
        }

        private void buttonModVeh_Click(object sender, EventArgs e)
        {
            if (this.vehiculosDataGridView.CurrentRow != null)
            {
                String id = this.vehiculosDataGridView.Rows[this.vehiculosDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (tBMatVeh.Text != "" && tBMarcaVeh.Text != "" && tBModVeh.Text != "")
                    {
                        try
                        {
                            c.Open();
                            cmd = new SqlCommand("UPDATE Administracion.Vehiculos SET Matricula='" + tBMatVeh.Text + "'" + ", " + "Modelo='" + tBModVeh.Text + "'" + ", " + "Marca='" + tBMarcaVeh.Text + "'" + " WHERE IdUnidad=" + id, c);
                            cmd.ExecuteNonQuery();
                            c.Close();
                            this.loadDataGrid("Select * from Administracion.Vehiculos", vehiculosDataGridView);
                            MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            updateCombos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            c.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Todos los campos son requeridos, introduzca valores");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un estado");
            }
            else
                MessageBox.Show("Seleccione un Estado de la lista");
        }

        private void buttonInsTipoC_Click(object sender, EventArgs e)
        {
            if (tBTipoCond.Text != "" && tBDescCond.Text != "")
            {
                try
                {
                    c.Open();
                    cmd = new SqlCommand("INSERT INTO Administracion.TipoConductor (Tipo, descripcion) VALUES('" + tBTipoCond.Text + "'" + "," + "'" + tBDescCond.Text + "')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.loadDataGrid("Select * From Administracion.TipoConductor", tipoConductorDataGridView);
                    MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    actualizarComboEdos();
                    tBTipoCond.Text = "";
                    tBDescCond.Text = "";
                    tBTipoCond.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Error: LLene todos los campos");
        }

        private void buttonElimCond_Click(object sender, EventArgs e)
        {
            if (this.tipoConductorDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.tipoConductorDataGridView.Rows[this.tipoConductorDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.TipoConductor WHERE IdTipoConductor=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.TipoConductor", tipoConductorDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK);
                        updateCombos();
                        tBTipoCond.Text = "";
                        tBDescCond.Text = "";
                        tBTipoCond.Focus();
                    }
                    else
                        MessageBox.Show("Seleccione un Tipo de conductor de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona un Tipo de conductor de la lista");
        }

        private void buttonModTipoC_Click(object sender, EventArgs e)
        {
            if (this.tipoConductorDataGridView.CurrentRow != null)
            {
                String id = this.tipoConductorDataGridView.Rows[this.tipoConductorDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (tBTipoCond.Text != null && tBDescCond.Text != "")
                    {
                        try
                        {
                            c.Open();
                            cmd = new SqlCommand("UPDATE Administracion.TipoConductor SET Tipo='" + tBTipoCond.Text + "'" + ", " + "descripcion='" + tBDescCond.Text + "'" + " WHERE IdTipoConductor=" + id, c);
                            cmd.ExecuteNonQuery();
                            c.Close();
                            this.loadDataGrid("Select * From Administracion.TipoConductor", tipoConductorDataGridView);
                            MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            updateCombos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            c.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Campo Estado Vacio");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un tipo de conductor");
            }
            else
                MessageBox.Show("Seleccione un tipo de conductor de la lista");
        }

        private void tipoConductorDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            tBTipoCond.Text = this.tipoConductorDataGridView.CurrentRow.Cells[1].Value.ToString();
            tBDescCond.Text = this.tipoConductorDataGridView.CurrentRow.Cells[2].Value.ToString();
        }

        private void buttonDelCond_Click(object sender, EventArgs e)
        {
            if (this.conductoresDataGridView.CurrentRow != null)
            {
                try
                {
                    String id = this.conductoresDataGridView.Rows[this.conductoresDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        c.Open();
                        cmd = new SqlCommand("DELETE FROM Administracion.Conductores WHERE IdConductor=" + Convert.ToInt32(id), c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.loadDataGrid("Select * From Administracion.Conductores", conductoresDataGridView);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK);
                        updateCombos();
                        tB_Nom_Cond.Text = "";
                        tB_Ap_Cond.Text = "";
                        tB_Am_Cond.Text = "";
                        tB_Dir_Cond.Text = "";
                        tBTelCond.Text = "";
                        tBCelCond.Text = "";
                        tBLicCond.Text = "";
                        tB_Nom_Cond.Focus();
                    }
                    else
                        MessageBox.Show("Seleccione un conductor de la lista");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.Close();
                }
            }
            else
                MessageBox.Show("Selecciona un conductor de la lista");
        }

        private void conductoresDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            tB_Nom_Cond.Text = this.conductoresDataGridView.CurrentRow.Cells[1].Value.ToString();
            tB_Ap_Cond.Text = this.conductoresDataGridView.CurrentRow.Cells[2].Value.ToString();
            tB_Am_Cond.Text = this.conductoresDataGridView.CurrentRow.Cells[3].Value.ToString();
            tB_Dir_Cond.Text = this.conductoresDataGridView.CurrentRow.Cells[4].Value.ToString();
            tBTelCond.Text = this.conductoresDataGridView.CurrentRow.Cells[5].Value.ToString();
            tBCelCond.Text = this.conductoresDataGridView.CurrentRow.Cells[6].Value.ToString();
            tBLicCond.Text = this.conductoresDataGridView.CurrentRow.Cells[7].Value.ToString();
            comboUniCond.SelectedValue = this.conductoresDataGridView.CurrentRow.Cells[8].Value.ToString();
            comboTipoCond.SelectedValue = this.conductoresDataGridView.CurrentRow.Cells[9].Value.ToString();
        }


        private void buttonModCond_Click(object sender, EventArgs e)
        {
            if (this.conductoresDataGridView.CurrentRow != null)
            {
                String id = this.conductoresDataGridView.Rows[this.conductoresDataGridView.CurrentRow.Index].Cells[0].Value.ToString();
                if (id != "")
                {
                    if (tB_Nom_Cond.Text != "" && tB_Ap_Cond.Text != "" && tB_Am_Cond.Text != "" && tB_Dir_Cond.Text != "" && tBTelCond.Text != "" && tBCelCond.Text != "" && tBLicCond.Text != "")
                    {
                        if (comboTipoCond.SelectedItem != null && comboUniCond.SelectedItem != null)
                        {
                            try
                            {
                                c.Open();
                                cmd = new SqlCommand("UPDATE Administracion.Conductores SET Nombre='" + tB_Nom_Cond.Text + "'" + ", " + "ApellidoPaterno='" + tB_Ap_Cond.Text + "'" + ", " + "ApellidoMaterno='" + tB_Am_Cond.Text + "'" + ", " + "Direccion='" + tB_Dir_Cond.Text + "'" + ", " + "Telefono='" + tBTelCond.Text + "'" + ", " + "Celular='" + tBCelCond.Text + "'" + ", " + "Licencia='" + tBLicCond.Text + "'" + ", " + "IdUnidad='" + comboUniCond.SelectedValue + "'" + ", " + "IdTipoConductor='" + comboTipoCond.SelectedValue + "'" + " WHERE IdConductor=" + id, c);
                                cmd.ExecuteNonQuery();
                                c.Close();
                                this.loadDataGrid("Select * From Administracion.Conductores", conductoresDataGridView);
                                MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                updateCombos();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                c.Close();
                            }
                        }
                        else
                            MessageBox.Show("Seleccione un valor en los comboBox");
                    }
                    else
                    {
                        MessageBox.Show("Error: Todos los campos son requeridos, introduzca valores");
                        textBoxEdo.Focus();
                    }
                }
                else
                    MessageBox.Show("Seleccione un conductor");
            }
            else
                MessageBox.Show("Seleccione un Conductor de la lista");
        }



        /// <summary>
        ///  Actualiza un Datagrid con la informacion recuperada por una consulta sql
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="db"></param>
        private void loadDataGrid(string consulta, DataGridView dgv)
        {
            bool open = true;
            try
            {
                if (c.State == ConnectionState.Closed)
                {
                    c.Open();
                    open = false;
                }
                da = new SqlDataAdapter(consulta, c);
                dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
                if (!open)
                {
                    c.Close();
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error 0x0ff");
            }

        }
    }
}
