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
    public partial class Form2 : Form
    {
        public static Form1 form1;

        public static String[] arrayComboAdd;
        public static String[] arrayComboRemove;

        public Form2()
        {
            InitializeComponent();

            updateArrayCombo();
            timer1.Start();

            dateText.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        private bool checkText()
        {
            bool enabled = false;
            if (string.IsNullOrEmpty(dateText.Text.Trim()) || string.IsNullOrEmpty(amountText.Text.Trim()))
            {
                enabled = false;
            }
            else
            {
                enabled = true;
            }

            return enabled;
        }

        private bool checkCombos()
        {
            bool enabled = false;
            if (string.IsNullOrEmpty(addRemoveCombo.Text.Trim()) || string.IsNullOrEmpty(typeCombo.Text.Trim()) || string.IsNullOrEmpty(cashBankCombo.Text.Trim()))
            {
                enabled = false;
            }
            else
            {
                enabled = true;
            }

            return enabled;
        }

        private bool ifEn()
        {
            bool enabled = false;

            if (!string.IsNullOrEmpty(addText.Text.Trim()) && (addRemoveCombo.Text.Equals("Ienākumi") || addRemoveCombo.Text.Equals("Izdevumi")))
            {
                if (!addText.Text.Equals("Iznākošie aizdevumi") && !addText.Text.Equals("Ienākošie aizdevumi"))
                {
                    enabled = true;
                }
            }

            return enabled;
        }

        private void checkAddCombo()
        {
            if (ifEn())
            {
                String value = addText.Text;
                bool ifContains = false;

                String[] array = (addRemoveCombo.Text.Equals("Ienākumi") ? arrayComboAdd : arrayComboRemove);

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Equals(value))
                    {
                        ifContains = true;
                    }
                }

                if (!ifContains)
                {
                    addButton1.Enabled = true;
                }
                else
                {
                    addButton1.Enabled = false;
                }
            }
            else
            {
                addButton1.Enabled = false;
            }
        }

        private void updateArrayCombo()
        {
            arrayComboAdd = readFile(Form1.basePath + @"\optionsAdd").Split('\n');
            arrayComboRemove = readFile(Form1.basePath + @"\optionsRemove").Split('\n');
        }

        private void updateComboBox()
        {
            if (addRemoveCombo.Text.Equals("Ienākumi"))
            {
                typeCombo.Items.Clear();
                for (int i = 0; i < arrayComboAdd.Length; i++)
                {
                    typeCombo.Items.Add(arrayComboAdd[i]);
                }
            }
            else if (addRemoveCombo.Text.Equals("Izdevumi"))
            {
                typeCombo.Items.Clear();
                for (int i = 0; i < arrayComboRemove.Length; i++)
                {
                    typeCombo.Items.Add(arrayComboRemove[i]);
                }
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

        

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            String year = DateTime.Parse(dateText.Text).Year.ToString();
            String month = DateTime.Parse(dateText.Text).Month.ToString();

            Directory.CreateDirectory(Form1.storagePath + @"\" + year + @"\" + month);
            form1.createFile(Form1.storagePath + @"\" + year + @"\" + month + @"\" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-FFF"), dateText.Text + "\n" + (addRemoveCombo.Text.Equals("Izdevumi") ? "-" : "") + amountText.Text + "\n" + typeCombo.Text + "\n" + commentsText.Text + "\n" + cashBankCombo.Text + "\n" + addRemoveCombo.Text);
            Close();
            form1.refreshDataGridView();
        }

        private void addRemoveCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateComboBox();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            checkAddCombo();

            if (checkCombos() && checkText())
            {
                addButton.Enabled = true;
            }
            else
            {
                addButton.Enabled = false;
            }
        }

        private void addButton1_Click(object sender, EventArgs e)
        {
            String typeText = (addRemoveCombo.Text.Equals("Ienākumi") ? "Add" : "Remove");
            String path = Form1.basePath + @"\options" + typeText;
            form1.createFile(path, form1.readFile(path) + "\n" + addText.Text);
            updateArrayCombo();
            updateComboBox();
            addText.Text = "";
        }
    }
}
