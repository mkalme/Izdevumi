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
using System.Diagnostics;

namespace Izdevumi
{
    public partial class Form4 : Form
    {
        public static Form1 form1;
        public static List<List<String>> listAll = new List<List<String>>();

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            saveEverything();

            dataGridView1_refresh();
            dataGridView2_refresh();
            dataGridView3_refresh();
        }

        private void dataGridView1_refresh()
        {
            //BASE
            addRows(dataGridView1, "Ienākumi");

            //MUST
            dataGridView1.Rows.Add("Kopējie ienākumi", Double.Parse(total(dataGridView1)));

            boldRows(dataGridView1, false);
        }

        private void dataGridView2_refresh()
        {
            //BASE
            addRows(dataGridView2, "Izdevumi");

            //MUST
            dataGridView2.Rows.Add("Kopējie izdevumi", Double.Parse(total(dataGridView2)));

            boldRows(dataGridView2, false);
        }

        private void dataGridView3_refresh()
        {
            addRows(dataGridView3, "DebtRemove");
            addRows(dataGridView3, "DebtAdd");

            //MUST
            dataGridView3.Rows.Add("Kopējie aizdevumi", Double.Parse(total(dataGridView3)));
            dataGridView3.Rows.Add("Bruto peļņa", Double.Parse(netIncome()));

            boldRows(dataGridView3, true);
        }

        private String netIncome()
        {
            String totalIncome = dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value.ToString();
            String totalExpenses = dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value.ToString();

            if (totalIncome.Equals("-"))
            {
                totalIncome = "0";
            }

            if (totalExpenses.Equals("-"))
            {
                totalExpenses = "0";
            }

            return (Double.Parse(totalIncome) + Double.Parse(totalExpenses)).ToString();
        }

        private void boldRows(DataGridView dataGridView, bool two)
        {
            dataGridView.ClearSelection();
            dataGridView.DefaultCellStyle.SelectionForeColor = dataGridView.DefaultCellStyle.ForeColor;

            dataGridView.Rows[dataGridView.RowCount - 1].Cells[0].Style.Font = new Font(dataGridView.Rows[dataGridView.RowCount - 1].Cells[0].InheritedStyle.Font, FontStyle.Bold);
            dataGridView.Rows[dataGridView.RowCount - 1].Cells[1].Style.Font = new Font(dataGridView.Rows[dataGridView.RowCount - 1].Cells[1].InheritedStyle.Font, FontStyle.Bold);

            if (two)
            {
                dataGridView.Rows[dataGridView.RowCount - 2].Cells[0].Style.Font = new Font(dataGridView.Rows[dataGridView.RowCount - 2].Cells[0].InheritedStyle.Font, FontStyle.Bold);
                dataGridView.Rows[dataGridView.RowCount - 2].Cells[1].Style.Font = new Font(dataGridView.Rows[dataGridView.RowCount - 2].Cells[1].InheritedStyle.Font, FontStyle.Bold);
            }
        }

        public String[] getAllNames(String type)
        {
            String text = "";

            if (type.Equals("Ienākumi"))
            {
                text = readFile(Form1.basePath + @"\optionsAdd").Replace("Ienākošie aizdevumi", "");
            }
            else if (type.Equals("Izdevumi"))
            {
                text = readFile(Form1.basePath + @"\optionsRemove").Replace("Iznākošie aizdevumi", "");
            }
            else if (type.Equals("DebtAdd"))
            {
                text = "Ienākošie aizdevumi";
            }
            else
            {
                text = "Iznākošie aizdevumi";
            }

            return text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public String[] getAmount(String[] name, String type)
        {
            String text = "";

            for (int i = 0; i < name.Length; i++)
            {
                Double temp = 0;
                for (int b = 0; b < listAll.Count; b++)
                {
                    if (type.Equals(listAll[b][5]) && name[i].Equals(listAll[b][2]))
                    {
                        temp += Double.Parse(listAll[b][1].Replace('.', ','));
                    }
                }

                text += temp + "\n";
            }

            return text.TrimEnd('\r', '\n').Split('\n');
        }

        public void addRows(DataGridView dataGridView, String type)
        {
            String[] names = getAllNames(type);
            type = (type.Equals("DebtAdd") ? "Ienākumi" : (type.Equals("DebtRemove") ? "Izdevumi" : type));

            String[] amount = getAmount(names, type);

            for (int i = 0; i < names.Length; i++)
            {
                dataGridView.Rows.Add(names[i], Double.Parse(amount[i]));
            }
        }

        public String total(DataGridView dataGridView)
        {
            Double number = 0;

            String text = "";

            int rowCount = dataGridView.RowCount;
            if (rowCount > 0)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    number += Double.Parse(dataGridView.Rows[i].Cells[1].Value.ToString());
                }
                text = number.ToString();
            }
            else
            {
                text = "0";
            }

            return text;
        }

        public void saveEverything()
        {
            listAll.Clear();
            DirectoryInfo d = new DirectoryInfo(Form1.storagePath + @"\" + Form1.dateOpened.Year + @"\" + Form1.dateOpened.Month);
            FileInfo[] Files = d.GetFiles();

            for (int i = 0; i < Files.Length; i++)
            {
                String[] text = readFile(Files[i].FullName).Split('\n');

                listAll.Add(new List<String> { text[0], text[1], text[2], text[3], text[4], text[5] });
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

            return text;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((dataGridView1.CurrentCell.RowIndex + 1) != dataGridView1.RowCount) {
                getList(0, dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((dataGridView2.CurrentCell.RowIndex + 1) != dataGridView2.RowCount)
            {
                getList(1, dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (((dataGridView3.CurrentCell.RowIndex + 1) != dataGridView3.RowCount) && ((dataGridView3.CurrentCell.RowIndex + 2) != dataGridView3.RowCount))
            {
                String name = dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex].Cells[0].Value.ToString();
                int type = (name.Equals("Ienākošie aizdevumi") ? 0 : 1);

                getList(type, name);
            }
        }

        private void getList(int type, String name)
        {
            List<List<String>> listInfo = new List<List<String>>();

            String addRemove = (type == 0 ? "Ienākumi" : "Izdevumi");

            for (int i = 0; i < listAll.Count; i++)
            {
                if (listAll[i][2].Equals(name) && listAll[i][5].Equals(addRemove))
                {
                    listInfo.Add(new List<String> { listAll[i][0], listAll[i][1], listAll[i][3] });
                }
            }

            Form5 form5 = new Form5();
            Form5.list = listInfo;
            form5.ShowDialog();
        }
    }
}
