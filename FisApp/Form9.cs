using System;
using System.Data;
using Npgsql;
using System.Windows.Forms;

namespace FisApp
{
    public partial class Form9 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";

        public Form9()
        {
            InitializeComponent();
            MostrarHistorialReservas();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombreUsuario = txtUsuario.Text.Trim();
            MostrarHistorialReservas(nombreUsuario);
        }

        private void MostrarHistorialReservas(string nombreUsuario = "")
        {
            string query = @"
                SELECT u.id_Usuario, u.Nombre, u.Apellido, r.hora_inicio, r.hora_fin, r.fecha, j.NombreJuego, p.precio
                FROM Usuario u
                INNER JOIN Reserva r ON u.id_Usuario = r.id_Usuario
                INNER JOIN Juego j ON r.id_Juego = j.id_Juego
                INNER JOIN Precio p ON j.id_Precio = p.id_Precio
                WHERE u.Nombre LIKE @NombreUsuario
                ORDER BY r.fecha DESC";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@NombreUsuario", $"%{nombreUsuario}%");
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Mostrar los datos en el DataGridView
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 form5 = new Form5();
            form5.Show();
        }
    }
}
