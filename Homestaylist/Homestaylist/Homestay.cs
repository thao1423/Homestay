using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homestaylist
{
    public partial class Homestay : Form
    {
        int roomID = 0;
        Room bkedit = new Room();
        HomestayEntities db = new HomestayEntities();

        public Homestay()
        {
            InitializeComponent();
            homestayown();
        }
        public Homestay(int ID)
        {
            InitializeComponent();
            roomID = ID;
            homestayown();
            room();
        }
        void homestayown()
        {
            HomestayEntities db = new HomestayEntities();
            cbxHomestayown.DataSource = db.Homestayown.ToList();
            cbxHomestayown.DisplayMember = "Name";
            cbxHomestayown.ValueMember = "ID";
        }
        void room()
        {
            bkedit = db.Room.Single(b => b.ID == roomID);
            txtRoom.Text = bkedit.Room_Name;
            txtStatus.Text = bkedit.Status;
            txtPrice.Text = bkedit.Price;
            txtSpace.Text = bkedit.Space;
            cbxHomestayown.SelectedValue = bkedit.Homestayown_ID;
            //this.pictureBox.Image = byteArrayToImage(bkedit.Image);
            if (bkedit.Image != null) { this.pictureBox.Image = byteArrayToImage(bkedit.Image); }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (roomID == 0)
            {
                try
                {
                    HomestayEntities db = new HomestayEntities();
                    Room bk = new Room();
                    bk.Room_Name = txtRoom.Text;
                    bk.Status = txtStatus.Text;
                    bk.Price = txtPrice.Text;
                    bk.Space = txtSpace.Text;
                    bk.Homestayown_ID = (int)cbxHomestayown.SelectedValue;
                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[])converter.ConvertTo(pictureBox.Image, typeof(byte[]));
                    Room upload = new Room();
                    upload.Image = image;
                    bk.Image = image;
                    db.Room.Add(bk);
                    db.SaveChanges();
                    MessageBox.Show("Add new product success");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    HomestayEntities db = new HomestayEntities();
                    bkedit = db.Room.Find(bkedit.ID);
                    bkedit.Room_Name = txtRoom.Text;
                    bkedit.Status = txtStatus.Text;
                    bkedit.Price = txtPrice.Text;
                    bkedit.Space = txtSpace.Text;
                    bkedit.Homestayown_ID = (int)cbxHomestayown.SelectedValue;

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[])converter.ConvertTo(pictureBox.Image, typeof(byte[]));
                    Room upload = new Room();
                    upload.Image = image;
                    bkedit.Image = image;
                    db.Entry(bkedit).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    MessageBox.Show("Edit product success");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (open.ShowDialog() == DialogResult.OK)
                txtImage.Text = open.FileName;
            pictureBox.ImageLocation = open.FileName;
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
