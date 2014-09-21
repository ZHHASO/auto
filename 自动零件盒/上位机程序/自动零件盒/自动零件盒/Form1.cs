using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动零件盒
{
    public partial class Form1 : Form
    {
        public static List<Componets> al = new List<Componets>();
        private List<Componets> temp;
        public static List<ushort> Boxes = new List<ushort>();
        private string FileNames;
        public Form1()
        {
            InitializeComponent();
        }

        private void 库ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (al.Count == 0)
            {
                toolStripLabel1.Text = "不能保存空文件";
                return;
            }
            SaveFileDialog sf = new SaveFileDialog();
            sf.InitialDirectory = "C:\\";
            sf.Filter = "所有文件|*.*|零件盒专用文件|*.LJH";
            sf.FilterIndex = 2;
            if (sf.ShowDialog() == DialogResult.OK)
            {
                FileNames = sf.FileName;
                FileStream fs = new FileStream(sf.FileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, Form1.al);
                fs.Flush();
                fs.Close();
            }
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.InitialDirectory = "C:\\";
            of.Filter = "所有文件|*.*|零件盒专用文件|*.LJH";
            of.FilterIndex = 2;
            if (of.ShowDialog() == DialogResult.OK)
            {
                FileNames = of.FileName;
                FileStream fs = new FileStream(of.FileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                al = (List<Componets>)bf.Deserialize(fs);
                fs.Flush();
                fs.Close();
                temp = al;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = temp;
            }
            textBox1_TextChanged(sender, e);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            foreach (string str in listBox1.Items)
            {
                f2.list.Add(str);
            }
            foreach (string str in listBox2.Items)
            {
                f2.lists.Add(str);
            }
            f2.list.RemoveAt(0);
            f2.lists.RemoveAt(0);
            f2.ShowDialog();
            textBox1_TextChanged(sender, e);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = temp;
        }
        public void RefreshListBox()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.Items.Add("全部");
            listBox2.Items.Add("全部");
            foreach (Componets cp in temp)
            {
                listBox1.Items.Add(cp.Type);
            }
            foreach (Componets cp in temp)
            {
                listBox2.Items.Add(cp.Typesmall);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            temp = al;
            if (checkBox1.Checked)
            {
                dataGridView1.DataSource = null;
                temp = temp.FindAll(P => P.Name == textBox1.Text);
                if (listBox1.SelectedIndex != 0)
                {
                    try { temp = temp.FindAll(P => P.Type == listBox1.SelectedItem.ToString()); }
                    catch { }
                }
                if (listBox2.SelectedIndex != 0)
                {
                    try { temp = temp.FindAll(P => P.Typesmall == listBox2.SelectedItem.ToString()); }
                    catch { }
                }
                dataGridView1.DataSource = temp;
            }
            else
            {
                dataGridView1.DataSource = null;
                temp = temp.FindAll(P => P.Name.Contains(textBox1.Text));
                if (listBox1.SelectedIndex != 0)
                {
                    try { temp = temp.FindAll(P => P.Type == listBox1.SelectedItem.ToString()); }
                    catch { }
                }
                if (listBox2.SelectedIndex != 0)
                {
                    try { temp = temp.FindAll(P => P.Typesmall == listBox2.SelectedItem.ToString()); }
                    catch { }
                }
                dataGridView1.DataSource = temp;
            }
            RefreshListBox();
        }

        private void 保存文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (al.Count == 0)
            {
                toolStripLabel1.Text = "不能保存空文件";
                return;
            }
            if (FileNames == null)
            { 新建文件ToolStripMenuItem_Click(sender, e); }
            else
            {
                FileStream fs = new FileStream(FileNames, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, Form1.al);
                fs.Flush();
                fs.Close();
            }

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            textBox1_TextChanged(sender, e);
        }

        private void 扫描接口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            { toolStripLabel1.Text = "端口已经连接，不可以再次连接"; }
            else
            {
                byte i = 1;
                serialPort1.BaudRate = 9600;
                for (; i < 30; i++)
                {
                    try
                    {
                        serialPort1.PortName = ("COM" + i.ToString());
                        serialPort1.Open();
                        serialPort1.WriteLine("K");
                        Thread.Sleep(100);
                        if (serialPort1.BytesToRead != 0)
                        {
                            if (serialPort1.ReadByte() == 'K')
                            {
                                break;
                            }
                            else { continue; }

                        }
                        else { continue; }
                    }
                    catch { }
                }
                if (i < 29)
                {
                    toolStripLabel1.Text = "打开端口成功，端口是" + serialPort1.PortName;
                }
                else
                {
                    toolStripLabel1.Text = "打开端口失败，请检查端口是否被其他程序占用或没有连接到电脑，或设备损坏";
                    try
                    {
                        serialPort1.Close();
                    }
                    catch { }
                }
            }
        }

        private void 断开接口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                    toolStripLabel1.Text = "端口关闭成功";
                }
                catch
                {
                    toolStripLabel1.Text = "出错了，请重新拔掉端口再插上";
                }
            }
            else { toolStripLabel1.Text = "端口已经关闭了，不可以关闭了"; }
        }

        private void 编辑硬件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            新建文件ToolStripMenuItem_Click(sender, e);
        }

        private void 弹出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen)
            {
                if (dataGridView1.SelectedRows.Count != 1)
                {
                    toolStripLabel1.Text = "没有正确选中行";
                    return;
                }

                foreach (ushort sf in Boxes)
                {
                    if (ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) == sf)
                    {
                        toolStripLabel1.Text = "这个箱子已经弹出了，不可以再次弹出";
                        return;
                    }
                }


                byte high = (byte)(ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) / 256);
                byte low = (byte)(ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) % 256);
                try
                {
                    serialPort1.Write(new byte[] { 0x7f, 0x80, high, low }, 0, 4);
                    Thread.Sleep(100);
                    if (serialPort1.BytesToRead == 0)
                    { throw (new Exception()); }
                    if(serialPort1.ReadByte() == 0xf3)
                    {
                        toolStripLabel1.Text = "请插入电源转换器";
                        return;
                    }
                    Boxes.Add(ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()));
                }
                catch { toolStripLabel1.Text = "出错了！请拔掉端口连线并重新接上"; }
            }
            else { toolStripLabel1.Text = "串口未打开，不可以操作"; }
        }

        private void 提示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Index = 0;
            foreach (Componets cp in al)
            {
                Index++;
                if (dataGridView1.SelectedRows[0].Cells[1].Value.ToString() == cp.Name)
                {
                    al.RemoveAt(Index - 1);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = temp;
                    break;
                }
            }
            textBox1_TextChanged(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (MessageBox.Show("要保存文件吗？", "保存", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes: 保存文件ToolStripMenuItem_Click(sender, e); break;
                case DialogResult.No: break;
                case DialogResult.Cancel: e.Cancel = true; break;
            }
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Index = 0;

            foreach (Componets cp in al)
            {
                Index++;
                if (dataGridView1.SelectedRows[0].Cells[1].Value.ToString() == cp.Name)
                {
                    al.RemoveAt(Index - 1);
                    Form2 f2 = new Form2(cp, dataGridView1.SelectedRows[0].Index);
                    foreach (string str in listBox1.Items)
                    {
                        f2.list.Add(str);
                    }
                    foreach (string str in listBox2.Items)
                    {
                        f2.lists.Add(str);
                    }
                    f2.list.RemoveAt(0);
                    f2.lists.RemoveAt(0);
                    f2.ShowDialog();
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = temp;
                    break;
                }
            }
            textBox1_TextChanged(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            打开文件ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            保存文件ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            添加ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            删除ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            编辑ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            扫描接口ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            断开接口ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            弹出ToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            提示ToolStripMenuItem_Click(sender, e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(sender, e);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                richTextBox1.Text = "以下是数据手册，来源于http://www.alldatasheetcn.com/\n";
                try
                {
                    Image a = Image.FromFile((string)dataGridView1.SelectedRows[0].Cells[6].Value);
                    pictureBox1.Image = a;
                }
                catch { }
                Uri ui = new Uri("http://www.alldatasheetcn.com/view.jsp?Searchword=" + (string)dataGridView1.SelectedRows[0].Cells[1].Value);
                string html = ui.ToString();
                string strHTML = "";
                WebClient myWebClient = new WebClient();
                Stream myStream = myWebClient.OpenRead(ui);
                StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                strHTML = sr.ReadToEnd();
                myStream.Close();
                string[] Htm = strHTML.Split(new string[] { "<tr  bgcolor=" }, System.StringSplitOptions.RemoveEmptyEntries);
                byte i = 0;
                foreach (string st in Htm)
                {
                    i++;
                    if (i == 1)
                    { continue; }
                    string[] tm = st.Split(new string[] { "\" >" }, System.StringSplitOptions.RemoveEmptyEntries);
                    string[] ss = tm[0].Split(new string[] { "<b>" }, System.StringSplitOptions.RemoveEmptyEntries);
                    string[] ghj = ss[1].Split(new string[] { "</b>" }, System.StringSplitOptions.RemoveEmptyEntries);
                    richTextBox1.Text += ghj[0] + " : \n";
                    string[] ssk = tm[0].Split(new string[] { "target =\"_blank\" href=\"" }, System.StringSplitOptions.RemoveEmptyEntries);
                    string[] ghjh = ssk[1].Split(new string[] { "\">" }, System.StringSplitOptions.RemoveEmptyEntries);
                    richTextBox1.Text += ghjh[0] + "数据手册(pdf资料) \n";
                    foreach (string ts in tm)
                    {
                        string[] tmd = ts.Split(new string[] { "</a>" }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (tmd[0].Length < 15)
                        {
                            richTextBox1.Text += tmd[0] + " ";
                        }
                    }
                    richTextBox1.Text += "\n\n";
                }
            }
            catch { richTextBox1.Text = "未搜索到，此零件不存在于www.alldatasheetcn.com中或网络连接问题"; }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            收回ToolStripMenuItem_Click(sender, e);
        }

        private void 收回ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            if (serialPort1.IsOpen)
            {
                if (dataGridView1.SelectedRows.Count != 1)
                {
                    toolStripLabel1.Text = "没有正确选中行";
                    return;
                }
                bool Yes = false;
                foreach (ushort sf in Boxes)
                {
                    if (ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) == sf)
                    {
                        Yes = true;
                    }
                }
                if (!Yes)
                {
                    toolStripLabel1.Text = "已经缩回了，不可以再次缩回";;
                    return;
                }
                byte high = (byte)(ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) / 256);
                byte low = (byte)(ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()) % 256);
                try
                {
                    serialPort1.Write(new byte[] { 0x7f, 0x81, high, low }, 0, 4);
                    Thread.Sleep(100);
                    if (serialPort1.BytesToRead == 0)
                    { throw (new Exception()); }
                    if (serialPort1.ReadByte() == 0xf3)
                    {
                        toolStripLabel1.Text = "请插入电源转换器";
                        return;
                    }
                    if (Boxes.Count != 0)
                    {
                        ushort now = Boxes.Find(s => s == ushort.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()));
                        Boxes.Remove(now);

                    }
                }
                catch { toolStripLabel1.Text = "出错了！请拔掉端口连线并重新接上"; }
            }
            else
            {
                toolStripLabel1.Text = "串口未打开，不可以操作";
            }
        }

        private void 控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_TextChanged(object sender, EventArgs e)
        {
            var thread = new Thread(LblInit);
            thread.Start();
        }
        
        public void LblInit()
        {
            Thread.Sleep(3000);
            toolStripLabel1.Text = "就绪";
        }
    }
}
