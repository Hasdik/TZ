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
    public partial class Realization : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\App_Data\DB.mdf;Integrated Security=True");
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\App_Data\DB.mdf;Integrated Security=True");
        string sessionCount;
        public Realization()
        {
            InitializeComponent();
        }

        private void Realization_Load(object sender, EventArgs e)
        {
            try
            {
                radioButton1.Checked = true;
                printtable();
                ColumsHeaderText();
                textBox5.Enabled = false;
                comboBox1.Text = dataGridView1.RowCount.ToString();
            }
            catch (Exception) { }
        }
        private void printtable()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select *from Realization", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Realization");
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
                dataGridView1.Columns[0].HeaderText = "Номер реализации";
                dataGridView1.Columns[1].HeaderText = "Сотрудник";
                dataGridView1.Columns[2].HeaderText = "Название товара";
                dataGridView1.Columns[3].HeaderText = "Группа";
                dataGridView1.Columns[4].HeaderText = "Единица измерения";
                dataGridView1.Columns[5].HeaderText = "Цена";
                dataGridView1.Columns[6].HeaderText = "Количество";
                dataGridView1.Columns[7].HeaderText = "Общая сумма";
                dataGridView1.Columns[8].HeaderText = "Дата";
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                dataGridView1.Font = new Font("Times New Roman", 10);
            }
            catch (Exception) { }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                comboBox1.Enabled = false;
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                comboBox4.Items.Clear();
                comboBox4.ResetText();
                comboBox3.ResetText();
                comboBox2.ResetText();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                comboBox1.ResetText();
                comboBox1.DropDownStyle = ComboBoxStyle.Simple;
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
                groupBox2.Text = "Добавить";
                buttonAddgr.Text = "Добавить";
                Copy_post();
                Copy_gr();
                Copy_tv();
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                comboBox3.Enabled = false;
                comboBox1.Text = dataGridView1.RowCount.ToString();
            }
            else if (radioButton2.Checked == true)
            {
                try
                {
                    comboBox1.Enabled = true;
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    comboBox3.Items.Clear();
                    comboBox4.Items.Clear();
                    comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox2.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBox3.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBox4.DropDownStyle = ComboBoxStyle.DropDown;
                    groupBox2.Text = "Изменить";
                    buttonAddgr.Text = "Изменить";
                    Copy_id();
                    Copy_post();
                    Copy_gr();
                    Copy_tv();
                    comboBox1.SelectedIndex = 0;
                    comboBox3.Enabled = true;
                    textBox3.Enabled = true;
                    textBox4.Enabled = true;
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
                string sqlSel = "select Id_relz from Realization";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["Id_relz"].ToString());
                }

                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
            }
            catch (Exception) { conn.Close(); }
        }
        private void Copy_post()
        {
            try
            {
                conn.Open();
                string sqlSel = "select FIO from Employees";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    comboBox2.Items.Add(dr["FIO"].ToString());
                }

                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
            }
            catch (Exception) { conn.Close(); }
        }
        private void Copy_rel()
        {
            try
            {
                sessionCount = "";
                string Name = "";
                conn.Open();
                string sqlSel = "select Name, [Group],Edinica_izmerenia,Price,Count from Goods";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    Name = dr["Name"].ToString();
                    if (Name == comboBox4.Text)
                    {
                        comboBox3.Text = (dr["Group"].ToString());
                        textBox4.Text = dr["Edinica_izmerenia"].ToString();
                        textBox3.Text = dr["Price"].ToString();
                        textBox6.Text = dr["Count"].ToString();
                        sessionCount = dr["Count"].ToString();
                    }
                }

                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
                comboBox3.Enabled = false;
            }
            catch (Exception) { conn.Close(); }
        }
        private void Copy_gr()
        {
            try
            {
                conn.Open();
                string sqlSel = "select Name from Product_group";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    comboBox3.Items.Add(dr["Name"].ToString());
                }

                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
            }
            catch (Exception) { conn.Close(); }
        }
        private void Copy_tv()
        {
            try
            {
                conn.Open();
                string sqlSel = "select Name from Goods";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    comboBox4.Items.Add(dr["Name"].ToString());
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
                string sql = "Delete from Realization where Id_relz='" + s + "'";
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
                comboBox2.ResetText();
                comboBox3.ResetText();
                comboBox4.ResetText();
                comboBox3.ResetText();
                textBox4.Clear();
                textBox3.Clear();
                textBox6.Clear();
                textBox5.Clear();
                string k = comboBox1.Text;
                int index = Convert.ToInt32(k);
                string s1 = dataGridView1.Rows[index - 1].Cells[1].Value.ToString();
                string s2 = dataGridView1.Rows[index - 1].Cells[2].Value.ToString();
                string s3 = dataGridView1.Rows[index - 1].Cells[3].Value.ToString();
                string s4 = dataGridView1.Rows[index - 1].Cells[4].Value.ToString();
                string s5 = dataGridView1.Rows[index - 1].Cells[5].Value.ToString();
                string s6 = dataGridView1.Rows[index - 1].Cells[6].Value.ToString();
                string s7 = dataGridView1.Rows[index - 1].Cells[7].Value.ToString();
                string s8 = dataGridView1.Rows[index - 1].Cells[8].Value.ToString();
                comboBox2.Text = s1;
                comboBox4.Text = s2;
                comboBox3.Text = s3;
                textBox4.Text = s4;
                textBox3.Text = s5;
                textBox6.Text = s6;
                textBox5.Text = s7;
                dateTimePicker3.Value = Convert.ToDateTime(s8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonAddgr_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                try
                {
                    if (textBox6.Text != String.Empty && comboBox2.Text != String.Empty && comboBox4.Text != String.Empty)
                    {
                        conn.Open();
                        int s = Convert.ToInt32(comboBox1.Text);
                        string s1 = comboBox2.Text;
                        string s2 = comboBox4.Text;
                        string s3 = comboBox3.Text;
                        string s4 = textBox4.Text;
                        string s5 = textBox3.Text;
                        string s6 = textBox6.Text;
                        string s7 = textBox5.Text;
                        string s8 = dateTimePicker3.Value.Date.ToShortDateString();
                        if (Convert.ToInt32(s6) <= Convert.ToInt32(sessionCount))
                        {
                            string sql = "insert into Realization (Id_relz,Eployees,Name,[Group],Edinica_izmerenia,Price,Count,All_sum,Date) values ('" + s + "',N'" + s1 + "',N'" + s2 + "',N'" + s3 + "',N'" + s4 + "',N'" + s5 + "',N'" + s6 + "',N'" + s7 + "',N'" + s8 + "')";
                            SqlCommand command = new SqlCommand(sql, conn);
                            command.ExecuteNonQuery();
                            conn.Close();
                            printtable();
                            MethodCheckTovar();
                            comboBox1.ResetText();
                            comboBox4.ResetText();
                            textBox5.Clear();
                            textBox6.Clear();
                            comboBox1.ResetText();
                            comboBox2.ResetText();
                            comboBox3.ResetText();
                            comboBox1.Text = dataGridView1.RowCount.ToString();

                            sessionCount = "";
                        }
                        else
                        {
                            MessageBox.Show("Количество товара '" + comboBox4.Text + "' на складе " + sessionCount + " штук. Нельза добавлять больше!", "Ошибка добавления!");
                            conn.Close();
                        }
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
        private void MethodCheckTovar()
        {
            try
            {
                string Group = " ";
                string Edinica = " ";
                string Count = " ";
                string nometovara = " ";
                string nomertovara = " ";
                string price = " ";
                conn.Open();
                string sqlSel = "select Id_goods,[Name],[Group],Edinica_izmerenia,Price,[Count] from [Goods]";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    nomertovara = dr["Id_goods"].ToString();
                    nometovara = dr["Name"].ToString();
                    Group = dr["Group"].ToString();
                    Edinica = dr["Edinica_izmerenia"].ToString();
                    price = dr["Price"].ToString();
                    Count = dr["Count"].ToString();
                    if ((nometovara == comboBox4.Text) && (Group == comboBox3.Text) && (Edinica == textBox4.Text) && (price == textBox3.Text))
                    {
                            connect.Open();
                            Update_goods(nomertovara);
                            connect.Close();
                    }
                }
                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connect.Close();
                conn.Close();
            }
        }
        private void Update_goods(string nomertovara)
        {
            int countnew = Convert.ToInt32(sessionCount) - (Convert.ToInt32(textBox6.Text));

            string sql = "Update Goods set [Name]=N'" + comboBox4.Text + "',[Group]=N'" + comboBox3.Text + "',[Edinica_izmerenia]=N'" + textBox4.Text + "',[Price]='" + textBox3.Text + "',[Count]='" + countnew.ToString() + "' where [Id_goods]='" + nomertovara + "'";
            SqlCommand command = new SqlCommand(sql, connect);
            command.ExecuteNonQuery();

            MessageBox.Show("Количество товара " + comboBox4.Text + " на складе = " + countnew);
        }
        private void Update_group()
        {
            conn.Open();
            string sql = "Update Realization set [Eployees]=N'" + comboBox2.Text + "',[Name]=N'" + comboBox4.Text + "',[Group]=N'" + comboBox3.Text + "',[Edinica_izmerenia]=N'" + textBox4.Text + "',[Price]=N'" + textBox3.Text + "',[Count]=N'" + textBox6.Text + "',[All_sum]=N'" + textBox5.Text + "',[Date]=N'" + dateTimePicker3.Value.Date.ToShortDateString() + "' where [Id_relz]='" + comboBox1.Text + "'";
            SqlCommand command = new SqlCommand(sql, connect);
            command.ExecuteNonQuery();
            conn.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                Copy_group();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (Char.IsPunctuation(e.KeyChar))) return;
            else
                if (e.KeyChar != (char)Keys.Back)
                    e.Handled = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != String.Empty)
                {
                    float price = Convert.ToSingle(textBox3.Text);
                    int Count = Convert.ToInt32(textBox6.Text);
                    textBox5.Text = (price * Count).ToString();
                }
                else if (textBox6.Text == String.Empty)
                {
                    textBox5.Text = textBox3.Text;
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != String.Empty)
                {
                    float price = Convert.ToSingle(textBox3.Text);
                    int Count = Convert.ToInt32(textBox6.Text);
                    textBox5.Text = (price * Count).ToString();
                }
                else if (textBox6.Text == String.Empty)
                {
                    textBox5.Text = textBox3.Text;
                }
            }
            catch (Exception)
            {
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Copy_rel();
        }
    }
}
