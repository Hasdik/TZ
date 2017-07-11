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
    public partial class Goods : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Products_stock\Products_stock\DB.mdf;Integrated Security=True");
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Products_stock\Products_stock\DB.mdf;Integrated Security=True");
        public Goods()
        {
            InitializeComponent();
        }

        private void Goods_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            product_group();
            printtable();
            ColumsHeaderText();
            Copy_id();
            All_count();
            comboBox1.SelectedIndex = 0;
            new_tovar();
        }
        void new_tovar()
        {
            try
            {
                string Group = " ";
                string Edinica = " ";
                string Count = " ";
                string data = " ";
                string nometovara = " ";
                string nomertovara = " ";
                string price = " ";
                int id = 0;
                int lenght = 0;

                conn.Open();
                string sqlSel = "select Id_order,[Name_goods],[Group],Edinica_izmerenia,Price,[Count],[Date_supply] from [Order_goods]";

                SqlDataReader dr = null;

                SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

                dr = cmdSel.ExecuteReader();

                while (dr.Read())
                {
                    nomertovara = dr["Id_order"].ToString();
                    nometovara = dr["Name_goods"].ToString();
                    Group = dr["Group"].ToString();
                    Edinica = dr["Edinica_izmerenia"].ToString();
                    price = dr["Price"].ToString();
                    Count = dr["Count"].ToString();
                    data = dr["Date_supply"].ToString();
                    if (data == dateTimePicker1.Value.Date.ToShortDateString())
                    {
                        lenght++;
                        connect.Open();
                        id = dataGridView1.RowCount;
                        string sql = "insert into Goods (Id_goods,[Name],[Group],Edinica_izmerenia,Price,[Count]) values ('" + id + "',N'" + nometovara + "',N'" + Group + "',N'" + Edinica + "',N'" + price + "',N'" + Count + "')";
                        SqlCommand command = new SqlCommand(sql, connect);
                        command.ExecuteNonQuery();
                        delete(nomertovara);
                        printtable1();
                        connect.Close();
                    }
                }
                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
                printtable();
                MessageBox.Show("Количество новых товаров: " + lenght.ToString(),"Поставка товаров");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                connect.Close();
                conn.Close();
            }
        }
        void printtable1()
        {
            SqlDataAdapter da = new SqlDataAdapter("select *from Goods", connect);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "Goods");
            dataGridView1.DataSource = ds.Tables[0];
        }
        void delete(string id)
        {
            try
            {
                string sql = "Delete from [Order_goods] where [Id_order]='" + id + "'";
                SqlCommand command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void printtable()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select *from Goods", conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "Goods");
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
            dataGridView1.Columns[0].HeaderText = "Номер товара";
            dataGridView1.Columns[1].HeaderText = "Название товара";
            dataGridView1.Columns[2].HeaderText = "Группа";
            dataGridView1.Columns[3].HeaderText = "Единица измерения";
            dataGridView1.Columns[4].HeaderText = "Цена за шутку";
            dataGridView1.Columns[5].HeaderText = "Количество на складе";
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            dataGridView1.Font = new Font("Times New Roman", 10);
        }

        private void Delete_GroupBox_Click(object sender, EventArgs e)
        {
            Delete();
        }
        private void Delete()
        {
            try
            {
                conn.Open();
                string s = dataGridView1.CurrentCell.Value.ToString();
                string sql = "Delete from Goods where Id_goods='" + s + "'";
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
        private void Copy_goods()
        {
            try
            {
                Name_t.Clear();
                Ed_t.Clear();
                Price_t.Clear();
                Count_t.Clear();
                comboBox2.ResetText();
                string k = comboBox1.Text;
                int index = Convert.ToInt32(k);
                string s1 = dataGridView1.Rows[index-1].Cells[1].Value.ToString();
                string s2 = dataGridView1.Rows[index-1].Cells[2].Value.ToString();
                string s3 = dataGridView1.Rows[index-1].Cells[3].Value.ToString();
                string s4 = dataGridView1.Rows[index-1].Cells[4].Value.ToString();
                string s5 = dataGridView1.Rows[index-1].Cells[5].Value.ToString();
                Name_t.Text = s1;
                comboBox2.Text = s2;
                Ed_t.Text = s3;
                Price_t.Text = s4;
                Count_t.Text = s5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void add_goods_Click(object sender, EventArgs e)
        {
            Update_goods();
            All_count();
        }
        private void Update_goods()
        {
            try
            {
                conn.Open();
                string sql = "Update Goods set [Name]=N'" + Name_t.Text + "',[Group]=N'" + comboBox2.Text + "',[Edinica_izmerenia]=N'" + Ed_t.Text + "',[Price]='" + Price_t.Text + "',[Count]='" + Count_t.Text + "' where [Id_goods]='" + comboBox1.Text + "'";
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
        private void product_group()
        {
            conn.Open();
            string sqlSel = "select Name from Product_group";

            SqlDataReader dr = null;

            SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

            dr = cmdSel.ExecuteReader();

            while (dr.Read())
            {
                comboBox2.Items.Add(dr["Name"].ToString());
            }

            cmdSel.Dispose();

            cmdSel = null;
            conn.Close();
        }
        private void Copy_id()
        {
            conn.Open();
            string sqlSel = "select Id_goods from Goods";

            SqlDataReader dr = null;

            SqlCommand cmdSel = new SqlCommand(sqlSel, conn);

            dr = cmdSel.ExecuteReader();

            while (dr.Read())
            {
                comboBox1.Items.Add(dr["Id_goods"].ToString());
            }

            cmdSel.Dispose();

            cmdSel = null;
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Copy_goods();
        }
        private void All_count()
        {
            textBox_Allcount.Enabled = false;
            int sum = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                sum += Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
            }
            textBox_Allcount.Text = sum.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.Text == "Номер товара")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Id_goods"], ListSortDirection.Ascending);
                }
                else if (comboBox3.Text == "Название товара")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Name"], ListSortDirection.Ascending);
                }
                else if (comboBox3.Text == "Группа")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Group"], ListSortDirection.Ascending);
                }
                else if (comboBox3.Text == "Единица измерения")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Edinica_izmerenia"], ListSortDirection.Ascending);
                }
                else if (comboBox3.Text == "Цена за шутку")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Price"], ListSortDirection.Ascending);
                }
                else if (comboBox3.Text == "Количество на складе")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Count"], ListSortDirection.Ascending);
                }           
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox4.Text == "Номер товара")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Id_goods"], ListSortDirection.Descending);
                }
                else if (comboBox4.Text == "Название товара")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Name"], ListSortDirection.Descending);
                }
                else if (comboBox4.Text == "Группа")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Group"], ListSortDirection.Descending);
                }
                else if (comboBox4.Text == "Единица измерения")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Edinica_izmerenia"], ListSortDirection.Descending);
                }
                else if (comboBox4.Text == "Цена за шутку")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Price"], ListSortDirection.Descending);
                }
                else if (comboBox4.Text == "Количество на складе")
                {
                    dataGridView1.Sort(dataGridView1.Columns["Count"], ListSortDirection.Descending);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Price_t_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) | (Char.IsPunctuation(e.KeyChar))) return;
            else
                if (e.KeyChar != (char)Keys.Back)
                    e.Handled = true;
        }

        private void Count_t_KeyPress(object sender, KeyPressEventArgs e)
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
