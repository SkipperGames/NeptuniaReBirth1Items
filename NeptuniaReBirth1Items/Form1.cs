using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NeptuniaReBirth1Items
{
    public partial class Form1 : Form
    {
        static readonly int NUM_BYTES = 0x2EE0;

        static OpenFileDialog openFileDialog = new OpenFileDialog();

        string[] lines;
        int i, j;
        short id;
        byte[] temp = new byte[2];

        byte[] tempdata;
        Dictionary<int, byte[]> inventory = new Dictionary<int, byte[]>();
        byte[] output = new byte[NUM_BYTES];

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                lines = File.ReadAllLines(
                    Application.StartupPath + "\\data.txt");

                tempdata = File.ReadAllBytes(Application.StartupPath + "\\asdf0.CEM");
            }
            catch (Exception)
            {
                MessageBox.Show("Missing some files", "!");
                return;
            }

            richTextBox1.Text = "";

            for (i = 0; i < lines.Length; i++)
            {
                if (!short.TryParse(lines[i], out id))
                    continue;

                temp = BitConverter.GetBytes(id);

                inventory.Add(
                    id,
                    new byte[4]
                    {
                        temp[0],
                        temp[1],
                        0x63,
                        0x00,
                    });
            }

            for (i = 0; i < tempdata.Length - 4; i += 4)
            {
                j = (tempdata[i]) + (tempdata[i + 1] << 8);

                if (!inventory.ContainsKey(j))
                {
                    inventory.Add(
                        j,
                        new byte[4]
                    {
                        tempdata[i],
                        tempdata[i + 1],
                        0x63,
                        0x00,
                    });
                }
            }

            output = inventory.Values.SelectMany(a => a).ToArray();

            richTextBox1.Text =
                BitConverter.ToString(output, 0)
                .Replace("-", "")
                .Replace("00000000", "")
                .Replace("00006300", "");
            richTextBox1.SelectAll();
            richTextBox1.Copy();

            Close();
        }
    }
}
