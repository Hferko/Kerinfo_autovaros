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
    public partial class Form5 : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=autovaros;uid=root"); //itt érdemes azonnal a database-hez felvenni az adatbázist is ez az autovaros lesz.
        MySqlCommand command;
        public Form5()
        {
            InitializeComponent();
            button1.Enabled = false;
        }
        public void openConnection() //Lényegében kihagyható lépés, ha mindenhol használjuk a connection.Open()-t vagy a connection.Close()-t.
        {
            if (connection.State == ConnectionState.Closed) // Megnézzük, hogy van-e kapcsolat, ha nincs csak akkor nyitjuk meg.
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
                    System.Threading.Thread.Sleep(1);
                }
                else
                {
                    System.Threading.Thread.Sleep(1);
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

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form1 = new Form1();
            form1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM igeny", connection);
                openConnection();
                DataSet ds = new DataSet();
                adapter.Fill(ds, "igeny");
                dataGridView2.DataSource = ds.Tables["igeny"];
                closeConnection();
            }
            catch (Exception hiba)
            {

                MessageBox.Show(hiba.Message);
            }
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM raktar", connection);
                openConnection();
                DataSet ds = new DataSet();
                adapter.Fill(ds, "raktar");
                dataGridView1.DataSource = ds.Tables["raktar"];
                closeConnection();
            }
            catch (Exception hiba)
            {

                MessageBox.Show(hiba.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> raktarmarka = new List<string>();
            List<string> raktarszin = new List<string>();
            List<string> raktarkod = new List<string>();
            List<string> raktarid = new List<string>();

            List<string> igenymarka = new List<string>();
            List<string> igenyszin = new List<string>();
            List<string> igenykod = new List<string>();
            List<string> igenyid = new List<string>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            { //Feltöltjük a Listákat a DGV soraiból.
                raktarmarka.Add(row.Cells[0].FormattedValue.ToString());
                raktarszin.Add(row.Cells[1].FormattedValue.ToString());
                raktarkod.Add(row.Cells[2].FormattedValue.ToString());
                raktarid.Add(row.Cells[3].FormattedValue.ToString());
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                igenymarka.Add(row.Cells[0].FormattedValue.ToString());
                igenyszin.Add(row.Cells[1].FormattedValue.ToString());
                igenykod.Add(row.Cells[2].FormattedValue.ToString());
                igenyid.Add(row.Cells[3].FormattedValue.ToString());
            }
            // Megkeressük az egyezőséget a két listában. Ha megvan, a raktárból töröljük a találatot és a raktár id-ját beírjuk az igény id-jába.
            for (int i = 0; i < igenymarka.Count; i++)
            {
                for (int j = 0; j < raktarmarka.Count; j++)
                {
                    if (igenymarka[i] == raktarmarka[j] && igenyszin[i] == raktarszin[j] && igenykod[i] == raktarkod[j])
                    {
                        igenyid[i] = raktarid[j];
                        string töröl = $"DELETE FROM raktar WHERE id='{raktarid[j]}' ";
                        executeQuery(töröl);
                        string módosít = $"UPDATE igeny SET id='{igenyid[i]}' WHERE id='{""}'AND marka='{igenymarka[i]}' AND szin='{igenyszin[i]}' AND kod='{igenykod[i]}' ";
                        executeQuery(módosít);
                    }
                }
            }
        }
    }
}
