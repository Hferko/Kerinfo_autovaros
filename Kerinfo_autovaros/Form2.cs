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
    public partial class Form2 : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=autovaros;uid=root");
        MySqlCommand command;
        public Form2()
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
        {//Mindegyik gombtól ide fogunk ugrani, ez a végrehajtásnak a része, query kapja meg a gomboktól ideküldött stringet, amiben az utasítások vannak.

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
        
        private void button1_Click(object sender, EventArgs e)
        {
            string raktarkeszletnovel = $"INSERT INTO raktar(marka,szin,kod,id) VALUES('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}')";
            //Ez itt egy sorban van, raktár nevű táblába a márka szin kod id-hoz tartozó új adatokat felvesszük a textboxokkal.  
            executeQuery(raktarkeszletnovel);
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
            catch (Exception kivetel)
            {

                MessageBox.Show(kivetel.Message);
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

        private void button2_Click(object sender, EventArgs e)
        {
            //raktár nevű táblába a márka szin kod-hoz tartozó új adatokat felvesszük a textboxokkal.
            string igenyfelvesz = $"INSERT INTO igeny(marka,szin,kod,id) VALUES('{textBox5.Text}','{textBox6.Text}','{textBox7.Text}','')";
            executeQuery(igenyfelvesz);
        }

        private void Form2_FormClosed_1(object sender, FormClosedEventArgs e)
        {//példányosítjuk form1-et, hogy meg lehessen jeleníteni form2 bezárásakor, minden másik formnál is megcsináljuk.
            var form1 = new Form1();
            form1.Show();
        }
    }
}
