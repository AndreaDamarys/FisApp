using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Npgsql;

namespace FisApp
{
    public partial class Form4 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";

        public Form4()
        {
            InitializeComponent();
            FillComboBox();
            panelGAMER.Visible = false;
            panelPRO.Visible = false;
            panelPLATINUM.Visible = false;
        }

        private void FillComboBox()
        {
            CBPlan.Items.Add(new ComboBoxItem("PRO", 1));
            CBPlan.Items.Add(new ComboBoxItem("GAMER", 2));
            CBPlan.Items.Add(new ComboBoxItem("PLATINUM", 3));
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string cedula = txtCedula.Text.Trim();
            int idPlan = ((ComboBoxItem)CBPlan.SelectedItem).Value;

            // Verificar el formato del correo electrónico usando expresiones regulares
            if (!Regex.IsMatch(correo, @"^\w+([\.-]?\w+)*@epn.edu.ec$"))
            {
                MessageBox.Show("El correo electrónico debe tener la extensión @epn.edu.ec.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO usuario (nombre, apellido, correo, cedula, id_plan) VALUES (@Nombre, @Apellido, @Correo, @Cedula, @IdPlan)";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", nombre);
                        cmd.Parameters.AddWithValue("@Apellido", apellido);
                        cmd.Parameters.AddWithValue("@Correo", correo);
                        cmd.Parameters.AddWithValue("@Cedula", cedula);
                        cmd.Parameters.AddWithValue("@IdPlan", idPlan);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Usuario agregado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form1 form1 = new Form1();
                            form1.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Error al agregar usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        // Resto del código omitido para mantener el enfoque en la validación del correo electrónico

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void CBPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CBPlan.SelectedIndex)
            {
                case 0: // Opción 1
                    panelGAMER.Visible = false;
                    panelPRO.Visible = true;
                    panelPLATINUM.Visible = false;
                    break;
                case 1: // Opción 2
                    panelGAMER.Visible = true;
                    panelPRO.Visible = false;
                    panelPLATINUM.Visible = false;
                    break;
                case 2: // Opción 2
                    panelGAMER.Visible = false;
                    panelPRO.Visible = false;
                    panelPLATINUM.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void panelPLATINUM_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public ComboBoxItem(string text, int value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
