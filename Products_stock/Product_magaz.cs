using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Products_stock
{
    public partial class Product_magaz : Form
    {
        public Product_magaz()
        {
            InitializeComponent();
        }

        private void Tovar_Click(object sender, EventArgs e)
        {
            Goods g = new Goods();
            g.ShowDialog();
        }

        private void button_groupTovar_Click(object sender, EventArgs e)
        {
            Product_group p = new Product_group();
            p.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Employees p = new Employees();
            p.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Postavwik p = new Postavwik();
            p.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OrderGoods o = new OrderGoods();
            o.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Realization r = new Realization();
            r.ShowDialog();
        }
        
        private void Product_magaz_Load(object sender, EventArgs e)
        {
            button1.FlatStyle = FlatStyle.Flat;
            Tovar.FlatStyle = FlatStyle.Flat;
            button_groupTovar.FlatStyle = FlatStyle.Flat;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatStyle = FlatStyle.Flat;
            button6.FlatStyle = FlatStyle.Flat;
        }
    }
}
