using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ЛР4
{
    public partial class Form2 : Form
    {
        string connectionString = @"Data Source=ADCLG1;Initial Catalog=Петрунина 319/4 ИЗ4;Integrated Security=True";

        SqlDataAdapter adapter;
        DataSet ds;
        public Form2()
        {
            InitializeComponent();
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;

            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            comboBox5.Visible = false;

            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;

            comboBox1.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            comboBox5.Visible = false;

            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;

            for (int i = 1; i <= numericUpDown1.Value; i++)
            {
                    (Controls["textBox" + i.ToString()] as TextBox).Visible = true;
                    (Controls["comboBox" + i.ToString()] as ComboBox).Visible = true;
                    (Controls["checkBox" + i.ToString()] as CheckBox).Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "CREATE TABLE [" + textBox6.Text + "](";
            for (int i = 1; i <= numericUpDown1.Value; i++)
            {
                    sql += "[" + (Controls["textBox" + i.ToString()] as TextBox).Text + "] [" + (Controls["comboBox" + i.ToString()] as ComboBox).Text + "] ";

                    if((Controls["checkBox" + i.ToString()] as CheckBox).Checked == true)
                    {
                        sql += "NOT NULL, ";
                    }
                    else
                    {
                        sql += "NULL, ";
                    }
            }

            sql = sql.Remove(sql.Length - 2);
            sql += ")";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
            }

            MessageBox.Show("Таблица создана");


        }
    }
}
