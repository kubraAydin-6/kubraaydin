using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace Proje1
{
    public partial class Form1 : Form
    {
        //müzik ekleme
        SoundPlayer splayer = new SoundPlayer();
        SoundPlayer s2player = new SoundPlayer();

        private int maxX; // Arabanýn saðdaki en uzak konumu
        private int minX; // Arabanýn soldaki en uzak konumu
        private List<PictureBox> grenades = new List<PictureBox>(); // Grenade'leri takip için liste
        private Random xRandom = new Random();
        private int skor = 0; // Skor deðiþkeni

        public Form1()
        {
            InitializeComponent();
            
            // Sýnýrlarý belirleme
            maxX = panel2.Left - pictureBox7.Width + 63; // Aracýn saðdaki sýnýrý, yeþil panelin baþlangýcýna ayarlandý
            minX = 76; // Aracýn soldaki sýnýrý, sol panelin sonu

            // Zamanlayýcý ayarlarý
            timer1.Interval = 50; // Hareket için ana zamanlayýcý
            timer1.Tick += timer1_Tick;
            timer1.Start();

            timer2.Interval = 1500; // Her saniyede bir araba oluþturma
            timer2.Tick += timer2_Tick;
            timer2.Start();

            // Timer3'ü ayarlayýn
            timer3.Interval = 300; // Timer her 1 saniyede bir çalýþacak
            timer3.Tick += timer3_Tick; // Timer3'ün Tick olayýný tanýmladýk
            timer3.Start(); // Timer3'ü baþlattýk

            // Görselleri yükleme
            LoadImages();

            // Paint event ekleme (hitbox görselleþtirme için)
            this.Paint += Form1_Paint;
        }

        private void LoadImages()
        {
            // Aðaç ve araba görsellerini yükleme
            string treePath = @"C:\\Users\\CASPER\\Downloads\\agac1.ico";
            


            pictureBox1.Image = Image.FromFile(treePath);
            pictureBox2.Image = Image.FromFile(treePath);
            pictureBox3.Image = Image.FromFile(treePath);
            pictureBox4.Image = Image.FromFile(treePath);
            pictureBox5.Image = Image.FromFile(treePath);
            pictureBox6.Image = Image.FromFile(treePath);


            // Görsellerin ayarlanmasý
            SetPictureBoxSizeMode(new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7 });
        }

        private void SetPictureBoxSizeMode(PictureBox[] pictureBoxes)
        {
            foreach (var pictureBox in pictureBoxes)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Þeritleri ve aðaçlarý hareket ettirme
            MoveLane(serit1);
            MoveLane(serit2);
            MoveLane(serit3);
            MoveLane(serit4);
            MoveLane(serit5);
            MoveLane(serit6);
            MoveLane(serit7);
            MoveLane(serit8);
            MoveLane(serit9);

            serit1.SendToBack();
            serit2.SendToBack();
            serit3.SendToBack();
            serit4.SendToBack();
            serit5.SendToBack();
            serit6.SendToBack();
            serit7.SendToBack();
            serit8.SendToBack();
            serit9.SendToBack();


            MoveP(pictureBox1);
            MoveP(pictureBox2);
            MoveP(pictureBox3);
            MoveP(pictureBox4);
            MoveP(pictureBox5);
            MoveP(pictureBox6);

            
            // Grenade'leri hareket ettirme
            MoveGrenades();

            // Çarpýþma kontrolü
            CheckCollision();

            // Hitbox görselleþtirme için repaint
            this.Invalidate();
            
            pictureBox7.BringToFront();


        }

        private void MoveGrenades()
        {
            foreach (var grenade in grenades.ToList())
            {
                grenade.Top += 6; // Grenade aþaðý doðru hareket eder

                if (grenade.Top >= this.ClientSize.Height) // Eðer ekran dýþýna çýkarsa
                {
                    this.Controls.Remove(grenade); // Formdan kaldýr
                    grenades.Remove(grenade); // Listeden çýkar
                }
            }
            pictureBox7.BringToFront();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Þeritlerin X koordinatlarý (dört yol için eþit aralýklar)
            int[] lanePositions =
            {
                panel1.Right + 0,
                serit3.Left + 20,
                serit6.Left + 20,
                serit9.Left + 30,
                panel2.Left - 76
            };

            // Random bir þeritten grenade seç
            int laneIndex = xRandom.Next(0, lanePositions.Length);
            int grenadeX = lanePositions[laneIndex];

            PictureBox grenade = new PictureBox
            {
                Image = Image.FromFile(@"C:\\Users\\CASPER\\Downloads\\redcar.ico"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Gray,
                Size = new Size(70, 80),
                Location = new Point(grenadeX, -50) // Grenade baþlangýcý ekranýn üstünden baþlýyor
            };

            Console.WriteLine($"Yeni grenade oluþturuldu: {grenade.Location}");

            grenades.Add(grenade); // Listeye ekle
            this.Controls.Add(grenade); // Form üzerine ekle

        }

        private void MoveLane(Panel lane)
        {
            // Þeridin aþaðý hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // Þerit ekranýn altýna ulaþtýysa, yukarý al
            }
            else
            {
                lane.Top += 7; // Þerit aþaðý kaydýrýlýyor
            }
        }

        private void MoveP(PictureBox lane)
        {
            // Aðaçlarýn aþaðý hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // Ekranýn altýna ulaþtýysa, yukarý al
            }
            else
            {
                lane.Top += 7; // Aðaçlar aþaðý kaydýrýlýyor
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Arabanýn mevcut konumunu al
            int currentX = pictureBox7.Location.X;
            int carWidth = pictureBox7.Width;

            // Sað ve sol sýnýrlarý tanýmla
            int leftBoundary = minX;
            int rightBoundary = maxX;

            // Sol ok tuþuna basýlýrsa ve araba sol sýnýrý aþmýyorsa
            if (e.KeyCode == Keys.Left && currentX > leftBoundary)
            {
                pictureBox7.Location = new Point(currentX - 5, pictureBox7.Location.Y);
            }
            // Sað ok tuþuna basýlýrsa ve araba sað sýnýrý aþmýyorsa
            else if (e.KeyCode == Keys.Right && currentX + carWidth < rightBoundary)
            {
                pictureBox7.Location = new Point(currentX + 5, pictureBox7.Location.Y);
            }
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Oyuncunun arabasý hitbox'ýný çiz
            Rectangle playerCar = new Rectangle(
                pictureBox7.Left,
                pictureBox7.Top,
                pictureBox7.Width,
                pictureBox7.Height
            );

            e.Graphics.DrawRectangle(Pens.Gray, playerCar);

            // Grenade hitbox'larýný çiz
            foreach (var grenade in grenades)
            {
                Rectangle grenadeBounds = new Rectangle(
                    grenade.Left,
                    grenade.Top,
                    grenade.Width,
                    grenade.Height
                );

                e.Graphics.DrawRectangle(Pens.Gray, grenadeBounds);
            }
        }

        private void CheckCollision()
        {
            // Oyuncunun arabasý hitbox'ý
            Rectangle playerCar = new Rectangle(
                pictureBox7.Left,
                pictureBox7.Top,
                pictureBox7.Width,
                pictureBox7.Height
            );

            foreach (var grenade in grenades.ToList())
            {
                // Grenade'in konumu
                Rectangle grenadeBounds = new Rectangle(
                    grenade.Left,
                    grenade.Top,
                    grenade.Width,
                    grenade.Height
                );

                Console.WriteLine($"PlayerCar: {playerCar} | Grenade: {grenadeBounds} | Grenade.Top: {grenade.Top}");



                // Çarpýþma kontrolü
                if (playerCar.IntersectsWith(grenadeBounds)) // Çarpýþma kontrolü
                {
                    timer1.Stop();
                    timer2.Stop();

                    splayer.Stop();
                    s2player.SoundLocation = @"C:\Users\CASPER\Downloads\kazashort.wav";
                    s2player.Play();

                    Form4 form4 = new Form4();
                    form4.SetSkor(skor);
                    this.Hide();
                    form4.Show();
                }

            }
        }
        public void UpdatePictureBox7(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
            {
                pictureBox7.Image = Image.FromFile(imagePath); // Resmi yükle
                pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage; // Görüntüyü ayarla
            }
            else
            {
                MessageBox.Show("Geçersiz resim yolu!");
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // Skoru artýr
            skor += 1;

            // Skoru label1'de göster
            label1.Text = "Skor: " + skor.ToString();
        }
    }
}

