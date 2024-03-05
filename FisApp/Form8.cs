using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace FisApp
{
    public partial class Form8 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";

        public Form8()
        {
            InitializeComponent();
            MostrarReservas();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener el ID de la reserva seleccionada
                int idReserva = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_reserva"].Value);

                // Confirmar antes de eliminar la reserva
                DialogResult dialogResult = MessageBox.Show("¿Está seguro de que desea eliminar esta reserva?", "Confirmar eliminación", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // Eliminar la reserva de la base de datos
                    EliminarReserva(idReserva);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una reserva para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void EliminarReserva(int idReserva)
        {
            string query = "DELETE FROM reserva WHERE id_reserva = @Id";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", idReserva);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Reserva eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Actualizar el DataGridView después de eliminar la reserva
                            MostrarReservas();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarReservas()
        {
            string query = "SELECT id_reserva, fecha, hora_inicio, hora_fin FROM reserva";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn))
                    {
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
