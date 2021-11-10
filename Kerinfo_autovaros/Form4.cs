using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Kerinfo_autovaros
{
    public partial class Form4 : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=autovaros;uid=root");
        MySqlCommand command;
        string id = "", igenykód = "", igenyszín = "", igenyid = "", igenymarka = "";

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM raktar", connection);
                openConnection();
                DataSet ds = new DataSet();
                adapter.Fill(ds, "raktar");
                dataGridView1.DataSource = ds.Tables["raktar"];
                closeConnection();
            }
            catch (Exception kivétel)
            {

                MessageBox.Show(kivétel.Message);
            }
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM igeny", connection);
                openConnection();
                DataSet ds = new DataSet();
                adapter.Fill(ds, "igeny");
                dataGridView2.DataSource = ds.Tables["igeny"];
                closeConnection();
            }
            catch (Exception kivétel)
            {

                MessageBox.Show(kivétel.Message);
            }
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form1 = new Form1();
            form1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        { //id is módosítható mivel nem a textből vesszük a tartalmát hanem lentebb elmentjük.
            string módosítás = $"UPDATE raktar SET marka='{textBox1.Text}', szin='{textBox2.Text}', kod='{textBox3.Text}', id='{textBox4.Text}' WHERE id='{id}' ";
            executeQuery(módosítás);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Kiszedjük a textekbe a cellákból az adatokat de id-t külön eltároljuk, hogy az is módosítható legyen
            dataGridView1.CurrentRow.Selected = true;
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();

            id = textBox4.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Mivel itt nincs id kigyűjtjük a sor kattintása után mindegyik textből változókba az adatokat így a text szabadon átírható.
            string módosítás = $"UPDATE igeny SET marka='{textBox5.Text}', szin='{textBox6.Text}', kod='{textBox7.Text}' WHERE marka='{igenymarka}' AND szin='{igenyszín}' AND kod='{igenykód}' AND id='{igenyid}'";

            executeQuery(módosítás);
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Kiszedjük a textekbe a cellákból az adatokat és utána elmentjük egy-egy változóba ezeket, hogy a textek megváltoztathatók legyenek.
            dataGridView2.CurrentRow.Selected = true;
            textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            textBox7.Text = dataGridView2.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            //textBox8.Text = dataGridView2.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();

            igenymarka = textBox5.Text;
            igenyszín = textBox6.Text;
            igenykód = textBox7.Text;
            //igényid = textBox8.Text;
        }

        public Form4()
        {
            InitializeComponent();
        }
        public void openConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public void executeQuery(string query)
        {
            try
            {
                openConnection();
                command = new MySqlCommand(query, connection);
                if (command.ExecuteNonQuery() >= 1)
                {
                    MessageBox.Show("Végrehajtva!");
                }
                else
                {
                    MessageBox.Show("Végrehajtás sikertelen!");

                }
            }
            catch (Exception kivétel)
            {

                MessageBox.Show(kivétel.Message);
            }
            finally
            {
                closeConnection();
            }
        }
    }
}
