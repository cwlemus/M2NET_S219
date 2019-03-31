using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M2NET_S219_CONEXIONDB
{
    public partial class Form1 : Form
    {
        DBHelper con;

        public Form1()
        {
            con = new DBHelper("LAPTOP-NIHGU3Q6", "net", "sql2018", "Northwind");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = con.mostrarClientes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Insertar();
        }
    }
}
