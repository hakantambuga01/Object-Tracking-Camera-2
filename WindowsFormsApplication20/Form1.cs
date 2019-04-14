using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.IO.Ports;
namespace WindowsFormsApplication20
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            comboBox2.DataSource = SerialPort.GetPortNames();
           
           
            toolStripStatusLabel1.Text = "";

            //int portsayisi = 0;

           // portsayisi = comboBox2.Items.Count;

        }
        int sayac;
        int R, G, B, yatayX, dikeyY;
        Graphics g;
        int mode;
        Bitmap video;
        Bitmap video2;
        private FilterInfoCollection CaptureDevice;//capture device isminde tanımladığımız değişken bilgisayara kaç kamera bağlıysa onları tutan bir dizi. 
        private VideoCaptureDevice CıkısVideo;//cıkısvideo ise bizim kullanacağımız aygıt.
        private void Form1_Load(object sender, EventArgs e)
        {

            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);// capture device dizisine mevcut kameraları dolduruyoruz.
            foreach (FilterInfo Device in CaptureDevice)
            {
                comboBox1.Items.Add(Device.Name);// kameraları combobox a dolduruyoruz.


            }

            comboBox1.SelectedIndex = 0;
            CıkısVideo = new VideoCaptureDevice();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            CıkısVideo = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            CıkısVideo.DesiredFrameRate = 100;//görüntü kalitesi için
            CıkısVideo.NewFrame += CıkısVideo_NewFrame;
            CıkısVideo.Start();
        }

        void CıkısVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            video = (Bitmap)eventArgs.Frame.Clone();  //aldığımız görüntüyü pictureBox1 a atarak görüntüyü alıyoruz. 
            Bitmap video2 = (Bitmap)eventArgs.Frame.Clone();






            switch (mode)
            {
                case 2:
                    {

                        g = Graphics.FromImage(video2);//Değişiklik için grafik nesnesi oluşturduk
                        g.DrawString(sayac.ToString(), new Font("Arial", 100), new SolidBrush(Color.Black), new PointF(2, 2));
                        g.Dispose();

                    }
                    break;

                case 1:
                    {


                        EuclideanColorFiltering filter = new EuclideanColorFiltering();

                        filter.CenterColor = new RGB(215, 30, 30);
                        filter.Radius = 100;
                        filter.ApplyInPlace(video2);

                        //blob filtre
                        BlobCounter blob = new BlobCounter();
                        blob.MinHeight = 200;
                        blob.MinWidth = 200;
                        blob.ObjectsOrder = ObjectsOrder.Size;
                        blob.ProcessImage(video2);
                        Rectangle[] rects = blob.GetObjectsRectangles();
                        if (rects.Length > 0)
                        {
                            Rectangle obje = rects[0];
                            Graphics g = Graphics.FromImage(video2);
                            using (Pen pen = new Pen(Color.White, 3))
                            {
                                g.DrawRectangle(pen, obje);
                            }
                            yatayX = obje.X;
                            dikeyY = obje.Y;
                            //string gelenVeri;
                            g = Graphics.FromImage(video2);
                            if (yatayX >= 0 && 150 > yatayX && dikeyY >= 0 && dikeyY < 150)
                            {
                                g.DrawString("1.bölge" + yatayX.ToString() + "X" + dikeyY.ToString() + "Y", new Font("Italic", 20), Brushes.White, new PointF(yatayX, dikeyY));
                                //gelenVeri = "1";
                                serialPort1.Write("1");
                            }
                            else if (yatayX >= 150 && yatayX < 300 && dikeyY >= 0 && dikeyY < 150)
                            {

                                g.DrawString("2.bölge" + yatayX.ToString() + "X" + dikeyY.ToString() + "Y", new Font("Italic", 20), Brushes.White, new PointF(yatayX, dikeyY));
                                serialPort1.Write("2");
                                //gelenVeri = "2";

                            }

                            else if (yatayX >= 300 && yatayX < 450 && dikeyY >= 0 && dikeyY < 150)
                            {
                                g.DrawString("3.bölge" + yatayX.ToString() + "X" + dikeyY.ToString() + "Y", new Font("Italic", 20), Brushes.White, new PointF(yatayX, dikeyY));
                                serialPort1.Write("3");
                                //gelenVeri = "3";
                            }

                            else if (yatayX >= 0 && yatayX < 150 && dikeyY >= 150 && 300 >= dikeyY)
                            {
                                g.DrawString("4.bölge" + yatayX.ToString() + "X" + dikeyY.ToString() + "Y", new Font("Italic", 20), Brushes.White, new PointF(yatayX, dikeyY));
                                serialPort1.Write("4");
                                //gelenVeri = "4";

                            }
                            else if (yatayX >= 150 && yatayX < 300 && dikeyY >= 150 && 300 >= dikeyY)
                            {
                                g.DrawString("5.bölge" + yatayX.ToString() + "X" + dikeyY.ToString() + "Y", new Font("Italic", 20), Brushes.White, new PointF(yatayX, dikeyY));
                                serialPort1.Write("5");
                                //gelenVeri = "5";
                            }
                            else if (yatayX >= 300 && 450 >= yatayX && dikeyY >= 150 && 300 >= dikeyY)
                            {
                                g.DrawString("6.bölge" + yatayX.ToString() + "X" + dikeyY.ToString() + "Y", new Font("Italic", 20), Brushes.White, new PointF(yatayX, dikeyY));
                                serialPort1.Write("6");
                                //gelenVeri = "6";

                            }

                            g.Dispose();
                        }
                        pictureBox2.Image = video2;

                    }
                    break;
            }


            pictureBox1.Image = video;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CıkısVideo.IsRunning == true)
            {
                CıkısVideo.Stop();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
            {
                mode = 1;
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {

            serialPort1.PortName = "COM3"; 
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            if (serialPort1.IsOpen == true)
            {

                toolStripStatusLabel1.Text = serialPort1.PortName + "portuna bağlandı";
                button4.Enabled = true;

                button3.Enabled = false;

            }
            else
            {

                toolStripStatusLabel1.Text = "porta bağlanmadı kontrol et";

            }
        }




        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            R = trackBar1.Value;
            label1.Text = "R: " + trackBar1.Value;

        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            G = trackBar2.Value;
            label2.Text = "G: " + trackBar2.Value;
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {

            B = trackBar3.Value;
            label3.Text = "B: " + trackBar3.Value;
        }


    }
}
