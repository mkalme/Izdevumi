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
    public partial class Form6 : Form
    {
        public static Form1 form1;

        public static List<List<String>> listAll = new List<List<String>>();
        public static int type = 0;
        public static int selectNum = 0;
        public static bool ifSelectedCombo = false;

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            timer1.Start();

            dataGridView1.ClearSelection();
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

            setListAll();
        }

        public void refresh_dataGridView(int type1) {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < listAll[type1].Count; i++) {
                dataGridView1.Rows.Add(listAll[type1][i], i);
            }

            type = type1;
            if (selectNum > -1) {
                dataGridView1.Rows[selectNum].Selected = true;
            }
        }

        public void setListAll() {
            String pathAdd = Form1.basePath + @"/optionsAdd";
            String pathRemove = Form1.basePath + @"/optionsRemove";

            listAll.Clear();

            listAll.Add(new List<String> { });
            listAll.Add(new List<String> { });

            String[] array = readFile(pathAdd).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < array.Length; i++) {
                listAll[0].Add(array[i]);
            }

            array = readFile(pathRemove).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < array.Length; i++)
            {
                listAll[1].Add(array[i]);
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            rowCount();

            newButton.Enabled = ifSelectedCombo;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                checkTextBoxes(newButton, newTextBox);
                removeButtonEnabled();
                upEnabled();
                downEnabled();
                checkRename();
            }
        }

        private void rowCount() {
            if (dataGridView1.SelectedRows.Count == 0) {
                renameButton.Enabled = false;
                removeButton.Enabled = false;
                upButton.Enabled = false;
                downButton.Enabled = false;
            }
        }

        private void checkRename() {
            String text = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString();
            bool ifDebt = text.Equals("Ienākošie aizdevumi", StringComparison.CurrentCultureIgnoreCase) || text.Equals("Iznākošie aizdevumi", StringComparison.CurrentCultureIgnoreCase);
            if (!ifDebt)
            {
                checkTextBoxes(renameButton, renameTextBox);
            }
            else {
                renameButton.Enabled = false;
            }
        }

        private void checkTextBoxes(Button button, TextBox textBox) {
            if (!string.IsNullOrEmpty(comboBox1.Text.Trim()))
            {
                bool ifDebt = textBox.Text.Equals("Ienākošie aizdevumi", StringComparison.CurrentCultureIgnoreCase) || textBox.Text.Equals("Iznākošie aizdevumi", StringComparison.CurrentCultureIgnoreCase);
                if (!string.IsNullOrEmpty(textBox.Text.Trim()) && !ifDebt)
                {
                    bool ifExists = false;
                    for (int i = 0; i < listAll[type].Count; i++)
                    {
                        bool ifStatement = listAll[type][i].Equals(textBox.Text, StringComparison.CurrentCultureIgnoreCase);
                        if (ifStatement)
                        {
                            ifExists = true;
                        }
                    }

                    button.Enabled = !ifExists;
                }
                else
                {
                    button.Enabled = false;
                }
            }
            else
            {
                button.Enabled = false;
            }
        }

        private void upEnabled() {
            if (dataGridView1.SelectedRows[0].Index > 0)
            {
                upButton.Enabled = true;
            }
            else {
                upButton.Enabled = false;
            }
        }

        private void downEnabled()
        {
            if (dataGridView1.SelectedRows[0].Index + 1 < dataGridView1.RowCount)
            {
                downButton.Enabled = true;
            }
            else
            {
                downButton.Enabled = false;
            }
        }

        private void removeButtonEnabled() {
            String text = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[0].Value.ToString();
            if (!text.Equals("Ienākošie aizdevumi") && !text.Equals("Iznākošie aizdevumi"))
            {
                removeButton.Enabled = true;
            }
            else {
                removeButton.Enabled = false;
            }
            selectNum = -1;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            String type0 = "";
            String type1 = "";

            for (int i = 0; i < listAll[0].Count; i++) {
                type0 += listAll[0][i] + "\n";
            }

            for (int i = 0; i < listAll[1].Count; i++)
            {
                type1 += listAll[1][i] + "\n";
            }

            form1.createFile(Form1.basePath + @"\optionsAdd", type0.TrimEnd('\r', '\n'));
            form1.createFile(Form1.basePath + @"\optionsRemove", type1.TrimEnd('\r', '\n'));

            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int type = comboBox1.Text.Equals("Izdevumi") ? 1 : 0;

            refresh_dataGridView(type);

            ifSelectedCombo = true;
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Vai Jūs tiešām vēlaties izņemt šo?                           ", "Noņemt", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                listAll[type].RemoveAt(Int32.Parse(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString()));
                refresh_dataGridView(type);
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            int num1 = Int32.Parse(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString());
            int num2 = Int32.Parse(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index - 1].Cells[1].Value.ToString());

            String temp1 = listAll[type][num1];
            String temp2 = listAll[type][num2];

            listAll[type][num1] = temp2;
            listAll[type][num2] = temp1;

            selectNum = num2;

            refresh_dataGridView(type);
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            int num1 = Int32.Parse(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells[1].Value.ToString());
            int num2 = Int32.Parse(dataGridView1.Rows[dataGridView1.SelectedRows[0].Index + 1].Cells[1].Value.ToString());

            String temp1 = listAll[type][num1];
            String temp2 = listAll[type][num2];

            listAll[type][num1] = temp2;
            listAll[type][num2] = temp1;

            selectNum = num2;

            refresh_dataGridView(type);
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            listAll[type].Add(newTextBox.Text);
            newTextBox.Text = "";
            refresh_dataGridView(type);
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            listAll[type][dataGridView1.SelectedRows[0].Index] = renameTextBox.Text;
            selectNum = dataGridView1.SelectedRows[0].Index;

            renameTextBox.Text = "";
            refresh_dataGridView(type);
        }
    }
}
