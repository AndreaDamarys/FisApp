using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace FisApp
{
    public partial class Form10 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";

        public Form10()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string nombreAdmin = txtNombreAdm.Text.Trim();
            string passwordAdmin = txtClave.Text;

            // Realizar la verificación de las credenciales en la base de datos
            if (AdminLogin(nombreAdmin, passwordAdmin))
            {
                MessageBox.Show("Ingreso exitoso.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form5 form5 = new Form5();
                form5.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas. Cambia tu contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Form11 form11 = new Form11();
                form11.Show();
                this.Hide();
            }
        }

        private bool AdminLogin(string nombreAdmin, string passwordAdmin)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM administrador WHERE nombre = @Nombre AND contrasena = @Contrasena";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombreAdmin);
                    cmd.Parameters.AddWithValue("@Contrasena", passwordAdmin);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
