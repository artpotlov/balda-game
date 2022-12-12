using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace BaldaGame
{

    public partial class GameProcess : Form
    {
        #region Определение переменных
        String str;
        static Random rnd = new Random(); 
        int numUser = rnd.Next(1, 3);  // Номер игрока, который должен ходить в данный момент
        int[] scor = new int[] { 0, 0 }; // Очки игроков
        int gameOver = 20; // Кол-во оставшихся ходов
        int InsOrAlloc = 0; // Метка определяет необходимость вставки буквы или выделения букв
        int rowIns = -1, colIns = -1; // Вспомогательные координаты для вставляемой буквы
        int rowAlloc = -1, colAlloc = -1; // Вспомогательные координаты для выделения букв
        int[] passCount = new int[] { 1, 1 };
        int sec = UserSettings.time; // 120 секунд до конца хода
        TimeSpan ms; // Для представления времени в нормальном виде
        List<String> lst = new List<String>(); // Список слов из словаря
        SoundPlayer player = new SoundPlayer();
        Color[] col = new Color[] { Color.FromArgb(255, 205, 220, 57), Color.FromArgb(255,244,67,54)}; // Цвета на поле
        #endregion


        public GameProcess()
        {
            InitializeComponent();
            initialize_table_game();
            show_us_now();
            
        }

        #region Инициализациия игрового поля
        private void initialize_table_game() 
        {
            panelAlphBut.Enabled = false;
            tableGame.DefaultCellStyle.SelectionBackColor = col[0];
            tableGame.ColumnCount = 5;
            tableGame.RowCount = 5;
            if (File.Exists("words.txt"))
            {
                lst.AddRange(File.ReadAllLines("words.txt"));
                    for (int i = 0; i < lst.Count; i++)
                        lst[i] = lst[i].ToUpper();
            }
            else
                {
                    MessageBox.Show("Не найден словарь \"words.txt\" :(", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(0);

                }
            if(UserSettings.centerWord == null)
               while (true)
               {
                  int ans = rnd.Next(0, lst.Count);
                  if (lst[ans].Length == 5)
                  {
                     str = lst[ans];
                     break;
                  }
               }
            else str = UserSettings.centerWord;
            for (int i = 0; i < 5; i++)
            {
                tableGame[i, 2].Value = str[i];
                tableGame.Rows[i].Height = tableGame.Height / 5;
                tableGame.Columns[i].Width = tableGame.Width / 5;

            }
            labelUs1.Text = UserSettings.us1;
            labelUs2.Text = UserSettings.us2;
            labelTime.Visible = false;
            labelWord.Text = null;
        }
        #endregion

        #region Функция, показывающая, кто ходит на данный момент
        private void show_us_now()
        {
            if (numUser == 1)
            {
                labWhoGo.Text = "Ходит " + UserSettings.us1;
                labelTime.TextAlign = ContentAlignment.MiddleRight;
                tableLayout.SetColumn(labelTime, 1);
                
            }
            else
            {
                labWhoGo.Text = "Ходит " + UserSettings.us2;
                tableLayout.SetColumn(labelTime,5);
                labelTime.TextAlign = ContentAlignment.MiddleLeft;
                
            }
        }
        #endregion

        #region Функция нажатия ячейки поля
        private void tableGame_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = tableGame.CurrentCell.RowIndex;
            int y = tableGame.CurrentCell.ColumnIndex;
            if (InsOrAlloc == 0 && tableGame.CurrentCell.Value == null)
                if (x > 0 && tableGame.Rows[x - 1].Cells[y].Value != null) panelAlphBut.Enabled = true;
                else if (x < 4 && tableGame.Rows[x + 1].Cells[y].Value != null) panelAlphBut.Enabled = true;
                else if (y > 0 && tableGame.Rows[x].Cells[y - 1].Value != null) panelAlphBut.Enabled = true;
                else if (y < 4 && tableGame.Rows[x].Cells[y + 1].Value != null) panelAlphBut.Enabled = true;
                else panelAlphBut.Enabled = false;
            else
            {
                panelAlphBut.Enabled = false;
                if (InsOrAlloc == 1 && tableGame.CurrentCell.Value != null)
                {
                    butOk.Enabled = true;
                    if (rowAlloc != -1 && colAlloc != -1)
                    {
                        if (((rowAlloc == x && (colAlloc == y + 1 || colAlloc == y - 1)) || (colAlloc == y && (rowAlloc == x + 1 || rowAlloc == x - 1))) && tableGame.CurrentCell.Style.BackColor != col[1])
                        {
                            rowAlloc = x;
                            colAlloc = y;
                            tableGame.DefaultCellStyle.SelectionBackColor = col[1];
                            tableGame.CurrentCell.Style.BackColor = col[1];
                            labelWord.Text += tableGame.CurrentCell.Value;
                        }
                    }
                    else
                    {
                        rowAlloc = x;
                        colAlloc = y;
                        tableGame.DefaultCellStyle.SelectionBackColor = col[1];
                        tableGame.CurrentCell.Style.BackColor = col[1];
                        labelWord.Text += tableGame.CurrentCell.Value;
                        tableGame.CurrentCell = null;
                    }
                }
            }
        }
        #endregion

        #region Функция нажатия алфавитной кнопки на экране
        private void butAlph_Click(object sender, EventArgs e)
        {
            Button _but = sender as Button;
            if (_but != null)
            {
                tableGame.CurrentCell.Value = _but.Text;
                InsOrAlloc = 1;
                rowIns = tableGame.CurrentCell.RowIndex;
                colIns = tableGame.CurrentCell.ColumnIndex;
                panelAlphBut.Enabled = false;
                butClear.Enabled = true;
                tableGame.ClearSelection();
            }
        }
        #endregion

        #region Функция сброса поля и вспомогательных переменных
        private void clear_default()
        {
            tableGame.DefaultCellStyle.SelectionBackColor = col[0];
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    tableGame.Rows[i].Cells[j].Style.BackColor = Color.White;
            InsOrAlloc = 0;
            labelWord.Text = null;
            rowAlloc = -1;
            colAlloc = -1;
            rowIns = -1;
            colIns = -1;
            butOk.Enabled = false;
            butClear.Enabled = false;
            tableGame.ClearSelection();
        }
        #endregion

        #region Нажатие на кнопку сброса
        private void butClear_Click(object sender, EventArgs e)
        {
            tableGame[colIns, rowIns].Value = null;
            clear_default();
        }
        #endregion

        #region Функция определения в какой лист записывать слово
        private void write_list()
        {
            if (numUser == 1)
            {
                scor[0] += labelWord.Text.Length;
                numUser = 2;
                listWordsUs1.Items.Add(labelWord.Text);
                labelUs1Scor.Text = "Очки: " + scor[0];
                show_us_now();
            }
            else
            {
                scor[1] += labelWord.Text.Length;
                numUser = 1;
                listWordsUs2.Items.Add(labelWord.Text);
                labelUs2Scor.Text = "Очки: " + scor[1];
                show_us_now();
            }
        }
        #endregion

        #region Нажатие на кнопку ОК
        private void butOk_Click(object sender, EventArgs e)
        {
            if (tableGame[colIns, rowIns].Style.BackColor != col[1])
            {
                MessageBox.Show("В вашем слове должна присутствовать новая буква!!!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(str == labelWord.Text)
            {
                MessageBox.Show("Данное слово используется в центре поля!!!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                 if (listWordsUs1.Items.Count>0 && listWordsUs1.FindItemWithText(labelWord.Text, false, 0, false) != null || listWordsUs2.Items.Count > 0 && listWordsUs2.FindItemWithText(labelWord.Text, false, 0, false) != null)
                {
                    MessageBox.Show("Это слово было уже использовано!!!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                bool fndMark = lst.Exists(x => x == labelWord.Text);
                if (fndMark)
                {
                    sec = UserSettings.time;
                    gameOver--;
                    write_list();//нужна функция для добавления слова в listview в зависимости от играющего игрока + подсчет очков и т.п.
                    clear_default();//в этой строке должен быть вызов функции сброса на значения по умолчанию
                    play(2);
                }
                else
                {
                    MessageBox.Show("Такого слова нет в словаре", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                tableGame.ClearSelection();
            if (gameOver == 0)
            {
                win_game();
            }
        }
        #endregion

        #region Функция определения выйгравшего игрока
        private void win_game()
        {
            timer.Stop();
            play(1);
            if (scor[0] > scor[1])
                MessageBox.Show("♕ Победил " + UserSettings.us1 + " ♛", "Игра закончена");
            else
            {
                if (scor[0] == scor[1]) MessageBox.Show("♕ Ничья ♛", "Игра закончена");
                else MessageBox.Show("♕ Победил " + UserSettings.us2 + " ♛", "Игра закончена");
            }
            
            File.AppendAllText("records.txt", UserSettings.us1 + "{}" + scor[0] + Environment.NewLine + UserSettings.us2+"{}"+scor[1] + Environment.NewLine);
            this.Close();
        }
        #endregion

        #region Функция "Сдаться"
        private void pass()
        {
            sec = UserSettings.time;
            if (colIns != -1) tableGame[colIns, rowIns].Value = null;
            clear_default();
            if (numUser == 1)
            {
                if (passCount[0] == 3)
                {
                    progressPassUs1.Value = progressPassUs1.Value - 1;
                    win_game();
                    this.Close();
                }
                else
                {
                    passCount[0]++;
                    progressPassUs1.Value = progressPassUs1.Value - 1;
                    numUser = 2;
                    show_us_now();
                }
            }
            else
            {
                if (passCount[1] == 3)
                {
                    progressPassUs2.Value = progressPassUs2.Value - 1;
                    win_game();
                    this.Close();
                }
                else
                {
                    progressPassUs2.Value = progressPassUs2.Value - 1;
                    passCount[1]++;
                    numUser = 1;
                    show_us_now();
                }
            }
        }
        #endregion

        #region Нажатие кнопки сдаться
        private void butPass_Click(object sender, EventArgs e)
        {
            pass();
        }
        #endregion

        #region Тик таймера
        private void timer_Tick(object sender, EventArgs e)
        {
            if (UserSettings.time != -1)
            {
                labelTime.Visible = true;
                sec--;
                if (sec <= 20) labelTime.ForeColor = Color.Red;
                else labelTime.ForeColor = Color.Black;
                if (sec == 20) play(3); 
                ms = TimeSpan.FromSeconds(sec);
                if (sec < 10) labelTime.Text = Convert.ToString(ms.Minutes + ":0" + ms.Seconds);
                else labelTime.Text = Convert.ToString(ms.Minutes + ":" + ms.Seconds);
                if (sec == 0) pass();
            }
            else timer.Stop();
        }
        #endregion

        #region Функция воспроизведения звука
        private void play(int numb)
        {
            if(numb == 1 && File.Exists("win.wav"))
            {
                player.SoundLocation = "win.wav";
                player.Play();
            }
            if(numb == 2 && File.Exists("hod.wav"))
            {
                player.SoundLocation = "hod.wav";
                player.Play();
            }
            if(numb == 3 && File.Exists("clock.wav"))
            {
                player.SoundLocation = "clock.wav";
                player.Play();
            }
        }
        #endregion

        #region Функция ресайза игрового поля
        private void tableGame_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                tableGame.Rows[i].Height = tableGame.Height / 5;
                tableGame.Columns[i].Width = tableGame.Width / 5;

            }    

        }
        #endregion


    }
    }
