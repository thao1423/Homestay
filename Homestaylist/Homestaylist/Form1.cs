using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homestaylist
{
    public partial class Homestaylist : Form
    {
        public Homestaylist()
        {
            InitializeComponent();
        }
        void homestayown()
        {
            HomestayEntities db = new HomestayEntities();
            cbxHomestayown.DataSource = db.Homestayown.ToList();
            cbxHomestayown.ValueMember = "ID";
            cbxHomestayown.DisplayMember = "Name";
            cbxHomestayown.SelectedValue = 1;
        }
        void room(int ID)
        {
            HomestayEntities db = new HomestayEntities();
            grdListview.DataSource = db.Room.Where(b => b.Homestayown_ID == ID).ToList();
            grdListview.Columns["ID"].Visible = false;
            grdListview.Columns["Homestayown"].Visible = false;
            grdListview.Columns["Homestayown_ID"].Visible = false;
        }

        private void Homestay_Load(object sender, EventArgs e)
        {
            homestayown();
            ShowList();
        }
        private void ShowList()
        {
            int number = grdListview.RowCount;
            txtDem.Text = number.ToString();
        }

        private void cbxHomestayown_SelectedValueChanged(object sender, EventArgs e)
        {
            var temp = cbxHomestayown.SelectedValue;
            int ID = 0;
            int.TryParse(temp.ToString(), out ID);
            if (ID != 0)
            {
                room(ID);
                ShowList();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdListview.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Do you want to delete this?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var row = grdListview.SelectedRows[0];
                    var cell = row.Cells["ID"];
                    int ID = (int)cell.Value;
                    HomestayEntities db = new HomestayEntities();
                    Room bk = db.Room.Single(b => b.ID == ID);
                    db.Room.Remove(bk);
                    db.SaveChanges();
                    room((int)cbxHomestayown.SelectedValue);
                    ShowList();

                }
            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            this.homestayown();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Homestay fr = new Homestay();
            fr.ShowDialog();
            room((int)cbxHomestayown.SelectedValue);
            ShowList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (grdListview.SelectedRows.Count == 1)
            {
                var row = grdListview.SelectedRows[0];
                var cell = row.Cells["ID"];
                int ID = (int)cell.Value;
                Homestay edit = new Homestay(ID);
                edit.ShowDialog();
                room((int)cbxHomestayown.SelectedValue);
                ShowList();
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (grdListview.SelectedRows.Count == 1)
            {
                Room upload = (Room)grdListview.SelectedRows[0].DataBoundItem;
                // show picture with upload.Image data
                Details form = new Details(upload.Image);
                form.ShowDialog();
            }
        }
    }
}
