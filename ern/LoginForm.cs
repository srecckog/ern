﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ern
{
    public partial class LoginForm : Form
    {
        public string connectionString = @"Data Source=192.168.0.5;Initial Catalog=RFIND;User ID=sa;Password=AdminFX9.";
        public static string idusera1,idprijave1,korisnik1;
        public LoginForm()
        {
            InitializeComponent();        
        }

        private void btn_login_Click(object sender, EventArgs e)
        {

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM korisnici where username='"+textBox2.Text.Trim()+"' and password='"+textBox3.Text.Trim()+"'", cn);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                idusera1 = "";
                while ( reader.Read() )
                {
                    idusera1 = reader["ID"].ToString();
                    DialogResult = DialogResult.OK;
                }

                if (idusera1=="")
                    MessageBox.Show("Neispravno korisničko ime ili lozinka !");

                cn.Close();
                idprijave1 = idusera1.ToString() + " - " + DateTime.Now.ToString();
                korisnik1 = textBox2.Text.Trim();

                using (SqlConnection cn4 = new SqlConnection(connectionString))
                {

                    cn4.Open();
                    sqlCommand = new SqlCommand("insert into erv_log (korisnik,idprijave,opis,datum) values('" + korisnik1 + "','" + idprijave1 + "','Prijava',getdate())",cn4);
                    reader = sqlCommand.ExecuteReader();
                    cn4.Close();

                }



            }




        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
