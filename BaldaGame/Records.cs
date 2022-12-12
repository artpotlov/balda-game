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
    public partial class Records : Form
    {
        public Records()
        {
            InitializeComponent();
        }

        private void Records_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Имя игрока", listView1.Width / 2, HorizontalAlignment.Left);
            listView1.Columns.Add("Колличество очков", listView1.Width / 2 - 21, HorizontalAlignment.Center);
            List<String> rec = new List<String>();
            rec.AddRange(File.ReadAllLines("records.txt"));
            int count = 0;
            for (int i = 0; i < rec.Count; i++)
            {
                int ind = rec[i].IndexOf("{}");
                int res;
                if (ind != -1)
                {
                    if (int.TryParse(rec[i].Substring(ind + 2), out res))
                    {
                        listView1.Items.Add(rec[i].Substring(0, ind));
                        listView1.Items[count].SubItems.Add(res.ToString());
                        count += 1;
                    }
                }
            }
            for (int i = 0; i < listView1.Items.Count;i++ )
                for (int j = 0; j < listView1.Items.Count - 1; j++)
                {
                    String tmpItem;
                    String tmpSubItem;
                    if (Convert.ToInt32(listView1.Items[j].SubItems[1].Text) < Convert.ToInt32(listView1.Items[j + 1].SubItems[1].Text))
                    {
                        tmpItem = listView1.Items[j].Text;
                        tmpSubItem = listView1.Items[j].SubItems[1].Text;
                        listView1.Items[j].Text = listView1.Items[j + 1].Text;
                        listView1.Items[j].SubItems[1].Text = listView1.Items[j + 1].SubItems[1].Text;
                        listView1.Items[j + 1].Text = tmpItem;
                        listView1.Items[j + 1].SubItems[1].Text = tmpSubItem;
                    }
                }
    
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = listView1.Width / 2;
            listView1.Columns[1].Width = listView1.Width / 2 - 21;
            listView1.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView1.Columns[1].TextAlign = HorizontalAlignment.Center;
        }
    }
}
