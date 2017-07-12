using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Products_stock
{
    public partial class Goods : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\App_Data\DB.mdf;Integrated Security=True");
        SqlConnection connect2 = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\App_Data\DB.mdf;Integrated Security=True");
        public Goods()
        {
            InitializeComponent();
        }

        private void Goods_Load(object sender, EventArgs e)
        {
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            printtable();
            new_tovar();
            product_group();
            ColumsHeaderText();
            Copy_id();
            All_count();
            comboBox1.SelectedIndex = 0;
            
        }
        private void new_tovar()
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
                        connect2.Open();
                        id = dataGridView1.RowCount;
                        string sql = "insert into Goods (Id_goods,[Name],[Group],Edinica_izmerenia,Price,[Count]) values ('" + id + "',N'" + nometovara + "',N'" + Group + "',N'" + Edinica + "',N'" + price + "',N'" + Count + "')";
                        SqlCommand command = new SqlCommand(sql, connect2);
                        command.ExecuteNonQuery();
                        delete(nomertovara);
                        printtable1();
                        connect2.Close();
                    }
                }
                cmdSel.Dispose();

                cmdSel = null;
                conn.Close();
                printtable();
                MessageBox.Show("Количество новых товаров: " + lenght.ToString(), "Поставка товаров");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connect2.Close();
                conn.Close();
            }
        }
        private void printtable1()
        {
            SqlDataAdapter da = new SqlDataAdapter("select *from Goods", connect2);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            DataSet ds = new DataSet();
            da.Fill(ds, "Goods");
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void delete(string id)
        {
            try
            {
                string sql = "Delete from [Order_goods] where [Id_order]='" + id + "'";
                SqlCommand command = new SqlCommand(sql, connect2);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
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
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 10, FontStyle.Bold);
            dataGridView1.Font = new System.Drawing.Font("Times New Roman", 10);
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
                string s1 = dataGridView1.Rows[index - 1].Cells[1].Value.ToString();
                string s2 = dataGridView1.Rows[index - 1].Cells[2].Value.ToString();
                string s3 = dataGridView1.Rows[index - 1].Cells[3].Value.ToString();
                string s4 = dataGridView1.Rows[index - 1].Cells[4].Value.ToString();
                string s5 = dataGridView1.Rows[index - 1].Cells[5].Value.ToString();
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
            catch (Exception ex)
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
        public void CloseProcess()
        {
            Process[] List;
            List = Process.GetProcessesByName("EXCEL");
            foreach (Process proc in List)
            {
                proc.Kill();
            }
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

        private void OchetExcel_Click(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            try
            {
                Microsoft.Office.Interop.Excel.Application oXL;
                Microsoft.Office.Interop.Excel._Workbook oWB;
                Microsoft.Office.Interop.Excel._Worksheet oSheet;
                Microsoft.Office.Interop.Excel.Range oRng;

                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = true;

                oWB = (Microsoft.Office.Interop.Excel.Workbook)(oXL.Workbooks.Add(XlWBATemplate.xlWBATWorksheet));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                int i, j;

                for (j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    oSheet.Cells[1, j + 1] = dataGridView1.Columns[j].HeaderText;
                }

                oSheet.Cells[1, 7] = "Общее количество товаров на складе";

                oSheet.Cells[2, 7] = textBox_Allcount.Text + " шт.";

                for (i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    for (j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[i + 2, j + 1];
                        oRng.NumberFormat = "@";
                        if (j == 0)
                            oSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        else
                            oSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;

                    }
                }
                string dt = dateTimePicker1.Value.Date.ToShortDateString();
                dt = dt.Replace("/", "-");
                string folderPath = "C:\\EXCEL\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string namef = "Список_товаров_" + dt;
                string filepath = folderPath + namef + ".xlsx";
                oWB.SaveAs(filepath);
                oWB.Close();
                oXL.Visible = false;
                MessageBox.Show("Файл успешно сохранен, по адресу: " + filepath, "Отчет в Excel", MessageBoxButtons.OK);
                CloseProcess();
            }
            catch (Exception)
            {
                MessageBox.Show("Файл не сохранён!", "Отчет в Excel", MessageBoxButtons.OK);
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

            }
        }

        private void buttonPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string folderPath = "C:\\PDF\\";
                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
                string dt = dateTimePicker1.Value.Date.ToShortDateString();
                dt = dt.Replace("/", "-");
                PdfPCell cell = new PdfPCell(new Phrase("Список товаров " + dt, font));

                cell.Colspan = dataGridView1.Columns.Count;
                cell.HorizontalAlignment = 1;
                cell.Border = 0;
                table.AddCell(cell);

                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    cell = new PdfPCell(new Phrase(new Phrase(dataGridView1.Columns[j].HeaderText, font)));
                    cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dataGridView1.Rows[i].Cells[j].Value.ToString(), font));
                    }
                }
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string namef = "Список_товаров_" + dt;
                string filepath = namef + ".pdf";
                using (FileStream stream = new FileStream(folderPath + filepath, FileMode.Create))
                {

                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(table);
                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Файл успешно сохранен, по адресу: " + folderPath + filepath, "Отчет в PDF", MessageBoxButtons.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось сохранить файл!", "Отчет в PDF", MessageBoxButtons.OK);
            }
        }
    }
}

