﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Zakharchenko_IKM621b_3_project
{
    public partial class Form1 : Form
    {
        private bool Mode; // Режим дозволу / заборони введення даних
        private SaveFileDialog sf;
        private MajorWork MajorObject;
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        Timer timer;
        public Form1()
        {
            InitializeComponent();
            MajorObject = new MajorWork();
            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Текущие дата и время:";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();
            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel);
            statusStrip1.Items.Add(timeLabel);
            timer = new Timer() { Interval = 1000 };
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }
        private void tClock_Tick(object sender, EventArgs e)
        {
            tClock.Stop();
            MessageBox.Show("Минуло 25 секунд", "Увага");// Виведення повідомлення "Минуло 25 секунд" на екран
            tClock.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MajorObject = new MajorWork();
            MajorObject.SetTime();
            MajorObject.Modify = false; // заборона запису
            this.Mode = true;
            About A = new About();
            A.tAbout.Start();
            A.ShowDialog();
            toolTip1.SetToolTip(bSearch, "Натисніть на кнопку для пошуку"); 
            toolTip1.IsBalloon = true;
        }
        private void bStart_Click(object sender, EventArgs e)
        {
            if (Mode)
            {
                tbInput.Enabled = true;// Режим дозволу введення tbInput.Focus();
                tClock.Start();
                bStart.Text = "Стоп"; // зміна тексту на кнопці на "Стоп"
                this.Mode = false;
                пускToolStripMenuItem.Text = "Стоп";
            }
            else
            {
                tbInput.Enabled = false;// Режим заборони введення
                tClock.Stop();
                bStart.Text = "Пуск";// зміна тексту на кнопці на "Пуск"
                this.Mode = true;
                MajorObject.Write(tbInput.Text);// Запис даних у об'єкт
                MajorObject.Task();// Обробка даних
                label1.Text = MajorObject.Read();// Відображення результату
                пускToolStripMenuItem.Text = "Старт";
            }
        }
        private void tbInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            tClock.Stop();
            tClock.Start();
            if ((e.KeyChar >= '0') & (e.KeyChar <= '9') | (e.KeyChar == (char)8))
            {
                return;
            }
            else
            {
                tClock.Stop();
                MessageBox.Show("Неправильний символ", "Помилка");
                tClock.Start();
                e.KeyChar = (char)0;
            }

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            string s;
            s = (System.DateTime.Now - MajorObject.GetTime()).ToString();
            MessageBox.Show(s, "Час роботи програми"); // Виведення часу роботи програми і повідомлення "Час роботи програми" на екран
        }
        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About A = new About();
            A.progressBar1.Hide();
            A.ShowDialog();
        }
        private void відкритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdOpen.ShowDialog() == DialogResult.OK) // Виклик діалогового вікна відкриття файлу
            {
                MajorObject.WriteOpenFileName(ofdOpen.FileName); // відкриття файлу
                MajorObject.ReadFromFile(dgwOpen); // читання даних з файлу
            }
        }
        private void проНакопичувачіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] disks = System.IO.Directory.GetLogicalDrives(); // Строковий масив з логічніх дисків
            string disk = "";
            for (int i = 0; i < disks.Length; i++)
            {
                try
                {
                    System.IO.DriveInfo D = new System.IO.DriveInfo(disks[i]);
                    disk += D.Name + "-" + Math.Round(float.Parse(D.TotalSize.ToString()) * 0.000000001) + " Gb " + "- " + Math.Round(float.Parse(D.TotalFreeSpace.ToString()) * 0.000000001) + " Gb " + (char)13;// змінній присвоюється ім’я диска, загальна кількість місця и вільне місце на диску
                }
                catch
                {
                    disk += disks[i] + "- не готовий" + (char)13; // якщо пристрій не готовий, то виведення на екран ім’я пристрою і повідомлення «не готовий»
                }
            }
            MessageBox.Show(disk, "Накопичувачі");
        }
        private void зберегтиЯкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSave.ShowDialog() == DialogResult.OK) // Виклик діалогу збереження файлу
            {
                MajorObject.WriteSaveFileName(sfdSave.FileName); // Запис імені файлу для збереження
                MajorObject.Generator();
                MajorObject.SaveToFile(); // метод збереження в файл
            }
        }
        private void зберегтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MajorObject.SaveFileNameExists()) // задане ім’я файлу існує?
                MajorObject.SaveToFile(); // зберегти дані в файл
            else
                зберегтиЯкToolStripMenuItem_Click(sender, e);
        }
        private void новийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MajorObject.NewRec();
            tbInput.Clear();// очистити вміст тексту
            label1.Text = "";
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
                if (MajorObject.Modify)
                    if (MessageBox.Show("Дані не були збережені. Продовжити вихід?", "УВАГА", MessageBoxButtons.YesNo) == DialogResult.No)
                        e.Cancel = true; // припинити закриття
        }
        private void bSearch_Click(object sender, EventArgs e)
        {
            MajorObject.Find(tbSearch.Text); //пошук
        }
        private void Push_Click(object sender, EventArgs e)
        {
            MajorObject.myStack.Push(Stacktb.Text);
            MajorObject.myArr[MajorObject.myArr.Length - MajorObject.myStack.Count] = Stacktb.Text;
            LabelStack.Text = "";
            for (int i = 0; i < MajorObject.myArr.Length; i++)
            {
                if (MajorObject.myArr[i] != null)
                {
                    LabelStack.Text += MajorObject.myArr[i] + (char)13;
                }
                else
                {
                    continue;
                }
            }

        }
        private void Pop_Click(object sender, EventArgs e)
        {
            if (MajorObject.myStack.Count == 0)
                MessageBox.Show("\nСтек пустий!");
            else
            {
                MajorObject.myArr[MajorObject.myArr.Length - MajorObject.myStack.Count] = null;
                if (MajorObject.myStack.Count > 0)
                {
                    MessageBox.Show("Pop " + MajorObject.myStack.Pop());
                }
                LabelStack.Text = "";
                for (int i = 0; i < MajorObject.myArr.Length; i++)
                {
                    if (MajorObject.myArr[i] != null)
                    {
                        LabelStack.Text += MajorObject.myArr[i] + (char)13;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (MajorObject.myStack.Count == 0)
                    MessageBox.Show("\nСтек пустий!");
            }
        }
        private void Enqueue_Click(object sender, EventArgs e)
        {
            MajorObject.myQueue.Enqueue(Queuetb.Text);
            MajorObject.smyQueue[MajorObject.myQueue.Count - 1] = Queuetb.Text;
            LabelQueue.Text = "";
            for (int i = 0; i < MajorObject.smyQueue.Length; i++)
            {
                if (MajorObject.smyQueue[i] != null)
                {
                    LabelQueue.Text += MajorObject.smyQueue[i] + (char)13;
                }
                else
                {
                    continue;
                }
            }
        }
        private void Peek_q_Click(object sender, EventArgs e)
        {
            if (MajorObject.myQueue.Count > 0)
            {
                MessageBox.Show("Peek " + MajorObject.myQueue.Peek());
            }
            if (MajorObject.myQueue.Count == 0)
                MessageBox.Show("\nЧерга порожня!");

        }
        private void Dequeue_Click(object sender, EventArgs e)
        {
            if (MajorObject.myQueue.Count == 0)
                MessageBox.Show("\nЧерга порожня!");
            else
            {
                MajorObject.smyQueue[0] = null;
                // Зрушення елементів вліво на 1 позицію
                for (int i = 0; i < MajorObject.smyQueue.Length - 1; i++)
                {
                    MajorObject.smyQueue[i] = MajorObject.smyQueue[i + 1];
                }
                // Витяг елемента з черги
                if (MajorObject.myQueue.Count > 0)
                {
                    MessageBox.Show("Dequeue " + MajorObject.myQueue.Dequeue());
                }
                // Формування текста для виведення на екран
                LabelQueue.Text = "";
                for (int i = 0; i < MajorObject.smyQueue.Length - 1; i++)
                {
                    if (MajorObject.smyQueue[i] != null)
                    {
                        LabelQueue.Text += MajorObject.smyQueue[i] + (char)13;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (MajorObject.myQueue.Count == 0)
                    MessageBox.Show("\nОчередь пустая!");
            }
        }
        private void зберегтиЯкToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = @"Текстовий файл (*.txt)|*.txt|Текстові файли TXT(*.txt)|*.txt|CSV-файл (*.csv)|*.csv|Bin-файл (*.bin)|*.bin";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                MajorObject.WriteSaveTextFileName(sf.FileName);
                MajorObject.SaveToTextFile(sf.FileName, dgwOpen);
            }
        }
        private void зберегтиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MajorObject.SaveTextFileNameExists())
                MajorObject.SaveToTextFile(MajorObject.ReadSaveTextFileName(), dgwOpen);
            else
                зберегтиЯкToolStripMenuItem1_Click(sender, e);
        }
        private void відкритиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = @"Текстовий файл (*.txt)|*.txt|Текстовий файл TXT(*.txt)|*.txt|CSV-файл (*.csv)|*.csv|Bin-файл (*.bin)|*.bin";
            if (o.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(o.FileName, Encoding.Default);
            }

        }
    }
}
