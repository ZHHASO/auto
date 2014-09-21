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

    public partial class Form2 : Form
    {
        private int Index = 0;
        private int inf = 0;
        private string Picture;
        public List<string> list = new List<string>();
        public List<string> lists = new List<string>();
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Componets cp,int Index)
        {
            inf = 1;
            InitializeComponent();
            textBox1.Text = cp.Name;
            comboBox1.Text = cp.Type;
            comboBox2.Text = cp.Type;
            textBox2.Text = cp.Pieces.ToString();
            textBox3.Text = cp.Location.ToString();
            richTextBox1.Text = cp.Information;
            button1.Text = "修改";
            this.Index = Index;
        }

         

        private void button1_Click(object sender, EventArgs e)
        {
            int num;
            ushort loc;
            try
            {
                loc = ushort.Parse(textBox3.Text);
                num = int.Parse(textBox2.Text);
                Componets cp = new Componets(textBox1.Text, comboBox1.Text, comboBox2.Text, num, richTextBox1.Text,loc,Picture);
                this.Close();
                if (inf == 0)
                {
                    Form1.al.Add(cp);
                }
                else
                {
                    Form1.al.Insert(Index,cp);
                }
            }
            catch
            {
                MessageBox.Show("不要输入错误数据"); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "图片文件|*.jpg;*.png;*.ico;*.gif;*.bmp";
            op.FilterIndex=1;
            op.InitialDirectory="c:\\";
            op.ShowDialog();
            Picture = op.FileName;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach(string str in list)
            {
                comboBox1.Items.Add(str);
            }
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            foreach (string str in lists)
            {
                comboBox2.Items.Add(str);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
