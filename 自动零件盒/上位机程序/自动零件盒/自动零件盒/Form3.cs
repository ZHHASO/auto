using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动零件盒
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            listBox1.Items.Add("已经弹出的箱子");
            foreach(ushort df in Form1.Boxes)
            {
                listBox1.Items.Add("位置：" + df.ToString());
            }
        }

       
    }
}
