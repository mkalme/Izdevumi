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

namespace Izdevumi
{
    public partial class Form3 : Form
    {
        public static Form1 form1;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            monthCalendar1.MinDate = minDate();
            monthCalendar1.MaxDate = maxDate();
            monthCalendar1.SetDate(Form1.dateOpened);
        }

        private DateTime minDate()
        {
            String date = "";

            String[] array = Directory.GetDirectories(Form1.storagePath);
            int[] arrayNames = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayNames[i] = Int32.Parse(Path.GetFileName(array[i]));
            }

            date = arrayNames.Min().ToString();

            array = Directory.GetDirectories(Form1.storagePath + @"\" + date);
            arrayNames = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayNames[i] = Int32.Parse(Path.GetFileName(array[i]));
            }

            date += "-" + arrayNames.Min().ToString() + "-1";

            return DateTime.Parse(date);
        }

        private DateTime maxDate()
        {
            String date = "";

            String[] array = Directory.GetDirectories(Form1.storagePath);
            int[] arrayNames = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayNames[i] = Int32.Parse(Path.GetFileName(array[i]));
            }

            date = arrayNames.Max().ToString();

            array = Directory.GetDirectories(Form1.storagePath + @"\" + date);
            arrayNames = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                arrayNames[i] = Int32.Parse(Path.GetFileName(array[i]));
            }

            String daysInMonth = DateTime.DaysInMonth(Int32.Parse(date), arrayNames.Max()).ToString();

            date += "-" + arrayNames.Max().ToString() + "-" + daysInMonth;

            return DateTime.Parse(date);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Parse(monthCalendar1.SelectionRange.Start.ToString());

            String year = dateTime.Year.ToString();
            String month = dateTime.Month.ToString();

            Directory.CreateDirectory(Form1.storagePath + @"\" + year + @"\" + month);

            Form1.currentYearMonth = Form1.storagePath + @"\" + year + @"\" + month;

            //ADD LABEL DATE
            CultureInfo ci = new CultureInfo("lv");
            form1.label1.Text = year + ". gada " + ci.DateTimeFormat.GetMonthName(Int32.Parse(month)) + ".";

            //ADD CURRENT OPENED DATE
            Form1.dateOpened = DateTime.Parse(year + "-" + month + "-1");

            form1.refreshDataGridView();

            Close();
        }
    }
}
