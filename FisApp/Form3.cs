using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace FisApp
{
    public partial class Form3 : Form
    {
        private string connString = "Host=localhost;Username=postgres;Password=12345;Database=DBFisApp";

        public Form3()
        {
            InitializeComponent();
            FillComboBoxes();
        }

        private void FillComboBoxes()
        {
            CBJuego.Items.Add("PING PONG");
            CBJuego.Items.Add("BILLAR");
            CBJuego.Items.Add("PLAY STATION");

            // Llenar ComboBox de horas de inicio
            for (int hour = 8; hour <= 16; hour++)
            {
                for (int minute = 0; minute <= 30; minute += 30)
                {
                    CBInicio.Items.Add($"{hour.ToString().PadLeft(2, '0')}:{minute.ToString().PadLeft(2, '0')}");
                }
            }

            // Llenar ComboBox de horas de fin
            for (int hour = 8; hour <= 17; hour++)
            {
                for (int minute = 30; minute <= 30; minute += 30)
                {
                    CBFin.Items.Add($"{hour.ToString().PadLeft(2, '0')}:{minute.ToString().PadLeft(2, '0')}");
                }
            }
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            int idJuego = 0;
            string horaInicioTexto = CBInicio.SelectedItem.ToString();
            string horaFinTexto = CBFin.SelectedItem.ToString();
            string fechaTexto = txtFecha.Text.Trim();

            // Convertir la hora de inicio de texto a un tipo de datos time
            TimeSpan horaInicio;
            if (!TimeSpan.TryParse(horaInicioTexto, out horaInicio))
            {
                MessageBox.Show("Error: La hora de inicio seleccionada no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Convertir la hora de fin de texto a un tipo de datos time
            TimeSpan horaFin;
            if (!TimeSpan.TryParse(horaFinTexto, out horaFin))
            {
                MessageBox.Show("Error: La hora de fin seleccionada no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Convertir la fecha de texto a un tipo de datos date
            DateTime fecha;
            if (!DateTime.TryParse(fechaTexto, out fecha))
            {
                MessageBox.Show("Error: La fecha ingresada no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string juego = CBJuego.SelectedItem.ToString();
            switch (juego)
            {
                case "PING PONG":
                    idJuego = 1;
                    break;
                case "BILLAR":
                    idJuego = 2;
                    break;
                case "PLAY STATION":
                    idJuego = 3;
                    break;
                default:
                    MessageBox.Show("Error: Juego seleccionado no válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            int idUsuario = ((Form2)this.Owner).GetIdUsuario();

            string query = "INSERT INTO reserva (hora_inicio, hora_fin, fecha, id_Usuario, id_Juego) VALUES (@HoraInicio, @HoraFin, @Fecha, @IdUsuario, @IdJuego)";

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@HoraInicio", horaInicio);
                        cmd.Parameters.AddWithValue("@HoraFin", horaFin);
                        cmd.Parameters.AddWithValue("@Fecha", fecha);
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@IdJuego", idJuego);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Reserva realizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Form1 form1 = new Form1();
                            form1.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Error al realizar la reserva.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
