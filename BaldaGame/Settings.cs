using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BaldaGame
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }
        private int dataChecking()
        {
            if (textBoxUs1.Text.Trim() != "") UserSettings.us1 = textBoxUs1.Text;
            else UserSettings.us1 = "Игрок 1";
            if (textBoxUs2.Text.Trim() != "") UserSettings.us2 = textBoxUs2.Text;
            else UserSettings.us2 = "Игрок 2";
            if (trackBar1.Value == 5)
                UserSettings.time = -1;
            else UserSettings.time = trackBar1.Value * 60;
            if (textBoxCW.Text.Length == 5)
                UserSettings.centerWord = textBoxCW.Text.ToUpper();
            else
            {
                if (textBoxCW.Text != "" && textBoxCW.Text != "не обязательно")
                {
                    MessageBox.Show("Слово должно состоять из пяти букв", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }
                else UserSettings.centerWord = null;
            }
            return 1;
        }
        private void butOK_Click(object sender, EventArgs e)
        {
            if (dataChecking() == 1) this.Close();
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataChecking() == 0) e.Cancel = true;
        }

        private void textBoxCW_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar >= (char)0x0410 && e.KeyChar <= (char)0x044F) return;
            if (e.KeyChar == (char)Keys.Back) return;
            e.Handled = true;
        }

        private void textBoxCW_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxCW.Text = null;
            textBoxCW.ForeColor = Color.Black;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            textBoxUs1.Select();
        }
    }
}
