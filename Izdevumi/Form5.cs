using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Izdevumi
{
    public partial class Form5 : Form
    {
        public static List<List<String>> list;

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

            refresh_dataGridView();
        }

        private void refresh_dataGridView() {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < list.Count; i++) {
                dataGridView1.Rows.Add(list[i][0], Double.Parse(list[i][1].Replace(".", ",")), list[i][2]);
            }

            //SORT
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
            dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            //ADD TOTAL
            //countAll();
        }

        private void countAll() {
            double number = 0.0;

            for (int i = 0; i < list.Count; i++) {
                number += Double.Parse(list[i][1]);
            }

            dataGridView1.Rows.Add("Kopā", number, "");

            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Style.Font = new Font(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].InheritedStyle.Font, FontStyle.Bold);
            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Style.Font = new Font(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].InheritedStyle.Font, FontStyle.Bold);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
