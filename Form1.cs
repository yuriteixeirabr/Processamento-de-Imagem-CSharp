using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;

namespace Trabalho
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap image = (Bitmap)ptbImage.Image;
            Bitmap image2 = (Bitmap)ptbImage2.Image;
            ptbTransformedImage.Image = or(image, image2);
            initializeHistogram((Bitmap)ptbImage.Image);
        }

        private Bitmap negativeImage(Bitmap image)
        {
            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    Color c = image.GetPixel(w, h);
                    c = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(w, h, c);
                }
            }
            return image;
        }

        private Bitmap thresholdImage(Bitmap image, int limiar)
        {
            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    Color p = image.GetPixel(w, h);
                    image.SetPixel(w, h, Color.FromArgb(p.R > limiar ? 255 : 0, p.G > limiar ? 255 : 0,
                        p.B > limiar ? 255 : 0));
                }
            }
            return image;
        }

        private Bitmap and(Bitmap image1, Bitmap image2)
        {
            Bitmap image = image1;
            bool[,] cR = new bool[image.Width, image.Height], cG = new bool[image1.Width, image.Height],
                cB = new bool[image1.Width, image.Height];

            bool[,] cR1 = new bool[image.Width, image.Height], cG1 = new bool[image1.Width, image.Height],
                cB1 = new bool[image1.Width, image.Height];

            for (int w = 0; w < image1.Width; w++)
            {
                for (int h = 0; h < image1.Height; h++)
                {
                    Color p = image1.GetPixel(w, h);
                    Color p1 = image2.GetPixel(w, h);

                    cR[w, h] = p.R == 0 ? false : true;
                    cR1[w, h] = p1.R == 0 ? false : true;

                    cG[w, h] = p.G == 0 ? false : true;
                    cG1[w, h] = p1.G == 0 ? false : true;

                    cB[w, h] = p.B == 0 ? false : true;
                    cB1[w, h] = p1.B == 0 ? false : true;
                }
            }

            for (int w = 0; w < image1.Width; w++)
            {
                for (int h = 0; h < image1.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(cR[w, h] && cR1[w, h] == true ? 255 : 0,
                        cG[w, h] && cG1[w, h] == true ? 255 : 0,
                        cB[w, h] && cB1[w, h] == true ? 255 : 0));
                }
            }

            return image;
        }

        private Bitmap or(Bitmap image1, Bitmap image2)
        {
            Bitmap image = image1;
            bool[,] cR = new bool[image.Width, image.Height], cG = new bool[image1.Width, image.Height],
                cB = new bool[image1.Width, image.Height];

            bool[,] cR1 = new bool[image.Width, image.Height], cG1 = new bool[image1.Width, image.Height],
                cB1 = new bool[image1.Width, image.Height];

            for (int w = 0; w < image1.Width; w++)
            {
                for (int h = 0; h < image1.Height; h++)
                {
                    Color p = image1.GetPixel(w, h);
                    Color p1 = image2.GetPixel(w, h);

                    cR[w, h] = p.R == 0 ? false : true;
                    cR1[w, h] = p1.R == 0 ? false : true;

                    cG[w, h] = p.G == 0 ? false : true;
                    cG1[w, h] = p1.G == 0 ? false : true;

                    cB[w, h] = p.B == 0 ? false : true;
                    cB1[w, h] = p1.B == 0 ? false : true;
                }
            }

            for (int w = 0; w < image1.Width; w++)
            {
                for (int h = 0; h < image1.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(cR[w, h] || cR1[w, h] == true ? 255 : 0,
                        cG[w, h] || cG1[w, h] == true ? 255 : 0,
                        cB[w, h] || cB1[w, h] == true ? 255 : 0));
                }
            }

            return image;
        }

        private Bitmap add(Bitmap image1, Bitmap image2)
        {
            Bitmap image = image1;
            int[,] cR = new int[image.Width, image.Height], cG = new int[image1.Width, image.Height],
                cB = new int[image1.Width, image.Height];

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    Color p = image1.GetPixel(w, h);
                    Color p1 = image2.GetPixel(w, h);
                    cR[w, h] = p.R + p1.R;
                    cG[w, h] = p.G + p1.G;
                    cB[w, h] = p.B + p1.B;
                }
            }

            int maxR = highestValue(cR, image);
            int maxG = highestValue(cG, image);
            int maxB = highestValue(cB, image);
            int minR = lowerValue(cR, image);
            int minG = lowerValue(cG, image);
            int minB = lowerValue(cB, image);

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    cR[w, h] = Escalonate(cR[w, h], maxR, minR);
                    cG[w, h] = Escalonate(cG[w, h], maxG, minG);
                    cB[w, h] = Escalonate(cB[w, h], maxB, minB);
                }
            }

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(cR[w, h], cG[w, h], cB[w, h]));
                }
            }
            return image;
        }

        private Bitmap subtract(Bitmap image1, Bitmap image2)
        {
            Bitmap image = image1;
            int[,] cR = new int[image.Width, image.Height], cG = new int[image1.Width, image.Height],
                cB = new int[image1.Width, image.Height];

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    Color p = image1.GetPixel(w, h);
                    Color p1 = image2.GetPixel(w, h);
                    cR[w, h] = p.R - p1.R;
                    cG[w, h] = p.G - p1.G;
                    cB[w, h] = p.B - p1.B;
                }
            }

            int maxR = highestValue(cR, image);
            int maxG = highestValue(cG, image);
            int maxB = highestValue(cB, image);
            int minR = lowerValue(cR, image);
            int minG = lowerValue(cG, image);
            int minB = lowerValue(cB, image);

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    cR[w, h] = Escalonate(cR[w, h], maxR, minR);
                    cG[w, h] = Escalonate(cG[w, h], maxG, minG);
                    cB[w, h] = Escalonate(cB[w, h], maxB, minB);
                }
            }

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(cR[w, h], cG[w, h], cB[w, h]));
                }
            }
            return image;
        }

        private Bitmap multiplication(Bitmap image1, Bitmap image2)
        {
            Bitmap image = image1;
            int[,] cR = new int[image.Width, image.Height], cG = new int[image1.Width, image.Height],
                cB = new int[image1.Width, image.Height];

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    Color p = image1.GetPixel(w, h);
                    Color p1 = image2.GetPixel(w, h);
                    cR[w, h] = p.R * p1.R;
                    cG[w, h] = p.G * p1.G;
                    cB[w, h] = p.B * p1.B;
                }
            }

            int maxR = highestValue(cR, image);
            int maxG = highestValue(cG, image);
            int maxB = highestValue(cB, image);
            int minR = lowerValue(cR, image);
            int minG = lowerValue(cG, image);
            int minB = lowerValue(cB, image);

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    cR[w, h] = Escalonate(cR[w, h], maxR, minR);
                    cG[w, h] = Escalonate(cG[w, h], maxG, minG);
                    cB[w, h] = Escalonate(cB[w, h], maxB, minB);
                }
            }

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(cR[w, h], cG[w, h], cB[w, h]));
                }
            }
            return image;
        }

        private Bitmap division(Bitmap image1, Bitmap image2)
        {
            Bitmap image = image1;
            int[,] cR = new int[image.Width, image.Height], cG = new int[image1.Width, image.Height],
                cB = new int[image1.Width, image.Height];

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    Color p = image1.GetPixel(w, h);
                    Color p1 = image2.GetPixel(w, h);

                    if (p.R > 0 && p1.R > 0)
                        cR[w, h] = p.R / p1.R;

                    if (p.G > 0 && p1.G > 0)
                        cG[w, h] = p.G / p1.G;

                    if (p.B > 0 && p1.B > 0)
                        cB[w, h] = p.B / p1.B;
                }
            }

            int maxR = highestValue(cR, image);
            int maxG = highestValue(cG, image);
            int maxB = highestValue(cB, image);
            int minR = lowerValue(cR, image);
            int minG = lowerValue(cG, image);
            int minB = lowerValue(cB, image);

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    cR[w, h] = Escalonate(cR[w, h], maxR, minR);
                    cG[w, h] = Escalonate(cG[w, h], maxG, minG);
                    cB[w, h] = Escalonate(cB[w, h], maxB, minB);
                }
            }

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(cR[w, h], cG[w, h], cB[w, h]));
                }
            }
            return image;
        }

        private int Escalonate(int value, int tmax, int tmin)
        {
            double M = 255;

            int r = (int)((M / (tmax - tmin)) * (value - tmin));

            return r;
        }

        private int lowerValue(int[,] value, Bitmap image)
        {
            int tmin = 255;

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    if (value[w, h] < tmin)
                        tmin = value[w, h];
                }
            }

            return tmin;
        }

        private int highestValue(int[,] value, Bitmap image)
        {
            int tmax = 0;

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    if (value[w, h] > tmax)
                        tmax = value[w, h];
                }
            }

            return tmax;
        }

        int[,] histoAcumulado;
        private void initializeHistogram(Bitmap image)
        {
            histoAcumulado = histogramaAcumulado(image);
            //Ejecutamos el botón del histograma rojo
            histogramRed();
            histogramGreen();
            histogramBlue();
        }

        public int[,] histogramaAcumulado(Bitmap bmp)
        {
            
            byte Red = 0;
            byte Green = 0;
            byte Blue = 0;
            
            int[,] matrizAcumulada = new int[3, 256];
            
            for (int i = 0; i <= bmp.Width - 1; i++)
            {
                for (int j = 0; j <= bmp.Height - 1; j++)
                {
                    Red = bmp.GetPixel(i, j).R;
                    
                    Green = bmp.GetPixel(i, j).G;
                    Blue = bmp.GetPixel(i, j).B;
                    
                    matrizAcumulada[0, Red] += 1;
                    matrizAcumulada[1, Green] += 1;
                    matrizAcumulada[2, Blue] += 1;
                }
            }
            return matrizAcumulada;
        }

        public void histogramRed()
        {
            chart1.Series["Histogram R"].Points.Clear();
            
            chart1.Series["Histogram R"].Color = Color.Red;
            for (int i = 0; i <= 255; i++)
            {
                chart1.Series["Histogram R"].Points.AddXY(i + 1, histoAcumulado[0, i]);
            }
        }

        public void histogramGreen()
        {
            chart2.Series["Histogram G"].Points.Clear();
            chart2.Series["Histogram G"].Color = Color.Green;
            for (int i = 0; i <= 255; i++)
            {
                chart2.Series["Histogram G"].Points.AddXY(i + 1, histoAcumulado[1, i]);
            }
        }

        public void histogramBlue()
        {
            chart3.Series["Histogram B"].Points.Clear();
            chart3.Series["Histogram B"].Color = Color.Blue;
            for (int i = 0; i <= 255; i++)
            {
                chart3.Series["Histogram B"].Points.AddXY(i + 1, histoAcumulado[2, i]);
            }
        }
    }
}
