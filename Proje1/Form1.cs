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
        //m�zik ekleme
        SoundPlayer splayer = new SoundPlayer();
        SoundPlayer s2player = new SoundPlayer();

        private int maxX; // Araban�n sa�daki en uzak konumu
        private int minX; // Araban�n soldaki en uzak konumu
        private List<PictureBox> grenades = new List<PictureBox>(); // Grenade'leri takip i�in liste
        private Random xRandom = new Random();
        private int skor = 0; // Skor de�i�keni

        public Form1()
        {
            InitializeComponent();
            
            // S�n�rlar� belirleme
            maxX = panel2.Left - pictureBox7.Width + 63; // Arac�n sa�daki s�n�r�, ye�il panelin ba�lang�c�na ayarland�
            minX = 76; // Arac�n soldaki s�n�r�, sol panelin sonu

            // Zamanlay�c� ayarlar�
            timer1.Interval = 50; // Hareket i�in ana zamanlay�c�
            timer1.Tick += timer1_Tick;
            timer1.Start();

            timer2.Interval = 1500; // Her saniyede bir araba olu�turma
            timer2.Tick += timer2_Tick;
            timer2.Start();

            // Timer3'� ayarlay�n
            timer3.Interval = 300; // Timer her 1 saniyede bir �al��acak
            timer3.Tick += timer3_Tick; // Timer3'�n Tick olay�n� tan�mlad�k
            timer3.Start(); // Timer3'� ba�latt�k

            // G�rselleri y�kleme
            LoadImages();

            // Paint event ekleme (hitbox g�rselle�tirme i�in)
            this.Paint += Form1_Paint;
        }

        private void LoadImages()
        {
            // A�a� ve araba g�rsellerini y�kleme
            string treePath = @"C:\\Users\\CASPER\\Downloads\\agac1.ico";
            


            pictureBox1.Image = Image.FromFile(treePath);
            pictureBox2.Image = Image.FromFile(treePath);
            pictureBox3.Image = Image.FromFile(treePath);
            pictureBox4.Image = Image.FromFile(treePath);
            pictureBox5.Image = Image.FromFile(treePath);
            pictureBox6.Image = Image.FromFile(treePath);


            // G�rsellerin ayarlanmas�
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
            // �eritleri ve a�a�lar� hareket ettirme
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

            // �arp��ma kontrol�
            CheckCollision();

            // Hitbox g�rselle�tirme i�in repaint
            this.Invalidate();
            
            pictureBox7.BringToFront();


        }

        private void MoveGrenades()
        {
            foreach (var grenade in grenades.ToList())
            {
                grenade.Top += 6; // Grenade a�a�� do�ru hareket eder

                if (grenade.Top >= this.ClientSize.Height) // E�er ekran d���na ��karsa
                {
                    this.Controls.Remove(grenade); // Formdan kald�r
                    grenades.Remove(grenade); // Listeden ��kar
                }
            }
            pictureBox7.BringToFront();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // �eritlerin X koordinatlar� (d�rt yol i�in e�it aral�klar)
            int[] lanePositions =
            {
                panel1.Right + 0,
                serit3.Left + 20,
                serit6.Left + 20,
                serit9.Left + 30,
                panel2.Left - 76
            };

            // Random bir �eritten grenade se�
            int laneIndex = xRandom.Next(0, lanePositions.Length);
            int grenadeX = lanePositions[laneIndex];

            PictureBox grenade = new PictureBox
            {
                Image = Image.FromFile(@"C:\\Users\\CASPER\\Downloads\\redcar.ico"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Gray,
                Size = new Size(70, 80),
                Location = new Point(grenadeX, -50) // Grenade ba�lang�c� ekran�n �st�nden ba�l�yor
            };

            Console.WriteLine($"Yeni grenade olu�turuldu: {grenade.Location}");

            grenades.Add(grenade); // Listeye ekle
            this.Controls.Add(grenade); // Form �zerine ekle

        }

        private void MoveLane(Panel lane)
        {
            // �eridin a�a�� hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // �erit ekran�n alt�na ula�t�ysa, yukar� al
            }
            else
            {
                lane.Top += 7; // �erit a�a�� kayd�r�l�yor
            }
        }

        private void MoveP(PictureBox lane)
        {
            // A�a�lar�n a�a�� hareketi
            if (lane.Top >= this.Height)
            {
                lane.Top = -lane.Height; // Ekran�n alt�na ula�t�ysa, yukar� al
            }
            else
            {
                lane.Top += 7; // A�a�lar a�a�� kayd�r�l�yor
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Araban�n mevcut konumunu al
            int currentX = pictureBox7.Location.X;
            int carWidth = pictureBox7.Width;

            // Sa� ve sol s�n�rlar� tan�mla
            int leftBoundary = minX;
            int rightBoundary = maxX;

            // Sol ok tu�una bas�l�rsa ve araba sol s�n�r� a�m�yorsa
            if (e.KeyCode == Keys.Left && currentX > leftBoundary)
            {
                pictureBox7.Location = new Point(currentX - 5, pictureBox7.Location.Y);
            }
            // Sa� ok tu�una bas�l�rsa ve araba sa� s�n�r� a�m�yorsa
            else if (e.KeyCode == Keys.Right && currentX + carWidth < rightBoundary)
            {
                pictureBox7.Location = new Point(currentX + 5, pictureBox7.Location.Y);
            }
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Oyuncunun arabas� hitbox'�n� �iz
            Rectangle playerCar = new Rectangle(
                pictureBox7.Left,
                pictureBox7.Top,
                pictureBox7.Width,
                pictureBox7.Height
            );

            e.Graphics.DrawRectangle(Pens.Gray, playerCar);

            // Grenade hitbox'lar�n� �iz
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
            // Oyuncunun arabas� hitbox'�
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



                // �arp��ma kontrol�
                if (playerCar.IntersectsWith(grenadeBounds)) // �arp��ma kontrol�
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
                pictureBox7.Image = Image.FromFile(imagePath); // Resmi y�kle
                pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage; // G�r�nt�y� ayarla
            }
            else
            {
                MessageBox.Show("Ge�ersiz resim yolu!");
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // Skoru art�r
            skor += 1;

            // Skoru label1'de g�ster
            label1.Text = "Skor: " + skor.ToString();
        }
    }
}

