using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace Izdevumi
{
    public partial class Form1 : Form
    {
        public static String basePath = Environment.ExpandEnvironmentVariables("%AppData%") + @"\Ienākumu Katalogs 2018";
        public static String storagePath = basePath + @"\Krātuve";
        public static String currentYearMonth = storagePath + @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString();

        public static DateTime dateOpened;

        public Form1()
        {
            InitializeComponent();

            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CREATE BASE
            Directory.CreateDirectory(basePath);

            //CREATE STORAGE
            Directory.CreateDirectory(storagePath);

            //CREATE OPTIONS ADD FILE
            if (!File.Exists(basePath + @"\optionsAdd"))
            {
                createFile(basePath + @"\optionsAdd", "Ienākošie aizdevumi");
            }

            //CREATE OPTIONS REMOVE FILE
            if (!File.Exists(basePath + @"\optionsRemove"))
            {
                createFile(basePath + @"\optionsRemove", "Iznākošie aizdevumi");
            }

            //REMOVE GRID
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            //CREATE YEAR FOLDER & MONTH FOLDER
            Directory.CreateDirectory(currentYearMonth);
            //REFRESH DATAGRIDVIEW
            refreshDataGridView();

            //ADD LABEL DATE
            CultureInfo ci = new CultureInfo("lv");
            label1.Text = DateTime.Now.ToString("yyyy") + ". gada " + DateTime.Now.ToString("MMMM", ci) + ".";

            //SET DATE OPENED
            dateOpened = DateTime.Now;

            //OPTIONS BUTTON
            optionsButton.BackgroundImageLayout = ImageLayout.None;
        }

        public void refreshDataGridView()
        {
            dataGridView1.Rows.Clear();

            DirectoryInfo d = new DirectoryInfo(currentYearMonth);
            FileInfo[] Files = d.GetFiles();

            for (int i = 0; i < Files.Length; i++)
            {
                String[] array = readFile(Files[i].FullName).Split('\n');
                dataGridView1.Rows.Add(array[0], Double.Parse(array[1].Replace('.', ',')), array[2], array[3], Files[i].Name);
                if (Double.Parse(array[1].Replace('.', ',')) < 0)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Style.ForeColor = Color.Red;
                }
            }

            //SORT
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }

        public void createFile(String path, String text)
        {
            using (var tw = new StreamWriter(path, false))
            {
                tw.WriteLine(text);
            }
        }

        public String readFile(String path)
        {
            String text = "";

            using (StreamReader sr = File.OpenText(path))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    text += s + "\n";
                }
            }

            return text.TrimEnd('\r', '\n');
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            Form2.form1 = this;
            form2.ShowDialog();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Vai Jūs tiešām vēlaties izņemt šo?                           ", "Noņemt", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                String date = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                String year = DateTime.Parse(date).Year.ToString();
                String month = DateTime.Parse(date).Month.ToString();
                String path = storagePath + @"\" + year + @"\" + month + @"\" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value.ToString();

                File.Delete(path);
                refreshDataGridView();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                removeButton.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
            }
        }

        private void calendarButton_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            Form3.form1 = this;
            form3.ShowDialog();
        }

        private void viewStatsButton_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            Form4.form1 = this;
            form4.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(basePath);
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            Form6.form1 = this;
            form6.ShowDialog();
        }
    }
}
