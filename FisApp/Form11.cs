using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace FisApp
{
    public partial class Form11 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";

        public Form11()
        {
            InitializeComponent();
        }

        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text.Trim();
            string nuevaClave = txtClaveNueva.Text;

            // Conecta con la base de datos y verifica si la cédula coincide
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM administrador WHERE cedula = @Cedula";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Cedula", cedula);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    // Si la cédula coincide con algún registro
                    if (count > 0)
                    {
                        // Actualiza la contraseña
                        string updateQuery = "UPDATE administrador SET contrasena = @NuevaClave WHERE cedula = @Cedula";
                        using (NpgsqlCommand updateCmd = new NpgsqlCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@NuevaClave", nuevaClave);
                            updateCmd.Parameters.AddWithValue("@Cedula", cedula);
                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Contraseña actualizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("No se pudo actualizar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("La cédula ingresada no coincide con ningún usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.Show();
            this.Hide();
        }
    }
}
