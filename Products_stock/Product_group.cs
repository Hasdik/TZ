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

namespace Products_stock
{
    public partial class Product_group : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\DB.mdf;Integrated Security=True");
        public Product_group()
        {
            InitializeComponent();
        }

        private void Product_group_Load(object sender, EventArgs e)
        {
            try
            {
                radioButton1.Checked = true;
                printtable();
                ColumsHeaderText();
                comboBox1.Text = dataGridView1.RowCount.ToString();
            }
            catch (Exception) { }
        }
        private void printtable()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select *from Product_group", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Product_group");
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        private void ColumsHeaderText()
        {
            try
            {
                dataGridView1.Columns[0].HeaderText = "Номер группы";
                dataGridView1.Columns[1].HeaderText = "Название группы";
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                dataGridView1.Font = new Font("Times New Roman", 10);
            }
            catch (Exception) { }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {
                comboBox1.Enabled = false;
                comboBox1.Items.Clear();
                textBox2.Clear();
                comboBox1.ResetText();
                comboBox1.DropDownStyle = ComboBoxStyle.Simple;
                groupBox2.Text = "Добавить";
                buttonAddgr.Text = "Добавить";
                comboBox1.Text = dataGridView1.RowCount.ToString();
            }
            else if(radioButton2.Checked==true)
            {
                try
                {
                    comboBox1.Enabled = true;
                    comboBox1.Items.Clear();
                    comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                    groupBox2.Text = "Изменить";
                    buttonAddgr.Text = "Изменить";
                    Copy_id();
                    comboBox1.SelectedIndex = 0;
                }
                catch(Exception)
                {

                }
            }
        }

        private void buttonAddgr_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                try
                {
                    if (textBox2.Text != String.Empty)
                    {
                        conn.Open();
                        int s = Convert.ToInt32(comboBox1.Text);
                        string s1 = textBox2.Text;
                        string sql = "insert into Product_group (Id_product,Name) values ('" + s + "',N'" + s1 + "')";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                        printtable();
                        comboBox1.ResetText();
                        textBox2.Clear();
                        comboBox1.Text = dataGridView1.RowCount.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!");
                    }
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Заполните все поля!", ex.Message);
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Запись имеется уже в БД!", ex.Message);
                    conn.Close();
                }
            }
            else if (radioButton2.Checked == true)
            {
                Update_group();
            }
        }
        private void Copy_group()
        {
            try
            {
                textBox2.Clear();
                string k = comboBox1.Text;
                int index = Convert.ToInt32(k);
                string s1 = dataGridView1.Rows[index-1].Cells[1].Value.ToString();
                textBox2.Text = s1;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Update_group()
        {
            try
            {
                conn.Open();
                string sql = "Update Product_group set [Name]=N'" + textBox2.Text + "' where [Id_product]='" + comboBox1.Text + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                printtable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        private void Copy_id()
        {
            try
            {
                conn.Open();
                string sqlSel = "select Id_product from Product_group";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["Id_product"].ToString());
                }

                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
            }
            catch (Exception) { conn.Close(); }
        }
        private void button_delgr_Click(object sender, EventArgs e)
        {
            Delete();
            comboBox1.Text = dataGridView1.RowCount.ToString();
        }
        private void Delete()
        {
            try
            {
                conn.Open();
                string s = dataGridView1.CurrentCell.Value.ToString();
                string sql = "Delete from Product_group where Id_product='" + s + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                printtable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                Copy_group();
            }
        }
    }
}
