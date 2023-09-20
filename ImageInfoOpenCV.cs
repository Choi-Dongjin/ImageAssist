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
        public ImageSize ImageSize { get; set; } = new();
        public byte[] Bytes { get; set; }
        public string FileName { get; init; }
        public EAssistExtension Extension { get; set; }

        [SupportedOSPlatform("windows")]
        public ImageInfoOpenCV(string fileName)
        {
            FileName = fileName;
            try
            {
                Bytes = System.IO.File.ReadAllBytes(fileName);
                Extension = DImageInfo.GetImageExtension(Bytes);
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
            ImageSize imageSize = new ImageSize();
            try
            {
                using (var mat = Cv2.ImDecode(Bytes, ImreadModes.AnyColor))
                {
                    imageSize.Width = mat.Width;
                    imageSize.Height = mat.Height;
                    imageSize.Channel = mat.Channels();
                    imageSize.BitDepth = mat.Depth() * 8;
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
                    imageSize.Width = image.Width;
                    imageSize.Height = image.Height;
                    using (var bitmap = new Bitmap(image))
                    {
                        imageSize.Channel = bitmap.PixelFormat.ToString().Contains("Indexed") ? 1 : System.Drawing.Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                        imageSize.BitDepth = System.Drawing.Image.GetPixelFormatSize(bitmap.PixelFormat);
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