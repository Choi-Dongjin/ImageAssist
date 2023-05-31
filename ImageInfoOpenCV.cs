using OpenCvSharp;
using System;
using System.Drawing;
using System.Runtime.Versioning;

namespace ImageAssist
{
    public class ImageInfoOpenCV : IDisposable
    {
        #region IDisposable

        // To detect redundant calls
        private bool _disposed = false;

        ~ImageInfoOpenCV() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) { return; }

            if (disposing)
            {
                Bytes = Array.Empty<byte>();
            }
            _disposed = true;
        }

        #endregion IDisposable


        private bool disposedValue = false;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ChannelCount { get; private set; }
        public int BitDepth { get; private set; }
        public byte[] Bytes { get; set; }
        public string FileName { get; init; }

        public ImageInfoOpenCV(string fileName)
        {
            FileName = fileName;
            try
            {
                Bytes = System.IO.File.ReadAllBytes(fileName);
                GetImageInfo();
            }
            catch (Exception ex)
            {
                throw new System.IO.IOException(ex.ToString());
            }
        }

        [SupportedOSPlatform("windows")]
        private void GetImageInfo()
        {
            try
            {
                using (var mat = Cv2.ImDecode(Bytes, ImreadModes.AnyColor))
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

                using (var image = System.Drawing.Image.FromFile(FileName))
                {
                    Width = image.Width;
                    Height = image.Height;
                    using (var bitmap = new Bitmap(image))
                    {
                        ChannelCount = bitmap.PixelFormat.ToString().Contains("Indexed") ? 1 : System.Drawing.Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                        BitDepth = System.Drawing.Image.GetPixelFormatSize(bitmap.PixelFormat);
                    }
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