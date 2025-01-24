using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proje1
{
    public partial class Form3 : Form
    {
        //müzik ekleme
        SoundPlayer splayer = new SoundPlayer();
        public string SelectedImagePath { get; private set; }

        private Form1 form1; // Form1 referansı tutmak için bir değişken
        public Form3()
        {
            InitializeComponent();
            this.form1 = form1;

            timer1.Interval = 50; // Hareket için ana zamanlayıcı
            timer1.Tick += timer1_Tick;
            timer1.Start();

            // Görselleri yükleme
            LoadImages();
            
        }

        private void LoadImages()
        {
            // Ağaç ve araba görsellerini yükleme
            string treePath = @"C:\\Users\\CASPER\\Downloads\\agac1.ico";

            pictureBox1.Image = Image.FromFile(treePath);
            pictureBox2.Image = Image.FromFile(treePath);
            pictureBox3.Image = Image.FromFile(treePath);
            pictureBox4.Image = Image.FromFile(treePath);
            pictureBox5.Image = Image.FromFile(treePath);
            pictureBox6.Image = Image.FromFile(treePath);

            SetPictureBoxSizeMode(new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 });
        }
        private void SetPictureBoxSizeMode(PictureBox[] pictureBoxes)
        {
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void MoveLane(Panel lane)
        {
            // Şeridin aşağı hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // Şerit ekranın altına ulaştıysa, yukarı al
            }
            else
            {
                lane.Top += 5; // Şerit aşağı kaydırılıyor
            }
        }

        private void MoveP(PictureBox lane)
        {
            // Ağaçların aşağı hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // Ekranın altına ulaştıysa, yukarı al
            }
            else
            {
                lane.Top += 5; // Ağaçlar aşağı kaydırılıyor
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Şeritleri ve ağaçları hareket ettirme
            MoveLane(serit1);
            MoveLane(serit2);
            MoveLane(serit3);
            MoveLane(serit4);
            MoveLane(serit5);
            MoveLane(serit6);
            MoveLane(serit7);
            MoveLane(serit8);
            MoveLane(serit9);

            MoveP(pictureBox1);
            MoveP(pictureBox2);
            MoveP(pictureBox3);
            MoveP(pictureBox4);
            MoveP(pictureBox5);
            MoveP(pictureBox6);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxmodel.Text == string.Empty)
            {
                errorProvider1.SetError(comboBoxmodel, "profil seç");
            }
            else
            {
                errorProvider1.Clear();
                splayer.SoundLocation = @"C:\Users\CASPER\Downloads\basgaza.wav";
                splayer.Play();

                // Form1'i açarken Form3 referansını gönderiyoruz
                Form1 form1 = new Form1();
                form1.UpdatePictureBox7(SelectedImagePath); // Resmi aktarıyoruz
                this.Hide(); // Form3'ü gizle
                form1.Show(); // Form1'i göster
            }
        }

        private void comboBoxmodel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxmodel.Text == string.Empty)
            {
                errorProvider1.SetError(comboBoxmodel, "lütfen profil resmi seçiniz.");
            }
            else
            {
                string basePath = @"C:\\Users\\CASPER\\Downloads\\arabamodelleri\\";
                string fileName = comboBoxmodel.Text.ToLower() + ".ico";
                string fullPath = System.IO.Path.Combine(basePath, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    combopictureBox.Image = Image.FromFile(fullPath);
                    SelectedImagePath = fullPath;
                }
                else
                {
                    errorProvider1.SetError(comboBoxmodel, "Seçilen dosya mevcut değil.");
                }

            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBoxmodel.Items.Clear();
            comboBoxmodel.Items.Add("trix");
            comboBoxmodel.Items.Add("tixer");
            comboBoxmodel.Items.Add("skywe");
            comboBoxmodel.Items.Add("cham");
            comboBoxmodel.Items.Add("blacker");
            comboBoxmodel.Items.Add("gojo");
            comboBoxmodel.Items.Add("maqque");
            comboBoxmodel.Items.Add("tejo");
            comboBoxmodel.Items.Add("kloye");
            comboBoxmodel.Items.Add("razes");
        }

        public void StopSound()
        {
            if (splayer != null)
            {
                splayer.Stop(); // Ses durduruluyor
            }
        }

    }

}
