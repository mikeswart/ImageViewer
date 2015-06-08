using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ImageViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (listBox1.Items.Count == 0)
            {
                return;
            }

            var image = (IImageContainer)listBox1.Items[e.Index];
            e.Graphics.DrawImage(image.Image, e.Bounds);

            e.DrawFocusRectangle();
        }

        private void trackBar1_ValueChanged(object sender, System.EventArgs e)
        {
            listBox1.ItemHeight = (trackBar1.Value * 10) + 10;
        }

        private void Form1_Activated(object sender, System.EventArgs e)
        {
            listBox1.Items.AddRange(new ImageFactory().CreateRainbow());
        }
    }

    internal sealed class ImageFactory
    {
        private static readonly Size imageSize = new Size(400, 400);

        public IImageContainer[] CreateRainbow()
        {
            var images = new List<IImageContainer>();

            for (var colorKey = 0; colorKey < 3; colorKey++)
            {
                for (var hue = 0; hue < 256; hue += 4)
                {
                    switch (colorKey)
                    {
                        case 0:
                            images.Add(CreateImage(Color.FromArgb(hue, 128, 128)));
                            break;
                        case 1:
                            images.Add(CreateImage(Color.FromArgb(128, hue, 128)));
                            break;
                        case 2:
                            images.Add(CreateImage(Color.FromArgb(128, 128, hue)));
                            break;
                    }
                }
            }

            return images.ToArray();
        }

        private IImageContainer CreateImage(Color color)
        {
            Bitmap tempImage = new Bitmap(imageSize.Width, imageSize.Height);
            using (var graphics = Graphics.FromImage(tempImage))
            {
                graphics.Clear(color);
                Thread.Sleep(50);
            }

            return new ImageContainer(tempImage);
        }
    }

    internal interface IImageContainer
    {
        Image Image { get; }
    }

    internal sealed class ImageContainer : IImageContainer
    {
        public Image Image { get; private set; }

        public ImageContainer(Image image)
        {
            Image = image;
        }
    }
}
