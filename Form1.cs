using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Cvb;

namespace Sustraer_Imagenes
{
    public partial class Form1 : Form
    {
        Image<Gray, byte> img1;
        Image<Gray, byte> img2;
        Image<Gray, byte> img3;
        Image<Gray, byte> img4;

        Graphics papel;
        Pen pluma = new Pen(Color.DarkGreen);
        



        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog()==DialogResult.OK)
            {
                img1 = new Image<Gray, byte>(ofd.FileName);
                pictureBox1.Image = img1.ToBitmap();
            }

        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                img2 = new Image<Gray, byte>(ofd.FileName);
                pictureBox2.Image = img2.ToBitmap();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            img3 = new Image<Gray, byte>(img1.Width,img1.Height);
            


           for(int i=0;i<img1.Height;i++)            
           {
                for(int j=0;j<img2.Width;j++)
                {
                    /* if (img1[i, j].Intensity == img2[i, j].Intensity)
                     {
                         img3[i, j] = new Gray(0);
                     }else
                     {
                         img3[i, j] = new Gray(255);
                     }    */

                    img3[i, j] = new Gray(diferenciaPixel(img1[i,j].Intensity,img2[i,j].Intensity));
                    
                }
           }

            //Binarización
            img3=img3.ThresholdBinary(new Gray(10),new Gray(255));
           //Tratamiento
            img3 = img3.Erode(2);
            img3 = img3.Dilate(2);
            //pictureBox3.Image = img3.ToBitmap();
            pictureBox3.Image = pictureBox2.Image;
        }

        private double diferenciaPixel(double fondo,double frame)
        {
            double dif=0;
            dif = fondo - frame;
            if (dif < 0)
                dif = 0;
            if (dif > 255)
                dif = 255;
            return dif;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat mat = new Mat();
            
            img4 = img3.Resize(pictureBox3.Width, pictureBox3.Height, Emgu.CV.CvEnum.Inter.Linear);
            CvInvoke.FindContours(img4, contours, mat, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            
            
                for (int i = 0; i < contours.Size; i++)
                {

                    var area = CvInvoke.ContourArea(contours[i]);
                    if (area > (int)trackBar1.Value)
                    {
                        Rectangle rect = CvInvoke.BoundingRectangle(contours[i]);
                        papel = pictureBox3.CreateGraphics();
                        pluma.Width = 5;
                        pluma.Color = Color.DarkBlue;
                        papel.DrawRectangle(pluma, rect);
                    }

                }
            
             
        }
    }
}
