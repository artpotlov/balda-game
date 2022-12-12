using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BaldaGame
{
    public partial class Helper : Form
    {
        public Helper()
        {
            InitializeComponent();
        }

        private void Helper_Load(object sender, EventArgs e)
        {
            if (File.Exists("Help.rtf"))
            {
                richTextBox1.LoadFile("Help.rtf", RichTextBoxStreamType.RichText);
            }
            else
            {
                MessageBox.Show("Не найден файл справки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
