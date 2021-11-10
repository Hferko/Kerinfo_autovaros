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

    public partial class Form3 : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=autovaros;uid=root");
        MySqlCommand command;
        public Form3()
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
            catch (Exception kivetel)
            {

                MessageBox.Show(kivetel.Message);
            }
            finally
            {
                closeConnection();
            }
        }

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
            catch (Exception kivetel)
            {

                MessageBox.Show(kivetel.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Törlés a textbox1.text-ben talátak alapján
            string torol = $"DELETE FROM raktar WHERE id='{ textBox1.Text}'";
            executeQuery(torol);
        }
               

        private void button2_Click(object sender, EventArgs e)
        {
            //mivel igénynél nincs id a textboxok tartalma alapján törünk
            string torol = $"DELETE FROM igeny WHERE marka='{ textBox2.Text}'AND szin='{textBox3.Text}' AND kod='{textBox4.Text}'";
            executeQuery(torol);
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //DGV2-nél keresünk cellclick eseményt és ha rákattintunk egy sorra beírja a textboxokba a sor tartalmát.
            dataGridView2.CurrentRow.Selected = true;
            textBox2.Text = dataGridView2.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            textBox3.Text = dataGridView2.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            textBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
        }

        private void Form3_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            var form1 = new Form1();
            form1.Show();
        }
    }
}
