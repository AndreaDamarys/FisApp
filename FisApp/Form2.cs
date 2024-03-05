using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace FisApp
{
    public partial class Form2 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";
        private int idUsuario;

        public Form2()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string cedula = txtBuscar.Text.Trim();

            string query = "SELECT id_Usuario FROM Usuario WHERE cedula = @Cedula";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Cedula", cedula);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            idUsuario = Convert.ToInt32(result);
                            MessageBox.Show("Cédula encontrada en la base de datos. Acceso concedido.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            Form3 form3 = new Form3();
                            form3.Owner = this;
                            form3.Show();
                        }
                        else
                        {
                            MessageBox.Show("La cédula no existe en la base de datos. No se puede ingresar al sistema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public int GetIdUsuario()
        {
            return idUsuario;
        }
    }
}
