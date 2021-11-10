using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Kerinfo_autovaros
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=autovaros;uid=root"); //itt érdemes azonnal a database-hez felvenni az adatbázist is ez az autovaros lesz.
        MySqlCommand command;

        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            toolStripTextBox2.Text = DateTime.Now.ToString("HH : mm : ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripTextBox1.Text = DateTime.Now.ToString("HH : mm : ss");
        }

        public void openConnection()  //kapcsolat megnyitása ha a kapcsolat zárva van
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void closeConnection() //kapcsolat zárása ha az nyitva van
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //kiírjuk a tábla tartalmát a datagridview-ba.
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM raktar", connection); //adapter megkapja a táblát
                openConnection();
                DataSet ds = new DataSet();
                adapter.Fill(ds, "raktar"); //Adapterből feltöltjük ds-t
                dataGridView1.DataSource = ds.Tables["raktar"];//Datagridview feltöltése a ds-ben eltárolt tábla alapján
                closeConnection();
            }
            catch (Exception kivetel)
            {

                MessageBox.Show(kivetel.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//igény adattáblát kiírjuk a DGV2-be
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {//A menustrip kattintása, először példányosítjuk a Formokat, hogy a megfelelő helyre kattintva megtudjuk jeleníteni őket.
            var form2 = new Form2();
            var form3 = new Form3();
            var form4 = new Form4();
            var form5 = new Form5();

            string text = e.ClickedItem.Text; //megnézzük mi a textje annak amire kattintottunk, ennek függvényében fogjuk a formokat is megjeleníteni
            switch (text)
            {
                case "ADATBÁZIS BŐVÍTÉSE":
                    this.Hide();
                    form2.Show();
                    break;
                case "TÖRLÉS AZ ADATBÁZISBÓL":
                    this.Hide();
                    form3.Show();
                    break;
                case "ADATBÁZIS MÓDOSÍTÁSA":
                    this.Hide();
                    form4.Show();
                    break;
                case "IGÉNYEK KISZOLGÁLÁSA":
                    this.Hide();
                    form5.Show();
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
            //bezár mindent, nem lesz olyan, hogy a háttérben fut a form4 és nem lehet bezárni. 
        }
    }
}
