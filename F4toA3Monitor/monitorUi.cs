using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace F4toA3Monitor
{
    public partial class monitorUi : Form
    {
        private Thread  falconLocatorThread;
        private int     locationX;
        private int     locationY;

        public monitorUi()
        {
            InitializeComponent();

            spawnLocatorThread();

            setInitialMarker();
        }

        public void setInitialMarker() {
            int pbheight = System.Convert.ToInt32((this.Height * 0.5) - 16.5);
            int pbwidth = System.Convert.ToInt32((this.Width * 0.5) - 12.5);

            pictureBox2.Location = new System.Drawing.Point(pbwidth, pbheight);
            pictureBox2.BackColor = Color.Transparent;
        }

        public void updateMap()
        {
            pictureBox1.Location = new System.Drawing.Point(locationX, locationY);

            //Load an image in from a file
            Image image = new Bitmap(@"L:\F4toA3Monitor\F4 Moving Map\F4toA3Monitor\Resources\stealthfighter.png");
            //Set our picture box to that image
            pictureBox2.Image = (Bitmap)image.Clone();

            //Store our old image so we can delete it
            Image oldImage = pictureBox2.Image;
            //Pass in our original image and return a new image rotated 35 degrees right
            pictureBox2.Image = RotateImage(image, new PointF(0, 0), 35);
            if (oldImage != null)
            {
                oldImage.Dispose();
            } 
        }

        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        public void updateLocationX(int value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(updateLocationX), new object[] { value });
                return;
            }

            this.locationX = value * -1;

            updateMap();
        }

        public void updateLocationY(int value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(updateLocationY), new object[] { value });
                return;
            }

            this.locationY = value;

            updateMap();
        }

        private void spawnLocatorThread()
        {
            this.abortLocatorThread();

            this.falconLocatorThread = new System.Threading.Thread(delegate()
            {
                falconCustomLocator.Start(this);
            });

            this.falconLocatorThread.Start();
        }

        private void abortLocatorThread()
        {
            if (this.falconLocatorThread != null && this.falconLocatorThread.IsAlive)
            {
                this.falconLocatorThread.Abort();
            }
        }
    }
}
