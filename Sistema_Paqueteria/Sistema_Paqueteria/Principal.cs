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

        public Principal()
        {
            InitializeComponent();
            c = new SqlConnection("Data Source=DESKTOP-O5C197G;Initial Catalog=Paqueteria;Integrated Security=True");
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
        }

        private void sucursalesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.sucursalesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.paqueteriaDataSet);
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'paqueteriaDataSet.Clientes' table. You can move, or remove it, as needed.
            this.clientesTableAdapter.Fill(this.paqueteriaDataSet.Clientes);
            // TODO: This line of code loads data into the 'paqueteriaDataSet.Ciudades' table. You can move, or remove it, as needed.
            this.ciudadesTableAdapter.Fill(this.paqueteriaDataSet.Ciudades);
            // TODO: This line of code loads data into the 'paqueteriaDataSet.Estados' table. You can move, or remove it, as needed.
            this.estadosTableAdapter.Fill(this.paqueteriaDataSet.Estados);
            // TODO: This line of code loads data into the 'paqueteriaDataSet.Sucursales' table. You can move, or remove it, as needed.
            this.sucursalesTableAdapter.Fill(this.paqueteriaDataSet.Sucursales);

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
                    this.estadosTableAdapter.Fill(this.paqueteriaDataSet.Estados);
                    MessageBox.Show("Inserción correcta", "Insertar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    textBoxEdo.Text = "";
                    updateCombos();
                }
                catch(Exception ex)
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
                        this.estadosTableAdapter.Fill(this.paqueteriaDataSet.Estados);
                        MessageBox.Show("Eliminación correcta", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        textBoxEdo.Text = "";
                        updateCombos(); 
                    }
                    else
                        MessageBox.Show("Seleccione un Estado de la lista");
                }
                catch(Exception ex)
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
                            this.estadosTableAdapter.Fill(this.paqueteriaDataSet.Estados);
                            MessageBox.Show("modificación correcta", "Modificar", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            updateCombos();
                        }
                        catch(Exception ex)
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
                        cmd = new SqlCommand("INSERT INTO Administracion.Ciudades (Nombre, IdEstado) VALUES('" + textBoxCd.Text + "', '"+ Convert.ToInt32(valor)+"')", c);
                        cmd.ExecuteNonQuery();
                        c.Close();
                        this.ciudadesTableAdapter.Fill(this.paqueteriaDataSet.Ciudades);
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
                        this.ciudadesTableAdapter.Fill(this.paqueteriaDataSet.Ciudades);
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
                                cmd = new SqlCommand("UPDATE Administracion.Ciudades SET Nombre='" + textBoxCd.Text + "'" +", " + "IdEstado='"+ Convert.ToInt32(valor)+"'" + " WHERE IdCiudad=" + id, c);
                                cmd.ExecuteNonQuery();
                                c.Close();
                                this.ciudadesTableAdapter.Fill(this.paqueteriaDataSet.Ciudades);
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
            comboSucEdo.SelectedValue = this.ciudadesDataGridView.CurrentRow.Cells[1].Value.ToString();
        }

        private void buttInsertSuc_Click(object sender, EventArgs e)
        {
            if (textBoxSuc.Text != "" && textBoxTel_Suc.Text != "" && textBoxDir_Suc.Text !="" && timeSucursal1.Text != "" && timeSucursal2.Text !="" && comboCd_Suc.SelectedValue != null)
            {
                try
                {
                    String valor = comboCd_Suc.SelectedValue.ToString(); 
                    c.Open();
                    cmd = new SqlCommand("INSERT INTO Administracion.Sucursales (IdCiudad, nombre, direccion, telefono, horaApertura, horaCierre) VALUES('" + Convert.ToInt32(valor) + "'"+","+"'"+textBoxSuc.Text+"'"+","+ "'"+textBoxDir_Suc.Text+"'"+","+"'"+textBoxTel_Suc.Text+"'"+","+"'"+timeSucursal1.Text+"'"+","+"'"+timeSucursal2.Text+"')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.sucursalesTableAdapter.Fill(this.paqueteriaDataSet.Sucursales);
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
                        this.sucursalesTableAdapter.Fill(this.paqueteriaDataSet.Sucursales);
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
                                cmd = new SqlCommand("UPDATE Administracion.Sucursales SET IdCiudad='" + Convert.ToInt32(valor) + "'" + ", " + "nombre='" + textBoxSuc.Text + "'"+", "+"direccion='"+textBoxDir_Suc.Text+"'"+", "+ "telefono='"+textBoxTel_Suc.Text+"'"+", "+"horaApertura='"+timeSucursal1.Text+"'"+", "+"horaCierre='"+timeSucursal2.Text+"'" + " WHERE IdSucursal=" + id, c);
                                cmd.ExecuteNonQuery();
                                c.Close();
                                this.sucursalesTableAdapter.Fill(this.paqueteriaDataSet.Sucursales);
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
            if (tB_Nombre_Clie.Text != "" && tB_Ap_Clie.Text != "" && tB_Am_Clie.Text != "" && tB_dir_Clie.Text != "" && tB_Cp_Clie.Text != "" && tB_tel_Clie.Text !="" && tB_cel_Clie.Text!="")
            {
                try
                {
                    c.Open();
                    cmd = new SqlCommand("INSERT INTO Administracion.Clientes (Nombre, ApellidoPaterno, ApellidoMaterno, Direccion, CP, telefono, celular) VALUES('" + tB_Nombre_Clie.Text + "'" + "," + "'" + tB_Ap_Clie.Text + "'" + "," + "'" + tB_Am_Clie.Text + "'" + "," + "'" + tB_dir_Clie.Text + "'" + "," + "'" + Convert.ToInt32(tB_Cp_Clie.Text) + "'" + "," + "'" + tB_tel_Clie.Text + "'" + ", " + "'" + tB_cel_Clie.Text + "')", c);
                    cmd.ExecuteNonQuery();
                    c.Close();
                    this.clientesTableAdapter.Fill(this.paqueteriaDataSet.Clientes);
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
                        this.clientesTableAdapter.Fill(this.paqueteriaDataSet.Clientes);
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
                                cmd = new SqlCommand("UPDATE Administracion.Clientes SET Nombre='" + tB_Nombre_Clie.Text + "'" + ", " + "ApellidoPaterno='" + tB_Ap_Clie.Text + "'" + ", " + "ApellidoMaterno='" + tB_Am_Clie.Text + "'" + ", " + "Direccion='" + tB_dir_Clie.Text + "'" + ", " + "CP='" + tB_Cp_Clie.Text + "'" + ", " + "telefono='" + tB_tel_Clie.Text + "'" +", "+"celular='"+tB_cel_Clie.Text+"'"+ " WHERE IdCliente=" + id, c);
                                cmd.ExecuteNonQuery();
                                c.Close();
                                this.clientesTableAdapter.Fill(this.paqueteriaDataSet.Clientes);
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
    }
}
