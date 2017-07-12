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
    public partial class Postavwik : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\App_Data\DB.mdf;Integrated Security=True");
        public Postavwik()
        {
            InitializeComponent();
        }

        private void Postavwik_Load(object sender, EventArgs e)
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
                SqlDataAdapter da = new SqlDataAdapter("select *from Postavwiki", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Postavwiki");
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
                dataGridView1.Columns[0].HeaderText = "Номер поставщика";
                dataGridView1.Columns[1].HeaderText = "Наименование";
                dataGridView1.Columns[2].HeaderText = "Адрес";
                dataGridView1.Columns[3].HeaderText = "Телефон";
                dataGridView1.Columns[4].HeaderText = "Электронный адрес";
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                dataGridView1.Font = new Font("Times New Roman", 10);
            }
            catch (Exception) { }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
               // comboBox1.Enabled = false;
                comboBox1.Items.Clear();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                comboBox1.ResetText();
                comboBox1.DropDownStyle = ComboBoxStyle.Simple;
                groupBox2.Text = "Добавить";
                buttonAddgr.Text = "Добавить";
                comboBox1.Text = dataGridView1.RowCount.ToString();
            }
            else if (radioButton2.Checked == true)
            {
                try
                {
                    comboBox1.Enabled = true;
                    comboBox1.Items.Clear();
                    comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
                    groupBox2.Text = "Изменить";
                    buttonAddgr.Text = "Изменить";
                    Copy_id();
                    comboBox1.SelectedIndex = 0;
                }
                catch (Exception)
                {

                }
            }
        }
        private void Copy_id()
        {
            try
            {
                conn.Open();
                string sqlSel = "select id_post from Postavwiki";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["id_post"].ToString());
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
                string sql = "Delete from Postavwiki where id_post='" + s + "'";
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
        private void Copy_group()
        {
            try
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                string k = comboBox1.Text;
                int index = Convert.ToInt32(k);
                string s1 = dataGridView1.Rows[index - 1].Cells[1].Value.ToString();
                string s2 = dataGridView1.Rows[index - 1].Cells[2].Value.ToString();
                string s3 = dataGridView1.Rows[index - 1].Cells[3].Value.ToString();
                string s4 = dataGridView1.Rows[index - 1].Cells[4].Value.ToString();
                textBox2.Text = s1;
                textBox1.Text = s2;
                textBox4.Text = s3;
                textBox3.Text = s4;

            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                Copy_group();
            }
        }

        private void buttonAddgr_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                try
                {
                    if (textBox1.Text != String.Empty && textBox2.Text != String.Empty && textBox3.Text != String.Empty && textBox4.Text != String.Empty)
                    {
                        conn.Open();
                        int s = Convert.ToInt32(comboBox1.Text);
                        string s1 = textBox2.Text;
                        string s2 = textBox1.Text;
                        string s3 = textBox4.Text;
                        string s4 = textBox3.Text;
                        string sql = "insert into Postavwiki (id_post,Name,Address,Phone,Mail) values ('" + s + "',N'" + s1 + "',N'" + s2 + "',N'" + s3 + "',N'" + s4 + "')";
                        SqlCommand command = new SqlCommand(sql, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                        printtable();
                        comboBox1.ResetText();
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
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
        private void Update_group()
        {
            try
            {
                conn.Open();
                string sql = "Update Postavwiki set [Name]=N'" + textBox2.Text + "',[Address]=N'" + textBox1.Text + "',[Phone]=N'" + textBox4.Text + "',[Mail]=N'" + textBox3.Text + "' where [id_post]='" + comboBox1.Text + "'";
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
