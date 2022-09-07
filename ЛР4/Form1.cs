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
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=ADCLG1;Initial Catalog=Петрунина 319/4 ИЗ4;Integrated Security=True";

        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        DataSet ds;
        DataTable dt;

        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = GetTables(connectionString);
        }
        public static List<string> GetTables(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                List<string> TableNames = new List<string>();
                foreach (DataRow row in schema.Rows)
                {
                    TableNames.Add(row[2].ToString());
                }
                return TableNames;
            }
        }

        public static string[] GetNames(string sql, string connectionString, SqlDataAdapter adapter, DataSet ds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var list = new List<string>();
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr.GetString(0));
                        }
                    }
                    return list.ToArray();
                }
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            string sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name='" + comboBox1.Text + "'";
            string[] Names = GetNames(sql, connectionString, adapter, ds);
            checkedListBox1.Items.AddRange(Names);

            comboBox2.DataSource = GetNames(sql, connectionString, adapter, ds);

            string sql1 = "SELECT * FROM [" + comboBox1.Text + "]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql1, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dt = ds.Tables[0];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int a = checkedListBox1.CheckedItems.Count;
            string[] Names = new string[a];
            Names = checkedListBox1.CheckedItems.Cast<string>().ToArray();
            string sql = "SELECT [" + Names[0] + "]";
            for (int i = 1; i < a; i++)
            {
                sql = sql + ", [" + Names[i] + "]";
            }
            sql = sql + " FROM [" + comboBox1.Text + "]";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dt = ds.Tables[0];
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string table = comboBox1.Text;
            string column = comboBox2.Text;
            string sql = "update " + table + " set " + column + " = '" + textBox2.Text + "' where id = " + textBox1.Text;
            string sql1 = "SELECT * FROM [" + comboBox1.Text + "]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql1, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            int a = dataGridView1.ColumnCount;
            string sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name='" + comboBox1.Text + "'";
            string[] Names = GetNames(sql, connectionString, adapter, ds);
            string ComAdd = " INSERT INTO [" + comboBox1.Text + "] (";
            for (int i = 1; i < a; i++)
            {
                ComAdd = ComAdd + "[" + Names[i] + "], ";
            }
            ComAdd = ComAdd.Remove(ComAdd.Length - 2);
            ComAdd += ") VALUES(";

            for (int i = 1; i < a; i++)
            {
                ComAdd = ComAdd + "N'" + dataGridView1[i, dataGridView1.CurrentRow.Index].Value.ToString() + "', ";
            }
            ComAdd = ComAdd.Remove(ComAdd.Length - 2);
            ComAdd += ")";
            string sql1 = "SELECT * FROM [" + comboBox1.Text + "]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd2 = new SqlCommand(ComAdd, connection);
                connection.Open();
                cmd2.ExecuteNonQuery();
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql1, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
            }
        }
    }
}