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

namespace Proje1
{
    public partial class Form4 : Form
    {
        //müzik ekleme
        
        SoundPlayer splayer2 = new SoundPlayer();


        // Geçmiş skorları tutacak liste
        private List<int> skorlar = new List<int>();

        public Form4()
        {
            InitializeComponent();
            
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

            MoveP(pictureBox1);
            MoveP(pictureBox2);
            MoveP(pictureBox3);
            MoveP(pictureBox4);
            MoveP(pictureBox5);
            MoveP(pictureBox6);
            MoveL(pictureBox7);
            MoveL(pictureBox8);
            MoveL(pictureBox9);
            MoveL(pictureBox10);
            MoveL(pictureBox11);
            pictureBox7.SendToBack();
            pictureBox8.SendToBack();
            pictureBox9.SendToBack();
            pictureBox10.SendToBack();
            pictureBox11.SendToBack();

        }
        private void MoveL(PictureBox lane)
        {
            // Ağaçların aşağı hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // Ekranın altına ulaştıysa, yukarı al
            }
            else
            {
                lane.Top += 7; // Ağaçlar aşağı kaydırılıyor
            }
        }

        public void SetSkor(int skor)
        {
            label2.Text = "Skor: " + skor.ToString(); // Skoru label2'e yazdır
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            splayer2.Stop();
            this.Hide();
            form3.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
