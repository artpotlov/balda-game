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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        Random rnd = new Random();
        private void butNewGame_Click(object sender, EventArgs e)
        {

            this.Hide();
            Settings st = new Settings();
            st.ShowDialog();
            st.Dispose();
            GameProcess gp = new GameProcess();
            gp.ShowDialog();
            gp.Dispose();
            this.Show();
            files_exist();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            files_exist();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AboutBox().ShowDialog();
            this.Show();
            files_exist();
        }

        private void butBestRec_Click(object sender, EventArgs e)
        {
            this.Hide();
            Records rec = new Records();
            rec.ShowDialog();
            rec.Dispose();
            this.Show();
            files_exist();
        }

        #region Функция проверки файлов
        private void files_exist()
        {
            if (!File.Exists("words.txt"))
            {
                MessageBox.Show("Не найден словарь \"words.txt\" :(", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            if (!File.Exists("records.txt"))
                butBestRec.Enabled = false;
            if (!File.Exists("Help.rtf"))
                butHelp.Enabled = false;
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.ForeColor = Color.FromArgb(255, rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        }

        private void butHelp_Click(object sender, EventArgs e)
        {
            this.Hide();
            Helper hlp = new Helper();
            hlp.ShowDialog();
            hlp.Dispose();
            this.Show();
            files_exist();
        }

    }
}
