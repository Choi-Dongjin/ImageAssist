using Image = SixLabors.ImageSharp.Image;

namespace ImageAssist
{
    public class ImageInfoImageSharp : IDisposable
    {
        #region IDisposable

        // To detect redundant calls
        private bool _disposed = false;

        ~ImageInfoImageSharp() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                Bytes = Array.Empty<byte>();
            }
            _disposed = true;
        }

        #endregion IDisposable

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int ChannelCount { get; private set; }
        public int BitDepth { get; private set; }
        public byte[] Bytes { get; set; }
        public string FileName { get; init; }

        public ImageInfoImageSharp(string fileName)
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

        private void GetImageInfo()
        {
            try
            {
                using var image = Image.Load<Rgba32>(Bytes);
                Width = image.Width;
                Height = image.Height;
                ChannelCount = 4; // RGBA32 이미지의 채널 수는 항상 4입니다.
                BitDepth = 32; // RGBA32 이미지의 비트 깊이는 항상 32입니다.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while getting image info: {ex.Message}");
                throw new Exception("Failed to read image in image helper.");
            }
        }
    }
}