using System.Drawing;
using OpenCvSharp;

namespace ImageAssist
{
    public class ImageInfo
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ChannelCount { get; private set; }
        public int BitDepth { get; private set; }

        public string FileName { get; init; }

        public ImageInfo(string fileName)
        {
            this.FileName = fileName;
        }

        public void GetImageInfoType1()
        {
            using (var mat = new Mat(this.FileName))
            {
                Width = mat.Width;
                Height = mat.Height;
                ChannelCount = mat.Channels();
                BitDepth = mat.ElemSize1() * 8;
            }
        }

        public void GetImageInfoType2()
        {
            using (var image = Image.FromFile(this.FileName))
            {
                Width = image.Width;
                Height = image.Height;
                Bitmap bitmap = new Bitmap(image);
                ChannelCount = bitmap.PixelFormat.ToString().Contains("Indexed") ? 1 : Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                BitDepth = Image.GetPixelFormatSize(bitmap.PixelFormat);
            }
        }
    }
}