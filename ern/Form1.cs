using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using ClosedXML.Excel;
using System.Globalization;
using System.Drawing;

namespace ern
{

    public partial class Form1 : Form
    {
        public class radnici
        {
            public string id { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string datumrodjenja { get; set; }
            public string rfid { get; set; }
            public string rfid2 { get; set; }
            public string rfidhex { get; set; }
            public string lokacija { get; set; }
            public string mt { get; set; }
        }

        public class weekID
        {
            public string id { get; set; }
            public string daterange { get; set; }
        }
        public class radnicil
        {
            public string id { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string hala { get; set; }
            public string smjena { get; set; }
            public string linija { get; set; }
            public string datumrodjenja { get; set; }
            public string lokacija { get; set; }
        }
        public class ERV
        {
            public int id { get; set; }
            public int minuta { get; set; }
            public string dan { get; set; }
        }
        public class ERV2
        {
            public int id { get; set; }
            public string dan { get; set; }
            public string prezime { get; set; }
            public string ime { get; set; }
            public string dosao { get; set; }
            public string otisao { get; set; }
            public int minuta { get; set; }
            public string mt { get; set; }
            public string lokacija { get; set; }
        }

        public List<DateTime> praznici = new List<DateTime>();
        public List<radnici> radnicii = new List<radnici>();
        public List<radnicil> radnicip = new List<radnicil>();
        public List<weekID> tjedni = new List<weekID>();
        public List<ERV> ervii = new List<ERV>();
        public List<ERV2> ervii2 = new List<ERV2>();
        public string connectionString   = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=sa;Password=AdminFX9.";
        public string connectionString2 = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";
        public string selectedid;
        public string idloged,idprijave,korisnik;

        public Form1()
        {
            InitializeComponent();

            DateTimePicker dynamicDTP = new DateTimePicker();
            dynamicDTP.ValueChanged += new System.EventHandler(dateP_datumUP_ValueChanged);
            idloged = LoginForm.idusera1.Trim();
            idprijave = LoginForm.idprijave1.Trim();
            korisnik = LoginForm.korisnik1.Trim();
            lbl_user.Text = idloged;

            //connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=fx_public;Password=.";
            string sql = "select * from praznici";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();

            dataGridView2.Visible = false;
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
            GridPregledRadnika.Visible = false;

            panel14.Visible = false;
            panel21.Visible = false;
            panel31.Visible = false;
            panel41.Visible = false;
            panelRucniUnos.Visible = false;
            comboBox8.SelectedText = DateTime.Now.Year.ToString();   // godina na mjesečnom izvještaju

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM korisnici where id='" + idloged + "'", cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    // reader["Datum"].ToString();
                    lbl_user.Text = "Korisnik: " + reader["Username"].ToString();

                }
                cn.Close();
            }

            // punjenje liste praznika
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM praznici", cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    // reader["Datum"].ToString();
                    DateTime dt = (DateTime)reader["Datum"];
                    praznici.Add(dt);  // ??? not sure what to put here  as add is not available
                }
                cn.Close();
            }

            // punjenje liste radnika
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                //SqlCommand sqlCommand = new SqlCommand("SELECT [Radnik id] as id,[ime x] as ime,[prezime x] as prezime,rfid,rfid2,rfidhex,lokacija,mt FROM radniciAT0 union all select [Radnik id] as id,[ime x] as ime,[prezime x] as prezime,rfid,rfid2,rfidhex,lokacija,mt from radniciTB0 where mt in ( 700,702,703,710,716) order by prezime", cn);
                // SqlCommand sqlCommand = new SqlCommand("SELECT id, ime, prezime,rfid,rfid2,rfidhex,lokacija,mt FROM  radnici_ where mt in ( 700,702,703,710,716)  order by prezime", cn);
                //SqlCommand sqlCommand = new SqlCommand("SELECT id, ime, prezime,rfid,rfid2,rfidhex,lokacija,mt FROM  radnici_  where neradi=0 order by prezime", cn);
                SqlCommand sqlCommand = new SqlCommand("SELECT id, ime, prezime,rfid,rfid2,rfidhex,lokacija,mt FROM  radnici_  where neradi>=0 order by prezime", cn);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    // reader["Datum"].ToString();

                    radnici radnik = new radnici();
                    radnik.id = ((int.Parse)(reader["ID"].ToString())).ToString();
                    radnik.ime = reader["Ime"].ToString();
                    radnik.prezime = reader["Prezime"].ToString();
                    radnik.rfidhex = reader["RFIDHex"].ToString();
                    radnik.rfid2 = reader["RFID2"].ToString();
                    radnik.rfid = reader["RFID"].ToString();
                    radnik.lokacija = reader["lokacija"].ToString();
                    radnik.mt = reader["mt"].ToString();

                    //  radnik.datumrodjenja = reader["DatumRodjenja"].ToString();
                    radnicii.Add(radnik);
                }
                cn.Close();

            }

            dataGridView2.Visible = false;
            if (idloged=="23")
            {
              ručniUnosToolStripMenuItem.PerformClick()   ;
                mjesečniPregledToolStripMenuItem.Enabled = false;
                pregledPrisustvaToolStripMenuItem.Enabled = false;
                karticeToolStripMenuItem.Enabled = false;
                pregledRadnikaToolStripMenuItem.Enabled = false;
                planiraniRasporedToolStripMenuItem.Enabled = false;
                odsustvaToolStripMenuItem.Enabled = false;
            }

        }

        // pregled prisustva 
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            panelRucniUnos.Visible = false;
            GridPregledRadnika.Visible = false;

            button2.Visible = true;
            button2.Text = "Export to excell ";
            //string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=fx_public;Password=.";
//            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";

            string ime1 = textBox1.Text;
            string sql;
            int lokacija1 = comboBox1.SelectedIndex;
            //string dat1 = DateTime.ParseExact(dateTimePicker1.Value.ToShortDateString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);

            string dat1 = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day;
            string dat2 = dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day + " 23:59:59.00";
            string lokacija;

            lokacija = "";
            switch (lokacija1 + 1)
            {
                case 1:
                    lokacija = "_";    // sve
                    break;
                case 2:
                    lokacija = "22293";    // uprava ulaz
                    break;
                case 3:
                    lokacija = "544666574";    // tehnologija
                    break;
                case 4:
                    lokacija = "544666577";   // garderoba p1
                    break;
                case 5:
                    lokacija = "544666590";   // hala3
                    break;
                case 6:
                    lokacija = "544666595";   // hala4
                    break;
                case 7:
                    lokacija = "544666584";   // zona
                    break;
                default:
                    break;
            }


            if (lokacija == "" || lokacija == "_")
                sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.eventtype!='SP23'  and ( e.dt>='" + dat1 + "' and e.dt<='" + dat2 + "') AND (lastname+' '+firstname) like '%" + textBox1.Text + "%' ";
            else
                sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.eventtype!='SP23'  and ( e.dt>='" + dat1 + "' and e.dt<='" + dat2 + "') AND  ( lastname+' '+firstname) like '%" + textBox1.Text + "%'  and  e.device_id='" + lokacija+ "' " ;

            if (checkBox1.Checked)
            {
                sql = sql + " and eventtype='SP40' order by dt desc";
            }
            else
            {
                sql = sql + " order by dt desc";
            }

            //sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.dt>='" + dat1+"'";

            SqlConnection connection = new SqlConnection(connectionString2);    // rfind
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "event";

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //dataGridView1.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 120;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pocetniEkran();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // export to excell iz pregled prisustva
        private void button2_Click(object sender, EventArgs e)
        {

            //Creating DataTable
            DataTable dt = new DataTable();

            //Adding the Columns
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                dt.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                dt.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                    }
                }
            }

            //Exporting to Excel
            string folderPath = "C:\\ERV\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Customers");
                wb.SaveAs(folderPath + "EvidencijaPrisustva.xlsx");

                FileInfo fi = new FileInfo("C:\\erv\\EvidencijaPrisustva.xlsx");
                if (fi.Exists)
                {
                    System.Diagnostics.Process.Start("C:\\erv\\EvidencijaPrisustva.xlsx");
                }
                else
                {
                    //file doesn't exist
                }

            }
            button2.Text = "Export done";
        }


        // sumarni mjesečni pregled po danima
        private void button3_Click(object sender, EventArgs e)
        {
            string dat22;
            int ukupnoVrijeme, sati1, godina_; // uupno vrijeme u mjesecu, dolazi na kraj tabele
            string godina;

            dataGridView1.Visible = false;
            dataGridView2.Visible = true;
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
            GridPregledRadnika.Visible = false;

            dataGridView2.Width = ActiveForm.Width - 100;
            dataGridView2.Height = ActiveForm.Height - 300;

            if (comboBox8.SelectedIndex != -1)
            {
                godina = comboBox8.SelectedItem.ToString();// izabrana godina
                godina_ = (int.Parse)(comboBox8.SelectedItem.ToString());
            }
            else
            {
                godina = DateTime.Now.Year.ToString();  // izabrana godina
                godina_ = DateTime.Now.Year;
            }
            DataTable dt1 = new DataTable();
            int mjesec1 = comboBox2.SelectedIndex + 1;   // izabrani mjesec

            string wd;
            if (comboBox2.SelectedIndex == -1)
            {
                return;
            }

            dt1.Columns.Add(new DataColumn("Prezime i ime", typeof(string)));
            int lastday = DateTime.DaysInMonth(godina_, (comboBox2.SelectedIndex + 1));

            for (int ii = 1; ii <= lastday; ii++)  // napuni imena kolona
            {
                dt1.Columns.Add(new DataColumn("Day" + ii.ToString(), typeof(string)));
                DateTime dateValue = new DateTime(godina_, (comboBox2.SelectedIndex + 1), ii);
                wd = dateValue.ToString("ddd");
                dt1.Columns["Day" + ii.ToString()].ColumnName = String.Format(ii.ToString()) + "-" + wd;
            }

            dt1.Columns.Add(new DataColumn("Ukupno", typeof(string)));

            dataGridView2.DataSource = dt1;
            object[] array = new object[lastday + 2];
            foreach (var radnik in radnicii)
            {
                array[0] = radnik.prezime + " " + radnik.ime + "-" + radnik.id;  // prezim i ime
                ukupnoVrijeme = 0;
                string connectionString2 = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=fx_public;Password=.";
                SqlDataReader rdr = null;
                using (SqlConnection cn = new SqlConnection(connectionString2))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.ERN_zbroji", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FXid", SqlDbType.Int, 5).Value = radnik.id;  // dodati parametar godinu i mjesec
                    cmd.Parameters.Add("@Godina", SqlDbType.Int, 5).Value = godina_;  // dodati parametar godinu i mjesec
                    cmd.Parameters.Add("@Mjesec", SqlDbType.Int, 5).Value = mjesec1;  // dodati parametar godinu i mjesec

                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        // reader["Datum"].ToString();
                        if (rdr.HasRows)
                        {
                            ERV erv1 = new ERV();
                            erv1.id = (int.Parse)(rdr["fxid"].ToString());
                            erv1.minuta = (int.Parse)(rdr["minuta"].ToString());
                            erv1.dan = rdr["dan"].ToString();
                            ervii.Add(erv1);
                            if (radnik.id == "1173")
                            {
                                int iw = 1;
                            }
                        }
                    }
                    cn.Close();

                }

                string dan1;

                for (int ii = 1; ii <= lastday; ii++)
                {
                    array[ii] = "0";
                    dan1 = ii.ToString() + "." + (comboBox2.SelectedIndex + 1).ToString() + "." + godina;
                    if (radnik.id == "1173")
                    {
                        int iw = 1;
                    }
                    foreach (var ervv in ervii)
                    {
                        if (ervv.dan == dan1)
                        {
                            sati1 = ervv.minuta / 60;
                            array[ii] = sati1.ToString() + "h " + (ervv.minuta - sati1 * 60).ToString() + "min";
                            ukupnoVrijeme = ukupnoVrijeme + ervv.minuta;   // ukupno vrijeme u minutama
                        }
                        //else
                        //{
                        //    array[ii] = 0;
                        //}
                    }

                }

                sati1 = ukupnoVrijeme / 60;
                array[lastday + 1] = sati1.ToString() + "." + (ukupnoVrijeme - sati1 * 60).ToString();

                if (radnik.id == "1173")
                {
                    int iw = 1;
                }

                dt1.Rows.Add(array);  // puni red sa podacima za tog radnika
                ervii.Clear();
                Array.Clear(array, 0, array.Count() - 1);

            }
            //dt.Rows.Add(DateTime.Now.AddDays(-1));
            //dt.Rows.Add(DateTime.Now.AddDays(1));
            //dt.Rows.Add(DateTime.Now.AddDays(2));
            //dt.Rows.Add(DateTime.Now.AddDays(-2));

            dataGridView2.DataSource = dt1;


            DataGridViewRow row1 = dataGridView2.Rows[0];
            dataGridView2.EnableHeadersVisualStyles = false;
            //    dataGridView2.Columns["NameOfColumn"].DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
            string s = "";

            for (int ii = 1; ii <= lastday; ii++)
            {
                s = dataGridView2.Columns[ii - 1].HeaderText;
                if (s.Contains("sub"))
                {
                    dataGridView2.Columns[s].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;  // subota
                }
                if (s.Contains("ned"))
                {
                    dataGridView2.Columns[s].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;    // nedjelja
                }

                dat22 = ii.ToString() + "." + mjesec1.ToString() + "." + godina;
                DateTime MyDateTime;
                MyDateTime = new DateTime(godina_, mjesec1, ii);
                s = dataGridView2.Columns[ii].HeaderText;
                if (praznici.Contains(MyDateTime))
                {
                    dataGridView2.Columns[s].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;      // praznici         
                }
            }

            FreezeBand(dataGridView2.Columns[1]);
            dataGridView2.AutoResizeColumns();
            dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView2.ReadOnly = true;

            return;

            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            DateTime dat1d = dateTimePicker1.Value;
            DateTime dat2d = dateTimePicker2.Value;

            while (dat1d <= dat2d)
            {
                string day1 = dat1d.ToShortDateString();
                table.Columns.Add(day1, typeof(int));
                dat1d = dat1d.AddDays(1);
            }

            // Here we add five DataRows.
            //table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            //table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            //table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            //table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            //table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

            button2.Visible = true;
            button2.Text = "Export to excell ";
            //string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=fx_public;Password=.";

            string ime1 = textBox1.Text;
            string sql;
            int lokacija1 = comboBox1.SelectedIndex;
            //string dat1 = DateTime.ParseExact(dateTimePicker1.Value.ToShortDateString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);

            string dat1 = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day;
            string dat2 = dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day + " 23:59:59.00";
            string lokacija;

            lokacija = "";
            switch (lokacija1 + 1)
            {
                case 1:
                    lokacija = "_";    // sve
                    break;
                case 2:
                    lokacija = "22293";    // uprava ulaz
                    break;
                case 3:
                    lokacija = "544666574";    // tehnologija
                    break;
                case 4:
                    lokacija = "544666577";   // garderoba p1
                    break;
                case 5:
                    lokacija = "544666590";   // hala3
                    break;
                case 6:
                    lokacija = "544666595";   // hala4
                    break;
                case 7:
                    lokacija = "544666584";   // zona
                    break;
                default:
                    break;
            }

            if (lokacija == "")
                sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.eventtype!='SP23'  and ( e.dt>='" + dat1 + "' and e.dt<='" + dat2 + "') AND lastname like '%" + textBox1.Text + "%' order by user,dt asc";
            else
                sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.eventtype!='SP23'  and ( e.dt>='" + dat1 + "' and e.dt<='" + dat2 + "') AND  ( lastname like '%" + textBox1.Text + "%'  and  e.device_id='" + lokacija + "') order by user,dt asc";

            //sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.dt>='" + dat1+"'";

            SqlConnection connection = new SqlConnection(connectionString2);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "event";
            string u1;
            DateTime d1, d11, d12;
            u1 = "";
            d12 = DateTime.Now;
            int i = 0;
            string ds1 = dataGridView1.Rows[0].Cells[0].Value.ToString();
            d1 = DateTime.ParseExact((dataGridView1.Rows[0].Cells[0].Value.ToString()), "yyyy-mm-dd 00:00:00", null);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (u1 == (row.Cells[8].Value.ToString()))
                {
                    if (d1 == DateTime.ParseExact((row.Cells[0].Value.ToString()), "yyyy-mm-dd", null) && u1 == row.Cells[8].Value.ToString())
                    {
                        d12 = DateTime.ParseExact((row.Cells[0].Value.ToString()), "yyyy-mm-dd", null);
                    }
                    else
                    {
                        DateTime start = DateTime.Now;
                        // Do some work
                        TimeSpan timeDiff = d12 - d1;
                        double min1 = timeDiff.TotalMinutes;
                        d1 = DateTime.ParseExact((row.Cells[0].Value.ToString()), "yyyy-mm-dd", null);

                    }

                }
                else
                {
                    u1 = ((DataGridViewCheckBoxCell)row.Cells[8]).Value.ToString();  // user
                }

            }
            //dataGridView1.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 120;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void plaćeniDupustToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // Umetanje plana prisustva
        private void planiraniRasporedToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void mjesečniPregledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            panel21.Visible = false;
            panel31.Visible = false;
            panelRucniUnos.Visible = false;
            panel41.Visible = true;
            panel21.SendToBack();
            panel31.SendToBack();
            panel41.BringToFront();
            dataGridView1.Visible = false;
            dataGridView2.Visible = true;
            dataGridView3.Visible = false;
            GridPregledRadnika.Visible = false;
        }

        // pregled pristustva pojedinačno
        private void pregledPrisustvaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            pocetniEkran();
            panel31.Visible = true;
            dataGridView1.Visible = true;
            dataGridView1.BringToFront();

            dataGridView1.Width = ActiveForm.Width * 2 / 3 - 100;
            dataGridView1.Height = ActiveForm.Height - 300;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            dateTimePicker2.Value = DateTime.Now.AddDays(-1);

            button2.Visible = false;
            button2.Text = "Export to excell ";
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // button za umetanje radnika u listu za raspored
        private void button4_Click(object sender, EventArgs e)
        {
            string[] imeprez = new string[3];
            radnicil radnik = new radnicil();
            radnik.id = comboBox7.SelectedValue.ToString();
            imeprez = comboBox7.Text.Split(' ');
            radnik.prezime = imeprez[0];
            radnik.ime = imeprez[1];

            try
            {

                switch (comboBox4.SelectedIndex)
                {
                    case 0:
                        radnik.hala = "1";
                        break;
                    case 1:
                        radnik.hala = "3";
                        break;
                    case 2:
                        radnik.hala = "4";
                        break;
                    case 3:
                        radnik.hala = "5";
                        break;
                    case 4:
                        radnik.hala = "6";
                        break;
                }

            }
            catch (NullReferenceException ee)
            {

                MessageBox.Show("Odaberite halu !");
            }

            try
            {
                radnik.smjena = (comboBox5.SelectedIndex + 1).ToString();
            }
            catch (NullReferenceException ee)
            {
                MessageBox.Show("Odaberite smjenu !");
            }

            try
            {
                radnik.linija = (comboBox6.SelectedIndex + 1).ToString();
            }
            catch (NullReferenceException ee)
            {
                MessageBox.Show("Odaberite liniju !");
            }
            radnicip.Add(radnik);

            dataGridView3.DataSource = null;
            dataGridView3.DataSource = radnicip;
            dataGridView3.Update();
            dataGridView3.Refresh();

        }


        // obriši izabranog radnika 
        private void button5_Click(object sender, EventArgs e)
        {
            radnicil radnik = new radnicil();
            string id1 = "";
            foreach (DataGridViewRow item in this.dataGridView3.SelectedRows)
            {
                radnik.id = item.Cells[0].Value.ToString();
                radnik.prezime = item.Cells[0].Value.ToString();

                //try
                //{
                //    radnik.hala = comboBox4.SelectedValue.ToString();
                //}
                //catch (NullReferenceException ee)
                //{

                //    MessageBox.Show("Odaberite halu !");
                //}

                //try
                //{
                //    radnik.smjena = comboBox5.SelectedValue.ToString();
                //}
                //catch (NullReferenceException ee)
                //{
                //    MessageBox.Show("Odaberite smjenu !");
                //}

                //try
                //{
                //    radnik.linija = comboBox6.SelectedValue.ToString();
                //}
                //catch (NullReferenceException ee)
                //{
                //    MessageBox.Show("Odaberite liniju !");
                //}

            }
            //radnik.id = selectedid;

            //radnik.id = selectedid;

            //radnik.prezime = comboBox7.Text;

            int ibrisi = -1, ii = 0;
            foreach (var rad1 in radnicip)
            {
                if (rad1.id == radnik.id)
                {
                    ibrisi = ii;
                }
                ii++;
            }
            if (ibrisi >= 0)
            {
                radnicip.RemoveAt(ibrisi);
            }

            dataGridView3.DataSource = null;
            dataGridView3.DataSource = radnicip;
            dataGridView3.Update();
            dataGridView3.Refresh();

        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {

        }
        // spremi podatake o smjenama u bazu
        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlDataReader reader;
                string wid = comboBox3.SelectedValue.ToString();
                string sql1, datum11;
                datum11 = DateTime.Now.ToShortDateString();
                datum11 = "04-01-2017";
                // foreach (DataGridViewRow item in this.dataGridView3.Rows)
                // {
                foreach (var radnic1 in radnicip)
                {
                    sql1 = "'" + wid + "','" + datum11 + "','" + radnic1.hala + "','" + radnic1.smjena + "','" + radnic1.linija + "','" + radnic1.linija + "','" + radnic1.id + "','" + radnic1.ime + "','" + radnic1.prezime + "',''";
                    SqlCommand sqlCommand = new SqlCommand("insert into rasporedradnika (Weekid,datum,Hala,Smjena,Linija,radnomjesto,RadnikId,Ime,prezime,comment) values ( " + sql1 + ")", cn);
                    sqlCommand.ExecuteNonQuery();
                }
                //}
                cn.Close();

            }
            radnicip.Clear();

            dataGridView3.Visible = false;
            panel21.Visible = false;

        }


        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        //private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        //{


        //}

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView2.CurrentCell.ColumnIndex.Equals(3) && e.RowIndex != -1)
            if (e.ColumnIndex >= 1 && e.RowIndex != -1)
            {
                if (dataGridView2.CurrentCell != null && dataGridView2.CurrentCell.Value != null)
                {
                    int columnIndex = dataGridView2.CurrentCell.ColumnIndex;
                    string columnName = dataGridView2.Columns[columnIndex].Name;
                    string dat11, id1, dat12;
                    dat11 = ""; id1 = ""; dat12 = "";
                    int l = columnName.IndexOf("-");
                    if (l > 0)
                    {
                        dat11 = columnName.Substring(0, l);
                    }

                    id1 = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    l = id1.IndexOf("-");
                    if (l > 0)
                    {
                        id1 = id1.Substring(l + 1, id1.Length - 1 - l);
                    }

                    dat12 = (comboBox2.SelectedIndex + 1).ToString() + "/" + dat11 + "/" + (comboBox8.SelectedItem) + " 23:59.00";
                    dat11 = (comboBox2.SelectedIndex + 1).ToString() + "/" + dat11 + "/" + (comboBox8.SelectedItem) + " 00:00:00";

                    //dat1 = 
              //      string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=fx_public;Password=.";

                    string sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija  from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.eventtype!='SP23'  and ( e.dt>='" + dat11 + "' and e.dt<='" + dat12 + "') AND  ( b.extid=" + id1 + " ) ";

                    //sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.dt>='" + dat1+"'";

                    SqlConnection connection = new SqlConnection(connectionString2);
                    SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                    DataSet ds = new DataSet();
                    connection.Open();

                    dataadapter.Fill(ds, "event");
                    connection.Close();
                    panel1.Visible = true;
                    dataGridView4.Visible = true;
                    dataGridView4.BringToFront();

                    dataGridView4.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    dataGridView4.DataSource = ds;
                    dataGridView4.DataMember = "event";

                    dataGridView4.AutoResizeColumns();
                    dataGridView4.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                    //dataGridView4.Visible = false;
                }
            }

        }

        // ručni unos događaja
        private void ručniUnosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            GridPregledRadnika.Visible = false;

            panel21.Visible = false;
            panel31.Visible = false;
            panel41.Visible = false;

            panelRucniUnos.Visible = true;
            panelRucniUnos.BringToFront();
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "dd.MM.yyyy HH:mm";

            var dataSource = new List<radnici>();
            foreach (var radnikk in radnicii)
            {
                dataSource.Add(new radnici() { prezime = radnikk.prezime + " " + radnikk.ime + " - " + radnikk.id.ToString() + " ", id = radnikk.id });
            }

            comboBox9.MaxDropDownItems = 60;
            this.comboBox9.DataSource = dataSource;
            this.comboBox9.DisplayMember = "prezime";
            this.comboBox9.ValueMember = "id";
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // unesi ( potvrda) ručno vrijeme
        private void button7_Click(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";
            string rfid1, no, no2, dt;

            string ime1 = textBox1.Text;
            string sql, sql1, sql0;
            int lokacija1 = comboBox11.SelectedIndex;   // lokacija uređaja
            //string dat1 = DateTime.ParseExact(dateTimePicker1.Value.ToShortDateString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);

            string dat1 = dateTimePicker4.Value.Month + "." + dateTimePicker4.Value.Day + "." + dateTimePicker4.Value.Year + " " + dateTimePicker4.Value.Hour + ":" + dateTimePicker4.Value.Minute;

            string lokacija;

            //DateTimePicker dateTimePicker4 = new DateTimePicker();

            // Set the MinDate and MaxDate.
            //dateTimePicker4.MinDate = new DateTime(1985, 6, 20);
            //dateTimePicker4.MaxDate = DateTime.Today;

            //// Set the CustomFormat string.
            //dateTimePicker4.CustomFormat = "MMMM dd, yyyy - dddd";
            //dateTimePicker4.Format = DateTimePickerFormat.Custom;

            //// Show the CheckBox and display the control as an up-down control.
            //dateTimePicker4.ShowCheckBox = true;
            //dateTimePicker4.ShowUpDown = true;

            lokacija = "";
            lokacija = "";
            switch (lokacija1 + 1)
            {
                case 1:
                    lokacija = "_";    // sve
                    break;
                case 2:
                    lokacija = "22293";    // uprava ulaz
                    break;
                case 3:
                    lokacija = "544666574";    // tehnologija
                    break;
                case 4:
                    lokacija = "544666577";   // garderoba p1
                    break;
                case 5:
                    lokacija = "544666590";   // hala3
                    break;
                case 6:
                    lokacija = "544666595";   // hala4
                    break;
                case 7:
                    lokacija = "544666584";   // zona
                    break;
                default:
                    break;
            }
            sql1 = ""; sql0 = "";
            no2 = ""; no = "";
            string IDradnika = (comboBox9.SelectedValue.ToString());
            if (lokacija == "" || lokacija == "_")
            {
                lokacija = "_";   // hala4
                IDradnika = (comboBox9.SelectedValue.ToString());
                foreach (var r in radnicii)
                {
                    if (r.id == IDradnika)
                    {
                        no2 = r.rfid.Substring(2);
                        if (r.rfidhex.Length == 16)
                        {
                            no = r.rfidhex.Substring(8);
                        }
                        else
                        {
                            no = r.rfidhex.Substring(7);
                        }

                    }
                }
                lokacija = "22293";    // uprava ulaz
                sql0 = "INSERT into event (NO,no2,dt,ispaired,[user],device_id,eventtype,tnaevent,optimisticlockfield,gcrecord) values('00000000','0','" + dat1 + "',null,null,'" + lokacija + "','SP138',null,null,null)";
                sql1 = "INSERT into event (NO,no2,dt,ispaired,[user],device_id,eventtype,tnaevent,optimisticlockfield,gcrecord) values('" + no + "','" + no2 + "','" + dat1 + "',null," + IDradnika.ToString() + ",'" + lokacija + "','MI',null,null,null)";

            }
            else
            {
                IDradnika = (comboBox9.SelectedValue.ToString());
                foreach (var r in radnicii)
                {
                    if (r.id == IDradnika)
                    {
                        no2 = r.rfid2.Substring(r.rfid2.IndexOf("-") + 1);
                        if (r.rfidhex.Length == 16)
                        {
                            no = r.rfidhex.Substring(8);
                        }
                        else
                        {
                            no = r.rfidhex.Substring(7);
                        }
                        IDradnika = nadjiId(IDradnika);
                        break;
                    }
                }
                sql0 = "INSERT into rfind.dbo.event (NO,no2,dt,ispaired,[user],device_id,eventtype,tnaevent,optimisticlockfield,gcrecord) values('00000000','0','" + dat1 + "',null,null,'" + lokacija + "','SP138',null,null,null)";
                sql1 = "INSERT into rfind.dbo.event (NO,no2,dt,ispaired,[user],device_id,eventtype,tnaevent,optimisticlockfield,gcrecord) values('" + no + "','" + no2 + "','" + dat1 + "',null," + IDradnika.ToString() + ",'" + lokacija + "','MI',null,null,null)";
                //  sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.eventtype!='SP23'  and ( e.dt>='" + dat1 + "' and e.dt<='" + dat2 + "') AND  ( lastname like '%" + textBox1.Text + "%'  and  e.device_id='" + lokacija + "') order by dt desc";

            }

            ////sql = "select dt Vrijeme,LastName Prezime,u.FirstName Ime,r.name Lokacija,e.Device_ID Uredaj,e.EventType,t.CodeName,b.extid FxId,e.[User],e.No2 Serial_number,e.no RFID_Hex from event e left join badge b on e.No= b.BadgeNo left join [dbo].[User] u on u.extid=b.extid left join eventtype t on e.EventType=t.Code left join reader r on r.id=e.device_id WHERE E.[USER] IS NOT NULL and e.dt>='" + dat1+"'";
            using (SqlConnection openCon = new SqlConnection(connectionString2))
            {
                using (SqlCommand querySaveStaff = new SqlCommand(sql0))
                {
                    querySaveStaff.Connection = openCon;
                    try
                    {
                        openCon.Open();
                        int recordsAffected = querySaveStaff.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // error here
                    }
                    finally
                    {
                        openCon.Close();
                    }
                }
                using (SqlCommand querySaveStaff = new SqlCommand(sql1))
                {
                    querySaveStaff.Connection = openCon;
                    try
                    {
                        openCon.Open();
                        int recordsAffected = querySaveStaff.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // error here
                    }
                    finally
                    {
                        openCon.Close();
                    }
                }

            }

            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into erv_log (datum,korisnik,idprijave,opis) values  ( getdate(),'" + korisnik + "','" + idprijave + "','Ručni unos rad.vremena za [User.id]= "+IDradnika.ToString()+"')", cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                cn.Close();

            }

            pocetniEkran();  // postavi pocetni erkan

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string nadjiId(string id0)
        {
            using (SqlConnection cn0 = new SqlConnection(connectionString))
            {
                cn0.Open();
                SqlCommand sqlCommand0 = new SqlCommand("SELECT oid from [user] where extid=" + id0, cn0);
                SqlDataReader reader0 = sqlCommand0.ExecuteReader();
                while (reader0.Read())
                {
                    // reader["Datum"].ToString();
                    return reader0["OID"].ToString();
                }
            }
            return "";
        }

        private void pocetniEkran()
        {
            panel21.Visible = false;
            panel31.Visible = false;
            panel41.Visible = false;
            panel14.Visible = false;      // unos radnog vremena za radnika
            panelRucniUnos.Visible = false;
            panel_pregled_radnih_mjesta.Visible = false;
            pl_UnosRadnihMjesta.Visible = false;
            pl_IzmjenaRadnogMjesta.Visible = false;
            pl_Planiranje_Unos.Visible = false;
            pl_PregledRadnogVremena.Visible = false;
            pnl_deaktiv.Visible = false;

            dataGridView2.Visible = false;
            dataGridView1.Visible = false;
            dataGridView3.Visible = false;
            GridPregledRadnika.Visible = false;
            pictureBox1.Visible = false;
            panelNovakartica.Visible = false;

            cbl_ListaOdjela.Visible = false;  // na unosu plana
            dateP_datumUP.Visible = false;
            btn_SpremiPlan.Visible = false;
            if (idloged=="23")
            {
                panelRucniUnos.Visible =true;
            }

        }

        // pregled radmnika iz feroimpexa
        private void feroimpexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void tokabuToolStripMenuItem_Click(object sender, EventArgs e)
        {

            pocetniEkran();
            pictureBox1.Visible = true;
            //Image image = Image.FromFile(@"logo-TB.jpg");
            ////// Set the PictureBox image property to this image.
            ////// ... Then, adjust its height and width properties.
            //pictureBox1.Image = image;
            //pictureBox1.Height = image.Height;
            //pictureBox1.Width = image.Width;

            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            //panelRucniUnos.Visible = false;
            GridPregledRadnika.Visible = true;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string sql;

                // sql = "SELECT [radnik id] as Id,[ime x] as Ime,[prezime x] as Prezime,oib as OIB,rfid as RFID ,rfid2 as'Serijski broj' ,rfidhex as 'RFID Hex',lokacija as 'Lokacija', MT as 'Mjesto troška' FROM radniciTB0 order by prezime";
                sql = "SELECT cast( a.[radnik id]  as int) as Id,a.[ime x] as Ime,a.[prezime x] as Prezime,a.oib as OIB,a.rfid as RFID ,a.rfid2 as 'Serijski broj' ,a.rfidhex as 'RFID Hex',l.naziv as 'Lokacija', mt.naziv as 'Mjesto troška' FROM radniciTB0 a left join lokacije l on l.id=a.lokacija left join mjestotroska mt on mt.id = a.mt order by prezime";
                sql = "SELECT cast( a.[id]  as int) as Id,a.[ime] as Ime,a.[prezime] as Prezime,a.rfid as RFID ,a.rfid2 as 'Serijski broj' ,a.rfidhex  as 'RFID Hex',l.naziv as 'Lokacija', mt.naziv as 'Mjesto troška',a.poduzece, rv.naziv as 'Radno vrijeme',a.fixnaisplata Fixna_isplata FROM radnici_ a left join lokacije l on l.id=a.lokacija left join mjestotroska mt on mt.id = a.mt left join rasporedvremena rv on rv.id=a.rv where poduzece='Tokabu' and a.neradi=0  order by prezime ";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                connection.Open();
                dataadapter.Fill(ds, "event");
                connection.Close();

                GridPregledRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                GridPregledRadnika.DataSource = ds;
                GridPregledRadnika.DataMember = "event";

                GridPregledRadnika.Width = ActiveForm.Width * 1 / 2 - 50;
                GridPregledRadnika.Top = ActiveForm.Height * 1 / 10;

                GridPregledRadnika.Height = ActiveForm.Height - 200;

                GridPregledRadnika.AutoResizeColumns();
                GridPregledRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void odsustvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();
        }
        // pregled prisustva2, ime prezime, došao , otišao
        private void button8_Click(object sender, EventArgs e)
        {

            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            panelRucniUnos.Visible = false;
            GridPregledRadnika.Visible = false;

            button2.Visible = true;
            button2.Text = "Export to excell ";
            //string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=fx_public;Password=.";

            string ime1 = textBox1.Text;
            string sql;
            int lokacija1 = comboBox1.SelectedIndex;
            //string dat1 = DateTime.ParseExact(dateTimePicker1.Value.ToShortDateString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);

            string dat1 = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " 00:00:00";
            string dat2 = dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day + " 23:59:59";

            DateTime dat11 = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0, 0, 0);
            DateTime dat22 = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, 0, 0, 0);

            string lokacija;

            lokacija = "";
            switch (lokacija1 + 1)
            {
                case 1:
                    lokacija = "_";    // sve
                    break;
                case 2:
                    lokacija = "22293";    // uprava ulaz
                    break;
                case 3:
                    lokacija = "544666574";    // tehnologija
                    break;
                case 4:
                    lokacija = "544666577";   // garderoba p1
                    break;
                case 5:
                    lokacija = "544666590";   // hala3
                    break;
                case 6:
                    lokacija = "544666595";   // hala4
                    break;
                case 7:
                    lokacija = "544666584";   // zona
                    break;
                default:
                    break;
            }

            CultureInfo provider = CultureInfo.InvariantCulture;
            System.Globalization.DateTimeStyles style = DateTimeStyles.None;

            DateTime dt1;
            dat1 = dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "/" + dateTimePicker1.Value.Year + " 00:00:59";
            dat1 = dateTimePicker2.Value.Month + "/" + dateTimePicker2.Value.Day + "/" + dateTimePicker2.Value.Year + " 00:00:59";

            DateTime.TryParseExact(dat1, "MM/dd/yyyy HH:mm:ss", provider, style, out dt1);

            DateTime dt2;
            DateTime.TryParseExact(dat2, "MM/dd/yyyy HH:mm:ss", provider, style, out dt2);

            dat1 = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " 00:00:59";
            dat1 = dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + "-" + dateTimePicker1.Value.Day + " 00:00:59";
            dat2 = dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day + " 23:59:59";

            string test = "2016-12-01";
            DateTime dt11 = DateTime.ParseExact(test, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            test = "2016-12-31";
            DateTime dt22 = DateTime.ParseExact(test, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=fx_public;Password=.";
            SqlDataReader rdr = null;
            //             ervii2.Clear();
            using (SqlConnection cn = new SqlConnection(connectionString2))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("dbo.sp_VrijemeUlazaIzlaza", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@location", SqlDbType.Int, 5).Value = 500;  // lokacija
                cmd.Parameters.Add("@DatumOd", SqlDbType.DateTime).Value = dt11;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                cmd.Parameters.Add("@DatumDo", SqlDbType.DateTime).Value = dt22;  //DateTime.ParseExact(dat2, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture );  // Od datuma
                cmd.Parameters.Add("@Prezimee", SqlDbType.VarChar).Value = ime1;  // Početak prezimena

                rdr = cmd.ExecuteReader();
                //SqlDataAdapter da;DataSet ds;
                //da = new SqlDataAdapter(cmd);
                //// created the dataset object
                //ds = new DataSet();
                //// fill the dataset and your result will be
                ////stored in dataset
                //da.Fill(ds)  ;

                while (rdr.Read())
                {
                    // reader["Datum"].ToString();

                    if (rdr.HasRows)
                    {

                        ERV2 erv2 = new ERV2();

                        erv2.prezime = rdr["lastname"].ToString();
                        erv2.id = (int.Parse)(rdr["fxid"].ToString());
                        erv2.dan = rdr["dan"].ToString();
                        erv2.dosao = rdr["dosao"].ToString();
                        erv2.otisao = rdr["otisao"].ToString();
                        erv2.minuta = (int.Parse)(rdr["minuta"].ToString());
                        erv2.lokacija = ""; // rdr["lokacija"].ToString();

                        ervii2.Add(erv2);

                    }
                }
                cn.Close();

            }



            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.DataSource = ervii2;

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //dataGridView1.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 120;


        }

        private static void FreezeBand(DataGridViewBand band)
        {
            band.Frozen = true;
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.WhiteSmoke;
            band.DefaultCellStyle = style;
        }

        // PREGLED 2
        private void button9_Click(object sender, EventArgs e)
        {

            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            panelRucniUnos.Visible = false;
            GridPregledRadnika.Visible = false;

            button2.Visible = true;
            button2.Text = "Export to excell ";
            // string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=fx_public;Password=.";

            string ime1 = textBox1.Text;
            string sql, vrata, str1, vdolaska, vodlaska;
            int lokacija1 = comboBox1.SelectedIndex;
            int id1, br, ulaz1, izlaz1, i1, vrata1, dosao, otisao;
            //string dat1 = DateTime.ParseExact(dateTimePicker1.Value.ToShortDateString(), "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);

            string dat1 = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " 00:00:00";
            string dat2 = dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day + " 23:59:59";

            DateTime dat11 = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0, 0, 0);
            DateTime dat22 = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, 0, 0, 0);

            string lokacija;

            lokacija = "";
            switch (lokacija1 + 1)
            {
                case 1:
                    lokacija = "_";    // sve
                    break;
                case 2:
                    lokacija = "22293";    // uprava ulaz
                    break;
                case 3:
                    lokacija = "544666574";    // tehnologija
                    break;
                case 4:
                    lokacija = "544666577";   // garderoba p1
                    break;
                case 5:
                    lokacija = "544666590";   // hala3
                    break;
                case 6:
                    lokacija = "544666595";   // hala4
                    break;
                case 7:
                    lokacija = "544666584";   // zona
                    break;
                default:
                    break;
            }

            CultureInfo provider = CultureInfo.InvariantCulture;
            System.Globalization.DateTimeStyles style = DateTimeStyles.None;

            DateTime dt1;
            dat1 = dateTimePicker1.Value.Month + "/" + dateTimePicker1.Value.Day + "/" + dateTimePicker1.Value.Year + " 00:00:59";
            dat1 = dateTimePicker2.Value.Month + "/" + dateTimePicker2.Value.Day + "/" + dateTimePicker2.Value.Year + " 00:00:59";

            DateTime.TryParseExact(dat1, "MM/dd/yyyy HH:mm:ss", provider, style, out dt1);

            DateTime dt2;
            DateTime.TryParseExact(dat2, "MM/dd/yyyy HH:mm:ss", provider, style, out dt2);

            dat1 = dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + " 00:00:59";
            dat1 = dateTimePicker1.Value.Month + "-" + dateTimePicker1.Value.Day + "-" + dateTimePicker1.Value.Day + " 00:00:59";
            dat2 = dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Month + "-" + dateTimePicker2.Value.Day + " 23:59:59";

            string test = "2016-12-01";
            DateTime dt11 = DateTime.ParseExact(test, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            test = "2016-12-31";
            DateTime dt22 = DateTime.ParseExact(test, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=fx_public;Password=.";
            SqlDataReader rdr = null;
            //             ervii2.Clear();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("dbo.sp_VrijemeUlazaIzlaza2", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@location", SqlDbType.Int, 5).Value = 500;  // lokacija
                cmd.Parameters.Add("@DatumOd", SqlDbType.DateTime).Value = dt11;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                cmd.Parameters.Add("@DatumDo", SqlDbType.DateTime).Value = dt22;  //DateTime.ParseExact(dat2, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture );  // Od datuma
                cmd.Parameters.Add("@Prezimee", SqlDbType.VarChar).Value = ime1;  // Početak prezimena

                rdr = cmd.ExecuteReader();
                //SqlDataAdapter da;DataSet ds;
                //da = new SqlDataAdapter(cmd);
                //// created the dataset object
                //ds = new DataSet();
                //// fill the dataset and your result will be
                ////stored in dataset
                //da.Fill(ds)  ;

                id1 = 0;
                br = 0;
                i1 = 0;
                str1 = "";
                dosao = 0; otisao = 0;

                while (rdr.Read())
                {
                    // reader["Datum"].ToString();

                    if (rdr.HasRows)
                    {

                        vrata = rdr["door"].ToString();
                        str1 = str1 + "," + vrata;
                        id1 = (int.Parse)(rdr["fxid"].ToString());
                        dat1 = rdr["dT"].ToString();

                        while (id1 == (int.Parse)(rdr["fxid"].ToString()) && dat1 == (rdr["dT"].ToString()))
                        {
                            // dat1[i1]   = rdr["dt"].ToString();

                            vrata1 = (int.Parse)(rdr["door"].ToString());

                            if (vrata1 == 7 || vrata1 == 8)

                            {
                                dosao = 1;
                                vdolaska = rdr["dT"].ToString();
                            }
                            else if (dosao == 1)
                            {
                                vdolaska = rdr["dt"].ToString();
                                dosao = 0;
                            }

                            if (vrata1 == 9 || vrata1 == 10)
                            {
                                otisao = 1;
                            }
                            // vrata1[i1] = rdr["door"].ToString();

                        }
                        i1 = 1;

                        //unosvremena[i] = rdr["fxid"].ToString();

                        if ((int.Parse)(rdr["fxid"].ToString()) == id1)
                        {
                            br++;
                        }
                        else
                        {
                            if (br == 0)
                            {

                                ulaz1 = 0; izlaz1 = 0;

                                if (rdr["door"].ToString() == "7" || rdr["door"].ToString() == "8")
                                {
                                    ulaz1 = 1;
                                }

                                if (rdr["door"].ToString() == "9" || rdr["door"].ToString() == "10")
                                {
                                    izlaz1 = 1;
                                }

                                //if ((br==0) && ( ulaz1=1))
                                if ((ulaz1 + izlaz1) > 0)
                                {

                                    ERV2 erv2 = new ERV2();
                                    erv2.prezime = rdr["lastname"].ToString();
                                    erv2.ime = rdr["ime"].ToString();
                                    erv2.id = (int.Parse)(rdr["fxid"].ToString());
                                    erv2.dan = rdr["dan"].ToString();
                                    erv2.dosao = rdr["dt"].ToString();
                                    erv2.mt = rdr["mt"].ToString();
                                    // erv2.minuta = (int.Parse)(rdr["minuta"].ToString());
                                    erv2.lokacija = rdr["door"].ToString();  // rdr["lokacija"].ToString();
                                    ervii2.Add(erv2);

                                }
                            }

                            id1 = (int.Parse)(rdr["fxid"].ToString());
                            br = 0;
                        }

                    }
                }
                cn.Close();
            }


            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.DataSource = ervii2;

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //dataGridView1.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 120;


        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void pregledRadnikaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void unosRadnigVremenaZaRadnikaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            panel14.Visible = true;

            pocetniEkran();
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            GridPregledRadnika.Visible = false;


            panel21.Visible = false;
            panel31.Visible = false;
            panel41.Visible = false;
            panel14.Visible = true;


            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "dd.MM.yyyy HH:mm";

            var dataSource = new List<radnici>();
            foreach (var radnikk in radnicii)
            {
                dataSource.Add(new radnici() { prezime = radnikk.prezime + " " + radnikk.ime + " ( " + radnikk.id.ToString() + ")", id = radnikk.id });
            }

            this.comboBox10.DataSource = dataSource;    // lista radnika, izbornik
            this.comboBox10.DisplayMember = "prezime";
            this.comboBox10.ValueMember = "id";


            string Query = "select naziv,id from rasporedvremena";
            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();
                DataTable dt2 = new DataTable();

                SqlCommand cmd = new SqlCommand(Query, cn);
                SqlDataReader myReader = cmd.ExecuteReader();
                dt2.Load(myReader);

                comboBox12.DataSource = dt2;
                comboBox12.ValueMember = "id";
                comboBox12.DisplayMember = "Naziv";
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {

            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=sa;Password=AdminFX9.";
            SqlDataReader rdr = null;
            //             ervii2.Clear();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("dbo.FX_UpdateRVForOneRadnik", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FXID", SqlDbType.Int, 5).Value = comboBox10.SelectedValue;  // lokacija
                cmd.Parameters.Add("@TIP_RV", SqlDbType.Int, 1).Value = comboBox12.SelectedIndex + 1;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma

                rdr = cmd.ExecuteReader();
                cn.Close();
            }


            using (SqlConnection cn4 = new SqlConnection(connectionString))
            {

                cn4.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into erv_log (korisnik,idprijave,opis,datum) values('" + korisnik + "','" + idprijave + "','Promjena radnog vremena za "+ comboBox10.SelectedValue.ToString()+"', getdate())",cn4);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                cn4.Close();

            }

            panel14.Visible = false;
            pocetniEkran();

        }

        private void novaKarticaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            pocetniEkran();
            panelNovakartica.Visible = true;

            var dataSource = new List<radnici>();
            foreach (var radnikk in radnicii)
            {
                dataSource.Add(new radnici() { prezime = radnikk.prezime + " " + radnikk.ime + " ( " + radnikk.id.ToString() + " )", id = radnikk.id });
            }

            CB_ListaRadnika.DataSource = dataSource;
            CB_ListaRadnika.DisplayMember = "prezime";
            CB_ListaRadnika.ValueMember = "id";
            CB_ListaRadnika.DropDownHeight = CB_ListaRadnika.Font.Height * 50;

        }

        // nova kartica btn, enter, rfid iz pantheona
        private void button12_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            panelNovakartica.Visible = false;
            string csn1, id1, imedjelatnika1;
            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=sa;Password=AdminFX9.";
            SqlDataReader rdr = null;
            csn1 = "";
            id1 = CB_ListaRadnika.SelectedValue.ToString();
            imedjelatnika1 = CB_ListaRadnika.Text.ToString();
            string[] lista1 = imedjelatnika1.Split(' ');
            string ime1 = lista1[1];
            string prezime1 = lista1[0];
            string rfid1 = "", rfidhex = "", custid1 = "";
            string poduzece = "";
            string connectionStringp = "";
            string ime0="", prezime0="", rfid0="", rfidhex0="", rfid20="",custid0="",rv0="";
            SqlConnection cn1 = new SqlConnection(connectionString);
            cn1.Open();
            SqlCommand sqlCommand1 = new SqlCommand("select * from radnici_  where id=" + id1.Trim(), cn1);
          
            SqlDataReader reader21 = sqlCommand1.ExecuteReader();
            reader21.Read();
            if (reader21.HasRows)
            {
                poduzece = reader21["poduzece"].ToString();
                ime0 = reader21["ime"].ToString();
                prezime0 = reader21["prezime"].ToString();
                rfid0 = reader21["rfid"].ToString();
                rfidhex0 = reader21["rfidhex"].ToString();
                rfid20 = reader21["rfid2"].ToString();
                custid0 = reader21["custid"].ToString();
                rv0 = reader21["rv"].ToString();
            }

            cn1.Close();
            if (poduzece.Contains("Fero"))
            {                
                connectionStringp = @"Data Source=192.168.0.6;Initial Catalog=PantheonFxAT;User ID=sa;Password=AdminFX9.";
            }
            else
            {
                connectionStringp = @"Data Source=192.168.0.6;Initial Catalog=PantheonTKB;User ID=sa;Password=AdminFX9.";
            }

            cn1 = new SqlConnection(connectionStringp);  // potraži u Pantheonu rfid
            //sqlCommand1 = new SqlCommand("select cast(acregno as int) as id, acname, acsurname, j.acCostDrv, j.acDept, d.acnumber, j.acjob, '' radni_staz, j.adDate, '' sifra_rm, p.acstreet, acpost, '' vrijeme, accity, j.acFieldSA vrsta_isplate, adDateExit from thr_prsn p " +
            //            "left join thr_prsnjob j on p.acworker = j.acworker left join thr_prsnadddoc d on d.acWorker = p.acworker and d.actype = 8 where d.actype=8 and p.acregno= " +  id1 + " order by cast(acregno as int) desc", cn1);

            sqlCommand1 = new SqlCommand("select cast(acregno as int) as id, acname, acsurname, j.acCostDrv, j.acDept, d.acnumber, j.acjob, '' radni_staz, j.adDate, '' sifra_rm, p.acstreet, acpost, '' vrijeme, accity, j.acFieldSA vrsta_isplate, adDateExit from thr_prsn p " +
                                          "left join thr_prsnjob j on p.acworker = j.acworker left join thr_prsnadddoc d on d.acWorker = j.acworker and d.actype = 8 where d.actype = 8 and p.acregno = "+id1+" and j.adDateEnd is null and d.acactive = 'T' order by cast(acregno as int) desc",cn1);

            sqlCommand1 = new SqlCommand("select cast(acregno as int) as id, acname, acsurname, d.acnumber, '' radni_staz, '' sifra_rm, p.acstreet, acpost, '' vrijeme, accity, adDateExit from thr_prsn p " +
                                          "left join thr_prsnadddoc d on d.acWorker = P.acworker and d.actype = 8 where d.actype = 8 and p.acregno = " + id1 + " and d.acactive = 'T' order by cast(acregno as int) desc", cn1);


            cn1.Open();
            reader21 = sqlCommand1.ExecuteReader();
            reader21.Read();
            if (reader21.HasRows)
            {
                rfid1 = reader21["acnumber"].ToString();
                ime1 = reader21["acname"].ToString().TrimEnd();
                prezime1 = reader21["acsurname"].ToString().TrimEnd();

                long rfidd = (long.Parse)(rfid1);
                rfidhex = rfidd.ToString("X");
                //string rfid2 = (int.Parse(rfidhex.Substring(0, 1), System.Globalization.NumberStyles.HexNumber)).ToString();
                //string custid = rfid2;
                //rfid2 = rfid2 + "-" + (int.Parse(rfidhex.Substring(1, rfidhex.Length - 1), System.Globalization.NumberStyles.HexNumber)).ToString();
                //

                rfidhex = rfidd.ToString("X");
                string rfid2 = (int.Parse(rfidhex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                string custid = rfid2;
                rfid2 = rfid2 + "-" + (long.Parse(rfidhex.Substring(2, rfidhex.Length - 2), System.Globalization.NumberStyles.HexNumber)).ToString();

                //
                
                long decValue = 0;
                
                decValue = rfidd;
                                
                string hexValue = decValue.ToString("X");
                string prvi = hexValue.Substring(0, 2);
                string drugi = hexValue.Substring(3, hexValue.Length - 3);
                int prvidec = int.Parse(prvi, System.Globalization.NumberStyles.HexNumber);
                int drugidec = int.Parse(drugi, System.Globalization.NumberStyles.HexNumber);
                csn1 = prvidec.ToString() + "-" + drugidec.ToString();
                custid1 = prvidec.ToString();

            }
            cn1.Close();
            // update rfind i radnici_
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                
                String dat1 = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                String dat2 = DateTime.Now.AddYears(10).ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();

                if (1 == 1)
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("rfind.dbo.FX_Import", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@EXT_ID", SqlDbType.VarChar, 6).Value = (id1).Trim();  // lokacija
                    cmd.Parameters.Add("@FNAME", SqlDbType.VarChar, 35).Value = ime1;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                    cmd.Parameters.Add("@LNAME", SqlDbType.VarChar, 35).Value = prezime1;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                    cmd.Parameters.Add("@CSN", SqlDbType.VarChar, 12).Value = csn1;  // csn1
                    cmd.Parameters.Add("@START_TIME", SqlDbType.DateTime).Value = dat1;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                                                                                         //                    cmd.Parameters.Add("@END_TIME", SqlDbType.DateTime).Value = dat2;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                                                                                         //                    cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = status1;  //  status 0 - nova, 1 - update , 2 disable  3 delete ???
                    cmd.Parameters.Add("@END_TIME", SqlDbType.DateTime).Value = DateTime.Now.AddYears(10); ;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                    cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = 1;  //  status 0 - nova, 1 - update , 2 disable  3 delete ???
                    SqlDataReader reader1 = cmd.ExecuteReader();

                    cn.Close();
                }
                cn.Open();

                SqlCommand sqlCommand = new SqlCommand("select * from radnici_  where id=" + id1, cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();

                if (reader2.HasRows)
                {
                    cn.Close();

                    cn.Open();
                    sqlCommand = new SqlCommand("update radnici_ set rfid='" + rfid1 + "',rfidhex='000000" + rfidhex + "',rfid2='" + csn1 + "',custid='" + custid1 + "' where id='" + id1 + "'", cn);
                    reader2 = sqlCommand.ExecuteReader();
                    cn.Close();

                }
                else
                {

                    cn.Close();
                }

                // radnici_log 
                cn.Open();
                string datumpromjene = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString()+".00";
                sqlCommand = new SqlCommand("insert into radnici_log (poduzece,rv,ime,prezime,id,rfid,rfidhex,rfid2,custid,datumpromjene) values('"+poduzece + "','" + rv0 + "','" +ime0+"','"+prezime0+"','"+ id1+"','"+rfid0+"','"+rfidhex0 + "','" + rfid20 + "','" + custid0 + "',getdate())",cn);
                reader2 = sqlCommand.ExecuteReader();
                cn.Close();

                using (SqlConnection cn3 = new SqlConnection(connectionString))
                {
                    cn3.Open();
                    string sql1 = "insert into erv_log (korisnik,idprijave,opis,datum) values('" + korisnik + "','" + idprijave + "','Promjena rfid za " + id1 + " stari rfid=" + rfid0 + "',getdate())";
                    sqlCommand = new SqlCommand( sql1 , cn3 );
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    cn3.Close();
                }


                MessageBox.Show("Podaci su nadopunjeni ! ");


            }

            panel14.Visible = false;
            pocetniEkran();
    }
                
    private void CB_ListaRadnika_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void NK_RFID_TextChanged(object sender, EventArgs e)
        {

        }

        private void uvozPodatakaToolStripMenuItem_Click(object sender, EventArgs e) // import podataka iz csv filea
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into erv_log (korisnik,idprijave,opis,datum) values('" + korisnik + "','" + idprijave + "','Unos novih djelatnika - import',getdate())",cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                cn.Close();

            }

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pregledToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pocetniEkran();

            dg_PregledRadnihMjesta.Visible = true;
            dg_PregledRadnihMjesta.BringToFront();
            panel_pregled_radnih_mjesta.Visible = true;
            panel_pregled_radnih_mjesta.BringToFront();
            dg_PregledRadnihMjesta.BringToFront();




            string sql = "Select id ID,Hala,radnapozicija,mt as Mjestotroška from radnepozicije order by id";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dg_PregledRadnihMjesta.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_PregledRadnihMjesta.DataSource = ds;
            dg_PregledRadnihMjesta.DataMember = "event";

            dg_PregledRadnihMjesta.AutoResizeColumns();
            dg_PregledRadnihMjesta.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            dg_PregledRadnihMjesta.BringToFront();

        }

        private void unosToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            pocetniEkran();
            pl_UnosRadnihMjesta.Visible = true;

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)   // spremi novo radno mjesto
        {

            string sql1 = "'" + cbx_PopisHala.Text + "','" + txb_nazivradnogmjesta.Text + "'";

            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into RadnePozicije ( hala , radnapozicija ) values (" + sql1 + ")", cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                cn.Close();

            }

            pocetniEkran();
        }

        private void izmjenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            pl_IzmjenaRadnogMjesta.Visible = true;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Select ID,Hala, RadnaPozicija from radnepozicije", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            cbx_IzmjenaRadnihMjesta.DisplayMember = "RadnaPozicija";
            cbx_IzmjenaRadnihMjesta.ValueMember = "id";
            cbx_IzmjenaRadnihMjesta.DataSource = dt;
            cbx_IzmjenaRadnihMjesta.MaxDropDownItems = 70;

        }

        private void button15_Click(object sender, EventArgs e)      // spremi promjene
        {

            string sql1 = "'" + cbx_PopisHala_i.Text + "','" + tb_novi_naziv_rm.Text + "'";
            int id1 = (int)cbx_IzmjenaRadnihMjesta.SelectedValue;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("update RadnePozicije set hala='" + cbx_PopisHala_i.Text + "', radnapozicija='" + tb_novi_naziv_rm.Text + "' where id=" + id1.ToString(), cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                cn.Close();
            }

            pocetniEkran();

        }


        // unos plana rasporeda
        private void unosToolStripMenuItem2_Click(object sender, EventArgs e)
        {

            pocetniEkran();

            dg_ListaRadnika.AllowDrop = true;
            btn_SpremiPlan.Visible = true;

            dg_Unos_PlanaRadnika.AllowDrop = true;


            //dg_ListaRadnika.MouseDown += new MouseEventHandler(dg_ListaRadnika_MouseDown);
            //dg_ListaRadnika.MouseMove += new MouseEventHandler(dg_ListaRadnika_MouseMove);
            //dg_ListaRadnika.DragOver += new DragEventHandler(dg_ListaRadnika_DragOver);
            //dg_ListaRadnika.DragDrop += new DragEventHandler(dg_ListaRadnika_DragDrop);

            pl_Planiranje_Unos.Visible = true;
            pl_Planiranje_Unos.Width = Width - 100;
            dg_Unos_PlanaRadnika.Height = Height - 150;
            dg_ListaRadnika.Height = Height - 150;
            cbx_MjestaTroska.Visible = true;
            //cbl_ListaOdjela.Visible = true;
            dateP_datumUP.Visible = true;
            DateTime dateValue = new DateTime(2008, 6, 11);
            Lbl_datum_u.Text = dateP_datumUP.Value.ToShortDateString() + " - " + dateP_datumUP.Value.ToString("dddd", new CultureInfo("hr-HR"));


            string sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in ( 705,704,715,708,706) and r.neradi=0 order by prezime";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.DataSource = ds;
            dg_ListaRadnika.DataMember = "event";

            dg_ListaRadnika.AutoResizeColumns();
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //
            if (idloged != "8")
                sql = "Select rbroj as ID, Hala, RadnaPozicija,Smjena1 , Smjena2 , Smjena3 , Bolovanje,Godišnji from plandjelatnika11 where grupa='" + idloged + "'";
            else
                sql = "Select rbroj as ID, Hala, RadnaPozicija,Smjena1 , Smjena2 , Smjena3 , Bolovanje,Godišnji from plandjelatnika11 ";

            SqlDataAdapter dataadapter1 = new SqlDataAdapter(sql, connection);
            DataSet ds1 = new DataSet();
            connection.Open();
            dataadapter1.Fill(ds1, "event1");
            connection.Close();

            dg_Unos_PlanaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_Unos_PlanaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_Unos_PlanaRadnika.DataSource = ds1;
            dg_Unos_PlanaRadnika.DataMember = "event1";
            dg_Unos_PlanaRadnika.AutoResizeColumns();

            for (int i = 0; i < dg_Unos_PlanaRadnika.ColumnCount; i++)
            {
                dg_Unos_PlanaRadnika.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dg_Unos_PlanaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            DataGridViewColumn column = dg_Unos_PlanaRadnika.Columns[3];
            column.Width = 200;
            column = dg_Unos_PlanaRadnika.Columns[4];
            column.Width = 200;
            column = dg_Unos_PlanaRadnika.Columns[5];
            column.Width = 200;

            column = dg_Unos_PlanaRadnika.Columns[6];
            column.Width = 200;
            column = dg_Unos_PlanaRadnika.Columns[7];
            column.Width = 200;

            //column = dg_Unos_PlanaRadnika.Columns[7];
            //column.Width = 200;
            //column = dg_Unos_PlanaRadnika.Columns[8];
            //column.Width = 200;

            //column = dg_Unos_PlanaRadnika.Columns[9];
            //column.Width = 200;


            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Select id, Naziv  from mjestotroska where id not in (7070)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbx_MjestaTroska.DisplayMember = "Naziv";
            cbx_MjestaTroska.ValueMember = "id";
            cbx_MjestaTroska.DataSource = dt;
            cbx_MjestaTroska.MaxDropDownItems = 80;

        }

        private void oldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            panel21.Visible = true;

            var dataSource = new List<radnici>();
            //string hala = comboBox4.SelectedValue.ToString()   ;
            int mt1;
            foreach (var radnikk in radnicii)
            {

                //if ( ( (int.Parse)(radnikk.mt)) in ( 700,702,703,710,716)  )
                mt1 = (int.Parse)(radnikk.mt);

                if (mt1 == 700 || mt1 == 702 || mt1 == 703 || mt1 == 710 || mt1 == 716)
                {
                    dataSource.Add(new radnici() { prezime = radnikk.prezime + " " + radnikk.ime + " -   " + radnikk.id, id = radnikk.id });
                }
            }

            this.comboBox7.DataSource = dataSource;
            this.comboBox7.DisplayMember = "prezime";
            this.comboBox7.ValueMember = "id";
            DateTime date1, prvidan, zadnjidan;
            int god;
            date1 = DateTime.Now;
            var dataSourceT = new List<weekID>();

            while (date1.Year <= 2017)    // ????
            {
                date1 = date1.AddDays(7);
                god = date1.Year;
                System.Globalization.CultureInfo cult_info = System.Globalization.CultureInfo.CreateSpecificCulture("no");
                System.Globalization.Calendar cal = cult_info.Calendar;
                int weekCount = cal.GetWeekOfYear(date1, cult_info.DateTimeFormat.CalendarWeekRule, cult_info.DateTimeFormat.FirstDayOfWeek);
                prvidan = FirstDateOfWeekISO8601(god, weekCount - 1);
                zadnjidan = prvidan.AddDays(6);
                dataSourceT.Add(new weekID() { id = weekCount.ToString(), daterange = prvidan.ToShortDateString() + " - " + zadnjidan.ToShortDateString() });  // napuni listu tjedana
            }

            this.comboBox3.DataSource = dataSourceT;  // lista tjedana
            this.comboBox3.DisplayMember = "daterange";
            this.comboBox3.ValueMember = "id";

            this.comboBox7.DataSource = dataSource;  // lista radnika
            this.comboBox7.DisplayMember = "prezime";
            this.comboBox7.ValueMember = "id";
            //this.comboBox7.M0axDropDownItems = 80;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Select id,(hala+' - '+radnapozicija) as radnapozicija from radnepozicije", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            comboBox6.DisplayMember = "RadnaPozicija";
            comboBox6.ValueMember = "id";
            comboBox6.DataSource = dt;
            comboBox6.MaxDropDownItems = 80;

            panel21.Visible = true;

            panel31.Visible = false;
            panel41.Visible = false;
            panelRucniUnos.Visible = false;
            panel31.SendToBack();
            panel41.SendToBack();
            panel21.BringToFront();

            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = true;
            GridPregledRadnika.Visible = false;
        }

        private void dg_Unos_PlanaRadnika_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            base.OnClick(e);
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ddg_Unos_PlanaRadnika_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        /* Drag & Drop */
        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;

        private void dg_ListaRadnika_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridView1.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }

        private void dg_ListaRadnika_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void dg_Unos_PlanaRadnika_DragOver(object sender, DragEventArgs e)
        {

        }

        private void dg_Unos_PlanaRadnika_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dg_Unos_PlanaRadnika.PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a copy then add the row to the other control.
            if (e.Effect == DragDropEffects.Copy)
            {
                string cellvalue = e.Data.GetData(typeof(string)) as string;
                var hittest = dg_Unos_PlanaRadnika.HitTest(clientPoint.X, clientPoint.Y);
                if (hittest.ColumnIndex != -1
                    && hittest.RowIndex != -1)
                    dg_Unos_PlanaRadnika[hittest.ColumnIndex, hittest.RowIndex].Value = cellvalue;
            }
        }

        private void dg_ListaRadnika_MouseDown_1(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            var hittestInfo = dg_ListaRadnika.HitTest(e.X, e.Y);
            //public int red1;

            if (hittestInfo.RowIndex != -1 && hittestInfo.ColumnIndex != -1)
            {
                valueFromMouseDown = dg_ListaRadnika.Rows[hittestInfo.RowIndex].Cells[hittestInfo.ColumnIndex].Value;
                if (valueFromMouseDown != null)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }


        private void dg_Unos_PlanaRadnika_DragDrop_1(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dg_Unos_PlanaRadnika.PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a copy then add the row to the other control.
            if (e.Effect == DragDropEffects.Copy)
            {
                string cellvalue = e.Data.GetData(typeof(string)) as string;
                var hittest = dg_Unos_PlanaRadnika.HitTest(clientPoint.X, clientPoint.Y);
                if (hittest.ColumnIndex > 1 && hittest.RowIndex >= 0)
                {
                    dg_Unos_PlanaRadnika[hittest.ColumnIndex, hittest.RowIndex].Value = cellvalue;
                    dg_ListaRadnika[1, dg_ListaRadnika.CurrentCell.RowIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                    dg_Unos_PlanaRadnika[hittest.ColumnIndex, hittest.RowIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                }

            }
        }

        private void dg_Unos_PlanaRadnika_DragOver_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void dg_ListaRadnika_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dg_ListaRadnika.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                    //red1 = dg_ListaRadnika.CurrentCell.RowIndex;
                }
            }

        }

        private void cbl_ListaOdjela_SelectedIndexChanged(object sender, EventArgs e)
        {

            int sindex = -1;
            for (int i = 0; i < cbl_ListaOdjela.Items.Count; i++)
            {
                if (cbl_ListaOdjela.GetItemChecked(i))
                {
                    string str = (string)cbl_ListaOdjela.Items[i];
                    sindex = i + 1;
                    //        MessageBox.Show(str);
                }
            }

            string sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in ( 705,704,715,708,707,706) and r.neradi=0 order by prezime";

            if (sindex != -1)
            {
                sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt left join odjeli o on o.mt=r.mt where r.mt not in ( 705,704,715,708,707,706) and r.neradi=0 and  o.id= " + sindex.ToString() + " order by prezime";

            }

            //cbl_ListaOdjela.SelectedItems;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.DataSource = ds;
            dg_ListaRadnika.DataMember = "event";

            dg_ListaRadnika.AutoResizeColumns();
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);


        }

        // spremi plan
        private void btn_SpremiPlan_Click(object sender, EventArgs e)
        {

            string sql1 = "", sql2 = "2", sql3 = "", sqlb = "", sqlg = "";
            string napomena1 = "";
            string napomena2 = "";
            string napomena3 = "";
            string data = "";
            string prezime = "";
            SqlCommand sqlCommand;
            SqlDataReader reader2;

            for (int i = 0; i < dg_Unos_PlanaRadnika.RowCount - 1; i++)
            {

                string hala1 = dg_Unos_PlanaRadnika[1, i].Value.ToString();
                if (Check_Hala(hala1))
                {
                    continue;
                }

                string RadnoMjesto = dg_Unos_PlanaRadnika[2, i].Value.ToString();


                string datum11;
                DateTime datum1 = dateP_datumUP.Value.Date;
                datum11 = dateP_datumUP.Value.Year.ToString() + "-" + dateP_datumUP.Value.Month.ToString() + "-" + dateP_datumUP.Value.Day.ToString(); //.ToShortDateString();
                string dat1 = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                string res = "";
                string smjena1 = "1";
                int rbroj1 = 0;

                if (DBNull.Value.Equals(dg_Unos_PlanaRadnika[3, i].Value))
                {
                    sql1 = "";
                }
                else
                {

                    data = (string)dg_Unos_PlanaRadnika[3, i].Value;
                    rbroj1 = (int)dg_Unos_PlanaRadnika[0, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;                        
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "1";
                        prezime = data;
                        sql1 = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',1,'" + hala1 + "','" + RadnoMjesto + "','" + napomena1 + "','" + dat1 + "'";
                        sql1 = "update pregledvremena2 set rbroj='" + rbroj1.ToString() + "',idradnika=" + res + ",prezimeime='" + prezime + "',datum='" + datum11 + "',smjena='1',hala='" + hala1 + "',radnomjesto='" + RadnoMjesto + "',datumunosa='" + dat1 + "'";

                    }
                    else
                    {
                        sql1 = "";
                    }
                }

                if (DBNull.Value.Equals(dg_Unos_PlanaRadnika[4, i].Value))
                {
                    sql2 = "";
                }
                else {

                    data = (string)dg_Unos_PlanaRadnika[4, i].Value;
                    rbroj1 = (int)dg_Unos_PlanaRadnika[0, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "2";
                        prezime = data;
                        sql2 = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',2,'" + hala1 + "','" + RadnoMjesto + "','" + napomena2 + "','" + dat1 + "'";
                    }
                    else
                    {
                        sql2 = "";
                    }
                }

                if (DBNull.Value.Equals(dg_Unos_PlanaRadnika[5, i].Value))
                {
                    sql3 = "";
                }
                else
                {

                    rbroj1 = (int)dg_Unos_PlanaRadnika[0, i].Value;
                    data = (string)dg_Unos_PlanaRadnika[5, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "3";
                        prezime = data;
                        sql3 = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',3,'" + hala1 + "','" + RadnoMjesto + "','" + napomena3 + "','" + dat1 + "'";
                    }
                    else
                    {
                        sql3 = "";
                    }
                }


                // bolovanje
                if (DBNull.Value.Equals(dg_Unos_PlanaRadnika[6, i].Value))
                {
                    sqlb = "";
                }
                else
                {

                    rbroj1 = (int)dg_Unos_PlanaRadnika[0, i].Value;
                    data = (string)dg_Unos_PlanaRadnika[6, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "4";
                        prezime = data;
                        RadnoMjesto = "Bolovanje";
                        sqlb = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',4,'" + hala1 + "','" + RadnoMjesto + "','" + napomena3 + "','" + dat1 + "'";
                    }
                    else
                    {
                        sqlb = "";
                    }
                }


                // godišnji
                if (DBNull.Value.Equals(dg_Unos_PlanaRadnika[7, i].Value))
                {
                    sqlg = "";
                }
                else
                {

                    rbroj1 = (int)dg_Unos_PlanaRadnika[0, i].Value;
                    data = (string)dg_Unos_PlanaRadnika[7, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "5";
                        prezime = data;
                        RadnoMjesto = "Godišnji";
                        sqlg = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',5,'" + hala1 + "','" + RadnoMjesto + "','" + napomena3 + "','" + dat1 + "'";
                    }
                    else
                    {
                        sqlg = "";
                    }
                }


                string rbroj = (i + 1).ToString();
                rbroj = rbroj1.ToString();

                using (SqlConnection cn = new SqlConnection(connectionString))
                {

                    cn.Open();

                    sqlCommand = new SqlCommand("select * from pregledvremena2 where datum='" + datum11 + "' and rbroj= " + rbroj, cn);
                    reader2 = sqlCommand.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        reader2.Read();

                        // if (DBNull.Value.Equals(reader2["Napomena"] ))
                        //{ napomena1 = ""; }
                        //else
                        // {
                        //            napomena1 = reader2["napomena"].ToString();
                        //}

                        cn.Close();
                        cn.Open();
                        sqlCommand = new SqlCommand("delete from  pregledvremena2 where  datum='" + datum11 + "' and rbroj=" + rbroj, cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();

                    }
                    else
                    {
                        cn.Close();
                    }

                    if (sql1.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sql1 + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }
                    if (sql2.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sql2 + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }
                    if (sql3.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sql3 + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }

                    if (sqlb.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sqlb + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }

                    if (sqlg.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sqlg + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }

                }
            }
            MessageBox.Show("Podaci su sačuvani !");

        }
        //private void dg_Unos_PlanaRadnika_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
        //    ch1 = (DataGridViewCheckBoxCell)dg_Unos_PlanaRadnika.Rows[dg_Unos_PlanaRadnika.CurrentRow.Index].Cells[0];

        //    if (ch1.Value == null)
        //        ch1.Value = false;
        //    switch (ch1.Value.ToString())
        //    {
        //        case "True":
        //            ch1.Value = false;
        //            break;
        //        case "False":
        //            ch1.Value = true;
        //            break;
        //    }
        //    MessageBox.Show(ch1.Value.ToString());
        //}
        private void dateP_datumUP_ValueChanged(object sender, EventArgs e)
        {
            dateP_datumUP.Format = DateTimePickerFormat.Custom;
            dateP_datumUP.CustomFormat = "dd.MM.yyyy HH:mm";

            DateTime datum1 = dateP_datumUP.Value.Date;
            this.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void karticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();
        }

        private void cbx_MjestaTroska_SelectedIndexChanged(object sender, EventArgs e)
        {

            string ssindex = "";
            ssindex = cbx_MjestaTroska.SelectedValue.ToString();


            string sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in ( 705,704,715,708,707,706) where r.neradi=0 order by prezime";
            sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.neradi=0 order by prezime";

            if (ssindex != "0")
            {
                //sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt left join odjeli o on o.mt=r.mt where r.mt not in ( 705,704,715,708,707,706) and  o.mt= " + ssindex + " order by prezime";
                sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where  r.neradi=0  and mt.id= " + ssindex + " order by prezime";
            }
            else
            {
                sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt  where  r.neradi=0 order by prezime";
            }

            //cbl_ListaOdjela.SelectedItems;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.DataSource = ds;
            dg_ListaRadnika.DataMember = "event";

            dg_ListaRadnika.AutoResizeColumns();
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }

        private void dg_Unos_PlanaRadnika_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        // pregled plana radnog vremena

        private void pregledToolStripMenuItem3_Click(object sender, EventArgs e)
        {

            pocetniEkran();

            dg_PlanRadnogVremena.Visible = true;
            dg_PlanRadnogVremena.AllowDrop = true;
            dg_ListaRadnikaP.Visible = true;
            dg_ListaRadnikaP.AllowDrop = true;

            dg_ListaRadnika.AllowDrop = true;
            btn_SpremiPlan.Visible = true;

            //dg_ListaRadnika.MouseDown += new MouseEventHandler(dg_ListaRadnika_MouseDown);
            //dg_ListaRadnika.MouseMove += new MouseEventHandler(dg_ListaRadnika_MouseMove);
            //dg_ListaRadnika.DragOver += new DragEventHandler(dg_ListaRadnika_DragOver);
            //dg_ListaRadnika.DragDrop += new DragEventHandler(dg_ListaRadnika_DragDrop);

            //pl_Planiranje_Unos.Visible = true;
            pl_PregledRadnogVremena.Visible = true;
            pl_PregledRadnogVremena.Width = Width - 100;
            pl_PregledRadnogVremena.Height = Height - 100;
            dg_PlanRadnogVremena.Height = Height - 150;

            dg_Unos_PlanaRadnika.Height = Height - 150;
            dg_ListaRadnika.Height = Height - 150;
            cbx_MjestaTroska.Visible = true;
            //cbl_ListaOdjela.Visible = true;
            dateP_datumUP.Visible = true;

            string sql = "Select r.ID,(rtrim(prezime)+' '+rtrim(ime)+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in ( 705,704,715,708,707,706) and r.neradi=0  order by prezime";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnika.DataSource = ds;
            dg_ListaRadnika.DataMember = "event";

            dg_ListaRadnika.AutoResizeColumns();
            dg_ListaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //
            if (idloged != "8")
                sql = "Select rbroj as ID, Hala, RadnaPozicija,Smjena1 ,od1,do1,napomena1, Smjena2 ,od2,do2,napomena2, Smjena3 , od3,do3,napomena3,Bolovanje,Godišnji,RV1,RV2,RV3 from plandjelatnika22 where grupa='" + idloged + "'";
            else
                sql = "Select rbroj as ID, Hala, RadnaPozicija,Smjena1 ,od1,do1,napomena1, Smjena2 ,od2,do2,napomena2, Smjena3 , od3,do3,napomena3,Bolovanje,Godišnji,RV1,RV2,RV3 from plandjelatnika22 ";

            //sql = "Select * from plandjelatnika22 ";
            SqlDataAdapter dataadapter1 = new SqlDataAdapter(sql, connection);
            DataSet ds1 = new DataSet();
            connection.Open();
            dataadapter1.Fill(ds1, "event1");
            connection.Close();

            dg_PlanRadnogVremena.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_PlanRadnogVremena.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_PlanRadnogVremena.DataSource = ds1;
            dg_PlanRadnogVremena.DataMember = "event1";
            dg_PlanRadnogVremena.AutoResizeColumns();
            FreezeBand(dg_PlanRadnogVremena.Columns[2]);

            for (int i = 0; i < dg_PlanRadnogVremena.ColumnCount; i++)
            {
                dg_PlanRadnogVremena.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dg_Unos_PlanaRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            DataGridViewColumn column = dg_PlanRadnogVremena.Columns[3];
            column.Width = 200;  // smjena1
            column = dg_PlanRadnogVremena.Columns[4];
            column.Width = 50;  // od1
            column = dg_PlanRadnogVremena.Columns[5];
            column.Width = 50;  //do1
            column = dg_PlanRadnogVremena.Columns[6];
            column.Width = 150;  // napomena1


            column = dg_PlanRadnogVremena.Columns[7];
            column.Width = 200;  // smjena1
            column = dg_PlanRadnogVremena.Columns[8];
            column.Width = 50;  // od1
            column = dg_PlanRadnogVremena.Columns[9];
            column.Width = 50;  //do1
            column = dg_PlanRadnogVremena.Columns[10];
            column.Width = 150;  // napomena1

            column = dg_PlanRadnogVremena.Columns[11];
            column.Width = 200;  // smjena1
            column = dg_PlanRadnogVremena.Columns[12];
            column.Width = 50;  // od1
            column = dg_PlanRadnogVremena.Columns[13];
            column.Width = 50;  //do1
            column = dg_PlanRadnogVremena.Columns[14];
            column.Width = 150;  // napomena1
            column = dg_PlanRadnogVremena.Columns[15];
            column.Width = 150;  // napomena1

            column = dg_PlanRadnogVremena.Columns[16];
            column.Width = 150;  // napomena1

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Select id, Naziv  from mjestotroska", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbx_MjestaTroska.DisplayMember = "Naziv";
            cbx_MjestaTroska.ValueMember = "id";
            cbx_MjestaTroska.DataSource = dt;
            cbx_MjestaTroska.MaxDropDownItems = 80;

            lbl_datum_pregled.Text = Lbl_datum_u.Text = dateP_datumUP.Value.ToShortDateString() + " - " + dateP_datumUP.Value.ToString("dddd", new CultureInfo("hr-HR"));

            sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in ( 705,704,715,708,706) order by prezime";
            SqlDataAdapter dataadapter2 = new SqlDataAdapter(sql, connection);
            DataSet ds2 = new DataSet();
            connection.Open();
            dataadapter2.Fill(ds2, "event2");
            connection.Close();

            dg_ListaRadnikaP.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnikaP.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnikaP.DataSource = ds2;
            dg_ListaRadnikaP.DataMember = "event2";
            dg_ListaRadnikaP.AutoResizeColumns();

            con = new SqlConnection(connectionString);
            cmd = new SqlCommand("Select id, Naziv  from mjestotroska where id not in ( 7070)", con);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);

            cbx_MT_P.DisplayMember = "Naziv";
            cbx_MT_P.ValueMember = "id";
            cbx_MT_P.DataSource = dt2;
            cbx_MT_P.MaxDropDownItems = 80;

            Ucitaj_iz_baze();

        }

        private void Ucitaj_iz_baze()
        {

            isprazni_pregledRV();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                int id1 = 0;
                string datum11;
                DateTime datum1 = date_DatumPR.Value.Date;
                DateTime datum2 = date_DatumPR.Value.Date.AddHours(23).AddMinutes(59);

                string smjena1 = "", hala1 = "", rm1 = "";
                date_DatumPR.Format = DateTimePickerFormat.Custom;
                date_DatumPR.CustomFormat = "dd.MM.yyyy HH:mm";
                int rbroj = 0;
                int rbroj1 = 0;
                string prezime1 = "", napomena1 = "";
                string rv1d = "", rv2d = "", rv3d = "";

                datum11 = date_DatumPR.Value.Year.ToString() + "-" + date_DatumPR.Value.Month.ToString() + "-" + date_DatumPR.Value.Day.ToString(); //.ToShortDateString();
                string dat1 = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                lbl_datum_pregled.Text = date_DatumPR.Value.ToShortDateString() + " - " + date_DatumPR.Value.ToString("dddd", new CultureInfo("hr-HR"));

                cn.Open();

                SqlCommand sqlCommand = new SqlCommand("select * from pregledvremena2 where datum='" + datum11 + "' order by hala,rbroj", cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();

                int i = 2;
                while (reader2.Read())
                {

                    smjena1 = (reader2["Smjena"]).ToString().Trim();
                    hala1 = (reader2["Hala"]).ToString().Trim();
                    rm1 = (reader2["RadnoMjesto"]).ToString().Trim();
                    rbroj = (int)(reader2["Rbroj"]);
                    id1 = (int)reader2["idradnika"];
                    napomena1 = reader2["Napomena"].ToString().Trim();
                    rv1d = ""; rv2d = ""; rv3d = "";

                    if (!(reader2["rv1"] is DBNull))
                        rv1d = reader2["rv1"].ToString().Trim();  // id radnog vremena za tu smjenu

                    if (!(reader2["rv2"] is DBNull))
                        rv2d = reader2["rv2"].ToString().Trim();  // id radnog vremena za tu smjenu

                    if (!(reader2["rv3"] is DBNull))
                        rv3d = reader2["rv3"].ToString().Trim();  // id radnog vremena za tu smjenu


                    //  if (id1 == 684)
                    //      id1 = 684;

                    if (smjena1 == "3")  // ako je treća smjena od podneva do drugog dana u podne
                    {
                        datum1 = date_DatumPR.Value.Date.AddHours(12);
                        datum2 = date_DatumPR.Value.Date.AddHours(36);
                    }
                    else if (smjena1 == "2")
                    {
                        datum1 = date_DatumPR.Value.Date;
                        datum2 = date_DatumPR.Value.Date.AddHours(23).AddMinutes(59);
                    }
                    else if (smjena1 == "1")
                    {
                        datum1 = date_DatumPR.Value.Date;
                        datum2 = date_DatumPR.Value.Date.AddHours(23).AddMinutes(59);
                    }

                    string dosao1 = "", dosao11 = "";
                    string otisao1 = "", otisao11 = "";
                    int rv = 0;

                    string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=fx_public;Password=.";
                    SqlDataReader rdr = null;
                    SqlConnection cnn = null;

                    using (SqlConnection cnn1 = new SqlConnection(connectionString))
                    {
                        cnn1.Open();
                        SqlCommand cmd = new SqlCommand("dbo.check_vrijeme", cnn1);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id1", SqlDbType.Int, 5).Value = id1;  // dodati parametar godinu i mjesec
                        cmd.Parameters.Add("@datum1", SqlDbType.DateTime, 5).Value = datum1;  // dodati parametar godinu i mjesec
                        cmd.Parameters.Add("@datum2", SqlDbType.DateTime, 5).Value = datum2;  // dodati parametar godinu i mjesec

                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            // reader["Datum"].ToString();
                            if (rdr.HasRows)
                            {
                                dosao1 = ((DateTime)(rdr["dosao"])).ToString("HH:mm");
                                otisao1 = ((DateTime)(rdr["otisao"])).ToString("HH:mm");

                                dosao11 = ((DateTime)(rdr["dosao"])).ToString("dd.MM.yyyy HH:mm");
                                otisao11 = ((DateTime)(rdr["otisao"])).ToString("dd.MM.yyyy HH:mm");


                                rv = (int)rdr["rv"];
                            }
                        }
                        cnn1.Close();
                    }


                    for (int ii = 0; ii < dg_PlanRadnogVremena.Rows.Count - 1; ii++)
                    {

                        string hala11 = dg_PlanRadnogVremena[1, ii].Value.ToString().TrimEnd();
                        if (Check_Hala(hala11))
                        {
                            continue;
                        }

                        if (1 == 2)
                        {

                            string data = "", res = "";
                            data = (string)dg_PlanRadnogVremena[3, ii].Value;                 //  1.smjena

                            if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                            {
                                res = new string(data.SkipWhile(c => c != '(')
                                .Skip(1)
                                .TakeWhile(c => c != ')')
                                .ToArray()).Trim();

                                smjena1 = "1";
                                id1 = (int.Parse)(res);

                                string srbroj1 = dg_PlanRadnogVremena[0, ii].Value.ToString();

                                cn.Open();

                                sqlCommand = new SqlCommand("select * from pregledvremena2 where  rbroj='" + srbroj1 + "' and  idradnika='" + id1.ToString() + "' and datum='" + datum11 + "' order by hala,rbroj", cn);
                                reader2 = sqlCommand.ExecuteReader();
                                rv1d = ""; rv2d = ""; rv3d = "";


                                connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=fx_public;Password=.";
                                rdr = null;
                                cnn = null;

                                using (SqlConnection cnn1 = new SqlConnection(connectionString))
                                {

                                    cnn1.Open();
                                    SqlCommand cmd = new SqlCommand("dbo.check_vrijeme", cnn1);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@id1", SqlDbType.Int, 5).Value = id1;  // dodati parametar godinu i mjesec
                                    cmd.Parameters.Add("@datum1", SqlDbType.DateTime, 5).Value = datum1;  // dodati parametar godinu i mjesec
                                    cmd.Parameters.Add("@datum2", SqlDbType.DateTime, 5).Value = datum2;  // dodati parametar godinu i mjesec

                                    rdr = cmd.ExecuteReader();
                                    while (rdr.Read())
                                    {
                                        // reader["Datum"].ToString();
                                        if (rdr.HasRows)
                                        {
                                            dosao1 = ((DateTime)(rdr["dosao"])).ToString("HH:mm");
                                            otisao1 = ((DateTime)(rdr["otisao"])).ToString("HH:mm");
                                            //     prezimeime1 = reader2["PrezimeIme"];
                                            rv = (int)rdr["rv"];
                                        }
                                    }

                                    cnn1.Close();

                                    dg_PlanRadnogVremena[3, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                                    dg_PlanRadnogVremena[6, ii].Value = reader2["napomena"].ToString().Trim();


                                    if (dosao1.Length > 0)
                                        dg_PlanRadnogVremena[4, ii].Value = (dosao1 == null) ? null : dosao1;

                                    if (otisao1.Length > 0)
                                        dg_PlanRadnogVremena[5, ii].Value = (otisao1 == null) ? null : otisao1;

                                    cn.Close();

                                }
                            }
                        }

                        string ime1 = dg_PlanRadnogVremena[2, ii].Value.ToString();
                        var row = dg_PlanRadnogVremena.Rows[ii];
                        rbroj1 = (int)row.Cells[0].Value;

                        //                        if ((rv1d != "") && rbroj1 == rbroj && smjena1 == "1")
                        //                        {
                        //                            dg_PlanRadnogVremena[17, ii].Value = rv1d;                            
                        //                        }

                        //                        if ((rv1d == "9") && rbroj1 == rbroj && smjena1=="1")
                        //                        {
                        //                            dg_PlanRadnogVremena[3, ii].Style.BackColor = System.Drawing.Color.Blue;
                        //                            dg_PlanRadnogVremena[7, ii].Style.BackColor = System.Drawing.Color.Blue;

                        ////                            dg_PlanRadnogVremena.Columns[3].DefaultCellStyle.BackColor = System.Drawing.Color.Blue;  //  preko 8 sati rada
                        ////                            dg_PlanRadnogVremena.Columns[7].DefaultCellStyle.BackColor = System.Drawing.Color.Blue;  //                                         
                        //                        }


                        string tdoci = "";
                        string totici = "";
                        int rv_1 = -2, rv_2 = -2, rv_3 = -2;


                        if (smjena1 == "4" && rbroj1 == rbroj)               // Bolovanje
                        {
                            dg_PlanRadnogVremena[15, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                            // dg_PlanRadnogVremena[18, ii].Value = reader2["napomena"].ToString().Trim();

                            if (smjena1 == "5" && rbroj1 == rbroj)
                            {

                            }
                        }

                        if (smjena1 == "5" && rbroj1 == rbroj)               // Godišnji
                        {
                            dg_PlanRadnogVremena[16, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                            // dg_PlanRadnogVremena[18, ii].Value = reader2["napomena"].ToString().Trim();

                            if (smjena1 == "6" && rbroj1 == rbroj)
                            {

                            }
                        }


                        if (row.Cells[1].Value.ToString().Trim() == hala1.Trim() && row.Cells[2].Value.ToString().Trim() == rm1.Trim())
                        {

                            if (smjena1 == "1" && rbroj1 == rbroj) {

                                if (rv1d == "9")
                                {
                                    dg_PlanRadnogVremena[17, ii].Value = (int.Parse)(rv1d);
                                    dg_PlanRadnogVremena[3, ii].Style.BackColor = System.Drawing.Color.LightCoral;
                                    dg_PlanRadnogVremena[7, ii].Style.BackColor = System.Drawing.Color.LightCoral;
                                }



                                switch (rv)
                                {
                                    case 1:
                                        tdoci = "06:00";
                                        totici = "14:00";
                                        break;

                                    case 2:
                                        tdoci = "07:00";
                                        totici = "15:00";
                                        break;

                                    case 3:
                                        tdoci = "8:00";
                                        totici = "16:00";
                                        break;

                                    case 4:
                                        tdoci = "12:00";
                                        totici = "20:00";
                                        break;

                                    case 5:               // zamjenici
                                        tdoci = "06:30";
                                        totici = "14:30";
                                        break;

                                    case 6:
                                        tdoci = "07:00";   // zona, sokolović
                                        totici = "15:00";
                                        break;

                                    case 7:
                                        tdoci = "10:00";   // peruša
                                        totici = "18:00";
                                        break;

                                    default:
                                        tdoci = "00:00";
                                        totici = "00:00";
                                        break;

                                }

                                if (id1 == 1125)
                                {
                                    id1 = 1125;
                                }

                                if (dosao1 != "")
                                {
                                    TimeSpan time1 = TimeSpan.Parse(dosao1);
                                    TimeSpan time2 = TimeSpan.Parse(tdoci);
                                    TimeSpan difference = time1 - time2;
                                    double minutes = difference.TotalMinutes;

                                    if (minutes > 0)
                                    {
                                        row.Cells[4].Style.BackColor = System.Drawing.Color.LightPink;
                                        row.Cells[5].Style.BackColor = System.Drawing.Color.LightPink;
                                    }

                                }
                                else
                                {

                                    if (dosao1 == otisao1)
                                    {
                                        row.Cells[4].Style.BackColor = System.Drawing.Color.LightBlue;
                                        row.Cells[5].Style.BackColor = System.Drawing.Color.LightBlue;
                                    }
                                }


                                dg_PlanRadnogVremena[3, ii].Value = reader2["PrezimeIme"];
                                dg_PlanRadnogVremena[6, ii].Value = reader2["napomena"].ToString().Trim();

                                if (dosao1.Length > 0)
                                    dg_PlanRadnogVremena[4, ii].Value = (dosao1 == null) ? null : dosao1;

                                if (otisao1.Length > 0)
                                    dg_PlanRadnogVremena[5, ii].Value = (otisao1 == null) ? null : otisao1;

                            }

                            if (smjena1 == "2" && rbroj1 == rbroj)
                            {

                                dg_PlanRadnogVremena[7, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                                dg_PlanRadnogVremena[10, ii].Value = reader2["napomena"].ToString().Trim();

                                if (smjena1 == "2" && rbroj1 == rbroj)
                                {

                                    if (rv2d == "9")
                                    {
                                        dg_PlanRadnogVremena[18, ii].Value = (int.Parse)(rv2d);
                                        dg_PlanRadnogVremena[7, ii].Style.BackColor = System.Drawing.Color.LightCoral;
                                        dg_PlanRadnogVremena[11, ii].Style.BackColor = System.Drawing.Color.LightCoral;
                                    }

                                    switch (rv)
                                    {
                                        case 1:
                                            tdoci = "14:00";
                                            totici = "22:00";
                                            break;

                                        case 5:
                                            tdoci = "14:30";
                                            totici = "21:30";
                                            break;


                                        case 6:
                                            tdoci = "14:00";
                                            totici = "22:00";
                                            break;


                                        default:
                                            tdoci = "00:00";
                                            totici = "00:00";
                                            break;
                                    }


                                    if (dosao1 != "")
                                    {
                                        TimeSpan time1 = TimeSpan.Parse(dosao1);
                                        TimeSpan time2 = TimeSpan.Parse(tdoci);
                                        TimeSpan difference = time1 - time2;
                                        double minutes = difference.TotalMinutes;

                                        if (minutes > 0)
                                        {
                                            row.Cells[8].Style.BackColor = System.Drawing.Color.LightPink;
                                            row.Cells[9].Style.BackColor = System.Drawing.Color.LightPink;
                                        }

                                    }
                                    else
                                    {
                                        if (dosao1 == otisao1)
                                        {
                                            row.Cells[8].Style.BackColor = System.Drawing.Color.LightBlue;
                                            row.Cells[9].Style.BackColor = System.Drawing.Color.LightBlue;
                                        }
                                    }

                                    if (dosao1.Length > 0)
                                        dg_PlanRadnogVremena[8, ii].Value = (dosao1 == null) ? null : dosao1;

                                    if (otisao1.Length > 0)
                                        dg_PlanRadnogVremena[9, ii].Value = (otisao1 == null) ? null : otisao1;
                                }
                            }


                            if (smjena1 == "3" && rbroj1 == rbroj)
                            {
                                dg_PlanRadnogVremena[11, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                                dg_PlanRadnogVremena[14, ii].Value = reader2["napomena"].ToString().Trim();

                                if (smjena1 == "3" && rbroj1 == rbroj)
                                {

                                    if (rv3d == "9")
                                    {
                                        dg_PlanRadnogVremena[19, ii].Value = (int.Parse)(rv3d);
                                        dg_PlanRadnogVremena[11, ii].Style.BackColor = System.Drawing.Color.LightCoral;
                                        dg_PlanRadnogVremena[3, ii].Style.BackColor = System.Drawing.Color.LightCoral;
                                    }


                                    switch (rv)
                                    {
                                        case 1:
                                            tdoci = "22:00";
                                            totici = "06:00";
                                            break;

                                        default:

                                            tdoci = "0:00";
                                            totici = "0:00";
                                            break;
                                    }


                                    //datum11 = date_DatumPR.Value.Year.ToString() + "-" + date_DatumPR.Value.Month.ToString() + "-" + date_DatumPR.Value.Day.ToString(); //.ToShortDateString();
                                    double minutes = 0;

                                    if (dosao1 != "")
                                    {
                                        TimeSpan time1 = TimeSpan.Parse(dosao1);
                                        TimeSpan time2 = TimeSpan.Parse(tdoci);
                                        TimeSpan difference = time1 - time2;
                                        minutes = difference.TotalMinutes;

                                        if (minutes > 0)
                                        {
                                            row.Cells[12].Style.BackColor = System.Drawing.Color.LightPink;
                                            row.Cells[13].Style.BackColor = System.Drawing.Color.LightPink;
                                        }
                                        else if (minutes < 120)
                                        {
                                            row.Cells[12].Style.BackColor = System.Drawing.Color.LightPink;
                                            row.Cells[13].Style.BackColor = System.Drawing.Color.LightPink;
                                        }


                                    }
                                    else
                                    {
                                        if (dosao1 == otisao1)
                                        {
                                            row.Cells[12].Style.BackColor = System.Drawing.Color.LightBlue;
                                            row.Cells[13].Style.BackColor = System.Drawing.Color.LightBlue;
                                        }
                                    }


                                    if ((dosao1.Length > 0) && Math.Abs(minutes) < 400)
                                        dg_PlanRadnogVremena[12, ii].Value = (dosao1 == null) ? null : dosao1;

                                    if ((otisao1.Length > 0) && Math.Abs(minutes) < 400)
                                        dg_PlanRadnogVremena[13, ii].Value = (otisao1 == null) ? null : otisao1;

                                }
                            }

                            //



                        }

                    }

                    int zz = 1;

                }
                cn.Close();
            }
        }


        // unos plana, filter hale
        private void cbx_Hala_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ssindex = "";
            ssindex = cbx_Hala.SelectedIndex.ToString();
            string hala1 = "";


            if (ssindex == "0")
            {
                hala1 = "0";
            }

            if (ssindex == "1")
            {
                hala1 = "1";
            }

            if (ssindex == "2")
            {
                hala1 = "3";
            }

            if (ssindex == "3")
            {
                hala1 = "Zona";

            }



            for (int i = 0; i < dg_Unos_PlanaRadnika.Rows.Count - 1; i++)
            {
                var row = dg_Unos_PlanaRadnika.Rows[i];


                if (row.Cells[1].Value.ToString().Trim() == hala1.Trim() || hala1 == "0")
                {
                    //row.Selected = true;
                    row.Visible = true;
                }
                else
                {
                    // row.Selected = false;
                    dg_Unos_PlanaRadnika.CurrentCell = null;
                    row.Visible = false;
                }
            }



        }

        private void dg_PlanRadnogVremena_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            panel_rv.Visible = true;

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

            Ucitaj_iz_baze();
        }

        private void isprazni_pregledRV()
        {

            for (int ii = 0; ii < dg_PlanRadnogVremena.Rows.Count - 1; ii++)
            {

                dg_PlanRadnogVremena.Rows[ii].Height = 20;
                //dg_PlanRadnogVremena.Rows[ii].DefaultCellStyle.Font.Height = 10;


                for (int jj = 3; jj < dg_PlanRadnogVremena.ColumnCount - 1; jj++)
                {

                    dg_PlanRadnogVremena[jj, ii].Value = DBNull.Value;

                    if (jj == 3 || jj == 7 || jj == 11)
                    {
                        dg_PlanRadnogVremena[jj, ii].Style.BackColor = System.Drawing.Color.Ivory;
                    }
                    else
                    {
                        dg_PlanRadnogVremena[jj, ii].Style.BackColor = System.Drawing.Color.White;

                    }


                }
            }


            for (int i = 0; i < dg_ListaRadnikaP.RowCount; i++)
            {
                dg_ListaRadnikaP.Rows[i].Height = 20;
            }


        }


        private void isprazni_pregledRV1()
        {

            for (int ii = 0; ii < dg_Unos_PlanaRadnika.Rows.Count - 1; ii++)
            {

                for (int jj = 3; jj < dg_Unos_PlanaRadnika.ColumnCount - 1; jj++)
                {

                    dg_Unos_PlanaRadnika[jj, ii].Value = DBNull.Value;
                    dg_Unos_PlanaRadnika[jj, ii].Style.BackColor = System.Drawing.Color.White;

                }
            }

        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (CloseCancel() == false)
            {
                e.Cancel = true;
            };
        }

        public static bool CloseCancel()
        {
            const string message = "Dali ste spremili sve podatke ?";
            const string caption = "Izlaz iz programa";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }


        private void cbx_ListaHalaP_SelectedIndexChanged(object sender, EventArgs e)
        {


            string ssindex = "";
            ssindex = cbx_ListaHalaP.SelectedIndex.ToString();
            string hala1 = "";


            if (ssindex == "0")
            {
                hala1 = "0";
            }

            if (ssindex == "1")
            {
                hala1 = "1";
            }

            if (ssindex == "2")
            {
                hala1 = "3";
            }

            if (ssindex == "3")
            {
                hala1 = "Zona";
            }



            for (int i = 0; i < dg_PlanRadnogVremena.Rows.Count - 1; i++)
            {
                var row = dg_PlanRadnogVremena.Rows[i];


                if (row.Cells[1].Value.ToString().Trim() == hala1.Trim() || hala1 == "0")
                {
                    //row.Selected = true;
                    row.Visible = true;
                }
                else
                {
                    // row.Selected = false;
                    dg_PlanRadnogVremena.CurrentCell = null;
                    row.Visible = false;
                }
            }



        }

        // Unos plana, button ucitaj postojeći plan rasporeda

        private void btn_ucitajPlanU_Click(object sender, EventArgs e)
        {
            //    ..................

            isprazni_pregledRV1();


            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                int id1 = 0;
                string datum11;
                DateTime datum1 = date_DatumPR.Value.Date;
                DateTime datum2 = date_DatumPR.Value.Date.AddHours(23).AddMinutes(59);

                string smjena1 = "", hala1 = "", rm1 = "";
                date_DatumPR.Format = DateTimePickerFormat.Custom;
                date_DatumPR.CustomFormat = "dd.MM.yyyy HH:mm";
                dateP_datumUP.Format = DateTimePickerFormat.Custom;
                dateP_datumUP.CustomFormat = "dd.MM.yyyy HH:mm";
                Lbl_datum_u.Text = dateP_datumUP.Value.ToShortDateString() + " - " + dateP_datumUP.Value.ToString("dddd", new CultureInfo("hr-HR"));

                int rbroj = 0;
                int rbroj1 = 0;
                string prezime1 = "";


                datum11 = date_DatumPR.Value.Year.ToString() + "-" + date_DatumPR.Value.Month.ToString() + "-" + date_DatumPR.Value.Day.ToString(); //.ToShortDateString();
                datum11 = dateP_datumUP.Value.Year.ToString() + "-" + dateP_datumUP.Value.Month.ToString() + "-" + dateP_datumUP.Value.Day.ToString(); //.ToShortDateString();
                string dat1 = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

                cn.Open();

                SqlCommand sqlCommand = new SqlCommand("select * from pregledvremena2 where datum='" + datum11 + "' order by hala,rbroj", cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                int i = 2;
                while (reader2.Read())
                {

                    smjena1 = (reader2["Smjena"]).ToString().Trim();
                    hala1 = (reader2["Hala"]).ToString().Trim();
                    rm1 = (reader2["RadnoMjesto"]).ToString().Trim();
                    rbroj = (int)(reader2["Rbroj"]);
                    id1 = (int)reader2["idradnika"];
                    string dosao1 = "";
                    string otisao1 = "";
                    int rv = 0;



                    for (int ii = 0; ii < dg_Unos_PlanaRadnika.Rows.Count - 1; ii++)
                    {


                        var row = dg_Unos_PlanaRadnika.Rows[ii];
                        rbroj1 = (int)row.Cells[0].Value;
                        string tdoci = "";
                        string totici = "";

                        if (row.Cells[1].Value.ToString().Trim() == hala1.Trim() && row.Cells[2].Value.ToString().Trim() == rm1.Trim())
                        {

                            if (smjena1 == "1" && rbroj1 == rbroj)
                            {
                                dg_Unos_PlanaRadnika[3, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                            }
                        }



                        if (row.Cells[1].Value.ToString().Trim() == hala1.Trim() && row.Cells[2].Value.ToString().Trim() == rm1.Trim())
                        {

                            if (smjena1 == "2" && rbroj1 == rbroj)
                            {

                                dg_Unos_PlanaRadnika[4, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                            }
                        }


                        if (row.Cells[1].Value.ToString().Trim() == hala1.Trim() && row.Cells[2].Value.ToString().Trim() == rm1.Trim())
                        {

                            if (smjena1 == "3" && rbroj1 == rbroj)
                            {

                                dg_Unos_PlanaRadnika[5, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                            }
                        }


                        if (smjena1 == "4" && rbroj1 == rbroj)   // Bolovanje
                        {

                            dg_Unos_PlanaRadnika[6, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                        }

                        if (smjena1 == "5" && rbroj1 == rbroj)    // godišnji
                        {

                            dg_Unos_PlanaRadnika[7, ii].Value = reader2["PrezimeIme"].ToString().Trim();
                        }

                    }

                }

                cn.Close();
            }
        }

        private void btn_PregledRV_spremi_Click(object sender, EventArgs e)
        {
            string srbroj1 = "", napomena = "";
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand sqlCommand;
            SqlDataReader reader2;
            string datum11 = date_DatumPR.Value.Year.ToString() + "-" + date_DatumPR.Value.Month.ToString() + "-" + date_DatumPR.Value.Day.ToString(); //.ToShortDateString();
            int y1 = date_DatumPR.Value.Year;
            int m1 = date_DatumPR.Value.Month;
            int d1 = date_DatumPR.Value.Day;

            int y2 = date_DatumPR.Value.AddDays(1).Year;
            int m2 = date_DatumPR.Value.AddDays(1).Month;
            int d2 = date_DatumPR.Value.AddDays(1).Day;

            string dosao1;
            string otisao1;

            for (int ii = 0; ii < dg_PlanRadnogVremena.Rows.Count - 1; ii++)
            {
                srbroj1 = dg_PlanRadnogVremena[0, ii].Value.ToString();

                string hala1 = dg_PlanRadnogVremena[1, ii].Value.ToString();
                if (Check_Hala(hala1))
                {
                    continue;
                }

                for (int jj = 3; jj < dg_PlanRadnogVremena.ColumnCount - 1; jj++)
                {

                    if (jj == 6)
                    {

                        napomena = dg_PlanRadnogVremena[6, ii].Value.ToString();

                        DateTime date1 = new DateTime(y1, m1, d1, 7, 0, 0);
                        //string[] dosao1 = (dg_PlanRadnogVremena[4, ii].Value.ToString()).Split(';');
                        //int h1 = (int.Parse)(dosao1[0]);  int min1 = (int.Parse)(dosao1[1]);

                        if (dg_PlanRadnogVremena[4, ii].Value != DBNull.Value)
                        {
                            dosao1 = y1.ToString() + "-" + m1.ToString() + "-" + d1.ToString() + " " + dg_PlanRadnogVremena[4, ii].Value.ToString();
                        }
                        else
                        {
                            dosao1 = "1900-01-01 00:00.0";
                        }


                        if (dg_PlanRadnogVremena[5, ii].Value != DBNull.Value)
                        {
                            otisao1 = y1.ToString() + "-" + m1.ToString() + "-" + d1.ToString() + " " + dg_PlanRadnogVremena[5, ii].Value.ToString();

                        }
                        else
                        {
                            otisao1 = "1900-01-01 00:00.0";
                        }

                        //DateTime otisao1 = (DateTime)(dg_PlanRadnogVremena[5, ii].Value.ToString());
                        if (dg_PlanRadnogVremena[6, ii].Value != DBNull.Value)
                        {
                            cn.Open();
                            sqlCommand = new SqlCommand("update pregledvremena2 set napomena='" + napomena + "',dosao='" + dosao1 + "', otisao='" + otisao1 + "' where smjena=1 and rbroj=" + srbroj1 + "and datum='" + datum11 + "'", cn);
                            reader2 = sqlCommand.ExecuteReader();
                            cn.Close();
                        }

                    }

                    if (dg_PlanRadnogVremena[8, ii].Value != DBNull.Value)
                    {
                        dosao1 = y1.ToString() + "-" + m1.ToString() + "-" + d1.ToString() + " " + dg_PlanRadnogVremena[8, ii].Value.ToString();
                    }
                    else
                    {
                        dosao1 = "1900-01-01 00:00.0";
                    }


                    if (dg_PlanRadnogVremena[9, ii].Value != DBNull.Value)
                    {
                        otisao1 = y1.ToString() + "-" + m1.ToString() + "-" + d1.ToString() + " " + dg_PlanRadnogVremena[9, ii].Value.ToString();
                    }
                    else
                    {
                        otisao1 = "1900-01-01 00:00.0";
                    }


                    if (jj == 10)
                    {

                        napomena = dg_PlanRadnogVremena[jj, ii].Value.ToString();
                        if (dg_PlanRadnogVremena[jj, ii].Value != DBNull.Value)
                        {
                            cn.Open();
                            sqlCommand = new SqlCommand("update pregledvremena2 set napomena='" + napomena + "',dosao='" + dosao1 + "', otisao='" + otisao1 + "' where smjena=2 and rbroj=" + srbroj1 + "and datum='" + datum11 + "'", cn);
                            reader2 = sqlCommand.ExecuteReader();
                            cn.Close();
                        }

                    }


                    if (dg_PlanRadnogVremena[12, ii].Value != DBNull.Value)
                    {
                        dosao1 = y1.ToString() + "-" + m1.ToString() + "-" + d1.ToString() + " " + dg_PlanRadnogVremena[12, ii].Value.ToString();
                    }
                    else
                    {
                        dosao1 = "1900-01-01 00:00.0";
                    }


                    if (dg_PlanRadnogVremena[13, ii].Value != DBNull.Value)
                    {

                        string[] otisao11 = dg_PlanRadnogVremena[13, ii].Value.ToString().Split(':');
                        int h1 = (int.Parse)(otisao11[0]);

                        if (h1 > 21 && h1 < 24)
                            otisao1 = y1.ToString() + "-" + m1.ToString() + "-" + d1.ToString() + " " + dg_PlanRadnogVremena[13, ii].Value.ToString();
                        else
                            otisao1 = y2.ToString() + "-" + m2.ToString() + "-" + d2.ToString() + " " + dg_PlanRadnogVremena[13, ii].Value.ToString();
                    }
                    else
                    {
                        otisao1 = "1900-01-01 00:00.0";
                    }

                    if (jj == 14)
                    {

                        napomena = dg_PlanRadnogVremena[jj, ii].Value.ToString();
                        if (dg_PlanRadnogVremena[jj, ii].Value != DBNull.Value)
                        {
                            cn.Open();
                            sqlCommand = new SqlCommand("update pregledvremena2 set napomena='" + napomena + "',dosao='" + dosao1 + "', otisao='" + otisao1 + "' where smjena=3 and rbroj=" + srbroj1 + "and datum='" + datum11 + "'", cn);
                            reader2 = sqlCommand.ExecuteReader();
                            cn.Close();
                        }

                    }

                }
            }

        }

        private void pl_Planiranje_Unos_Paint(object sender, PaintEventArgs e)
        {

        }
        private Boolean Check_Hala(string hala1)
        {

            if (idloged == "1" && hala1 == "3")   // gradiški
                return false;

            if (idloged == "2" && hala1 == "1")   // kičin
                return false;

            if (idloged == "3" && hala1 == "3")   // sok0lović
                return false;

            if (idloged == "4" && hala1 == "3")   // franceković
                return false;

            if (idloged == "5" && hala1 == "3")   // hajtok
                return false;

            if (idloged == "6" && (hala1 != "3" || hala1 != "1"))    // tadić
                return false;

            if (idloged == "8")    // admin
                return false;

            return true;

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dg_Unos_PlanaRadnika_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dg_Unos_PlanaRadnika_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            var hittestInfo = dg_Unos_PlanaRadnika.HitTest(e.X, e.Y);
            //public int red1;

            if (hittestInfo.RowIndex != -1 && hittestInfo.ColumnIndex != -1)
            {
                valueFromMouseDown = dg_Unos_PlanaRadnika.Rows[hittestInfo.RowIndex].Cells[hittestInfo.ColumnIndex].Value;
                if (valueFromMouseDown != null)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;

        }

        private void dg_Unos_PlanaRadnika_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dg_Unos_PlanaRadnika.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                    //red1 = dg_ListaRadnika.CurrentCell.RowIndex;
                }
            }
        }

        private void dg_Unos_PlanaRadnika_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_Unos_PlanaRadnika_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            {
                if (e.RowIndex != -1 && e.Value != null && e.Value.ToString().Length > 5 && e.ColumnIndex == -333)
                {
                    if (!e.Handled)
                    {
                        e.Handled = true;
                        e.PaintBackground(e.CellBounds, dg_Unos_PlanaRadnika.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected);
                    }
                    if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != DataGridViewPaintParts.None)
                    {
                        string text = e.Value.ToString();
                        string textPart1 = text.Substring(0, text.Length);
                        string textPart2 = text.Substring(0, text.Length);
                        Size fullsize = TextRenderer.MeasureText(text, e.CellStyle.Font);
                        Size size1 = TextRenderer.MeasureText(textPart1, e.CellStyle.Font);
                        Size size2 = TextRenderer.MeasureText(textPart2, e.CellStyle.Font);
                        Rectangle rect1 = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
                        using (Brush cellForeBrush = new SolidBrush(e.CellStyle.BackColor))
                        {
                            e.Graphics.DrawString(textPart1, e.CellStyle.Font, cellForeBrush, rect1);
                        }
                        rect1.X += (fullsize.Width);
                        rect1.Width = e.CellBounds.Width;
                        e.Graphics.DrawString(textPart2, e.CellStyle.Font, Brushes.Crimson, rect1);

                        using (Brush cellForeBrush = new SolidBrush(System.Drawing.Color.LightGreen))
                        {
                            e.Graphics.FillRectangle(cellForeBrush, rect1);
                        }
                        e.Graphics.DrawString(textPart2, e.CellStyle.Font, Brushes.Crimson, rect1);
                    }
                }




                //if (e.RowIndex != -1 && e.Value != null && e.Value.ToString().Length > 5 && e.ColumnIndex == 3)
                //{
                //    if (!e.Handled)
                //    {
                //        e.Handled = true;
                //        e.PaintBackground(e.CellBounds, dg_Unos_PlanaRadnika.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected);
                //    }
                //    if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != DataGridViewPaintParts.None)
                //    {
                //        string text = e.Value.ToString();
                //        string textPart1 = text.Substring(0, text.Length - 5);
                //        string textPart2 = text.Substring(text.Length - 5, 5);
                //        Size fullsize = TextRenderer.MeasureText(text, e.CellStyle.Font);
                //        Size size1 = TextRenderer.MeasureText(textPart1, e.CellStyle.Font);
                //        Size size2 = TextRenderer.MeasureText(textPart2, e.CellStyle.Font);
                //        Rectangle rect1 = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
                //        using (Brush cellForeBrush = new SolidBrush(e.CellStyle.ForeColor))
                //        {
                //            e.Graphics.DrawString(textPart1, e.CellStyle.Font, cellForeBrush, rect1);
                //        }
                //        rect1.X += (fullsize.Width - size2.Width);
                //        rect1.Width = e.CellBounds.Width;
                //        e.Graphics.DrawString(textPart2, e.CellStyle.Font, Brushes.Crimson, rect1);
                //    }
                //}

            }
        }

        private void dg_ListaRadnikaP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dg_ListaRadnikaP.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                    //red1 = dg_ListaRadnika.CurrentCell.RowIndex;
                }
            }

        }


        private void dg_ListaRadnikaP_MouseDown_1(object sender, MouseEventArgs e)
        {

        }

        private void dg_PlanRadnogVremena_DragDrop_1(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dg_PlanRadnogVremena.PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a copy then add the row to the other control.
            if (e.Effect == DragDropEffects.Copy)
            {
                string cellvalue = e.Data.GetData(typeof(string)) as string;
                var hittest = dg_PlanRadnogVremena.HitTest(clientPoint.X, clientPoint.Y);
                if (hittest.ColumnIndex > 1 && hittest.RowIndex >= 0)
                {
                    dg_PlanRadnogVremena[hittest.ColumnIndex, hittest.RowIndex].Value = cellvalue;
                    dg_ListaRadnikaP[1, dg_ListaRadnikaP.CurrentCell.RowIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                }

            }
        }

        private void dg_PlanRadnogVremena_DragOver_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


        private void cbx_MT_P_SelectedIndexChanged(object sender, EventArgs e)
        {

            string ssindex = "";
            ssindex = cbx_MT_P.SelectedValue.ToString();

            string sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in ( 705,704,715,708,707,706) and r.neradi=0 order by prezime";
            sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where r.mt not in (707) and r.neradi=0 order by prezime";

            if (ssindex != "0")
            {
                //sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt left join odjeli o on o.mt=r.mt where r.mt not in ( 705,704,715,708,707,706) and  o.mt= " + ssindex + " order by prezime";
                sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where mt.id not in ( 7070) and mt.id= " + ssindex + " and r.neradi=0 order by prezime";
            }
            else
            {
                sql = "Select r.ID,(prezime+' '+ime+ ' ('+  str(r.id,4) +' ) ' ) as Prezime_Ime,r.MT ,mt.naziv NazivMjestaTroška from radnici_ r left join mjestotroska mt on mt.id=r.mt where mt.id not in (7070) and r.neradi=0 order by prezime";
            }

            //cbl_ListaOdjela.SelectedItems;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "event");
            connection.Close();

            dg_ListaRadnikaP.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnikaP.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dg_ListaRadnikaP.DataSource = ds;
            dg_ListaRadnikaP.DataMember = "event";

            dg_ListaRadnikaP.AutoResizeColumns();
            //dg_ListaRadnikaP.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);



        }

        private void dg_ListaRadnikaP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dg_ListaRadnikaP_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dg_ListaRadnikaP.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }

        private void dg_ListaRadnikaP_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            var hittestInfo = dg_ListaRadnikaP.HitTest(e.X, e.Y);
            //public int red1;

            if (hittestInfo.RowIndex != -1 && hittestInfo.ColumnIndex != -1)
            {
                valueFromMouseDown = dg_ListaRadnikaP.Rows[hittestInfo.RowIndex].Cells[hittestInfo.ColumnIndex].Value;
                if (valueFromMouseDown != null)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dg_PlanRadnogVremena_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void btn_SpremiPlan2()
        {

            string sql1 = "", sql2 = "2", sql3 = "", sqlb = "", sqlg = "";
            string napomena1 = "";
            string napomena2 = "";
            string napomena3 = "";
            string napomenab = "";
            string napomenag = "";
            string data = "";
            string prezime = "";
            string tdoci = "6:0", totici = "14:0";
            int rv = 1;
            SqlCommand sqlCommand;
            SqlDataReader reader2, rdr;

            for (int i = 0; i < dg_PlanRadnogVremena.RowCount - 1; i++)
            {

                napomena1 = "";
                napomena2 = "";
                napomena3 = "";
                napomenab = "";
                napomenag = "";

                string hala1 = dg_PlanRadnogVremena[1, i].Value.ToString().TrimEnd();
                if (Check_Hala(hala1))
                {
                    continue;
                }

                string RadnoMjesto = dg_PlanRadnogVremena[2, i].Value.ToString();

                string datum11;
                datum11 = date_Spremi22.Value.Year.ToString() + "-" + date_Spremi22.Value.Month.ToString() + "-" + date_Spremi22.Value.Day.ToString(); //.ToShortDateString();

                DateTime datum1 = date_Spremi22.Value.Date;
                DateTime datum2 = date_Spremi22.Value.Date.AddHours(23).AddMinutes(59);

                string dat1 = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                string res = "";
                string smjena1 = "1";
                string rv1 = "";
                int rbroj1 = 0;

                if (DBNull.Value.Equals(dg_PlanRadnogVremena[3, i].Value))
                {
                    sql1 = "";
                }
                else
                {

                    data = (string)dg_PlanRadnogVremena[3, i].Value;
                    rv1 = (string)dg_PlanRadnogVremena[17, i].Value.ToString();
                    napomena1 = (string)dg_PlanRadnogVremena[6, i].Value;
                    rbroj1 = (int)dg_PlanRadnogVremena[0, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;                        
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "1";
                        prezime = data;


                        int id1 = (int.Parse)(res);
                        rv = 0;
                        string dosao1 = "", dosao11 = "";
                        string otisao1 = "", otisao11 = "";

                        using (SqlConnection cnn1 = new SqlConnection(connectionString))
                        {

                            cnn1.Open();
                            SqlCommand cmd = new SqlCommand("dbo.check_vrijeme", cnn1);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id1", SqlDbType.Int, 5).Value = id1;  // dodati parametar godinu i mjesec
                            cmd.Parameters.Add("@datum1", SqlDbType.DateTime, 5).Value = datum1;  // dodati parametar godinu i mjesec
                            cmd.Parameters.Add("@datum2", SqlDbType.DateTime, 5).Value = datum2;  // dodati parametar godinu i mjesec

                            rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                // reader["Datum"].ToString();
                                if (rdr.HasRows)
                                {
                                    dosao1 = ((DateTime)(rdr["dosao"])).ToString("HH:mm");
                                    otisao1 = ((DateTime)(rdr["otisao"])).ToString("HH:mm");
                                    //dosao11 = ((DateTime)(rdr["dosao"])).ToString("dd.MM.yyyy HH:mm:ss");
                                    //otisao11 = ((DateTime)(rdr["otisao"])).ToString("dd.MM.yyyy HH:mm:ss");

                                    dosao11 = ((DateTime)(rdr["dosao"])).ToString("yyyy-MM-dd HH:mm:ss");
                                    otisao11 = ((DateTime)(rdr["otisao"])).ToString("yyyy-MM-dd HH:mm:ss");

                                    //     prezimeime1 = reader2["PrezimeIme"];
                                    rv = (int)rdr["rv"];
                                }
                                else
                                {
                                    dosao1 = "1900-01-01 00:00.0";
                                    otisao1 = "1900-01-01 00:00.0";
                                    dosao11 = dosao1;
                                    otisao11 = otisao1;
                                }
                            }

                            cnn1.Close();
                        }

                        long minutes = 0;
                        long uminutes = 0;
                        if (dosao1 != "")
                        {
                            DateTime dos1 = DateTime.ParseExact(dosao11, "yyyy-MM-dd HH:mm:ss", null);
                            DateTime oti1 = DateTime.ParseExact(otisao11, "yyyy-MM-dd HH:mm:ss", null);
                            tdoci = dos1.ToString("dd.MM.yyyy") + " " + TrebaDoci(rv, "1") + ":00";
                            DateTime tdos1 = DateTime.ParseExact(tdoci, "dd.MM.yyyy HH:mm:ss", null);

                            TimeSpan t = dos1 - tdos1;
                            TimeSpan ut = oti1 - dos1;
                            minutes = (long)t.TotalMinutes;
                            uminutes = (long)ut.TotalMinutes;

                            //double minutes = difference.TotalMinutes;

                            if (minutes > 0)
                            {

                            }

                        }
                        else
                        {
                            if (dosao1 == otisao1)
                            {

                            }
                        }


                        sql1 = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',1,'" + hala1 + "','" + RadnoMjesto + "','" + napomena1 + "','" + dat1 + "'";
                        sql1 = sql1 + ",'" + dosao11 + "','" + otisao11 + "'," + minutes.ToString() + "," + uminutes.ToString() + ",'" + rv1 + "'";

                        //sql1 = "update pregledvremena2 set rbroj='" + rbroj1.ToString() + "',idradnika=" + res + ",prezimeime='" + prezime + "',datum='" + datum11 + "',smjena='1',hala='" + hala1 + "',radnomjesto='" + RadnoMjesto + "',datumunosa='" + dat1 + "'";

                    }
                    else
                    {
                        sql1 = "";
                    }
                }

                // druga smjena
                string rv2 = "";
                if (DBNull.Value.Equals(dg_PlanRadnogVremena[7, i].Value))
                {
                    sql2 = "";
                }
                else {

                    data = (string)dg_PlanRadnogVremena[7, i].Value;
                    napomena2 = (string)dg_PlanRadnogVremena[10, i].Value;
                    rv2 = (string)dg_PlanRadnogVremena[18, i].Value.ToString();
                    rbroj1 = (int)dg_PlanRadnogVremena[0, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "2";
                        prezime = data;
                        int id1 = (int.Parse)(res);
                        rv = 0;
                        string dosao1 = "", dosao11 = "";
                        string otisao1 = "", otisao11 = "";

                        using (SqlConnection cnn1 = new SqlConnection(connectionString))
                        {

                            cnn1.Open();
                            SqlCommand cmd = new SqlCommand("dbo.check_vrijeme", cnn1);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id1", SqlDbType.Int, 5).Value = id1;  // dodati parametar godinu i mjesec
                            cmd.Parameters.Add("@datum1", SqlDbType.DateTime, 5).Value = datum1;  // dodati parametar godinu i mjesec
                            cmd.Parameters.Add("@datum2", SqlDbType.DateTime, 5).Value = datum2;  // dodati parametar godinu i mjesec

                            rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                // reader["Datum"].ToString();
                                if (rdr.HasRows)
                                {
                                    dosao1 = ((DateTime)(rdr["dosao"])).ToString("HH:mm");
                                    otisao1 = ((DateTime)(rdr["otisao"])).ToString("HH:mm");
                                    dosao11 = ((DateTime)(rdr["dosao"])).ToString("yyyy-MM-dd HH:mm:ss");
                                    otisao11 = ((DateTime)(rdr["otisao"])).ToString("yyyy-MM-dd HH:mm:ss");
                                    //     prezimeime1 = reader2["PrezimeIme"];
                                    rv = (int)rdr["rv"];
                                }
                                else
                                {
                                    dosao1 = "1900-01-01 00:00.0";
                                    otisao1 = "1900-01-01 00:00.0";
                                    dosao11 = dosao1;
                                    otisao11 = otisao1;
                                }
                            }

                            cnn1.Close();
                        }


                        long minutes = 0;
                        long uminutes = 0;
                        if (dosao1 != "")
                        {
                            DateTime dos1 = DateTime.ParseExact(dosao11, "yyyy-MM-dd HH:mm:ss", null);
                            DateTime oti1 = DateTime.ParseExact(otisao11, "yyyy-MM-dd HH:mm:ss", null);
                            tdoci = dos1.ToString("dd.MM.yyyy") + " " + TrebaDoci(rv, "1") + ":00";
                            DateTime tdos1 = DateTime.ParseExact(tdoci, "dd.MM.yyyy HH:mm:ss", null);

                            TimeSpan t = dos1 - tdos1;
                            TimeSpan ut = oti1 - dos1;
                            minutes = (long)t.TotalMinutes;
                            uminutes = (long)ut.TotalMinutes;

                            if (minutes > 0)
                            {

                            }

                        }
                        else
                        {

                            if (dosao1 == otisao1)
                            {

                            }
                        }


                        sql2 = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',2,'" + hala1 + "','" + RadnoMjesto + "','" + napomena2 + "','" + dat1 + "'";
                        sql2 = sql2 + ",'" + dosao1 + "','" + otisao1 + "'," + minutes.ToString() + "," + uminutes.ToString() + ",'" + rv2 + "'";

                        //sql2 = "update pregledvremena2 set rbroj='" + rbroj1.ToString() + "',idradnika=" + res + ",prezimeime='" + prezime + "',datum='" + datum11 + "',smjena='2',hala='" + hala1 + "',radnomjesto='" + RadnoMjesto + "',datumunosa='" + dat1 + "'";
                    }
                    else
                    {
                        sql2 = "";
                    }
                }

                // 3.smjena

                string rv3 = "";

                if (DBNull.Value.Equals(dg_PlanRadnogVremena[11, i].Value))
                {
                    sql3 = "";
                }
                else
                {

                    rbroj1 = (int)dg_PlanRadnogVremena[0, i].Value;
                    data = (string)dg_PlanRadnogVremena[11, i].Value;
                    rv3 = (string)dg_PlanRadnogVremena[19, i].Value.ToString();
                    napomena3 = (string)dg_PlanRadnogVremena[14, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "3";
                        prezime = data;
                        int id1 = (int.Parse)(res);
                        rv = 0;
                        string dosao1 = "", dosao11 = "";
                        string otisao1 = "", otisao11 = "";

                        using (SqlConnection cnn1 = new SqlConnection(connectionString))
                        {

                            cnn1.Open();
                            SqlCommand cmd = new SqlCommand("dbo.check_vrijeme", cnn1);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id1", SqlDbType.Int, 5).Value = id1;  // dodati parametar godinu i mjesec
                            cmd.Parameters.Add("@datum1", SqlDbType.DateTime, 5).Value = datum1;  // dodati parametar godinu i mjesec
                            cmd.Parameters.Add("@datum2", SqlDbType.DateTime, 5).Value = datum2;  // dodati parametar godinu i mjesec

                            rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                // reader["Datum"].ToString();
                                if (rdr.HasRows)
                                {
                                    dosao1 = ((DateTime)(rdr["dosao"])).ToString("HH:mm");
                                    otisao1 = ((DateTime)(rdr["otisao"])).ToString("HH:mm");
                                    dosao11 = ((DateTime)(rdr["dosao"])).ToString("yyyy-MM-dd HH:mm:ss");
                                    otisao11 = ((DateTime)(rdr["otisao"])).ToString("yyyy-MM-dd HH:mm:ss");
                                    //     prezimeime1 = reader2["PrezimeIme"];
                                    rv = (int)rdr["rv"];
                                }
                                else
                                {
                                    dosao1 = "1900-01-01 00:00.0";
                                    otisao1 = "1900-01-01 00:00.0";
                                    dosao11 = dosao1;
                                    otisao11 = otisao1;
                                }
                            }

                            cnn1.Close();
                        }


                        long minutes = 0;
                        long uminutes = 0;
                        if (dosao1 != "")
                        {
                            DateTime dos1 = DateTime.ParseExact(dosao11, "yyyy-MM-dd HH:mm:ss", null);
                            DateTime oti1 = DateTime.ParseExact(otisao11, "yyyy-MM-dd HH:mm:ss", null);
                            tdoci = dos1.ToString("dd.MM.yyyy") + " " + TrebaDoci(rv, "1") + ":00";
                            DateTime tdos1 = DateTime.ParseExact(tdoci, "dd.MM.yyyy HH:mm:ss", null);

                            TimeSpan t = dos1 - tdos1;
                            TimeSpan ut = oti1 - dos1;
                            minutes = (long)t.TotalMinutes;
                            uminutes = (long)ut.TotalMinutes;

                            if (minutes > 0)
                            {

                            }

                        }
                        else
                        {

                            if (dosao1 == otisao1)
                            {

                            }
                        }


                        sql3 = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',3,'" + hala1 + "','" + RadnoMjesto + "','" + napomena3 + "','" + dat1 + "'";
                        sql3 = sql3 + ",'" + dosao1 + "','" + otisao1 + "'," + minutes.ToString() + "," + uminutes.ToString() + ",'" + rv3 + "'";
                        //sql3 = "update pregledvremena2 set rbroj='" + rbroj1.ToString() + "',idradnika=" + res + ",prezimeime='" + prezime + "',datum='" + datum11 + "',smjena='3',hala='" + hala1 + "',radnomjesto='" + RadnoMjesto + "',datumunosa='" + dat1 + "'";
                    }
                    else
                    {
                        sql3 = "";
                    }
                }


                // bolovanje
                if (DBNull.Value.Equals(dg_PlanRadnogVremena[15, i].Value))
                {
                    sqlb = "";
                }
                else
                {

                    rbroj1 = (int)dg_PlanRadnogVremena[0, i].Value;
                    data = (string)dg_PlanRadnogVremena[15, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "4";
                        prezime = data;
                        RadnoMjesto = "Bolovanje";
                        sqlb = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',4,'" + hala1 + "','" + RadnoMjesto + "','" + napomenab + "','" + dat1 + "'";
                        //sqlb = "update pregledvremena2 set rbroj='" + rbroj1.ToString() + "',idradnika=" + res + ",prezimeime='" + prezime + "',datum='" + datum11 + "',smjena='4',hala='" + hala1 + "',radnomjesto='" + RadnoMjesto + "',datumunosa='" + dat1 + "'";
                    }
                    else
                    {
                        sqlb = "";
                    }
                }


                // godišnji
                if (DBNull.Value.Equals(dg_PlanRadnogVremena[16, i].Value))
                {
                    sqlg = "";
                }
                else
                {

                    rbroj1 = (int)dg_PlanRadnogVremena[0, i].Value;
                    data = (string)dg_PlanRadnogVremena[16, i].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        res = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();


                        // napomena1 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena2 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        //    napomena3 = (string)dg_Unos_PlanaRadnika[j, i].Value;
                        smjena1 = "5";
                        prezime = data;
                        RadnoMjesto = "Godišnji";
                        sqlg = rbroj1.ToString() + "," + res + ",'" + prezime + "','" + datum11 + "',5,'" + hala1 + "','" + RadnoMjesto + "','" + napomenag + "','" + dat1 + "'";
                        //sqlg = "update pregledvremena2 set rbroj='" + rbroj1.ToString() + "',idradnika=" + res + ",prezimeime='" + prezime + "',datum='" + datum11 + "',smjena='5',hala='" + hala1 + "',radnomjesto='" + RadnoMjesto + "',datumunosa='" + dat1 + "'";
                    }
                    else
                    {
                        sqlg = "";
                    }
                }

                if (res == "")
                    continue;

                string rbroj = (i + 1).ToString();
                rbroj = rbroj1.ToString();

                using (SqlConnection cn = new SqlConnection(connectionString))
                {

                    cn.Open();

                    sqlCommand = new SqlCommand("select * from pregledvremena2 where datum='" + datum11 + "' and rbroj= " + rbroj, cn);
                    reader2 = sqlCommand.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        reader2.Read();

                        // if (DBNull.Value.Equals(reader2["Napomena"] ))
                        //{ napomena1 = ""; }
                        //else
                        // {
                        //            napomena1 = reader2["napomena"].ToString();
                        //}

                        cn.Close();
                        cn.Open();
                        sqlCommand = new SqlCommand("delete from  pregledvremena2 where  datum='" + datum11 + "' and rbroj=" + rbroj, cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();

                    }
                    else
                    {
                        cn.Close();
                    }

                    if (sql1.Length > 0)
                    {

                        switch (rv)
                        {
                            case 1:
                                tdoci = "6:00";
                                totici = "14:00";
                                break;

                            case 2:
                                tdoci = "7:00";
                                totici = "15:00";
                                break;

                            case 3:
                                tdoci = "8:00";
                                totici = "16:00";
                                break;

                            case 4:
                                tdoci = "12:00";
                                totici = "20:00";
                                break;

                            case 5:
                                tdoci = "6:30";
                                totici = "14:30";
                                break;

                            case 6:
                                tdoci = "7:00";
                                totici = "15:0";
                                break;

                            case 7:
                                tdoci = "10:00";
                                totici = "18:00";
                                break;

                            default:
                                tdoci = "0:00";
                                totici = "0:00";
                                break;
                        }

                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa,dosao,otisao,kasni,ukupno_minuta,rv1) values (" + sql1 + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }


                    if (sql2.Length > 0)
                    {

                        switch (rv)
                        {
                            case 1:
                                tdoci = "14:00";
                                totici = "22:00";
                                break;

                            case 5:
                                tdoci = "14:30";
                                totici = "21:30";
                                break;

                            default:
                                tdoci = "00:00";
                                totici = "00:00";
                                break;
                        }


                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa,dosao,otisao,kasni,ukupno_minuta,rv2) values (" + sql2 + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }
                    if (sql3.Length > 0)
                    {

                        switch (rv)
                        {
                            case 1:
                                tdoci = "22:00";
                                totici = "06:00";
                                break;

                            default:

                                tdoci = "0:00";
                                totici = "0:00";
                                break;
                        }


                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa,dosao,otisao,kasni,ukupno_minuta,rv3) values (" + sql3 + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }

                    if (sqlb.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sqlb + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }

                    if (sqlg.Length > 0)
                    {
                        cn.Open();
                        sqlCommand = new SqlCommand("insert into pregledvremena2 ( rbroj,idradnika,prezimeime,datum,smjena,hala,radnomjesto,napomena,datumunosa) values (" + sqlg + ")", cn);
                        reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                    }

                }
            }
            MessageBox.Show("Podaci su sačuvani !");

        }


        private void dg_PlanRadnogVremena_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dg_PlanRadnogVremena.PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a copy then add the row to the other control.
            if (e.Effect == DragDropEffects.Copy)
            {
                string cellvalue = e.Data.GetData(typeof(string)) as string;
                var hittest = dg_PlanRadnogVremena.HitTest(clientPoint.X, clientPoint.Y);

                if (hittest.ColumnIndex != -1 && hittest.RowIndex != -1)
                    dg_PlanRadnogVremena[hittest.ColumnIndex, hittest.RowIndex].Value = cellvalue;

                dg_ListaRadnikaP[1, dg_ListaRadnikaP.CurrentCell.RowIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                dg_PlanRadnogVremena[hittest.ColumnIndex, hittest.RowIndex].Style.BackColor = System.Drawing.Color.LightGreen;

            }
        }

        private void btn_PRV_spremi2_Click(object sender, EventArgs e)
        {
            int i = 1;
            pl_spremi2.Visible = true;
            //btn_SpremiPlan2();

        }

        private void btn_PRV_spremi22_Click(object sender, EventArgs e)
        {
            int i = 1;
            pl_spremi2.Visible = false;
            btn_SpremiPlan2();

        }

        private void btn_spremi22_odustani_Click(object sender, EventArgs e)
        {
            pl_spremi2.Visible = true;
        }

        private void bt_a_Click(object sender, EventArgs e)
        {

        }

        private void btn_searchP_Click(object sender, EventArgs e)
        {
            string searchValue = tbx_search.Text.ToUpper();
            dg_ListaRadnikaP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool valueResult = false;
                foreach (DataGridViewRow row in dg_ListaRadnikaP.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().Substring(0, searchValue.Length) == searchValue)
                    {
                        int rowIndex = row.Index;
                        dg_ListaRadnikaP.Rows[rowIndex].Selected = true;
                        dg_ListaRadnikaP.FirstDisplayedScrollingRowIndex = rowIndex;
                        valueResult = true;
                        break;
                    }
                    else
                    {
                        dg_ListaRadnikaP.Rows[row.Index].Selected = false;
                    }


                }
                if (!valueResult)
                {
                    MessageBox.Show("Nije pronađen " + tbx_search.Text + " !");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void tbx_search_Enter(object sender, EventArgs e)
        {
            string searchValue = tbx_search.Text.ToUpper();
            dg_ListaRadnikaP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool valueResult = false;
                foreach (DataGridViewRow row in dg_ListaRadnikaP.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().Substring(0, searchValue.Length) == searchValue)
                    {
                        int rowIndex = row.Index;
                        dg_ListaRadnikaP.Rows[rowIndex].Selected = true;
                        dg_ListaRadnikaP.FirstDisplayedScrollingRowIndex = rowIndex;
                        valueResult = true;
                        break;
                    }
                    else
                    {
                        dg_ListaRadnikaP.Rows[row.Index].Selected = false;
                    }


                }
                if (!valueResult)
                {
                    MessageBox.Show("Nije pronađen " + tbx_search.Text, " !");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void tbx_search_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbx_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_searchP_Click(this, new EventArgs());
            }

        }

        private void btn_shiftP_Click(object sender, EventArgs e)
        {

            for (int ii = 0; ii < dg_PlanRadnogVremena.Rows.Count - 1; ii++)
            {

                for (int jj = 3; jj < dg_PlanRadnogVremena.ColumnCount - 1; jj++)
                {

                    if (jj == 3)
                    {

                        string s1 = dg_PlanRadnogVremena[3, ii].Value.ToString();
                        string s2 = dg_PlanRadnogVremena[7, ii].Value.ToString();

                        dg_PlanRadnogVremena[3, ii].Value = dg_PlanRadnogVremena[11, ii].Value;
                        dg_PlanRadnogVremena[7, ii].Value = s1;
                        dg_PlanRadnogVremena[11, ii].Value = s2;

                        dg_PlanRadnogVremena[4, ii].Value = DBNull.Value;
                        dg_PlanRadnogVremena[5, ii].Value = DBNull.Value;

                        dg_PlanRadnogVremena[8, ii].Value = DBNull.Value;
                        dg_PlanRadnogVremena[9, ii].Value = DBNull.Value;

                        dg_PlanRadnogVremena[12, ii].Value = DBNull.Value;
                        dg_PlanRadnogVremena[13, ii].Value = DBNull.Value;


                        dg_PlanRadnogVremena[4, ii].Style.BackColor = System.Drawing.Color.White;
                        dg_PlanRadnogVremena[5, ii].Style.BackColor = System.Drawing.Color.White;

                        dg_PlanRadnogVremena[8, ii].Style.BackColor = System.Drawing.Color.White;
                        dg_PlanRadnogVremena[9, ii].Style.BackColor = System.Drawing.Color.White;

                        dg_PlanRadnogVremena[12, ii].Style.BackColor = System.Drawing.Color.White;
                        dg_PlanRadnogVremena[13, ii].Style.BackColor = System.Drawing.Color.White;
                    }



                }
            }
        }

        private void dg_PlanRadnogVremena_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

            // Get the index of the item the mouse is below.
            var hittestInfo = dg_PlanRadnogVremena.HitTest(e.X, e.Y);
            int curcol = dg_PlanRadnogVremena.CurrentCell.ColumnIndex;
            //public int red1;

            if (hittestInfo.RowIndex != -1 && hittestInfo.ColumnIndex != -1)
            {
                valueFromMouseDown = dg_PlanRadnogVremena.Rows[hittestInfo.RowIndex].Cells[hittestInfo.ColumnIndex].Value;
                if (valueFromMouseDown != null)
                {
                    // Remember the point where the mouse down occurred. 
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);

                }
                else
                {


                }
            }
            else

            if ((!panel_rv.Visible) && (curcol == 39 || curcol == 79 || curcol == 119))
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
                panel_rv.Visible = true;

                string Query = "select naziv,id from rasporedvremena";
                using (var cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    DataTable dt2 = new DataTable();


                    SqlCommand cmd = new SqlCommand(Query, cn);
                    SqlDataReader myReader = cmd.ExecuteReader();
                    dt2.Load(myReader);
                    //dt2.Rows.InsertAt(dr, 0);
                    DataRow dr = dt2.NewRow();

                    dr[0] = "Select";
                    dr[1] = -1;

                    dt2.Rows.InsertAt(dr, 0);
                    comboBox13.DataSource = dt2;

                    comboBox13.ValueMember = "id";
                    comboBox13.DisplayMember = "Naziv";
                    comboBox13.SelectedIndex = -1;

                }

                panel_rv.Location = new Point(
                this.ClientSize.Width / 2 - panel_rv.Size.Width / 2,
                this.ClientSize.Height / 2 - panel_rv.Size.Height / 2);
                panel_rv.Anchor = AnchorStyles.None;
            }
        }


        private void dg_PlanRadnogVremena_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dg_PlanRadnogVremena.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }

        private void dg_PlanRadnogVremena_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == (Keys.Delete) && (dg_PlanRadnogVremena.CurrentCell.ColumnIndex == 3 || dg_PlanRadnogVremena.CurrentCell.ColumnIndex == 7 || dg_PlanRadnogVremena.CurrentCell.ColumnIndex == 11))
            {

                dg_PlanRadnogVremena.CurrentCell.Value = "";
                int iColumn = dg_PlanRadnogVremena.CurrentCell.ColumnIndex;
                int iRow = dg_PlanRadnogVremena.CurrentCell.RowIndex;
                dg_PlanRadnogVremena[iColumn + 1, iRow].Value = DBNull.Value;
                dg_PlanRadnogVremena[iColumn + 2, iRow].Value = DBNull.Value;

                //save data
            }

        }

        private string TrebaDoci(int rv, string smjena)
        {

            string tdoci = "6:00";

            if (smjena == "1")
            {
                switch (rv)
                {
                    case 1:
                        tdoci = "06:00";
                        break;

                    case 2:
                        tdoci = "07:00";
                        break;

                    case 3:
                        tdoci = "08:00";
                        break;

                    case 4:
                        tdoci = "12:00";
                        break;

                    case 5:
                        tdoci = "06:30";
                        break;

                    case 6:
                        tdoci = "07:00";
                        break;

                    case 7:
                        tdoci = "10:00";
                        break;

                    default:
                        tdoci = "00:00";
                        break;
                }




            }
            else if (smjena == "2")
            {

                switch (rv)
                {
                    case 1:
                        tdoci = "14:00";
                        break;

                    case 2:
                        tdoci = "15:00";
                        break;

                    case 3:
                        tdoci = "16:00";
                        break;

                    case 4:
                        tdoci = "20:00";
                        break;

                    case 5:
                        tdoci = "06:30";
                        break;

                    case 6:
                        tdoci = "14:00";
                        break;

                    default:
                        tdoci = "00:00";
                        break;
                }

            }
            else if (smjena == "3")
            {

                switch (rv)
                {
                    case 1:
                        tdoci = "22:00";
                        break;


                    default:
                        tdoci = "22:00";
                        break;
                }

            }

            return tdoci;
        }


        private string TrebaOtici(int rv, string smjena)
        {

            string totici = "6:00";

            if (smjena == "1")
            {
                switch (rv)
                {
                    case 1:
                        totici = "6:00";
                        break;

                    case 2:
                        totici = "7:00";
                        break;

                    case 3:
                        totici = "8:00";
                        break;

                    case 4:
                        totici = "12:00";
                        break;

                    case 5:
                        totici = "6:30";
                        break;

                    case 6:
                        totici = "7:00";
                        break;

                    default:
                        totici = "0:00";
                        break;
                }




            }
            else if (smjena == "2")
            {

                switch (rv)
                {
                    case 1:
                        totici = "14:00";
                        break;

                    case 2:
                        totici = "15:00";
                        break;

                    case 3:
                        totici = "16:00";
                        break;

                    case 4:
                        totici = "20:00";
                        break;

                    case 5:
                        totici = "6:30";
                        break;

                    case 6:
                        totici = "14:00";
                        break;

                    default:
                        totici = "0:00";
                        break;
                }

            }
            else if (smjena == "3")
            {

                switch (rv)
                {
                    case 1:
                        totici = "22:00";
                        break;

                    default:
                        totici = "22:00";
                        break;
                }

            }

            return totici;

        }


        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

            string id1 = comboBox10.SelectedValue.ToString();
            if (id1 == "" || id1 == null)
                return;

            string rv1 = "0";

            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                cn.Open();

                SqlCommand sqlCommand = new SqlCommand("select rv.naziv as naziv1 from radnici_ r left join rasporedvremena rv on rv.id=r.rv where r.neradi=0 and r.id='" + id1 + "'", cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                if (reader2.HasRows)
                {
                    reader2.Read();
                    rv1 = reader2["naziv1"].ToString();
                    cn.Close();
                }
            }


            lbl_current_rv.Text = "Trenutno radno vrijeme " + rv1;

        }

        private void duplikatKarticeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            pocetniEkran();
            panelNovakartica.Visible = true;

            var dataSource = new List<radnici>();
            foreach (var radnikk in radnicii)
            {
                dataSource.Add(new radnici() { prezime = radnikk.prezime + " " + radnikk.ime, id = radnikk.id });
            }

            CB_ListaRadnika.DataSource = dataSource;
            CB_ListaRadnika.DisplayMember = "prezime";
            CB_ListaRadnika.ValueMember = "id";
            CB_ListaRadnika.DropDownHeight = CB_ListaRadnika.Font.Height * 50;

        }

        private void button16_Click(object sender, EventArgs e)  // strelica desno , jedan dan više
        {
            pl_spremi2.Visible = true;
            date_DatumPR.Value = date_DatumPR.Value.AddDays(1);
            Ucitaj_iz_baze();
        }

        private void btn_nazad_Click(object sender, EventArgs e)    // strelica lijevo , jedan dan manje
        {
            date_DatumPR.Value = date_DatumPR.Value.AddDays(-1);
            Ucitaj_iz_baze();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            using (SqlConnection cn = new SqlConnection(connectionString))
            {

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into erv_log (korisnik,idprijave,opis,datum) values('"+korisnik+"','"+idprijave+"','Odjava',getdate())",cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                cn.Close();
             
            }
            this.Close();
        }

        private void dg_PlanRadnogVremena_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //pl_spremi2.Visible = true;

            //string id1 = comboBox13.SelectedValue.ToString();
            //if (id1 == "" || id1 == null)
            //    return;

            //string rv1 = "0" ;
            //var hittestInfo = dg_PlanRadnogVremena.HitTest(e.X, e.Y);
            int curcol = dg_PlanRadnogVremena.CurrentCell.ColumnIndex;
            int currow1 = dg_PlanRadnogVremena.CurrentCell.RowIndex;
            string rv11 = "", rv22 = "", rv33 = "";
            string id11 = "", id22 = "", id33 = "";
            string id1 = "";
            string rv10 = "";

            switch (curcol)
            {
                case 3:

                    rv10 = dg_PlanRadnogVremena[17, currow1].Value.ToString();

                    if (rv10 != "")
                    {

                    }

                    id11 = dg_PlanRadnogVremena[3, currow1].Value.ToString();
                    var data = (string)dg_PlanRadnogVremena[3, currow1].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        id1 = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();

                        string prezime = data;
                        rv11 = dg_PlanRadnogVremena[17, currow1].Value.ToString();

                    }
                    break;

                case 7:
                    rv22 = dg_PlanRadnogVremena[18, currow1].Value.ToString();
                    break;

                case 11:
                    rv33 = dg_PlanRadnogVremena[19, currow1].Value.ToString();
                    break;

                default:
                    break;

            }

            if (id1 == "")
                return;

            string Query;

            if (rv10 == "")
                Query = "select r.*,rv.naziv from radnici_ r left join  rasporedvremena rv on r.rv = rv.id where r.id = " + id1;
            else
                Query = "select rv.naziv from rasporedvremena rv where rv.id = " + rv10;

            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();
                DataTable dt2 = new DataTable();

                SqlCommand cmd = new SqlCommand(Query, cn);
                SqlDataReader myReader = cmd.ExecuteReader();
                myReader.Read();
                string rv = myReader["naziv"].ToString();
                lbl_current_rv_2.Text = "Trenutno radno vrijeme:" + rv;

                if (rv10 == "")
                    lbl_poduzece.Text = "Poduzeće : " + myReader["poduzece"].ToString();
                else
                    lbl_poduzece.Text = "";
            }


            Query = "select naziv,id from rasporedvremena";
            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();
                DataTable dt2 = new DataTable();

                SqlCommand cmd = new SqlCommand(Query, cn);
                SqlDataReader myReader = cmd.ExecuteReader();
                dt2.Load(myReader);

                comboBox13.DataSource = dt2;
                comboBox13.ValueMember = "id";
                comboBox13.DisplayMember = "Naziv";
            }

            //lbl_current_rv.Text = "Trenutno radno vrijeme " + rv1 ;

            //using (SqlConnection cn = new SqlConnection(connectionString))
            //{

            //    cn.Open();

            //    SqlCommand sqlCommand = new SqlCommand("select rv.naziv as naziv1 from radnici_ r left join rasporedvremena rv on rv.id=r.rv where r.neradi=0 and r.id='" + id1 + "'", cn);
            //    SqlDataReader reader2 = sqlCommand.ExecuteReader();
            //    if (reader2.HasRows)
            //    {
            //        reader2.Read();
            //        rv1 = reader2["naziv1"].ToString();
            //        cn.Close();
            //    }
            //}




        }

        private void btn_zatvori_rv_Click(object sender, EventArgs e)
        {
            panel_rv.Visible = false;
        }

        private void radnoVrijemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
          

        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!(comboBox13.SelectedValue == null))
            {
                string test = comboBox13.SelectedValue.ToString();

                if (!(test.Contains("System")))
                {
                    if (test != "-1")
                    {

                        int iColumn = dg_PlanRadnogVremena.CurrentCell.ColumnIndex;
                        int iRow = dg_PlanRadnogVremena.CurrentCell.RowIndex;

                        if (iColumn == 3)

                            dg_PlanRadnogVremena[17, iRow].Value = (int.Parse)(comboBox13.SelectedValue.ToString());

                        if (iColumn == 7)
                            dg_PlanRadnogVremena[18, iRow].Value = (int.Parse)(comboBox13.SelectedValue.ToString());

                        if (iColumn == 11)
                            dg_PlanRadnogVremena[19, iRow].Value = (int.Parse)(comboBox13.SelectedValue.ToString());
                    }

                }
            }
        }

        private void pl_spremi2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pl_PregledRadnogVremena_Paint(object sender, PaintEventArgs e)
        {

        }

        // Neeee
        private void dg_PlanRadnogVremena_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //var hittestInfo = dg_PlanRadnogVremena.HitTest(e.X, e.Y);
            int curcol = dg_PlanRadnogVremena.CurrentCell.ColumnIndex;
            int currow1 = dg_PlanRadnogVremena.CurrentCell.ColumnIndex;
            string rv11 = "", rv22 = "", rv33 = "";
            string id11 = "", id22 = "", id33 = "";
            string id1 = "";

            switch (curcol)
            {
                case 3:
                    id11 = dg_PlanRadnogVremena[3, currow1].Value.ToString();
                    var data = (string)dg_PlanRadnogVremena[3, currow1].Value;

                    if ((data.Trim().Length > 0) && (data.IndexOf("(") >= 0))
                    {

                        id1 = new string(data.SkipWhile(c => c != '(')
                           .Skip(1)
                           .TakeWhile(c => c != ')')
                           .ToArray()).Trim();

                        string prezime = data;
                        rv11 = dg_PlanRadnogVremena[17, currow1].Value.ToString();

                    }
                    break;

                case 7:
                    rv22 = dg_PlanRadnogVremena[18, currow1].Value.ToString();
                    break;

                case 11:
                    rv33 = dg_PlanRadnogVremena[19, currow1].Value.ToString();
                    break;

                default:
                    break;

            }

            if (id1 == "")
                return;



            string Query = "select r.*,rv.naziv from radnici_ r left join  rasporedvremena rv oon r.rv=rv.id where r.id=" + id1;
            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();
                DataTable dt2 = new DataTable();


                SqlCommand cmd = new SqlCommand(Query, cn);
                SqlDataReader myReader = cmd.ExecuteReader();
                string rv = myReader["naziv"].ToString();
                lbl_current_rv.Text = "trenutno radno vrijeme:" + rv;


            }

            if ((!panel_rv.Visible) && (curcol == 3 || curcol == 7 || curcol == 11))
            {
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
                panel_rv.Visible = true;

                Query = "select naziv,id from rasporedvremena";
                using (var cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    DataTable dt2 = new DataTable();


                    SqlCommand cmd = new SqlCommand(Query, cn);
                    SqlDataReader myReader = cmd.ExecuteReader();
                    dt2.Load(myReader);
                    //dt2.Rows.InsertAt(dr, 0);
                    DataRow dr = dt2.NewRow();

                    dr[0] = "Select";
                    dr[1] = -1;

                    dt2.Rows.InsertAt(dr, 0);
                    comboBox13.DataSource = dt2;

                    comboBox13.ValueMember = "id";
                    comboBox13.DisplayMember = "Naziv";
                    comboBox13.SelectedIndex = -1;

                }

                panel_rv.Location = new Point(
                this.ClientSize.Width / 2 - panel_rv.Size.Width / 2,
                this.ClientSize.Height / 2 - panel_rv.Size.Height / 2);
                panel_rv.Anchor = AnchorStyles.None;
            }
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }
        // import novih radnika iz pantheona, FX ili TB
        private void ImportNovihRadnikaIzPantheon(int idp)
        {
            Encoding ae = Encoding.GetEncoding("utf-8");
            var reader = new StreamReader(File.OpenRead(@"C:\brisi\radnici\radniciat0.csv"), Encoding.GetEncoding(1250));
            string sql1;
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            List<int> ListaTKBrad = new List<int> { 162, 165, 169, 172, 181, 197, 211 };

            string rfid = "", rfidhex = "", custid = "", rfid2 = "", sifrarm = "";
            long rfidd = 0;
            int id1 = 0, idd1 = 0, idzadnji = 0;
            //string connectionString  = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";
            string connectionStringf = @"Data Source=192.168.0.3;Initial Catalog=FeroApp;User ID=sa;Password=AdminFX9.";
            string connectionStringp = "";
            if (idp == 1 || idp==3)
            {
                connectionStringp = @"Data Source=192.168.0.6;Initial Catalog=PantheonFxAT;User ID=sa;Password=AdminFX9.";
            }
            else
            {
                connectionStringp = @"Data Source=192.168.0.6;Initial Catalog=PantheonTKB;User ID=sa;Password=AdminFX9.";
            }
            
//            SqlConnection connection = new SqlConnection(connectionString);
//            connection.Open();
            int header = 1;
            int ba = 0;
            var line = "";
            //idzadnji = 154;

            string id = "", ime = "", prezime = "", lokacija = "", mt = "", radnomjesto = "", radido = "", vrstarada = "", neradi = "", datumzaposlenja = "", datumprestanka = "", ulica = "", grad = "", posta = "", vrstaisplate = "S";
            string poduzece1 = "Feroimpex";

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                //       nadji zadnjeg u radnici_
                SqlCommand sqlCommand=new SqlCommand("select top 1 id from radnici_ ", cn); ;
                if (idp==1)
                {
                    sqlCommand = new SqlCommand("select top 1 id from radnici_ where id < 8000 and poduzece='Feroimpex' order by id desc", cn);
                    poduzece1="Feroimpex";
                } 
                else if (idp == 2)
                {
                    sqlCommand = new SqlCommand("select top 1 id from radnici_ where id < 8000 and poduzece='Tokabu' order by id desc", cn);
                    poduzece1 = "Tokabu";
                }
                else if (idp == 3)
                {
                    sqlCommand = new SqlCommand("select top 1 id from radnici_ where id >900000 and poduzece='Feroimpex' order by id desc", cn);
                    poduzece1 = "Feroimpex";
                }

                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                reader2.Read();
                idzadnji = (int.Parse)(reader2["id"].ToString());
                // ako >90000
                //idzadnji = 155;
                // idzadnji = 0;
                cn.Close();

            }
            //idzadnji = 87;
            using (SqlConnection cnp = new SqlConnection(connectionStringp))
            {
                cnp.Open();
                // pogledaj u pantheon, listu zadnjih radnika

                SqlCommand sqlCommand = new SqlCommand("select cast(acregno as int) as id, acname, acsurname, j.acCostDrv, j.acDept, d.acnumber, j.acjob, '' radni_staz, j.adDate, '' sifra_rm, p.acstreet, acpost, '' vrijeme, accity, j.acFieldSA vrsta_isplate, adDateExit from thr_prsn p " +
                            "left join thr_prsnjob j on p.acworker = j.acworker left join thr_prsnadddoc d on d.acWorker = p.acworker and d.actype = 8 where acregno!='999999' and cast(acregno as int)<8000 order by cast(acregno as int) desc", cnp);

                if (idp == 1 || idp == 2)
                {  // fx ili tkb , id<8000
                    sqlCommand = new SqlCommand("select cast(acregno as int) as id, acname, acsurname, j.acCostDrv, j.acDept, d.acnumber, j.acjob, '' radni_staz, j.adDate, '' sifra_rm, p.acstreet, acpost, '' vrijeme, accity, j.acFieldSA vrsta_isplate, adDateExit from thr_prsn p " +
                                "left join thr_prsnjob j on p.acworker = j.acworker left join thr_prsnadddoc d on d.acWorker = p.acworker and d.actype = 8 where acregno!='999999' and cast(acregno as int)<8000 order by cast(acregno as int) desc", cnp);
                }
                else if (idp==3)
                { //fx, id>900000
                    sqlCommand = new SqlCommand("select cast(acregno as int) as id, acname, acsurname, j.acCostDrv, j.acDept, d.acnumber, j.acjob, '' radni_staz, j.adDate, '' sifra_rm, p.acstreet, acpost, '' vrijeme, accity, j.acFieldSA vrsta_isplate, adDateExit from thr_prsn p " +
                    "left join thr_prsnjob j on p.acworker = j.acworker left join thr_prsnadddoc d on d.acWorker = p.acworker and d.actype = 8 where acregno!='999999' and cast(acregno as int)>=900000 order by cast(acregno as int) desc", cnp);
                }

                SqlDataReader reader2 = sqlCommand.ExecuteReader();
              
                int jos= 1,posebni=0;
                while (reader2.Read() && jos==1)
                {
                    id = (reader2["id"].ToString());
                    if (id=="900070" )
                    {
                        continue;
                    }
                    if ((int.Parse(id))>9000)
                    {
                        posebni =1;
                    }
        //            posebni = 0;

                    ime = (reader2["acname"].ToString()).Trim();
                    prezime = (reader2["acsurname"].ToString()).Trim();                    
                    mt= (reader2["accostdrv"].ToString()).TrimEnd();
                    lokacija = (reader2["acdept"].ToString()).TrimEnd();
                    radnomjesto = (reader2["acjob"].ToString()).TrimEnd();
                    radido = (reader2["addateexit"].ToString());
                    vrstarada = (reader2["vrsta_isplate"].ToString());
                    //prezime = (reader2["acsurname"].ToString());
                    if (reader2["acnumber"] is DBNull)
                    {
                        MessageBox.Show("Nije upisan RFID za " + id + " !");
                        continue;
                    }

                    rfidd = (long.Parse)(reader2["acnumber"].ToString());
                    string[] datumzaposlenjaa = {""};
                    if (posebni == 0)
                    {
                        datumzaposlenjaa = (reader2["addate"].ToString()).Split('.');
                        datumzaposlenja = datumzaposlenjaa[2] + "-" + datumzaposlenjaa[1] + "-" + datumzaposlenjaa[0];
                        datumprestanka = radido;
                        ulica = (reader2["acstreet"].ToString()).TrimEnd();
                        grad = (reader2["accity"].ToString()).TrimEnd();
                        posta = (reader2["acpost"].ToString());  // posta HR-10290
                        int p1 = posta.IndexOf("#");
                        if (p1 > 0)
                        {
                            posta = posta.Substring(0, p1);
                        }

                        posta = posta.Substring(3, posta.Length - 3);

                        vrstaisplate = vrstarada == "S" ? "0" : "1";
                        if (vrstarada == "F")
                        {
                            sifrarm = "Režija";
                            vrstaisplate = "1";
                        }
                        else
                        {
                            vrstaisplate = "0";
                            sifrarm = "proizvodnja";
                        }
                    }
                    else
                    {
                        datumzaposlenja = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        ulica   = "" ;
                        grad    = "" ;
                        posta   = "" ;
                        sifrarm = "Režija" ;
                        vrstaisplate = "1";
                    }

                    rfidhex = rfidd.ToString("X");
                    int l1 = rfidhex.Length-8;
                    rfid2 = (int.Parse(rfidhex.Substring(0, l1), System.Globalization.NumberStyles.HexNumber)).ToString();
                    custid = rfid2;   
                    string rfid1 = rfidhex.Substring(rfidhex.Length - 8);
                    custid = rfid2;
                    rfid2 = custid + "-" + (long.Parse(rfid1, System.Globalization.NumberStyles.HexNumber)).ToString();
                    //rfid2 = custid + "-" + (long.Parse(rfidhex.Substring(2, rfidhex.Length - 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                    //sifrarm = "";
                    neradi = "0";
                    if (posta.Length == 0)
                    {
                        posta = "''";
                    }                    

                    id = id.Replace(",", "");
                    id = id.Replace("\"", "");
                    prezime = prezime.Replace("\"", "");
                    idd1 = (int.Parse)(id);   // novi id iz filea                                        
                    int idd10 = idd1;
                    rfidhex = "000000" + rfidd.ToString("X");
                    //if (idd1==104)
                    //{
                    //    continue;
                    //}
                    //// provjera dali
                    //if (idd1>900020 || idd1<900013)
                    //{
                    //    continue;
                    // }
                 //   id = "8153";
                 // idd1 = 8153;
                 if (ListaTKBrad.Contains(idd1))  // ako takav broj već postoji u Feroimpexu
                 {
                        idd1 = idd1 + 8000;
                        id = idd1.ToString();
                 }

                    
                    if (idd1 == 900074){
                        idzadnji = 900071;
                        continue;
                    }
                        
                    if ( idd1 > idzadnji && neradi == "0" )


//                   if ((idd1 > idzadnji && ( idd1 < 8000 || idd1>=10000)) && neradi == "0") //|| ( idd1==1329) )// ako je novi
//if (idd1 ==74 )     //|| ( idd1==1329) )// ako je novi u tokabu  slobodno 72-78
//if (idd1 > idzadnji && idd1>900000 && neradi == "0") //|| ( idd1==1329) )// ako je novi

                    //                        if (idd1==900012)                                                             //                if (idd1 ==70  ) //|| ( idd1==1329) )// ako je novi
                    {
                        //jos = 0;
                            SqlDataReader reader1 = null;
                        SqlDataReader reader11 = null;
                        //if (idd1 == 91 || idd1 == 8999)
                        //{ }
                        //else
                        //{ 
                        //    continue;
                        //}

                        using (SqlConnection cn = new SqlConnection(connectionString))
                        {
                            cn.Close();
                            cn.Open();

                            //sql1 = "update radnici_  set ulica=" + ulica + ",grad=" + grad + ",posta=" + posta + ",datumzaposlenja='" + datumzaposlenja + "',datumprestanka='" + datumprestanka + "',rfid='" + rfidd.ToString() + "',rfidhex='" + rfidhex + "',rfid2='" + rfid2 + "' where id=" + id + " and poduzece='Feroimpex'";
                            sql1 = id + ",'" + ime.Trim() + "','" + prezime.Trim() + "','" + ulica + "','" + grad + "','" + posta + "','" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "','" + radnomjesto + "','" + rfid2 + "','"+poduzece1+"','" + sifrarm + "',1," + neradi + ",'" + vrstaisplate.ToString() + "','" + datumzaposlenja + "'";
                            sqlCommand = new SqlCommand("insert into radnici_ (id,ime,prezime, ulica , grad , posta , custid,rfid,rfidhex,lokacija,mt,radnomjesto,rfid2,poduzece,sifrarm,rv,neradi,fixnaisplata,datumzaposlenja) values(" + sql1 + ")", cn);
                            //SqlCommand sqlCommand = new SqlCommand( sql1 , cn);

                            //lCommand sqlCommand = new SqlCommand("update radnici_ set sifram = " + sifrarm + " where id = " + id ,  cn ) ;
                            SqlDataReader readerp = sqlCommand.ExecuteReader();
                            cn.Close();
                            cn.Open();
                           
                                prezime = prezime.Replace("'", "");
                                ime = ime.Replace("'", "").Trim();
                            // update rfind, kartice
                                
                                SqlCommand cmd = new SqlCommand("rfind.dbo.FX_Import", cn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@EXT_ID", SqlDbType.VarChar, 6).Value = id;  // lokacija
                                cmd.Parameters.Add("@FNAME", SqlDbType.VarChar, 35).Value = ime;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                                cmd.Parameters.Add("@LNAME", SqlDbType.VarChar, 35).Value = prezime;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                                cmd.Parameters.Add("@CSN", SqlDbType.VarChar, 12).Value = rfid2;  // csn1
                                cmd.Parameters.Add("@START_TIME", SqlDbType.DateTime).Value = DateTime.Now;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                                cmd.Parameters.Add("@END_TIME", SqlDbType.DateTime).Value = DateTime.Now.AddYears(10); ;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                                cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = 0;  //  status 0 - nova, 1 - update , 2 disable  3 delete ???
                                reader1 = cmd.ExecuteReader();                                                                                              
                            
                            cn.Close();                        
                        }

                        using (SqlConnection cna = new SqlConnection(connectionString))
                        {
                            cna.Open();
                            SqlCommand sqlCommanda = new SqlCommand("insert into erv_log (datum,korisnik,idprijave,opis) values  ( getdate(),'" + korisnik + "','" + idprijave + "','Unos novog radnika [User.id]= " + idd1.ToString() + "')", cna);
                            SqlDataReader readera = sqlCommanda.ExecuteReader();
                            cna.Close();
                        }


                        if (1 == 2)
                        {
                            // dodavanje u feroapp.dbo.radnici
                            using (SqlConnection cnr2 = new SqlConnection(connectionStringf))
                            {

                                cnr2.Open();
                                string radnastroju = "1";
                                if (sifrarm.Contains("Režija"))
                                {
                                    radnastroju = "0";
                                }

                                sql1 = "insert into feroapp.dbo.radnici(id_fink, id_firme, ime, sifrarm, hala, radnastroju, steler, kontrola, bravar, pilar, neradi) select id idd10,case when poduzece = 'Feroimpex' then 1 else 3 end poduzece, prezime+' ' + ime as ime,sifrarm,case when lokacija = 500 then 1 else 3 end hala," + radnastroju + " radnastroju,0 steler,0 kontrola,0 bravar,0 pilar,0 neradi " +
                                       "from rfind.dbo.radnici_ where id =" + idd1 + " and poduzece ='" + poduzece1 + "'";
                                sqlCommand = new SqlCommand(sql1, cnr2);
                                SqlDataReader readerp = sqlCommand.ExecuteReader();
                                cnr2.Close();
                            }
                        }

                    }
                    else
                    {
                        jos = 0;
                    }             
                  }

                cnp.Close();
            }
            //idzadnji = 74;

            ba = 0;
            // update kompetencije  
            if (1 == 1)
            {
                using (SqlConnection cnk = new SqlConnection(connectionString))
                {
                    cnk.Open();
                    SqlCommand cmd = new SqlCommand("dbo.sp_kompetencije_", cnk);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader readerp = cmd.ExecuteReader();
                    cnk.Close();
                }
            }
            MessageBox.Show("Gotov import !");

        }

        private void feroimpex900ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vobr = 1;
            if (vobr == 1)
            {
                ImportNovihRadnikaIzPantheon(3);
                return;

            }

        }

        private void feroimpexToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            pictureBox1.Visible = true;

            //string dir = Path.GetDirectoryName(Application.ExecutablePath);
            // Image image = Image.FromFile(@"logo-FX.png");
            //string filename = Path.Combine(dir, @"logo-FX.png");

            //Image image = Image.FromFile(filenamew);
            //// Set the PictureBox image property to this image.
            //// ... Then, adjust its height and width properties.
            //pictureBox1.Image = image;
            //pictureBox1.Height = image.Height;
            //pictureBox1.Width = image.Width;


            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            panelRucniUnos.Visible = false;
            GridPregledRadnika.Visible = true;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string sql;

                sql = "SELECT cast( a.[radnik id]  as int) as Id,a.[ime x] as Ime,a.[prezime x] as Prezime,a.oib as OIB,a.rfid as RFID ,a.rfid2 as 'Serijski broj' ,a.rfidhex as 'RFID Hex',l.naziv as 'Lokacija', mt.naziv as 'Mjesto troška' FROM radniciAT0 a left join lokacije l on l.id=a.lokacija left join mjestotroska mt on mt.id = a.mt order by prezime";
                sql = "SELECT cast( a.[id]  as int) as Id,a.[ime] as Ime,a.[prezime] as Prezime,a.datumzaposlenja 'Datum Zaposlenja',a.datumprestanka 'Datum prestanka',a.radnomjesto RadnoMjesto,a.rfid as RFID ,a.rfid2 as 'Serijski broj' ,a.rfidhex  as 'RFID Hex',l.naziv as 'Lokacija', mt.naziv as 'Mjesto troška',a.poduzece, rv.naziv as 'Radno vrijeme',a.fixnaisplata Fixna_isplata,a.Neradi FROM radnici_ a left join lokacije l on l.id=a.lokacija left join mjestotroska mt on mt.id = a.mt left join rasporedvremena rv on rv.id=a.rv where poduzece like 'Feroimpex%' order by prezime ";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                connection.Open();
                dataadapter.Fill(ds, "event");
                connection.Close();

                GridPregledRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                GridPregledRadnika.DataSource = ds;
                GridPregledRadnika.DataMember = "event";

                GridPregledRadnika.Width = ActiveForm.Width * 1 / 2 + 650;
                GridPregledRadnika.Top = ActiveForm.Height * 1 / 10;

                GridPregledRadnika.Height = ActiveForm.Height - 200;

                GridPregledRadnika.AutoResizeColumns();
                GridPregledRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        private void tokabuToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            pictureBox1.Visible = true;

            //string dir = Path.GetDirectoryName(Application.ExecutablePath);
            // Image image = Image.FromFile(@"logo-FX.png");
            //string filename = Path.Combine(dir, @"logo-FX.png");

            //Image image = Image.FromFile(filenamew);
            //// Set the PictureBox image property to this image.
            //// ... Then, adjust its height and width properties.
            //pictureBox1.Image = image;
            //pictureBox1.Height = image.Height;
            //pictureBox1.Width = image.Width;


            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            dataGridView3.Visible = false;
            panelRucniUnos.Visible = false;
            GridPregledRadnika.Visible = true;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string sql;

                sql = "SELECT cast( a.[radnik id]  as int) as Id,a.[ime x] as Ime,a.[prezime x] as Prezime,a.oib as OIB,a.rfid as RFID ,a.rfid2 as 'Serijski broj' ,a.rfidhex as 'RFID Hex',l.naziv as 'Lokacija', mt.naziv as 'Mjesto troška' FROM radniciAT0 a left join lokacije l on l.id=a.lokacija left join mjestotroska mt on mt.id = a.mt order by prezime";
                sql = "SELECT cast( a.[id]  as int) as Id,a.[ime] as Ime,a.[prezime] as Prezime,a.datumzaposlenja 'Datum Zaposlenja',a.datumprestanka 'Datum prestanka',a.radnomjesto RadnoMjesto,a.rfid as RFID ,a.rfid2 as 'Serijski broj' ,a.rfidhex  as 'RFID Hex',l.naziv as 'Lokacija', mt.naziv as 'Mjesto troška',a.poduzece, rv.naziv as 'Radno vrijeme',a.fixnaisplata Fixna_isplata,a.Neradi FROM radnici_ a left join lokacije l on l.id=a.lokacija left join mjestotroska mt on mt.id = a.mt left join rasporedvremena rv on rv.id=a.rv where poduzece like 'Tokabu%' order by prezime ";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                connection.Open();
                dataadapter.Fill(ds, "event");
                connection.Close();

                GridPregledRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                GridPregledRadnika.DataSource = ds;
                GridPregledRadnika.DataMember = "event";

                GridPregledRadnika.Width = ActiveForm.Width * 1 / 2 + 650;
                GridPregledRadnika.Top = ActiveForm.Height * 1 / 10;

                GridPregledRadnika.Height = ActiveForm.Height - 200;

                GridPregledRadnika.AutoResizeColumns();
                GridPregledRadnika.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        private void ComboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DeaktivacijaKarticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pocetniEkran();

            if ((idloged == "9") || (idloged == "8"))
            {                
                pnl_deaktiv.Visible = true;
            }
            else
            {
                pnl_deaktiv.Visible = false;
                return;
            }


            var dataSource = new List<radnici>();
            foreach (var radnikk in radnicii)
            {
                dataSource.Add(new radnici() { prezime = radnikk.prezime + " " + radnikk.ime + " ( " + radnikk.id.ToString() + " )", id = radnikk.id });
            }

            cbx_listaradnika_deaktiv.DataSource = dataSource;            
            cbx_listaradnika_deaktiv.DisplayMember = "prezime";
            cbx_listaradnika_deaktiv.ValueMember = "id";
            cbx_listaradnika_deaktiv.DropDownHeight = CB_ListaRadnika.Font.Height * 50;
        }

        private void Btn_zatvori_deakt_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            pnl_deaktiv.Visible = false;
            string csn1, id1, imedjelatnika1;
            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=sa;Password=AdminFX9.";
            SqlDataReader rdr = null;
            csn1 = "";
            id1 =  cbx_listaradnika_deaktiv.SelectedValue.ToString();
            imedjelatnika1 = cbx_listaradnika_deaktiv.Text.ToString();
            string[] lista1 = imedjelatnika1.Split(' ');
            string ime1 = lista1[1];
            string prezime1 = lista1[0];
            string rfid1 = "", rfidhex = "", custid1 = "";
            string poduzece = "";
            string connectionStringp = "";
            string ime0 = "", prezime0 = "", rfid0 = "", rfidhex0 = "", rfid20 = "", custid0 = "", rv0 = "";

            SqlConnection cn1 = new SqlConnection(connectionString);
            cn1.Open();
            SqlCommand sqlCommand1 = new SqlCommand("update rfind.dbo.badge set active=0 where extid=" + id1.Trim(), cn1);
            SqlDataReader reader21 = sqlCommand1.ExecuteReader();            
            cn1.Close();


            using (SqlConnection cn10 = new SqlConnection(connectionString))
            {
                cn10.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into erv_log (korisnik,idprijave,opis,datum) values('" + korisnik + "','" + idprijave + "','Deaktivacija kartice za " + id1.ToString() + "', getdate())", cn10);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                cn10.Close();
            }


        }

        private void Btn_aktiviraj_Click(object sender, EventArgs e)
        {
            pocetniEkran();
            pnl_deaktiv.Visible = false;
            string csn1, id1, imedjelatnika1;
            string connectionString = @"Data Source=192.168.0.3;Initial Catalog=fx_RFIND;User ID=sa;Password=AdminFX9.";
            SqlDataReader rdr = null;
            csn1 = "";
            id1 = cbx_listaradnika_deaktiv.SelectedValue.ToString();
            imedjelatnika1 = cbx_listaradnika_deaktiv.Text.ToString();
            string[] lista1 = imedjelatnika1.Split(' ');
            string ime1 = lista1[1];
            string prezime1 = lista1[0];
            string rfid1 = "", rfidhex = "", custid1 = "";
            string poduzece = "";
            string connectionStringp = "";
            string ime0 = "", prezime0 = "", rfid0 = "", rfidhex0 = "", rfid20 = "", custid0 = "", rv0 = "";

            SqlConnection cn1 = new SqlConnection(connectionString);
            cn1.Open();
            SqlCommand sqlCommand1 = new SqlCommand("update rfind.dbo.badge set active=1 where extid=" + id1.Trim(), cn1);
            SqlDataReader reader21 = sqlCommand1.ExecuteReader();
            cn1.Close();


            using (SqlConnection cn10 = new SqlConnection(connectionString))
            {
                cn10.Open();
                SqlCommand sqlCommand = new SqlCommand("insert into erv_log (korisnik,idprijave,opis,datum) values('" + korisnik + "','" + idprijave + "','Deaktivacija kartice za " + id1.ToString() + "', getdate())", cn10);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                cn10.Close();
            }
        }

        private void feroimpexToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int vobr = 1;
            if (vobr==1)
            {
                ImportNovihRadnikaIzPantheon(1);
                return;

            }

            Encoding ae = Encoding.GetEncoding("utf-8");
            var reader = new StreamReader(File.OpenRead(@"C:\brisi\radnici\radniciat0.csv"), Encoding.GetEncoding(1250));
            string sql1;
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            string rfid, rfidhex, custid, rfid2, sifrarm;
            long rfidd;
            int id1, idd1, idzadnji;
            //string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            int header = 1;
            int ba = 0;
            var line = "";
            idzadnji = 1381;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
//                SqlCommand sqlCommand = new SqlCommand("select top 1 extid from [user] where extid < 8000 order by oid desc", cn);
                SqlCommand sqlCommand = new SqlCommand("select top 1 id from radnici_ where id < 8000 and poduzece='Feroimpex' order by id desc", cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                reader2.Read();
                idzadnji = (int.Parse)(reader2["id"].ToString());
                // idzadnji = 0;
                cn.Close();

            }
            //idzadnji = 74;

            while (!reader.EndOfStream)
            {

                if (header == 1)
                {
                    header = 0;
                    line = reader.ReadLine();
                    continue;
                }

                line = reader.ReadLine();
                var values = line.Split(';');

                if ((values[7].ToString().Trim()) == "1" || (values[5].ToString().Length == 0))
                {   // ako ne radi ili nema karticu
                    header = 0;
                    //line = reader.ReadLine();
                    continue;
                }

                // id  0
                // ime 1
                // prezime 2
                // loakcija 3
                // mt 4
                // rfid 5
                // radn mjesto 6
                // ne radi 7
                string id = values[0].ToString().Trim().Replace(" ", "").Replace(",", "");
                string ime = values[1].ToString().Trim().Replace("\"", "'");
                string prezime = values[2].ToString().Trim().Replace("\"", "'");
                string lokacija = values[4].ToString().Trim().Replace("\"", "'");
                string mt = values[3].ToString().Trim().Replace("\"", "'");

                string radnomjesto = values[6].ToString().Trim().Replace("\"", "'");

                if (radnomjesto.Length < 2)
                    radnomjesto = "' '";
                //  0   1     2     3   4         5      6         7    8    9               10       11  12   13    14    15      16         17
                // id,imex,prezime,mt,lokacija, rfid,radnimjesto,neradi,, datumzaposlenja,sifrram, ulica,posta,     ,grad,  , datumprestanka, vrstrada,


                //string neradi = values[7].ToString().Trim().Replace("\"", "'");
                string radido = values[16].ToString().Trim().Replace("\"", "'");
                string vrstarada = values[17].ToString().Trim().Replace("\"", "'");
                int z1 = radido.Length;
                string neradi = "0";

                if (radido.Length != 8)   // ako j eupidan datum do kada radi
                    neradi = "1";

                int vrstaisplate = 0;   //  fixnaisplata

                if (vrstarada == "'Sati'")   //  po satu 
                    vrstaisplate = 1;

                rfidd = (long.Parse)(values[5].ToString());
                rfidhex = rfidd.ToString("X");
                rfid2 = (int.Parse(rfidhex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                custid = rfid2;
                rfid2 = rfid2 + "-" + (int.Parse(rfidhex.Substring(2, rfidhex.Length - 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                sifrarm = values[10].ToString().Trim().Replace("\"", "'");

                string datumzaposlenja = values[9].ToString().Trim().Replace("\"", "'");
                string datumprestanka = values[16].ToString().Trim().Replace("\"", "'");

                string[] adatz = datumzaposlenja.Replace("'", "").Split('.');
                string[] adatp = datumprestanka.Replace("'", "").Split('.');

                datumzaposlenja = adatz[2] + "-" + adatz[1] + "-" + adatz[0];
                datumprestanka = adatp[2] + "-" + adatp[1] + "-" + adatp[0];
                datumprestanka = datumzaposlenja;
                //string test = "2016-12-01";
                //DateTime datzz = DateTime.ParseExact(datumzaposlenja , "yyyy-MM-dd" , CultureInfo.InvariantCulture ) ;


                //string sdatz = datzz.ToShortDateString().Substring(0,datzz.ToShortDateString().Length-1);
                //string sdatp = "    - - ";
                if (adatp[2] == "")
                {
                    datumprestanka = "1900-01-01";
                }

                string ulica = values[11].ToString().Trim().Replace("\"", "'");
                string grad = values[14].ToString().Trim().Replace("\"", "'");
                string posta = values[12].ToString().Trim().Replace("\"", "'");

                if (posta.Length == 0)
                {
                    posta = "''";
                }

                id = id.Replace(",", "");
                id = id.Replace("\"", "");
                prezime = prezime.Replace("\"", "");

                idd1 = (int.Parse)(id);   // novi id iz filea

                //            if (id == "65")
                //                  id = "8065" ;

                if (id.IndexOf(",") > 0)
                {
                    //' id1 = (int.Parse)(id.Replace(",", "")) * 1000 ;
                    //' id = id1.ToString();
                }

                rfidhex = "000000" + rfidd.ToString("X");


                //sql1 = id + "," + ime + "," + prezime + ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Feroimpex'," + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString()+",'" + datumzaposlenja+"'";

                //sql1 = id + "," + ime + "," + prezime + ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Tokabu'," + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString()+",'" + datumzaposlenja+"'"; 

                //                sql1 = id + "," + ime + "," + prezime + ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Tokabu',"    + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString();


                if (id == "1173")
                    id = id;


                if (1 == 2)
                {
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Open();
                        SqlCommand sqlCommand = new SqlCommand("select top 1 extid from [user] order by oid desc", cn);
                        SqlDataReader reader2 = sqlCommand.ExecuteReader();
                        reader2.Read();

                        idd1 = (int.Parse)(reader2["extid"].ToString());
                        cn.Close();

                    }
                }

                // idzadnji iz radnici_,   idd1 iz file -csv


                if ((idd1 > idzadnji && idd1 < 8000) && neradi == "0") //|| ( idd1==1329) )// ako je novi
                                                                       //if (idd1 ==74 )     //|| ( idd1==1329) )// ako je novi u tokabu  slobodno 72-78
                                                                       //  if (1==1)                                                             //                if (idd1 ==70  ) //|| ( idd1==1329) )// ako je novi
                {
                    SqlDataReader reader1 = null;

                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Close();
                        cn.Open();

                        //sql1 = "update radnici_  set ulica=" + ulica + ",grad=" + grad + ",posta=" + posta + ",datumzaposlenja='" + datumzaposlenja + "',datumprestanka='" + datumprestanka + "',rfid='" + rfidd.ToString() + "',rfidhex='" + rfidhex + "',rfid2='" + rfid2 + "' where id=" + id + " and poduzece='Feroimpex'";
                        sql1 = id + "," + ime + "," + prezime + "," + ulica + "," + grad + "," + posta + ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Feroimpex'," + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString() + ",'" + datumzaposlenja + "'";
                        SqlCommand sqlCommand = new SqlCommand("insert into radnici_ (id,ime,prezime, ulica , grad , posta , custid,rfid,rfidhex,lokacija,mt,radnomjesto,rfid2,poduzece,sifrarm,rv,neradi,fixnaisplata,datumzaposlenja) values(" + sql1 + ")", cn);
                        //SqlCommand sqlCommand = new SqlCommand( sql1 , cn);




                        //lCommand sqlCommand = new SqlCommand("update radnici_ set sifram = " + sifrarm + " where id = " + id ,  cn ) ;
                        SqlDataReader reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                        cn.Open();



                        if (1 == 1)
                        {


                            prezime = prezime.Replace("'", "");
                            ime = ime.Replace("'", "");

                            SqlCommand cmd = new SqlCommand("rfind.dbo.FX_Import", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@EXT_ID", SqlDbType.VarChar, 5).Value = id;  // lokacija
                            cmd.Parameters.Add("@FNAME", SqlDbType.VarChar, 35).Value = ime;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@LNAME", SqlDbType.VarChar, 35).Value = prezime;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@CSN", SqlDbType.VarChar, 12).Value = rfid2;  // csn1
                            cmd.Parameters.Add("@START_TIME", SqlDbType.DateTime).Value = DateTime.Now;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@END_TIME", SqlDbType.DateTime).Value = DateTime.Now.AddYears(10); ;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = 0;  //  status 0 - nova, 1 - update , 2 disable  3 delete ???
                            reader1 = cmd.ExecuteReader();
                        }
                        cn.Close();

                    }


                }
                ba = 0;


            }

            MessageBox.Show("Gotov import !");

        }

        private void tokabuToolStripMenuItem1_Click(object sender, EventArgs e)
        {

                ImportNovihRadnikaIzPantheon(2);
                return;

            

            Encoding ae = Encoding.GetEncoding("utf-8");
            var reader = new StreamReader(File.OpenRead(@"C:\brisi\radnici\radniciat0.csv"), Encoding.GetEncoding(1250));
            string sql1;
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            string rfid, rfidhex, custid, rfid2, sifrarm;
            long rfidd;
            int id1, idd1, idzadnji;
            //string connectionString = @"Data Source=192.168.0.3;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            int header = 1;
            int ba = 0;
            var line = "";
            idzadnji = 1381;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("select top 1 id from radnici_  where poduzece='Tokabu' order by id desc", cn);
                SqlDataReader reader2 = sqlCommand.ExecuteReader();
                reader2.Read();
                idzadnji = (int.Parse)(reader2["id"].ToString());
                // idzadnji = 0;
                cn.Close();

            }
            //idzadnji = 74;

            while (!reader.EndOfStream)
            {

                if (header == 1)
                {
                    header = 0;
                    line = reader.ReadLine();
                    continue;
                }

                line = reader.ReadLine();
                var values = line.Split(';');

                if ((values[7].ToString().Trim()) == "1" || (values[5].ToString().Length == 0))
                {   // ako ne radi ili nema karticu
                    header = 0;
                    //line = reader.ReadLine();
                    continue;
                }

                // id  0
                // ime 1
                // prezime 2
                // loakcija 3
                // mt 4
                // rfid 5
                // radn mjesto 6
                // ne radi 7
                string id = values[0].ToString().Trim().Replace(" ", "").Replace(",", "");
                string ime = values[1].ToString().Trim().Replace("\"", "'");
                string prezime = values[2].ToString().Trim().Replace("\"", "'");
                string lokacija = values[4].ToString().Trim().Replace("\"", "'");
                string mt = values[3].ToString().Trim().Replace("\"", "'");

                string radnomjesto = values[6].ToString().Trim().Replace("\"", "'");

                if (radnomjesto.Length < 2)
                    radnomjesto = "' '";
                //  0   1     2     3   4         5      6         7    8    9               10       11  12   13    14    15      16         17
                // id,imex,prezime,mt,lokacija, rfid,radnimjesto,neradi,, datumzaposlenja,sifrram, ulica,posta,     ,grad,  , datumprestanka, vrstrada,


                //string neradi = values[7].ToString().Trim().Replace("\"", "'");
                string radido = values[16].ToString().Trim().Replace("\"", "'");
                string vrstarada = values[17].ToString().Trim().Replace("\"", "'");
                int z1 = radido.Length;
                string neradi = "0";

                if (radido.Length != 8)   // ako j eupidan datum do kada radi
                    neradi = "1";

                int vrstaisplate = 1;   //  fixnaisplata

                if (vrstarada == "'Sati'")   //  po satu 
                    vrstaisplate = 0;

                rfidd = (long.Parse)(values[5].ToString());
                rfidhex = rfidd.ToString("X");
                rfid2 = (int.Parse(rfidhex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                custid = rfid2;
                rfid2 = rfid2 + "-" + (int.Parse(rfidhex.Substring(2, rfidhex.Length - 2), System.Globalization.NumberStyles.HexNumber)).ToString();
                sifrarm = values[10].ToString().Trim().Replace("\"", "'");

                string datumzaposlenja = values[9].ToString().Trim().Replace("\"", "'");
                string datumprestanka = values[16].ToString().Trim().Replace("\"", "'");

                string[] adatz = datumzaposlenja.Replace("'", "").Split('.');
                string[] adatp = datumprestanka.Replace("'", "").Split('.');

                datumzaposlenja = adatz[2] + "-" + adatz[1] + "-" + adatz[0];
                datumprestanka = adatp[2] + "-" + adatp[1] + "-" + adatp[0];
                datumprestanka = datumzaposlenja;
                //string test = "2016-12-01";
                //DateTime datzz = DateTime.ParseExact(datumzaposlenja , "yyyy-MM-dd" , CultureInfo.InvariantCulture ) ;


                //string sdatz = datzz.ToShortDateString().Substring(0,datzz.ToShortDateString().Length-1);
                //string sdatp = "    - - ";
                if (adatp[2] == "")
                {
                    datumprestanka = "1900-01-01";
                }

                string ulica = values[11].ToString().Trim().Replace("\"", "'");
                string grad = values[14].ToString().Trim().Replace("\"", "'");
                string posta = values[12].ToString().Trim().Replace("\"", "'");

                if (posta.Length == 0)
                {
                    posta = "''";
                }

                id = id.Replace(",", "");
                id = id.Replace("\"", "");
                prezime = prezime.Replace("\"", "");

                idd1 = (int.Parse)(id);   // novi id iz filea

                //            if (id == "65")
                //                  id = "8065" ;

                if (id.IndexOf(",") > 0)
                {
                    //' id1 = (int.Parse)(id.Replace(",", "")) * 1000 ;
                    //' id = id1.ToString();
                }

                rfidhex = "000000" + rfidd.ToString("X");


                //sql1 = id + "," + ime + "," + prezime + ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Feroimpex'," + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString()+",'" + datumzaposlenja+"'";

                

                //                sql1 = id + "," + ime + "," + prezime + ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Tokabu',"    + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString();


                if (id == "1173")
                    id = id;


                if (1 == 2)
                {
                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Open();
                        SqlCommand sqlCommand = new SqlCommand("select top 1 extid from [user] order by oid desc", cn);
                        SqlDataReader reader2 = sqlCommand.ExecuteReader();
                        reader2.Read();

                        idd1 = (int.Parse)(reader2["extid"].ToString());
                        cn.Close();

                    }
                }

                // idzadnji iz radnici_,   idd1 iz file -csv


                if ((idd1 > idzadnji && idd1 < 8000) && neradi == "0") //|| ( idd1==1329) )// ako je novi
                                                                       //if (idd1 ==74 )     //|| ( idd1==1329) )// ako je novi u tokabu  slobodno 72-78
                                                                       //  if (1==1)                                                             //                if (idd1 ==70  ) //|| ( idd1==1329) )// ako je novi
                {
                    SqlDataReader reader1 = null;

                    using (SqlConnection cn = new SqlConnection(connectionString))
                    {
                        cn.Close();
                        cn.Open();

//                        sql1 = "update radnici_  set ulica=" + ulica + ",grad=" + grad + ",posta=" + posta + ",datumzaposlenja='" + datumzaposlenja + "',datumprestanka='" + datumprestanka + "',rfid='" + rfidd.ToString() + "',rfidhex='" + rfidhex + "',rfid2='" + rfid2 + "' where id=" + id + " and poduzece='Tokabu'";
                        sql1 = id + "," + ime + "," + prezime + "," +ulica+","+grad+","+posta+   ",'" + custid + "','" + rfidd.ToString() + "','" + rfidhex + "','" + lokacija + "','" + mt + "'," + radnomjesto + ",'" + rfid2 + "','Tokabu'," + sifrarm + ",1," + neradi + "," + vrstaisplate.ToString() + ",'" + datumzaposlenja + "'";

                        SqlCommand sqlCommand = new SqlCommand("insert into radnici_ (id,ime,prezime,ulica,grad,posta,custid,rfid,rfidhex,lokacija,mt,radnomjesto,rfid2,poduzece,sifrarm,rv,neradi,fixnaisplata,datumzaposlenja) values(" + sql1 + ")", cn);
                        //SqlCommand sqlCommand = new SqlCommand( sql1 , cn);

                        //lCommand sqlCommand = new SqlCommand("update radnici_ set sifram = " + sifrarm + " where id = " + id ,  cn ) ;
                        SqlDataReader reader2 = sqlCommand.ExecuteReader();
                        cn.Close();
                        cn.Open();

                        if (1 == 1)
                        {


                            prezime = prezime.Replace("'", "");
                            ime = ime.Replace("'", "");

                            SqlCommand cmd = new SqlCommand("rfind.dbo.FX_Import", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@EXT_ID", SqlDbType.VarChar, 5).Value = id;  // lokacija
                            cmd.Parameters.Add("@FNAME", SqlDbType.VarChar, 35).Value = ime;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@LNAME", SqlDbType.VarChar, 35).Value = prezime;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@CSN", SqlDbType.VarChar, 12).Value = rfid2;  // csn1
                            cmd.Parameters.Add("@START_TIME", SqlDbType.DateTime).Value = DateTime.Now;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@END_TIME", SqlDbType.DateTime).Value = DateTime.Now.AddYears(10); ;  // """; //  DateTime.ParseExact(dat1, "yyyy-MM-dd HH:mm:ss" , System.Globalization.CultureInfo.InvariantCulture);  // Od datuma
                            cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = 0;  //  status 0 - nova, 1 - update , 2 disable  3 delete ???
                            reader1 = cmd.ExecuteReader();
                        }
                        cn.Close();

                    }


                }
                ba = 0;


            }

            MessageBox.Show("Gotov import !");

        }
    }
}