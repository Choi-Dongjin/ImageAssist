using OpenCvSharp;
using System.Drawing;

namespace ImageAssist
{
    public class ImageInfo : IDisposable
    {
        #region IDisposable Support

        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) 
                {
                    Bytes = Array.Empty<byte>(); ;
                }
               
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ChannelCount { get; private set; }
        public int BitDepth { get; private set; }
        public byte[] Bytes { get; set; }

        public string FileName { get; init; }

        public ImageInfo(string fileName)
        {
            try
            {
                Bytes = File.ReadAllBytes(fileName);
            }
            catch (Exception ex)
            {
                throw new System.IO.IOException(ex.ToString());
            }

            Mat imageMat = Cv2.ImDecode(Bytes, ImreadModes.AnyColor);
            Width = imageMat.Width;
            Height = imageMat.Height;
            ChannelCount = imageMat.Channels();
            BitDepth = imageMat.Depth() * 8;
            FileName = fileName;
            imageMat.Dispose();
        }

        public void GetImageInfo()
        {
            try
            {
                using (var mat = new Mat(this.FileName))
                {
                    Width = mat.Width;
                    Height = mat.Height;
                    ChannelCount = mat.Channels();
                    BitDepth = mat.Depth() * 8;
                }
            }
            catch (OpenCvSharp.OpenCVException)
            {
                // 현재 운영 체제 정보 가져오기
                OperatingSystem os = Environment.OSVersion;
                // 플랫폼이 Windows인지 확인
                bool isWindows = (os.Platform == PlatformID.Win32NT);
                if (!isWindows)
                {
                    throw new OpenCvSharp.OpenCVException("Failed to read image in image helper.\r\nSince it is not a Windows operating system, I have not tried other methods.");
                }

                using (var image = Image.FromFile(this.FileName))
                {
                    Width = image.Width;
                    Height = image.Height;
                    Bitmap bitmap = new Bitmap(image);
                    ChannelCount = bitmap.PixelFormat.ToString().Contains("Indexed") ? 1 : Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                    BitDepth = Image.GetPixelFormatSize(bitmap.PixelFormat);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while getting image info: {ex.Message}");
                throw new OpenCvSharp.OpenCVException("Failed to read image in image helper.");
            }
        }
    }
}